//
// Shake - C# Make
//
// MSBuild task
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
using Shake.Tasks.Base;

namespace Shake.Tasks
{
    /// <summary>
    /// MSBuild task.
    /// </summary>
    public class MsBuildTask : CommandLineTask
    {
        public string Solution
        {
            get { return _solution; }
            set { _solution = Path.GetFullPath(value); }
        }
        private string _solution;

        public List<string> Targets { get; private set; }
        public DynamicProperties Properties { get; private set; }

        public MsBuildTask(string solution = null)
        {
            Targets = new List<string>();
            Properties = new DynamicProperties();

            if (!string.IsNullOrEmpty(solution))
                Solution = solution;
        }

        public void Build()
        {
            var buildParams = FormatBuildParams();
            Exec(EnvironmentHelper.MsBuildExe, buildParams);
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", EnvironmentHelper.MsBuildExe, FormatBuildParams());
        }

        private string FormatBuildParams()
        {
            var buildParamas = string.Format("\"{0}\" {1} {2}", Solution, BuildTargets(), BuildProperties());
            return buildParamas.Replace("  ", " "); // remove double-spaces
        }

        private string BuildTargets()
        {
            return Targets.Join("/t:");
        }

        private string BuildProperties()
        {
            var props = new List<string>();

            foreach (var p in Properties.GetMembers())
                props.Add("{0}=\"{1}\"", p.Key, p.Value);

            return props.Join("/p:");
        }
    }
}