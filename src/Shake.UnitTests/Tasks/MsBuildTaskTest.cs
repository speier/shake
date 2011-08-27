using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shake.Tasks;

namespace Shake.Tests.Tasks
{
    /// <summary>
    /// This is a test class for MSBuildTaskTest
    ///</summary>
    [TestClass()]
    public class MsBuildTaskTest
    {
        [TestMethod()]
        public void MsBuildTaskParamsTest()
        {
            dynamic msb = new MsBuildTask();

            msb.Solution = "Shake.sln";
            msb.Targets.Add("clean");
            msb.Targets.Add("build");
            msb.Properties.Configuration = "release";
            msb.Properties.OutputPath = "msbuild_deploy";

            Assert.AreEqual(msb.Properties["configuration"], "release");
        }
    }
}