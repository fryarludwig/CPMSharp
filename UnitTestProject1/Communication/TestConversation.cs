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
        public string SCCallback(string something)
        {
            return "Loud and clear, " + something;
        }

        [TestMethod]
        public void TestConversationEvents()
        {
            SimpleConversation simple = new SimpleConversation();
            Assert.IsNotNull(simple);
            Assert.IsNull(simple.EventResponse);
            Assert.IsNull(simple.ReceivedMessage);
            Assert.IsNull(simple.SentMessage);
            simple.Updated += new SimpleConversation.SimpleMessageReceived(SCCallback);
            simple.OnUpdate();
            Assert.AreEqual("Loud and clear, SimpleConversation", simple.EventResponse);
            Assert.AreEqual(typeof(LoginRequest), simple.ReceivedMessage.Message.GetType());
            Assert.AreEqual(typeof(LoginReply), simple.SentMessage.Message.GetType());
        }

        [TestMethod]
        public void TestConversationDefaults()
        {
            SimpleConversation simple = new SimpleConversation();
            Assert.IsNotNull(simple);
            Assert.IsNull(simple.EventResponse);
            Assert.IsNull(simple.ReceivedMessage);
            Assert.IsNull(simple.SentMessage);
            Assert.IsNotNull(simple.Properties);

            ConversationManager.PrimaryCommunicator.LocalEndpoint = new IPEndPoint(IPAddress.Any, 0);

            Assert.AreEqual(simple.Id.Pid, 0);
            Assert.AreEqual(simple.MaxRetries, (UInt32)5);
            Assert.AreEqual(simple.Timeout, 1000);

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
            SimpleConversation simple = new SimpleConversation();
            Assert.IsNotNull(simple);
            Assert.IsNull(simple.EventResponse);
            Assert.IsNull(simple.ReceivedMessage);
            Assert.IsNull(simple.SentMessage);
            Assert.IsNotNull(simple.Properties);

            ConversationManager.PrimaryCommunicator = null;

            Assert.AreEqual(simple.Id.Pid, 0);
            Assert.AreEqual(simple.MaxRetries, (UInt32)5);
            Assert.AreEqual(simple.Timeout, 1000);

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
            MessageNumber initiatorNumber = new MessageNumber();
            initiatorNumber.Pid = 0;
            initiatorNumber.Seq = 10;
            SimpleConversation convInitiator = new SimpleConversation(initiatorNumber);
            IPEndPoint initiatorEndpoint = new IPEndPoint(IPAddress.Loopback, 6789);
            convInitiator.Communicator.LocalEndpoint = initiatorEndpoint;

            MessageNumber responderNumber = new MessageNumber();
            responderNumber.Pid = 1;
            responderNumber.Seq = 10;
            SimpleConversation convResponder = new SimpleConversation(responderNumber);
            IPEndPoint responderEndpoint = new IPEndPoint(IPAddress.Loopback, 6788);
            convResponder.Communicator.LocalEndpoint = responderEndpoint;

            Assert.AreNotEqual(convResponder.Id, convInitiator.Id);

            //ConversationManager.ConversationDictionary[convInitiator.Id] = convInitiator;
            //ConversationManager.ConversationDictionary[convResponder.Id] = convResponder;

            Assert.IsFalse(convResponder.IsActive());
            Assert.IsFalse(convInitiator.IsActive());
            Assert.AreNotEqual(convResponder.Communicator.LocalEndpoint, convInitiator.Communicator.LocalEndpoint);
            Assert.AreNotEqual(ConversationManager.PrimaryCommunicator.LocalEndpoint, convInitiator.Communicator.LocalEndpoint);
            Assert.AreNotEqual(convResponder.Communicator.LocalEndpoint, ConversationManager.PrimaryCommunicator.LocalEndpoint);

            convResponder.Start();
            convInitiator.Start();

            Thread.Sleep(100);
            Assert.IsTrue(convResponder.IsActive());
            Assert.IsTrue(convInitiator.IsActive());
            Assert.IsTrue(convInitiator.Communicator.IsActive());
            Assert.IsTrue(convResponder.Communicator.IsActive());

            convInitiator.SendHeartbeatRequest(responderEndpoint);
            Thread.Sleep(250);
            Assert.IsNotNull(convResponder.ReceivedMessage);

            convResponder.SendHeartbeatReply(initiatorEndpoint);
            Thread.Sleep(250);
            Assert.IsNotNull(convInitiator.ReceivedMessage);
        }
    }
}
