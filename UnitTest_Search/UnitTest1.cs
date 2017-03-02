using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using RegistryServices;

namespace UnitTest_Search
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void testQuery()
        {
            var reference = "16.0";

            var result = Service.QueryRegistry(Registry.ClassesRoot,
                    @"Excel.Application\CurVer")
                .Replace(".0", "").Split('.').Last() + ".0";

            Assert.AreEqual(reference, result);
        }
        [TestMethod]
        public void testFindKey_skip1 ()
        {
            var target =
            Registry.CurrentUser.OpenSubKey("SOFTWARE");
            target = target?.OpenSubKey("Microsoft");
            target = target?.OpenSubKey("Office");
            target = target?.OpenSubKey("16.0");

            var result = Service.FindKey(Registry.CurrentUser,
                new List<string> {"SOFTWARE", "Microsoft", "16.0"});

            Assert.AreEqual(result.Name, target?.Name, result.Name);
        }
        [TestMethod]
        public void testFindKey_skip2 ()
        {
            var target =
            Registry.CurrentUser.OpenSubKey("SOFTWARE");
            target = target?.OpenSubKey("Microsoft");
            target = target?.OpenSubKey("Office");
            target = target?.OpenSubKey("16.0");

            var result = Service.FindKey(Registry.CurrentUser,
                new List<string> { "SOFTWARE", "16.0" });

            Assert.AreEqual(result.Name, target?.Name, result.Name);
        }
        [TestMethod]
        public void testFindKey_LastOnly ()
        {
            var target =
            Registry.CurrentUser.OpenSubKey("SOFTWARE");
            target = target?.OpenSubKey("Microsoft");
            target = target?.OpenSubKey("Office");
            target = target?.OpenSubKey("16.0");

            var result = Service.FindKey(Registry.CurrentUser,
                new List<string> { "16.0" });

            Assert.AreEqual(result.Name, target?.Name, result.Name);
        }

        [TestMethod]
        public void isCaseSensitive()
        {
            var target =
            Registry.CurrentUser.OpenSubKey("SOFTWARE");
            target = target?.OpenSubKey("Microsoft");
            target = target?.OpenSubKey("Office");
            target = target?.OpenSubKey("16.0");

            var result = Service.FindKey(Registry.CurrentUser,
                new List<string> { "sOfTWArE", "16.0" });

            Assert.AreNotEqual(result.Name, target?.Name, result.Name);
        }
    }
}
