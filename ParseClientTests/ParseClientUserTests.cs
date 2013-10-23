using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParseClient;

namespace ParseClientTests
{
    [TestClass]
    public class ParseClientUserTests
    {
        private string currentObjectId;

        private Client client;

        [TestMethod]
        public void TestPostMethod()
        {
            client = new Client();
            currentObjectId = client.PostUserData("some data");

            Assert.IsTrue(currentObjectId.Length > 1);
        }

        [TestMethod]
        public void TestPutMethod()
        {
            TestPostMethod();

            client.PutUserData(currentObjectId, "some new data");

            var actual = client.GetUserData(currentObjectId);

            Assert.AreEqual(currentObjectId, actual.objectId);
            Assert.AreEqual("some new data", actual.encryptedData);
        }

        [TestMethod]
        public void TestGetMethod()
        {
            TestPostMethod();

            var actual = client.GetUserData(currentObjectId);

            Assert.AreEqual(currentObjectId, actual.objectId);
            Assert.AreEqual("some data", actual.encryptedData);
        }
    }
}
