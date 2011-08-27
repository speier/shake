//
// Shake - C# Make
//
// Shake arguments
//
// Author:
//   Kalman Speier (kalman.speier@gmail.com)
//
// Licensed under the terms of the MIT X11
//
// Copyright (c) 2010 Kalman Speier
//
using System.Collections.Generic;
using System.IO;
using Shake.Helpers;

namespace Shake.Core
{
    /// <summary>
    /// Shake arguments class.
    /// </summary>
    internal class ShakeArgs
    {
        internal bool ShowHelp { get; private set; }

        internal string ShakeFilePath
        {
            get { return Path.GetFullPath(_shakeFilePath); }
            private set { _shakeFilePath = value; }
        }
        private string _shakeFilePath;

        internal string ShakeClassName { get; private set; }

        internal bool WriteLog { get; private set; }

        internal string LogFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_logFilePath))
                    _logFilePath = Path.ChangeExtension(ShakeFilePath, ".log");

                _logFilePath = Path.GetFullPath(_logFilePath);

                if (Path.GetDirectoryName(_logFilePath) == Directory.GetCurrentDirectory())
                    _logFilePath = Path.Combine(Path.GetDirectoryName(ShakeFilePath),
                            Path.GetFileName(_logFilePath));

                return _logFilePath;
            }
            private set { _logFilePath = value; }
        }
        private string _logFilePath;

        internal List<string> Targets { get; private set; }
        internal DynamicProperties Properties { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        internal ShakeArgs(IEnumerable<string> args)
        {
            ShakeFilePath = Path.Combine(Directory.GetCurrentDirectory(), ShakeConsts.DefaultFileName);
            ShakeClassName = ShakeConsts.DefaultClassName;

            Targets = new List<string>();
            Properties = new DynamicProperties();

            Parse(args);

            // if there is no target specified, then we add "Default" target
            if (Targets.Count == 0)
                Targets.Add(ShakeConsts.DefaultTargetName);
        }

        /// <summary>
        /// Parsing command line arguments.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        private void Parse(IEnumerable<string> args)
        {
            foreach (var arg in args)
            {
                if (arg[0] != '/') // non-switch, assume target
                {
                    Targets.Add(arg);
                }
                
                if (arg[0] == '/') // switch
                {
                    if (arg[1] == '?' || arg == "/help" || arg == "/h") // shake help
                    {
                        ShowHelp = true;
                        return;
                    }

                    if (arg[1] == 'f' && arg[2] == ':') // shakefile path
                    {
                        ShakeFilePath = arg.Substring(3);
                    }

                    if (arg[1] == 'c' && arg[2] == ':') // target class name
                    {
                        ShakeClassName = arg.Substring(3);
                    }

                    if (arg[1] == 'p' && arg[2] == ':') // property
                    {
                        var props = arg.Substring(3).Split(';');
                        foreach (var prop in props)
                        {
                            if (string.IsNullOrEmpty(prop))
                                continue;

                            var parts = prop.Split('=');
                            if (parts.Length != 2)
                                continue;

                            Properties.Set(parts[0], parts[1]);
                        }
                    }

                    if (arg[1] == 'l') // log
                    {
                        WriteLog = true;

                        if (arg.Length > 2 && arg[2] == ':') // logfile specified
                        {
                            LogFilePath = arg.Substring(3);
                        }
                    }
                }
            }
        }
    }
}