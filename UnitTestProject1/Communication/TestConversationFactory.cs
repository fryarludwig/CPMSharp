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
        public void ConvFactorySucessfulInstance()
        {
            ConversationManager.RegisterNewConversationType(typeof(AliveRequest), typeof(SimpleRequestReplyInitiator));
            Conversation testConv = ConversationManager.CreateNewConversation(new Envelope(new AliveRequest()));
            Assert.IsNotNull(testConv);
            Assert.AreEqual(testConv.GetType(), typeof(SimpleRequestReplyInitiator));
            Assert.IsFalse(testConv.IsActive());
            testConv.Stop();
        }

        [TestMethod]
        public void ConvFactoryGracefulFailure()
        {
            ConversationManager.RegisterNewConversationType(typeof(AliveRequest), typeof(SimpleRequestReplyInitiator));
            Conversation testConv = ConversationManager.CreateNewConversation(new Envelope(new AliveReply()));
            Assert.IsNull(testConv);
        }

        [TestMethod]
        public void ConvFactoryCommunicatorInstance()
        {
            ConversationManager.RegisterNewConversationType(typeof(AliveRequest), typeof(SimpleRequestReplyInitiator));

            Assert.IsNotNull(ConversationManager.PrimaryCommunicator);
            Assert.IsFalse(ConversationManager.PrimaryCommunicator.IsActive());

            Conversation testConv = ConversationManager.CreateNewConversation(new Envelope(new AliveRequest()));
            ConversationManager.RegisterConversation(testConv);
            Assert.IsNotNull(testConv);
            Assert.IsFalse(testConv.IsActive());
            Assert.AreEqual(testConv.GetType(), typeof(SimpleRequestReplyInitiator));
            Assert.IsTrue(ConversationManager.ConversationDictionary.ContainsKey(testConv.Id));
            testConv.Stop();
            testConv.Start();
            Thread.Sleep(250);
            Assert.IsTrue(testConv.IsActive());
            //((SimpleRequestReplyInitiator)testConv).LocalEndpoint = new IPEndPoint(IPAddress.Any, 54345);
            testConv.Stop();
            Thread.Sleep(250);
            Assert.IsFalse(testConv.IsActive());
            ConversationManager.PrimaryCommunicator.Stop();
            Thread.Sleep(250);
            Assert.IsFalse(ConversationManager.PrimaryCommunicator.IsActive());

            //Assert.IsFalse(true);
        }

        [TestCleanup]
        public void CleanupConversations()
        {
            ConversationManager.ClearConversations();
        }
    }
}
