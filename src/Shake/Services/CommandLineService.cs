using Shake.Core;
using Shake.Helpers;
using Shake.Interfaces;

namespace Shake.Services
{
    public class CommandLineService : IShakeService
    {
        public dynamic Properties
        {
            get { return ShakeApp.Arguments.Properties; }
        }

        public void Exec(string fileName, string arguments = "")
        {
            new ProcessWrapper().Exec(fileName, arguments);
        }
    }
}