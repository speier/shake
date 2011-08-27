//
// Shake - C# Make
//
// Shake errors and exceptions
//
// Author:
//   Kalman Speier (kalman.speier@gmail.com)
//
// Licensed under the terms of the MIT X11
//
// Copyright (c) 2010 Kalman Speier
//
using System;

namespace Shake.Core
{
    /// <summary>
    /// Shake error codes (command line exit codes).
    /// </summary>
    public class ShakeErrors
    {
        public const int Exception = 1;
        public const int ShakefileNotFound = 2;
    }

    /// <summary>
    /// Shake's base exception.
    /// </summary>
    public class ShakeException : Exception
    {
        public ShakeException(string message = "")
            : base(message)
        {
        }

        public ShakeException(string format, params object[] args)
            : this(string.Format(format, args))
        {
        }
    }
}