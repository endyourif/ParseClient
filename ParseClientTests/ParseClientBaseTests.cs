using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParseClient;

namespace ParseClientTests
{
    [TestClass]
    public class ParseClientBaseTests
    {
        private string currentObjectId;

        private Client client;

        private const string testClassName = "testClass";

        [TestMethod]
        public void TestPostMethod()
        {
            client = new Client();
            currentObjectId = client.Post(testClassName, "some data");

            Assert.IsTrue(currentObjectId.Length > 1);
        }

        [TestMethod]
        public void TestPutMethod()
        {
            TestPostMethod();

            client.Put(testClassName, currentObjectId, "some new data");

            var actual = client.Get(testClassName, currentObjectId);

            Assert.AreEqual(currentObjectId, actual.objectId);
            Assert.AreEqual("some new data", actual.encryptedData);
        }

        [TestMethod]
        public void TestGetMethod()
        {
            TestPostMethod();

            var actual = client.Get(testClassName, currentObjectId);

            Assert.AreEqual(currentObjectId, actual.objectId);
            Assert.AreEqual("some data", actual.encryptedData);
        }
    }
}
