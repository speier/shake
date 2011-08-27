using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shake.Tasks;

namespace Shake.Tests.Tasks
{
    [TestClass]
    public class SvnTaskTest
    {
        private const string SvnPath = @"c:\temp\";

        [TestMethod]
        public void CheckoutTest()
        {
            // new SvnTask().Checkout(SvnPath, "https://fasterflect.svn.codeplex.com/svn");
        }

        [TestMethod]
        public void UpdateTest()
        {
            // new SvnTask().Update(SvnPath);
        }
    }
}