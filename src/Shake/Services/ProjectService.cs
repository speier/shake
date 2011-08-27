using Shake.Interfaces;
using System.IO;

namespace Shake.Services
{
    public class ProjectService : IShakeService
    {
        public string Directory
        {
            get { return System.IO.Directory.GetCurrentDirectory(); }
        }

        public string DirectoryCombine(string dir)
        {
            return Path.Combine(Directory, dir);
        }
    }
}