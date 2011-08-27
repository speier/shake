//
// Shake - C# Make
//
// Shake console application
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
using Shake.Helpers;
using Shake.Services;
using Castle.DynamicProxy;

namespace Shake.Core
{
    public class LogInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.Write("{0}: ", DateTime.Now);

            invocation.Proceed();
        }
    }

    public class Logger
    {
        public virtual void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }

    /// <summary>
    /// Shake console application.
    /// </summary>
    public class ShakeApp
    {
        static Logger logger;

        static void SetupCastle()
        {
            logger = new ProxyGenerator().CreateClassProxy<Logger>(new LogInterceptor());
        }

        /// <summary>
        /// Shake arguments.
        /// </summary>
        internal static ShakeArgs Arguments { get; private set; }

        /// <summary>
        /// Shake console app main entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        private static void Main(string[] args)
        {

            SetupCastle();
            return;

            // log console output
            var logger = new TextWriterLogger(Console.Out);
            Console.SetOut(logger);

            // parse command line args
            Arguments = new ShakeArgs(args);

            Console.Title = ReflectionHelper.AssemblyTitle;
            Console.WriteLine("{0} v{1}", ReflectionHelper.AssemblyTitle, ReflectionHelper.ShortAssemblyVersion);
            Console.WriteLine("Copyright (c) 2010 Kalman Speier");
            Console.WriteLine("http://shake.codeplex.com\n");

            if (Arguments.ShowHelp)
            {
                WriteOutUsage();
                return;
            }

            if (File.Exists(Arguments.ShakeFilePath))
            {
                var stopWatch = Stopwatch.StartNew();
                try
                {
                    // start shake
                    Run(Arguments);
                }
                catch (Exception ex)
                {
                    Environment.ExitCode = ShakeErrors.Exception;

                    if (!string.IsNullOrEmpty(ex.Message))
                        Console.WriteLine(ex.Message);
                }
                finally
                {
                    stopWatch.Stop();
                    var elapsed = stopWatch.Elapsed;

                    Console.WriteLine("\nFinished.\n");
                    Console.WriteLine("Total time: {0}.{1} seconds.",
                        elapsed.Seconds, elapsed.Milliseconds / 100);

                    if (Arguments.WriteLog)
                        logger.SaveToFile(Arguments.LogFilePath);
                }
            }
            else
            {
                Environment.ExitCode = ShakeErrors.ShakefileNotFound;

                Console.WriteLine("Shakefile not found.\n{0}\n", Arguments.ShakeFilePath);
                WriteOutUsage();
            }

            if (Debugger.IsAttached)
                Console.ReadKey();
        }

        /// <summary>
        /// Init shake runner and evaluate shakefile.
        /// </summary>
        /// <param name="arguments">Shake arguments.</param>
        private static void Run(ShakeArgs arguments)
        {
            // Init shake runner
            ShakeRunner.Init(ReflectionHelper.ExecutingAssembly, "Shake.Services", "Shake.Tasks");

            // Evaluate shake API
            ShakeRunner.EvalApiClass("shake",
                new ApiClassWrapper(typeof(CommandLineService), true),
                new ApiClassWrapper(typeof(ProjectService), false));

            ShakeRunner.OnEvaluateTarget += (e) => Console.WriteLine("Target: {0}\n", e.TargetName);

            // Evaluate shakefile
            ShakeRunner.EvaluateShakeFile(arguments);
        }

        /// <summary>
        /// Basic method to write out the usage text to the console.
        /// </summary>
        private static void WriteOutUsage()
        {
            Console.WriteLine("Syntax:              Shake.exe [targets] [options]");
            Console.WriteLine();
            Console.WriteLine("Description:         Executes the specified targets in the shakefile.");
            Console.WriteLine("                     Example:");
            Console.WriteLine("                       shake clean build /p:Configuration=Release");
            Console.WriteLine();
            Console.WriteLine("Targets:             Target names.");
            Console.WriteLine("                     Use space to separate multiple targets.");
            Console.WriteLine("                     Default is \"Default\"");
            Console.WriteLine();
            Console.WriteLine("Switches:");
            Console.WriteLine();
            Console.WriteLine("   /f:<shakefile>    Shakefile name.");
            Console.WriteLine("                     Default is \"Shakefile.cs\"");
            Console.WriteLine();
            Console.WriteLine("   /c:<targetclass>  Targets class name.");
            Console.WriteLine("                     Default is \"Targets\"");
            Console.WriteLine();
            Console.WriteLine("   /p:<n>=<v>        Project-level properties.");
            Console.WriteLine("                     <n> is property name, <v> is property value.");
            Console.WriteLine("                     Use semicolon to separate multiple properties.");
            Console.WriteLine("                     Example:");
            Console.WriteLine("                       /p:WarningLevel=2;OutDir=bin\\Debug\\");
            Console.WriteLine();
            Console.WriteLine("   /l:<logfile>      Log filename.");
            Console.WriteLine("                     Example:");
            Console.WriteLine("                       /l");
            Console.WriteLine("                       /l:mylog.txt");
            Console.WriteLine("                       /l:c:\\shake.log");
            Console.WriteLine();
        }
    }
}