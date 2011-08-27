//
// Shake - C# Make
//
// Command line tasks base
//
// Author:
//   Kalman Speier (kalman.speier@gmail.com)
//
// Licensed under the terms of the MIT X11
//
// Copyright (c) 2010 Kalman Speier
//
using Shake.Helpers;
using Shake.Interfaces;

namespace Shake.Tasks.Base
{
    /// <summary>
    /// Command line tasks base class
    /// </summary>
    public abstract class CommandLineTask : ProcessWrapper, IShakeTask
    {

    }
}