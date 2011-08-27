//
// Shake - C# Make
//
// AssemblyInfo task
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
using System.Text;
using Shake.Helpers;
using Shake.Interfaces;

namespace Shake.Tasks
{
    /// <summary>
    /// AssemblyInfo task.
    /// </summary>
    public class AssemblyInfoTask : IShakeTask
    {
        private const string DefaultName = "AssemblyInfo.cs";

        private readonly string _assemblyInfoDir;
        private readonly string _assemblyInfoName;

        public List<string> Usings { get; private set; }

        public string AssemblyTitle { get; set; }
        public string AssemblyDescription { get; set; }
        public string AssemblyConfiguration { get; set; }
        public string AssemblyCompany { get; set; }
        public string AssemblyProduct { get; set; }
        public string AssemblyCopyright { get; set; }
        public string AssemblyTrademark { get; set; }
        public string AssemblyCulture { get; set; }

        public Version AssemblyVersion { get; set; }
        public Version AssemblyFileVersion { get; set; }

        public bool ComVisible { get; set; }
        public Guid Guid { get; set; }

        public List<string> UnknownAttributes { get; private set; }

        public int BuildNumber
        {
            get { return AssemblyVersion.Build; }
            set { SetBuildNumber(value); }
        }

        public AssemblyInfoTask()
        {
            _assemblyInfoName = DefaultName;
            Guid = Guid.NewGuid();

            // standard AssemblyInfo usings
            Usings = new List<string>
            {
                "System.Reflection",
                "System.Runtime.CompilerServices",
                "System.Runtime.InteropServices"
            };

            UnknownAttributes = new List<string>();
        }

        public AssemblyInfoTask(string dir, string name = DefaultName)
            : this()
        {
            _assemblyInfoDir = dir;
            _assemblyInfoName = name;

            Load();
        }

        public void IncBuildNumber(int incNumber = 1)
        {
            IncAssemblyVersion(build: incNumber);
        }

        public void IncAssemblyVersion(int major = 0, int minor = 0, int build = 0, int revision = 0)
        {
            major += AssemblyVersion.Major;
            minor += AssemblyVersion.Minor;
            build += AssemblyVersion.Build;
            revision += AssemblyVersion.Revision;

            SetAssemblyVersion(major, minor, build, revision);
        }

        public void SetBuildNumber(int build = 0)
        {
            SetAssemblyVersion(AssemblyVersion.Major, AssemblyVersion.Minor, build, AssemblyVersion.Revision);
        }

        public void SetAssemblyVersion(int major = 0, int minor = 0, int build = 0, int revision = 0)
        {
            AssemblyVersion = new Version(major, minor, build, revision);
        }

        public void Load()
        {
            Load(_assemblyInfoDir, _assemblyInfoName);
        }

        public void Load(string dir, string name = DefaultName)
        {
            var path = Path.GetFullPath(Path.Combine(dir, name));

            if (!File.Exists(path))
                return;

            // parse existing AssemblyInfo
            Parse(path);
        }

        public void Save()
        {
            if (Directory.Exists(_assemblyInfoDir))
                Save(_assemblyInfoDir, _assemblyInfoName);
        }

        public void Save(string dir, string name = DefaultName)
        {
            File.WriteAllText(Path.Combine(dir, name), this.ToString(), Encoding.UTF8);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var u in Usings)
                sb.AppendLine("using {0};", u);

            sb.AppendLine();
            sb.AppendLine(PropertyToAssemblyAttribute("AssemblyTitle", AssemblyTitle));
            sb.AppendLine(PropertyToAssemblyAttribute("AssemblyDescription", AssemblyDescription));
            sb.AppendLine(PropertyToAssemblyAttribute("AssemblyConfiguration", AssemblyConfiguration));
            sb.AppendLine(PropertyToAssemblyAttribute("AssemblyCompany", AssemblyCompany));
            sb.AppendLine(PropertyToAssemblyAttribute("AssemblyProduct", AssemblyProduct));
            sb.AppendLine(PropertyToAssemblyAttribute("AssemblyCopyright", AssemblyCopyright));
            sb.AppendLine(PropertyToAssemblyAttribute("AssemblyTrademark", AssemblyTrademark));
            sb.AppendLine(PropertyToAssemblyAttribute("AssemblyCulture", AssemblyCulture));
            sb.AppendLine();
            sb.AppendLine(PropertyToAssemblyAttribute("ComVisible", ComVisible));
            sb.AppendLine(PropertyToAssemblyAttribute("Guid", Guid));
            sb.AppendLine();
            sb.AppendLine(PropertyToAssemblyAttribute("AssemblyVersion", AssemblyVersion));
            sb.AppendLine(PropertyToAssemblyAttribute("AssemblyFileVersion", AssemblyFileVersion));
            sb.AppendLine();

            foreach (var ua in UnknownAttributes)
                sb.AppendLine(ua);

            return sb.ToString();
        }

        private string PropertyToAssemblyAttribute(string name, object property)
        {
            if (property is bool)
                return string.Format("[assembly: {0}({1})]", name, property.ToString().ToLower());

            return string.Format("[assembly: {0}(\"{1}\")]", name, property);
        }

        private void Parse(string path)
        {
            var text = File.ReadAllText(path);

            // assembly info usings
            var usingMatches = RegexHelper.GetUsings(text);
            foreach (var um in usingMatches.Where(u => !Usings.Contains(u)))
                Usings.Add(um);

            // assembly attributes
            var attribMatches = RegexHelper.GetAssemblyAttributes(text);
            foreach (var a in attribMatches)
                SetProperty(a.Key, a.Value);
        }

        private void SetProperty(string name, string value)
        {
            value = value.Replace("\"", "");

            switch (name)
            {
                default:
                    if (!ReflectionHelper.TrySetPropertyValue(this, name, value))
                        UnknownAttributes.Add(PropertyToAssemblyAttribute(name, value));
                    break;

                case AssemlbyAttributeNames.AssemblyVersion:
                    AssemblyVersion = new Version(value);
                    break;

                case AssemlbyAttributeNames.AssemblyFileVersion:
                    AssemblyFileVersion = new Version(value);
                    break;

                case AssemlbyAttributeNames.ComVisible:
                    ComVisible = value.ToLower().Equals("true");
                    break;

                case AssemlbyAttributeNames.Guid:
                    Guid = Guid.Parse(value);
                    break;
            }
        }
    }

    /// <summary>
    /// Assembly attribute name constants
    /// </summary>
    internal class AssemlbyAttributeNames
    {
        internal const string AssemblyVersion = "AssemblyVersion";
        internal const string AssemblyFileVersion = "AssemblyFileVersion";
        internal const string ComVisible = "ComVisible";
        internal const string Guid = "Guid";
    }
}