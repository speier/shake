//
// Shake - C# Make
//
// Revision control tasks base
//
// Author:
//   Kalman Speier (kalman.speier@gmail.com)
//
// Licensed under the terms of the MIT X11
//
// Copyright (c) 2010 Kalman Speier
//
namespace Shake.Tasks.Base
{
    /// <summary>
    /// Revision control tasks base class
    /// </summary>
    public abstract class RevisionControlTask : CommandLineTask
    {

    }

    /// <summary>
    /// Revision control task commands
    /// </summary>
    public enum RevisionControlTaskCommands
    {
        Checkout,
        Update,
        Add,
        Commit
    }
}