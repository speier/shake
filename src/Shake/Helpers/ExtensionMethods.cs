//
// Shake - C# Make
//
// Extension methods
//
// Author:
//   Kalman Speier (kalman.speier@gmail.com)
//
// Licensed under the terms of the MIT X11
//
// Copyright (c) 2010 Kalman Speier
//
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shake.Helpers;

/// <summary>
/// Extension methods.
/// </summary>
internal static class ExtensionMethods
{
    /// <summary>
    /// Returns whether string contains any of the specified keywords as a substring.
    /// Case-insensitive.
    /// </summary>
    /// <param name="value">Value of the string</param>
    /// <param name="keywords">Array with keywords</param>
    /// <returns>True when value contains any of the specified keywords</returns>
    internal static bool ContainsAny(this string value, params string[] keywords)
    {
        return keywords.Any(k => value.ToLower().Contains(k.ToLower()));
    }

    internal static bool MatchAny(this string value, params string[] patterns)
    {
        return patterns.Any(p => RegexHelper.Wildcard(p).IsMatch(value));
    }

    internal static bool NotMatchAny(this string value, params string[] patterns)
    {
        return (patterns == null) ? true : !MatchAny(value, patterns);
    }

    /// <summary>
    /// Append new line foramtted.
    /// </summary>
    /// <param name="sb">String builder.</param>
    /// <param name="format">Composite format string.</param>
    /// <param name="args">Objects to format.</param>
    internal static void AppendLine(this StringBuilder sb, string format, params object[] args)
    {
        sb.AppendFormat(format, args);
        sb.AppendLine();
    }

    /// <summary>
    /// Adds new formatted string into a generic string list.
    /// </summary>
    /// <param name="list">Generic string list.</param>
    /// <param name="format">Composite format string.</param>
    /// <param name="args">Objects to format.</param>
    internal static void Add(this List<string> list, string format, params object[] args)
    {
        list.Add(string.Format(format, args));
    }

    /// <summary>
    /// Joins the list.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="prefix">The prefix.</param>
    /// <param name="separator">The separator.</param>
    /// <returns></returns>
    internal static string Join(this List<string> list, string prefix = "", string separator = ";")
    {
        return (list.Count > 0)
           ? string.Format("{0}{1}", prefix, string.Join(separator, list))
           : string.Empty;
    }
}