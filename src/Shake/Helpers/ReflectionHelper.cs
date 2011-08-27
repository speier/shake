//
// Shake - C# Make
//
// Reflection helper
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
using System.Reflection;

namespace Shake.Helpers
{
    /// <summary>
    /// Reflection helper methods.
    /// </summary>
    internal class ReflectionHelper
    {
        # region Property Get/Set helper methods

        internal static PropertyInfo GetPropertyInfo(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName);
        }

        internal static void SetPropertyValue(object obj, string propertyName, object value)
        {
            var proprety = GetPropertyInfo(obj, propertyName);

            proprety.SetValue(obj, value, null);
        }

        /// <summary>
        /// Tries to set the property value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        internal static bool TrySetPropertyValue(object obj, string propertyName, object value)
        {
            try
            {
                SetPropertyValue(obj, propertyName, value);

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Assembly helper methods

        internal static Version AssemblyVersion
        {
            get { return ExecutingAssembly.GetName().Version; }
        }

        internal static string ShortAssemblyVersion
        {
            get { return string.Format("{0}.{1}.{2}", AssemblyVersion.Major, AssemblyVersion.Minor, AssemblyVersion.Build); }
        }

        internal static string AssemblyTitle
        {
            get
            {
                if (string.IsNullOrEmpty(_assemblyTitle))
                {
                    var attributes = ExecutingAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                    if (attributes.Length > 0)
                    {
                        var titleAttribute = (AssemblyTitleAttribute)attributes[0];
                        if (!string.IsNullOrEmpty(titleAttribute.Title))
                            return titleAttribute.Title;
                    }

                    _assemblyTitle = Path.GetFileNameWithoutExtension(ExecutingAssembly.CodeBase);
                }

                return _assemblyTitle;
            }
        }
        private static string _assemblyTitle;

        internal static Assembly ExecutingAssembly
        {
            get { return Assembly.GetExecutingAssembly(); }
        }

        internal static string AssemblyPath
        {
            get { return ExecutingAssembly.Location; }
        }

        internal static string AssemblyDirectory
        {
            get { return Path.GetDirectoryName(AssemblyPath); }
        }

        /// <summary>
        /// Creates a new instance of AssemblyName class from path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>AssemblyName instance.</returns>
        internal static AssemblyName CreateAssemblyName(string path)
        {
            return new AssemblyName(Path.GetFileNameWithoutExtension(path)) { CodeBase = path };
        }

        /// <summary>
        /// Trying to load and reference assembly by namspace.
        /// </summary>
        /// <param name="nameSpaces">Namespaces to look for.</param>
        /// <param name="asmCallback">Callback method.</param>
        internal static void TryReferenceAssemblyByNamspace(List<string> nameSpaces,
            Action<Assembly> asmCallback)
        {
            var assemblyPaths = Directory.GetFiles(ReflectionHelper.AssemblyDirectory, "*.dll");

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve +=
                (o, e) => Assembly.ReflectionOnlyLoad(e.Name); // GAC

            foreach (var path in assemblyPaths)
            {
                // load assembly into reflection only context
                // in this way we don't need to create and use another app domain
                var refasm = Assembly.ReflectionOnlyLoadFrom(path);
                try
                {
                    // GetExportedTypes method throws's an exception when we can't resolve references
                    // we will hide this exception and let the compiler threw a type exception
                    // might be we will change to CCI or Cecil...
                    if (refasm.GetExportedTypes().Any(t => nameSpaces.Contains(t.Namespace)))
                    {
                        var asm = Assembly.Load(refasm.GetName());
                        asmCallback(asm);
                    }
                }
                catch
                {
                    // hiding unresolved assembly exceptions
                    // compiler will throw a type/namespace could not be found exception
                }
            }
        }

        #endregion
    }
}