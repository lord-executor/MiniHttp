using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.Utilities
{
    [TestFixture]
    class UrlPathFixture
    {
        [Test]
        public void TestAbsolutePath()
        {
            var path = new UrlPath("/test/bla");
            Assert.IsTrue(path.IsAbsolute);
            Assert.AreEqual("/test/bla", path.Path);
        }

        [Test]
        public void TestRelativePath()
        {
            var path = new UrlPath("test/path");
            Assert.IsFalse(path.IsAbsolute);
            Assert.AreEqual("test/path", path.Path);
        }

        [Test]
        public void TestRootPath()
        {
            var path = new UrlPath("/");
            Assert.IsTrue(path.IsAbsolute);
            Assert.AreEqual("/", path.Path);
        }

        [Test]
        public void TestEmptyPath()
        {
            var path = new UrlPath("");
            Assert.IsFalse(path.IsAbsolute);
            Assert.AreEqual("", path.Path);
        }

        [Test]
        public void TestTrailingSlashTrim()
        {
            var path = new UrlPath("path/with/trailing/");
            Assert.AreEqual("path/with/trailing", path.Path);
        }
    }
}
