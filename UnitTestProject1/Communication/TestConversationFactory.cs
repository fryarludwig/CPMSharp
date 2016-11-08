using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Communication;
using Common.Messages;
using Common.Messages.Replies;
using Common.Messages.Requests;
using TestCommon.TestObjects;

namespace TestCommon.Communication
{
    [TestClass]
    public class TestConversationFactory
    {
        [TestMethod]
        public void SucessfulInstance()
        {
            ConversationFactory.RegisterNewConversationType(typeof(AliveRequest), typeof(TestHeartbeatConversation));
            Conversation testConv = ConversationFactory.CreateNewConversation(new Envelope(new AliveRequest()));
            Assert.IsNotNull(testConv);
            Assert.AreEqual(testConv.GetType(), typeof(TestHeartbeatConversation));
            testConv.Stop();
        }

        [TestMethod]
        public void GracefulFailure()
        {
            ConversationFactory.RegisterNewConversationType(typeof(AliveRequest), typeof(TestHeartbeatConversation));
            Conversation testConv = ConversationFactory.CreateNewConversation(new Envelope(new AliveReply()));
            Assert.IsNull(testConv);
        }
    }
}
