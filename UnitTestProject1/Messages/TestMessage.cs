using Common.Messages;
using Common.Messages.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCommon.Messages
{
    [TestClass]
    public class TestMessage
    {
        [TestMethod]
        public void EncodeDecodeBaseMessage()
        {
            Message testMessage = new Message();
            Assert.AreEqual(typeof(Message), testMessage.GetType());
            byte[] encodedBytes = testMessage.Encode();
            Assert.IsNotNull(encodedBytes);
            Message decodedMessage = Message.Decode(encodedBytes);
            Assert.IsNotNull(decodedMessage);
            Assert.AreEqual(typeof(Message), decodedMessage.GetType());
        }

        [TestMethod]
        public void EncodeDecodeLoginRequest()
        {
            LoginRequest testMessage = new LoginRequest();
            Assert.AreEqual(typeof(LoginRequest), testMessage.GetType());
            Assert.IsNull(testMessage.IdentityInfo);
            byte[] encodedBytes = testMessage.Encode();
            Assert.IsNotNull(encodedBytes);
            Message decodedMessage = Message.Decode(encodedBytes);
            Assert.IsNotNull(decodedMessage);
            Assert.AreEqual(typeof(LoginRequest), decodedMessage.GetType());

            testMessage = new LoginRequest();
            Assert.IsNull(testMessage.IdentityInfo);
            testMessage.IdentityInfo = new Common.Users.Identity();
            Assert.AreEqual(typeof(LoginRequest), testMessage.GetType());
            Assert.IsNotNull(testMessage.IdentityInfo);
            encodedBytes = testMessage.Encode();
            Assert.IsNotNull(encodedBytes);
            decodedMessage = Message.Decode(encodedBytes);
            Assert.IsNotNull(decodedMessage);
            Assert.IsNotNull(((LoginRequest)decodedMessage).IdentityInfo);
            Assert.AreEqual(typeof(LoginRequest), decodedMessage.GetType());
        }
    }
}
