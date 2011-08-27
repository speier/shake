//
// Shake - C# Make
//
// Regex helper
//
// Author:
//   Kalman Speier (kalman.speier@gmail.com)
//
// Licensed under the terms of the MIT X11
//
// Copyright (c) 2010 Kalman Speier
//
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Shake.Helpers
{
    /// <summary>
    /// Regular Expression helper
    /// </summary>
    internal class RegexHelper
    {
        internal static readonly Regex UsingNamespaces = new Regex(@"using\W*(?<Using>.+);");
        internal static readonly Regex AssemblyAttributes = new Regex(@"\[assembly:\W*(?<Title>.+)\((?<Value>.+)\)\]");

        /// <summary>
        /// Creating Regex from a wildcard pattern.
        /// Supports * and ? wildcards.
        /// </summary>
        /// <param name="wildcardPattern">The pattern string.</param>
        /// <returns>Newly created Regex instance with wildcard pattern.</returns>
        internal static Regex Wildcard(string wildcardPattern)
        {
            var regexPattern = string.Format("^{0}$",
                Regex.Escape(wildcardPattern).Replace("\\*", ".*").Replace("\\?", "."));
            
            return new Regex(regexPattern, RegexOptions.IgnoreCase);
        }

        internal static List<string> GetUsings(string text)
        {
            var usings = new List<string>();

            var usingMatches = UsingNamespaces.Matches(text);
            foreach (Match um in usingMatches)
            {
                if (um.Groups.Count < 1)
                    continue;

                var v = um.Groups["Using"].Value;

                usings.Add(v);
            }

            return usings;
        }

        internal static Dictionary<string, string> GetAssemblyAttributes(string text)
        {
            var attributes = new Dictionary<string, string>();

            var attribMatches = AssemblyAttributes.Matches(text);
            foreach (Match am in attribMatches)
            {
                if (am.Groups.Count < 3)
                    continue;

                var k = am.Groups["Title"].Value;
                var v = am.Groups["Value"].Value;

                attributes.Add(k, v);
            }

            return attributes;
        }

        internal static Match FormatMatch(string value, string patternFormat, params object[] patternArgs)
        {
            var pattern = string.Format(patternFormat, patternArgs);
            return Regex.Match(value, pattern, RegexOptions.IgnoreCase);
        }
    }
}