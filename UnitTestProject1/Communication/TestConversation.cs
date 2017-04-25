using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Common.Communication;
using Common.Messages;
using Common.Messages.Replies;
using Common.Messages.Requests;

using TestCommon.TestObjects;

namespace TestCommon.Communication
{
    [TestClass]
    public class TestConversation
    {
        [TestMethod]
        public void TestConversationDefaults()
        {
            SimpleRequestReplyInitiator simple = new SimpleRequestReplyInitiator();
            Assert.IsNotNull(simple);
            Assert.IsNull(simple.EventResponse);
            Assert.IsNull(simple.SentMessage);
            Assert.IsNotNull(simple.Properties);

            ConversationManager.PrimaryCommunicator.LocalEndpoint = new IPEndPoint(IPAddress.Any, 0);

            Assert.AreEqual(simple.Id.Pid, 0);
            Assert.AreEqual(simple.MaxRetries, (uint)5);
            Assert.AreEqual(simple.Timeout, 2000);

            simple.Start();
            Thread.Sleep(250);
            Assert.IsTrue(simple.IsActive());
            simple.Stop();
            Thread.Sleep(250);
            Assert.IsFalse(simple.IsActive());
        }

        [TestMethod]
        public void TestConversationNoCommunicator()
        {
            SimpleRequestReplyInitiator simple = new SimpleRequestReplyInitiator();
            Assert.IsNotNull(simple);
            Assert.IsNull(simple.EventResponse);
            Assert.IsNull(simple.SentMessage);
            Assert.IsNotNull(simple.Properties);

            ConversationManager.PrimaryCommunicator = null;

            Assert.AreEqual(simple.Id.Pid, 0);
            Assert.AreEqual(simple.MaxRetries, (uint)5);
            Assert.AreEqual(simple.Timeout, 2000);

            simple.Start();
            Thread.Sleep(250);
            Assert.IsTrue(simple.IsActive());
            simple.Stop();
            Thread.Sleep(250);
            Assert.IsFalse(simple.IsActive());
        }

        [TestMethod]
        public void ConversationSendAndReceive()
        {
            MessageNumber initiatorNumber = new MessageNumber
            {
                Pid = 0,
                Seq = 10
            };
            MessageNumber responderNumber = new MessageNumber
            {
                Pid = 1,
                Seq = 10
            };

            IPEndPoint initiatorEndpoint = new IPEndPoint(IPAddress.Loopback, 6789);
            IPEndPoint responderEndpoint = new IPEndPoint(IPAddress.Loopback, 6788);

            UdpCommunicator initiatorCommunicator = new UdpCommunicator(initiatorEndpoint.Port);
            UdpCommunicator responderCommunicator = new UdpCommunicator(responderEndpoint.Port);

            SimpleRequestReplyInitiator convInitiator = new SimpleRequestReplyInitiator(initiatorCommunicator, responderEndpoint);
            SimpleRequestReplyResponder convResponder = new SimpleRequestReplyResponder(responderCommunicator);

            Assert.AreNotEqual(convResponder.Id, convInitiator.Id);
            Assert.IsFalse(ConversationManager.ConversationDictionary.ContainsKey(convInitiator.Id));
            Assert.IsFalse(ConversationManager.ConversationDictionary.ContainsKey(convResponder.Id));
            Assert.IsFalse(convResponder.IsActive());
            Assert.IsFalse(convInitiator.IsActive());

            convResponder.Start();
            convInitiator.Start();

            Thread.Sleep(5000);
            Assert.IsTrue(convResponder.IsActive());
            Assert.IsTrue(convInitiator.IsActive());
            Assert.IsNotNull(convInitiator.SentMessage);
            Assert.IsNotNull(convResponder.ReceivedMessage);
            Assert.AreNotEqual(convResponder.ReceivedMessages.Count, 0);

            Thread.Sleep(5000);
            Assert.IsNotNull(convResponder.SentMessage);
            Assert.IsNotNull(convInitiator.ReceivedMessage);
            Assert.AreNotEqual(convInitiator.ReceivedMessages.Count, 0);
        }

        [TestCleanup]
        public void CleanupConversations()
        {
            ConversationManager.ClearConversations();
        }
    }
}
