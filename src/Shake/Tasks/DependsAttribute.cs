using System;
using System.Collections.Generic;

namespace Shake.Tasks
{
    [Obsolete]
    public class DependsAttribute : Attribute
    {
        public IEnumerable<string> TargetNames { get; private set; }

        public DependsAttribute(string targetsCsv)
            : this(targetsCsv.Split(','))
        {
        }

        public DependsAttribute(params string[] targets)
        {
            TargetNames = targets;
        }
    }
}