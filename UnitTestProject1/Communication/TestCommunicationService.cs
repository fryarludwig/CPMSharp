using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Communication;
using Common.Messages;
using Common.Messages.Replies;
using Common.Messages.Requests;
using TestCommon.TestObjects;
using System.Threading;
using System.Net;

namespace TestCommon.Communication
{
    [TestClass]
    public class TestCommunicationService
    {
        [TestMethod]
        public void SendAndReceive()
        {
            int testerPort = 15559;
            int comServicePort = 15557;
            string testerAddress = "127.0.0.1";
            string comServiceAddress = "127.0.0.1";
            TestUdpSocket testerSocket = new TestUdpSocket(testerPort);
            CommunicationService testComService = CommunicationService.GetUniqueUdpInstance(comServicePort);

            testerSocket.Start();
            AliveRequest request = new AliveRequest();
            AliveReply reply = new AliveReply();
            Envelope requestEnvelope = null;
            Envelope replyEnvelope = null;

            int retries = 20;
            while (!(testerSocket.IsActive() && testComService.IsActive()) && retries-- > 0 && wait(50)) { }

            Assert.IsNotNull(testComService);
            Assert.IsTrue(testComService.IsActive());
            Assert.IsTrue(testerSocket.IsActive());

            testComService.Send(new Envelope(new IPEndPoint(IPAddress.Parse(testerAddress), testerPort), request));
            retries = 20;
            while (!testerSocket.ReplyWaiting && retries-- > 0 && wait(50)) { }

            Assert.IsTrue(testerSocket.ReplyWaiting);
            Assert.IsTrue(testerSocket.Receive(out requestEnvelope));
            Assert.IsNotNull(requestEnvelope);
            Assert.IsTrue(requestEnvelope.Message.GetType() == request.GetType());
            Assert.IsTrue(requestEnvelope.Message.MsgId == request.MsgId);
            Assert.IsTrue(requestEnvelope.Message.ConvId == request.ConvId);
            Assert.IsTrue(requestEnvelope.Address.Address.ToString() == comServiceAddress);
            Assert.IsTrue(requestEnvelope.Address.Port == comServicePort);

            testerSocket.Send(new Envelope(new IPEndPoint(IPAddress.Parse(comServiceAddress), comServicePort), reply));

            retries = 20;
            while (!testComService.ReplyWaiting && retries-- > 0 && wait(50)) { }

            Assert.IsTrue(testComService.ReplyWaiting);
            Assert.IsTrue(testComService.Receive(out replyEnvelope));
            Assert.IsNotNull(replyEnvelope);
            Assert.IsTrue(replyEnvelope.Message.GetType() == reply.GetType());
            Assert.IsTrue(replyEnvelope.Message.MsgId == reply.MsgId);
            Assert.IsTrue(replyEnvelope.Message.ConvId == reply.ConvId);
            Assert.IsTrue(replyEnvelope.Address.Address.ToString() == testerAddress);
            Assert.IsTrue(replyEnvelope.Address.Port == testerPort);
            testerSocket.Stop();
            testComService.Stop();

            retries = 20;
            while ((testerSocket.IsActive() || testComService.IsActive()) && retries-- > 0 && wait(50)){ }

            Assert.IsFalse(testComService.IsActive());
            Assert.IsFalse(testerSocket.IsActive());
        }

