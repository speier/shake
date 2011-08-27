//
// Shake - C# Make
//
// SVN task
//
// Author:
//   Kalman Speier (kalman.speier@gmail.com)
//
// Licensed under the terms of the MIT X11
//
// Copyright (c) 2010 Kalman Speier
//
using System;
using Shake.Core;
using Shake.Tasks.Base;

namespace Shake.Tasks
{
    /// <summary>
    /// SVN task.
    /// </summary>
    public class SvnTask : RevisionControlTask
    {
        // we assume that svn.exe is in path
        private const string SvnExe = "svn";

        public string Username { get; set; }
        public string Password { get; set; }

        public void Checkout(string path, string url)
        {
            Execute(RevisionControlTaskCommands.Checkout, path, url);
        }

        public void Update(string path)
        {
            Execute(RevisionControlTaskCommands.Update, path);
        }

        public void Add(string path)
        {
            Execute(RevisionControlTaskCommands.Add, path);
        }

        public void Commit(string path, string message)
        {
            Execute(RevisionControlTaskCommands.Commit, path, message: message);
        }

        private void Execute(RevisionControlTaskCommands command, string path,
            string url = null, string message = null)
        {
            var args = string.Format("{0} {1} {2}",
                command.ToString().ToLower(), url, path)
                .Replace("  ", " "); // remove double-spaces

            if (!string.IsNullOrEmpty(message))
                args += String.Format(" --message \"{0}\"", message);

            if (!string.IsNullOrEmpty(Username))
                args += String.Format(" --username \"{0}\"", Username);

            args += string.Format(" --password \"{0}\"", Password);

            Exec(SvnExe, args);
        }

        private bool ValidSnvResult(RevisionControlTaskCommands command, string result)
        {
            if (string.IsNullOrEmpty(result))
                return false;

            switch (command)
            {
                case RevisionControlTaskCommands.Checkout:
                    return result.Contains("Checked out");

                case RevisionControlTaskCommands.Update:
                    return (result.Contains("At revision") || result.Contains("Updated to revision"));

                case RevisionControlTaskCommands.Add:
                    return result[0].Equals('A');

                case RevisionControlTaskCommands.Commit:
                    return result.Contains("Committed");

                default:
                    return false;
            }
        }
    }
}