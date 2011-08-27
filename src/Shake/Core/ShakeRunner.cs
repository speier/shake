//
// Shake - C# Make
//
// Shake runner
//
// Author:
//   Kalman Speier (kalman.speier@gmail.com)
//
// Licensed under the terms of the MIT X11
//
// Copyright (c) 2010 Kalman Speier
//
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Mono.CSharp;
using Shake.Helpers;

namespace Shake.Core
{
    /// <summary>
    /// Delegate for evaluate target event.
    /// </summary>
    /// <param name="e">Event args for shake's evaluate target event.</param>
    public delegate void ShakeRunnerEvaluateTarget(ShakeRunnerEvaluateTargetEventArgs e);

    /// <summary>
    /// Shake's runner.
    /// Using Mono's C# compiler evaluator.
    /// </summary>
    public static class ShakeRunner
    {
        /// <summary>
        /// Evaluate target event.
        /// </summary>
        public static event ShakeRunnerEvaluateTarget OnEvaluateTarget;

        /// <summary>
        /// Initialize shake runner.
        /// </summary>
        /// <param name="assembly">Default reference Assembly.</param>
        /// <param name="usings">Default name space(s).</param>
        public static void Init(Assembly assembly = null, params string[] usings)
        {
            // init mono's c# evaluator
            Evaluator.Init(new string[0]);

            // evaluate default usings
            Run("using System;");
            Run("using System.Collections;");
            Run("using System.Collections.Generic;");
            Run("using System.Dynamic;");
            Run("using System.IO;");
            Run("using System.Linq;");
            Run("using System.Reflection;");
            Run("using System.Text;");

            // reference assembly if assigned
            if (assembly != null)
                Evaluator.ReferenceAssembly(assembly);

            // evaluate using params if any
            foreach (var u in usings)
                EvaluateStatement("using {0};", u);
        }

        /// <summary>
        /// Evaluate a file.
        /// </summary>
        /// <param name="dir">Directory name.</param>
        /// <param name="filename">Filename.</param>
        public static void EvaluateFile(string dir, string filename)
        {
            EvaluateFile(Path.Combine(dir, filename));
        }

        /// <summary>
        /// Evaluate a file.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <returns>The evaluated text.</returns>
        public static string EvaluateFile(string path)
        {
            path = Path.GetFullPath(path);
            var text = File.ReadAllText(path);

            var nameSpaces = RegexHelper.GetUsings(text);
            ReflectionHelper.TryReferenceAssemblyByNamspace(nameSpaces, Evaluator.ReferenceAssembly);

            Evaluator.FileName = Path.GetFileName(path);

            Run(text);

            return text;
        }

        /// <summary>
        /// Evaluate a statement
        /// </summary>
        /// <param name="format">Composite format string</param>
        /// <param name="args">Objects to format</param>
        public static void EvaluateStatement(string format, params object[] args)
        {
            Run(string.Format(format, args));
        }

        /// <summary>
        /// Evaluate a shakefile.
        /// </summary>
        /// <param name="args">Shake arguments.</param>
        internal static void EvaluateShakeFile(ShakeArgs args)
        {
            var file = EvaluateFile(args.ShakeFilePath);

            var classMatch = RegexHelper.FormatMatch(file, "class (?<Name>{0})", args.ShakeClassName);
            if (classMatch.Success)
            {
                var className = classMatch.Groups["Name"].Value;
                var classVar = className.ToLower();

                EvaluateStatement("var {0} = new {1}();", classVar, className);

                foreach (var targetName in args.Targets)
                {
                    var targetMatch = RegexHelper.FormatMatch(file, @"public void {0}\(\)", targetName);
                    
                    if (targetMatch.Success)
                        EvaluateTarget(classVar, targetName);
                }
            }
        }

        /// <summary>
        /// Evaluating target.
        /// </summary>
        /// <param name="className">Class variable name.</param>
        /// <param name="targetName">Target name.</param>
        internal static void EvaluateTarget(string classVar, string targetName)
        {
            if (OnEvaluateTarget != null)
                OnEvaluateTarget(new ShakeRunnerEvaluateTargetEventArgs(targetName));

            EvaluateStatement("{0}.{1}();", classVar, targetName);
        }

        /// <summary>
        /// Evaluates the API class.
        /// A public static class which holds service classes as static or dynamic members.
        /// </summary>
        /// <param name="className">Name of the API class.</param>
        /// <param name="apiClassMembers">The API class members.</param>
        internal static void EvalApiClass(string className, params ApiClassWrapper[] apiClassMembers)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("public static class {0}", className);
            stringBuilder.AppendLine("{");
            foreach (var apiClassMember in apiClassMembers)
            {
                stringBuilder.AppendLine("public static readonly {0} {1} = new {2}();",
                    apiClassMember.IsDynamic ? "dynamic" : apiClassMember.TypeName,
                    apiClassMember.FriendlyName,
                    apiClassMember.TypeName);
            }
            stringBuilder.AppendLine("}");

            Run(stringBuilder.ToString());
        }

        internal static void Run(string statement)
        {
            Evaluator.Run(statement);

            if (Evaluator.ctx.Report.Errors != 0)
                throw new ShakeException();
        }
    }

    /// <summary>
    /// Event args for shake's evaluate target event.
    /// </summary>
    public class ShakeRunnerEvaluateTargetEventArgs : EventArgs
    {
        public string TargetName { get; set; }

        public ShakeRunnerEvaluateTargetEventArgs(string targetName)
        {
            TargetName = targetName;
        }
    }
}