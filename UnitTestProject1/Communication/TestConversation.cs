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
            simple.Updated += new SimpleConversation.MessageReceived(SCCallback);
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

            ConversationFactory.PrimaryCommunicator.LocalEndpoint = new IPEndPoint(IPAddress.Any, 0);

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
    }
}
