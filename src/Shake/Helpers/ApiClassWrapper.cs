using System;

namespace Shake.Helpers
{
    /// <summary>
    /// Helper class for shake api.
    /// Experimental, needs to refactor.
    /// </summary>
    internal class ApiClassWrapper
    {
        private const string UnwantedSuffix = "Service";

        internal bool IsDynamic { get; set; }
        internal string TypeName { get; set; }

        internal string FriendlyName
        {
            get { return TypeName.Replace(UnwantedSuffix, ""); }
        }

        internal ApiClassWrapper(Type type, bool dynamic)
        {
            TypeName = type.Name;
            IsDynamic = dynamic;
        }
    }
}