        [TestMethod]
        public void SendAndReceiveMultiple()
        {
            int testerPort = 45558;
            int comServicePort = 45556;
            string testerAddress = "127.0.0.1";
            string comServiceAddress = "127.0.0.1";
            TestUdpSocket testerSocket = new TestUdpSocket(testerPort);
            CommunicationService testComService = CommunicationService.GetUniqueUdpInstance(comServicePort);

            testerSocket.Start();
            AliveRequest request = new AliveRequest();
            LoginRequest request2 = new LoginRequest();
            AliveReply reply = new AliveReply();

            Envelope requestEnvelope = null;
            Envelope replyEnvelope = null;

            int retries = 20;
            while (!(testerSocket.IsActive() && testComService.IsActive()) && retries-- > 0 && wait(50)) { }

            Assert.IsNotNull(testComService);
            Assert.IsTrue(testComService.IsActive());
            Assert.IsTrue(testerSocket.IsActive());

            testComService.Send(new Envelope(new IPEndPoint(IPAddress.Parse(testerAddress), testerPort), request));
            testComService.Send(new Envelope(new IPEndPoint(IPAddress.Parse(testerAddress), testerPort), reply));
            testComService.Send(new Envelope(new IPEndPoint(IPAddress.Parse(testerAddress), testerPort), request2));
            retries = 20;
            while (!testerSocket.ReplyWaiting && retries-- > 0 && wait(50)) { }

            Assert.IsTrue(testerSocket.ReplyWaiting);
            Assert.IsTrue(testerSocket.Receive(out requestEnvelope));
            Assert.IsNotNull(requestEnvelope);
            Assert.IsTrue(requestEnvelope.Message.GetType() == request.GetType());
            Assert.IsTrue(requestEnvelope.Message.MsgId == request.MsgId);
            Assert.IsTrue(requestEnvelope.Message.ConvId == request.ConvId);
            Assert.IsTrue(requestEnvelope.Address.Address.ToString() == comServiceAddress);
            Assert.IsTrue(requestEnvelope.Address.Port == comServicePort);
            requestEnvelope = null;

            Assert.IsTrue(testerSocket.Receive(out requestEnvelope));
            Assert.IsNotNull(requestEnvelope);
            Assert.IsTrue(requestEnvelope.Message.GetType() == reply.GetType());
            Assert.IsTrue(requestEnvelope.Message.MsgId == reply.MsgId);
            Assert.IsTrue(requestEnvelope.Message.ConvId == reply.ConvId);
            Assert.IsTrue(requestEnvelope.Address.Address.ToString() == comServiceAddress);
            Assert.IsTrue(requestEnvelope.Address.Port == comServicePort);
            requestEnvelope = null;

            Assert.IsTrue(testerSocket.Receive(out requestEnvelope));
            Assert.IsNotNull(requestEnvelope);
            Assert.IsTrue(requestEnvelope.Message.GetType() == request2.GetType());
            Assert.IsTrue(requestEnvelope.Message.MsgId == request2.MsgId);
            Assert.IsTrue(requestEnvelope.Message.ConvId == request2.ConvId);
            Assert.IsTrue(requestEnvelope.Address.Address.ToString() == comServiceAddress);
            Assert.IsTrue(requestEnvelope.Address.Port == comServicePort);

            testerSocket.Send(new Envelope(new IPEndPoint(IPAddress.Parse(comServiceAddress), comServicePort), reply));

            retries = 20;
            while (!testComService.ReplyWaiting && retries-- > 0 && wait(50)) { }

            Assert.IsTrue(testComService.ReplyWaiting);
            Assert.IsTrue(testComService.Receive(out replyEnvelope));
            Assert.IsNotNull(replyEnvelope);
            Assert.IsTrue(replyEnvelope.Message.GetType() == reply.GetType());
            Assert.IsTrue(replyEnvelope.Message.MsgId == reply.MsgId);
            Assert.IsTrue(replyEnvelope.Message.ConvId == reply.ConvId);
            Assert.IsTrue(replyEnvelope.Address.Address.ToString() == testerAddress);
            Assert.IsTrue(replyEnvelope.Address.Port == testerPort);
            testerSocket.Stop();
            testComService.Stop();

            retries = 20;
            while ((testerSocket.IsActive() || testComService.IsActive()) && retries-- > 0 && wait(50)) { }

            Assert.IsFalse(testComService.IsActive());
            Assert.IsFalse(testerSocket.IsActive());
        }

        public bool wait(int timeout)
        {
            Thread.Sleep(timeout);
            return true; 
        }
    }
}
