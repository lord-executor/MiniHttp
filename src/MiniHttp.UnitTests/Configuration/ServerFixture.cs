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
    public class ServerFixture
    {
        [Test]
        public void TestLoadConfig()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(ServerFixture), "sample.xml"))
            {
                var config = Server.LoadConfig(new StreamReader(stream));
                Assert.AreEqual(42, config.Port);
            }
        }
    }
}
