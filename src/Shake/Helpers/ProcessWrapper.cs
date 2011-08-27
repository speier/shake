//
// Shake - C# Make
//
// Process wrapper
//
// Author:
//   Kalman Speier (kalman.speier@gmail.com)
//
// Licensed under the terms of the MIT X11
//
// Copyright (c) 2010 Kalman Speier
//
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Shake.Core;

namespace Shake.Helpers
{
    /// <summary>
    /// Command line process.
    /// </summary>
    public class ProcessWrapper
    {
        private readonly Process _process;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ProcessWrapper()
        {
            _process = new Process
                           {
                               StartInfo =
                                   {
                                       CreateNoWindow = true,
                                       UseShellExecute = false,
                                       RedirectStandardOutput = true,
                                       RedirectStandardError = true
                                   }
                           };
        }

        /// <summary>
        /// Starts a command line process.
        /// </summary>
        /// <param name="fileName">Filename.</param>
        /// <param name="arguments">Arguments.</param>
        public void Exec(string fileName, string arguments = "")
        {
            // set default filename to "cmd.exe" with path
            // and use the filename and arguments as parameters after cmd's /c switch
            // because in this way we can execute commands which is in path environment
            _process.StartInfo.FileName = Path.Combine(Environment.SystemDirectory, "cmd.exe");
            _process.StartInfo.Arguments = string.Format("/c {0} {1}", fileName, arguments);

            if (!_process.Start())
                throw new ShakeException("Could not start command line process: {0}", _process.StartInfo.FileName);

            _process.OutputDataReceived += Process_OutputDataReceived;
            _process.ErrorDataReceived += Process_ErrorDataReceived;

            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            _process.WaitForExit();

            if (_process.ExitCode != 0)
                throw new ShakeException(); // decide what message to throw here
        }

        void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        /// <summary>
        /// Reads out console standard and error output.
        /// </summary>
        /// <returns>Standard and error output as tuple.</returns>
        [Obsolete]
        private Tuple<string, string> ReadProcessOutput()
        {
            Func<string> outputStreamAsyncReader = _process.StandardOutput.ReadToEnd;
            Func<string> errorStreamAsyncReader = _process.StandardError.ReadToEnd;

            var outAsyncResult = outputStreamAsyncReader.BeginInvoke(null, null);
            var errAsyncResult = errorStreamAsyncReader.BeginInvoke(null, null);

            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                // WaitHandle.WaitAll fails on single-threaded apartments.
                // Poll for completion instead:
                while (!(outAsyncResult.IsCompleted && errAsyncResult.IsCompleted))
                    Thread.Sleep(10); // check again every 10 milliseconds
            }
            else
            {
                var arWaitHandles = new WaitHandle[2];
                arWaitHandles[0] = outAsyncResult.AsyncWaitHandle;
                arWaitHandles[1] = errAsyncResult.AsyncWaitHandle;

                if (!WaitHandle.WaitAll(arWaitHandles))
                    throw new ShakeException("Command line process aborted: {0}", _process.StartInfo.FileName);
            }

            var output = outputStreamAsyncReader.EndInvoke(outAsyncResult);
            var errors = errorStreamAsyncReader.EndInvoke(errAsyncResult);

            // At this point the process should surely have exited,
            // since both the error and output streams have been fully read.
            // To be paranoid, let's check anyway...
            if (!_process.HasExited)
                _process.WaitForExit();

            return Tuple.Create(output, errors);
        }
    }
}