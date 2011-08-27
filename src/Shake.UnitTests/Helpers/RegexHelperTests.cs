using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shake.Helpers;

namespace Shake.Tests.Helpers
{
    [TestClass]
    public class RegexHelperTests
    {
        [TestMethod]
        public void TestWildcard()
        {
            Assert.IsTrue(RegexHelper.Wildcard("*").IsMatch("template"));

            Assert.IsTrue(RegexHelper.Wildcard("*.exe").IsMatch("cmd.exe"));
            Assert.IsTrue(RegexHelper.Wildcard("*.DLL").IsMatch("Foo.dll"));

            Assert.IsTrue(RegexHelper.Wildcard("*.config").IsMatch("web.config"));
            Assert.IsFalse(RegexHelper.Wildcard("*.config").IsMatch("config"));
            Assert.IsFalse(RegexHelper.Wildcard("*.config").IsMatch("configuration"));

            Assert.IsFalse(RegexHelper.Wildcard("*.cs").IsMatch("css"));
            Assert.IsFalse(RegexHelper.Wildcard("*.cs").IsMatch("graphics"));
        }
    }
}