using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shake.Tasks;

namespace Shake.Tests.Tasks
{
    /// <summary>
    /// Summary description for AssemblyInfoTaskTest
    /// </summary>
    [TestClass]
    public class AssemblyInfoTaskTest
    {
        [TestMethod]
        public void TestAssemblyInfoLoadSave()
        {
            var asmInfo = new AssemblyInfoTask(@"..\..\..\Shake\Properties");
            Assert.AreEqual(asmInfo.AssemblyVersion.Minor, 1);
            Assert.AreEqual(asmInfo.BuildNumber, 0);
        }
    }
}