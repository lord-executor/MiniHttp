using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MiniHttp.Configuration
{
    [TestFixture]
    public class ConfigurationFixture
    {
        private Server GetSampleConfig()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(ConfigurationFixture), "sample.xml"))
            {
                return Server.LoadConfig(new StreamReader(stream));
            }
        }

        [Test]
        public void TestLoadConfig()
        {
            var config = GetSampleConfig();
            Assert.AreEqual(42, config.Port);
        }

        [Test]
        public void TestPreHooks()
        {
            var config = GetSampleConfig();
            Assert.NotNull(config.PreHooks);
            Assert.AreEqual(1, config.PreHooks.Length);
            Assert.AreEqual("pre-hook-type", config.PreHooks[0].TypeName);
        }

        [Test]
        public void TestPostHooks()
        {
            var config = GetSampleConfig();
            Assert.NotNull(config.PostHooks);
            Assert.AreEqual(1, config.PostHooks.Length);
            Assert.AreEqual("post-hook-type", config.PostHooks[0].TypeName);
        }
    }
}
