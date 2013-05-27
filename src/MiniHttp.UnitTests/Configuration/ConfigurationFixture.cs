using MiniHttp.Server;
using MiniHttp.Utilities;
using Moq;
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
        public void TestLoadPreHooks()
        {
            var config = GetSampleConfig();
            Assert.NotNull(config.PreHooks);
            Assert.AreEqual(1, config.PreHooks.Length);
            Assert.AreEqual("pre-hook-type", config.PreHooks[0].Type);
        }

        [Test]
        public void TestLoadPostHooks()
        {
            var config = GetSampleConfig();
            Assert.NotNull(config.PostHooks);
            Assert.AreEqual(1, config.PostHooks.Length);
            Assert.AreEqual("post-hook-type", config.PostHooks[0].Type);
        }

        [Test]
        public void TestLoadRoute()
        {
            var config = GetSampleConfig();
            Assert.NotNull(config.Routes);
            Assert.AreEqual(3, config.Routes.Length);
            Assert.AreEqual("*.html", config.Routes[0].RouteExpression);
            Assert.Null(config.Routes[0].Processors);
        }

        [Test]
        public void TestInstantiateWithDefaultConstructor()
        {
            var config = GetSampleConfig();
            var mapperMock = new Mock<IUrlMapper>(MockBehavior.Strict);
            
            var handler = config.Routes[0].Instantiate(mapperMock.Object);
            Assert.NotNull(handler);
            Assert.IsInstanceOf<DummyHandler>(handler);
            mapperMock.VerifyAll();
        }

        [Test]
        public void TestInstantiateWithUrlMapper()
        {
            var config = GetSampleConfig();
            var mapperMock = new Mock<IUrlMapper>(MockBehavior.Strict);

            var handler = config.Routes[1].Instantiate(mapperMock.Object);
            Assert.NotNull(handler);
            Assert.IsInstanceOf<DummyHandlerWithMapper>(handler);
            Assert.AreEqual(mapperMock.Object, (handler as DummyHandlerWithMapper).Mapper);
            mapperMock.VerifyAll();
        }

        [Test]
        public void TestInstantiateWithProcessors()
        {
            var config = GetSampleConfig();
            var mapperMock = new Mock<IUrlMapper>(MockBehavior.Strict);

            var handler = config.Routes[2].Instantiate(mapperMock.Object);
            Assert.NotNull(handler);
            Assert.IsInstanceOf<DummyHandlerWithProcessors>(handler);
            var processingHandler = handler as DummyHandlerWithProcessors;

            Assert.AreEqual(mapperMock.Object, processingHandler.Mapper);
            Assert.AreEqual(2, processingHandler.Processors.Count());
            Assert.IsInstanceOf<DummyProcessor>(processingHandler.Processors.First());
            Assert.IsInstanceOf<DummyProcessor>(processingHandler.Processors.Skip(1).First());
            mapperMock.VerifyAll();
        }
    }
}
