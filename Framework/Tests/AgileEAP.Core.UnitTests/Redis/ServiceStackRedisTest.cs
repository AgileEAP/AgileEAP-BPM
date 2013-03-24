using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AgileEAP.Core.Caching;
using AgileEAP.Core;

namespace AgileEAP.Core.UnitTests.Redis
{
    [TestClass]
    public class ServiceStackRedisTest
    {
        [TestMethod]
        public void CacheTest()
        {
            ServiceStackRedis client = new ServiceStackRedis();

            string key = "redis_test_" + Guid.NewGuid().ToString();
            client.Add(key, "hello is" + key);

            string value = client.Get<string>(key);
            Assert.AreEqual("hello is" + key, value);

            AgileEAPVersion ver = new AgileEAPVersion()
            {
                ID = "AgileEAP2.0",
                AppName = "AgileEAP",
                Description = "AgileEAP",
                ICP = "AgileEAP",
                eClientName = "AgileEAP",
                eCloudName = "AgileEAP",
                EnglishName = "AgileEAP",
                FullName = "AgileEAP",
                ShortName = "AgileEAP",
                Url = "AgileEAP"
            };

            client.Add(ver.ID, ver);

            AgileEAPVersion value2 = client.Get<AgileEAPVersion>(ver.ID);
            Assert.AreEqual(ver, value2);
        }
    }
}
