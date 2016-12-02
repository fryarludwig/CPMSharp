using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Communication;
using Common.Messages;
using Common.Messages.Replies;
using Common.Messages.Requests;
using TestCommon.TestObjects;
using System.Net;
using System.Threading;

namespace TestCommon.Communication
{
    [TestClass]
    public class TestConversationFactory
    {
        [TestMethod]
        public void SucessfulInstance()
        {
            ConversationFactory.RegisterNewConversationType(typeof(AliveRequest), typeof(SimpleConversation));
            Conversation testConv = ConversationFactory.CreateNewConversation(new Envelope(new AliveRequest()));
            Assert.IsNotNull(testConv);
            Assert.AreEqual(testConv.GetType(), typeof(SimpleConversation));
            testConv.Stop();
        }

        [TestMethod]
        public void GracefulFailure()
        {
            ConversationFactory.RegisterNewConversationType(typeof(AliveRequest), typeof(SimpleConversation));
            Conversation testConv = ConversationFactory.CreateNewConversation(new Envelope(new AliveReply()));
            Assert.IsNull(testConv);
        }

        [TestMethod]
        public void CommunicatorInstance()
        {
            ConversationFactory.RegisterNewConversationType(typeof(AliveRequest), typeof(SimpleConversation));

            Assert.IsNotNull(ConversationFactory.PrimaryCommunicator);
            Assert.IsFalse(ConversationFactory.PrimaryCommunicator.IsActive());

            Conversation testConv = ConversationFactory.CreateNewConversation(new Envelope(new AliveRequest()));
            Assert.IsNotNull(testConv);
            Assert.IsFalse(testConv.IsActive());
            Assert.AreEqual(testConv.GetType(), typeof(SimpleConversation));
            testConv.Stop();
            testConv.Start();
            Thread.Sleep(250);
            Assert.IsTrue(ConversationFactory.PrimaryCommunicator.IsActive());
            ConversationFactory.PrimaryCommunicator.LocalEndpoint = new IPEndPoint(IPAddress.Any, 54345);
            testConv.Stop();
            Thread.Sleep(250);
            Assert.IsTrue(ConversationFactory.PrimaryCommunicator.IsActive());
            ConversationFactory.PrimaryCommunicator.Stop();
            Thread.Sleep(250);
            Assert.IsFalse(ConversationFactory.PrimaryCommunicator.IsActive());
        }
    }
}
