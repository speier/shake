//
// Shake - C# Make
//
// Environment helper
//
// Author:
//   Kalman Speier (kalman.speier@gmail.com)
//
// Licensed under the terms of the MIT X11
//
// Copyright (c) 2010 Kalman Speier
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shake.Helpers
{
    internal class EnvironmentHelper
    {
        internal const string CmdExe = "cmd.exe";
        internal static readonly string MsBuildExe = "msbuild.exe";

        internal const string NetVersion35 = "3.5";
        internal const string NetVersion40 = "4.0.30319";

        static EnvironmentHelper()
        {
            MsBuildExe = GetMsBuildExePath();
        }

        internal static string NetFrameworkRoot
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                    "Microsoft.NET", Environment.Is64BitOperatingSystem ? "Framework64" : "Framework");
            }
        }

        internal static string NetFramework35Path
        {
            get { return GetNetFrameworkPathByVersion(NetVersion35); }
        }

        internal static string NetFramework40Path
        {
            get { return GetNetFrameworkPathByVersion(NetVersion40); }
        }

        private static string GetNetFrameworkPathByVersion(string version)
        {
            return Path.Combine(NetFrameworkRoot, "v" + version);
        }

        private static string GetMsBuildExePath()
        {
            // try .net 4 msbuild.exe first
            var msb40 = Path.Combine(NetFramework40Path, MsBuildExe);
            if (File.Exists(msb40))
                return msb40;

            // obsolete to check 3.5, because shake is targeted .net 4 :)

            // try .net 3.5 msbuild.exe
            var msb35 = Path.Combine(NetFramework35Path, MsBuildExe);
            if (File.Exists(msb35))
                return msb35;

            // return default, leave "msbuild.exe" without path
            return MsBuildExe;
        }

        internal static string ProgramFilesFolder
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            }
        }

        internal static List<string> GetEnvironmentPaths()
        {
            var paths = Environment.GetEnvironmentVariable("pAtH");
            return (paths == null) ? new List<string>() : paths.Split(';').ToList();
        }
    }
}