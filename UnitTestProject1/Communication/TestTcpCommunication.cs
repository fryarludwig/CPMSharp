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
    public class TestTcpCommunication
    {
        [TestMethod]
        public void TcpStreamerConstructor()
        {
            IPEndPoint testEndpoint = new IPEndPoint(IPAddress.Loopback, 15650);
            TcpStreamer tcpTester1 = new TcpStreamer();

            Assert.IsNotNull(tcpTester1);
            Assert.IsFalse(tcpTester1.IsActive);
            Assert.IsFalse(tcpTester1.ReplyWaiting);
            Assert.IsFalse(tcpTester1.Connected);

            tcpTester1.StartAsServer();

            Thread.Sleep(1000);
            Assert.IsTrue(tcpTester1.IsActive);
            Assert.IsFalse(tcpTester1.ReplyWaiting);
            Assert.IsFalse(tcpTester1.Connected);

            tcpTester1.Stop();
            Thread.Sleep(1000);

            Assert.IsFalse(tcpTester1.IsActive);
            Assert.IsFalse(tcpTester1.ReplyWaiting);
            Assert.IsFalse(tcpTester1.Connected);
        }

        [TestMethod]
        public void TcpSendReceiveValid()
        {
            IPEndPoint endpoint1 = new IPEndPoint(IPAddress.Loopback, 15655);
            IPEndPoint endpoint2 = new IPEndPoint(IPAddress.Loopback, 15651);

            TcpStreamer tcpStreamer1 = new TcpStreamer("Client");
            TcpStreamer tcpStreamer2 = new TcpStreamer("Server");

            Assert.IsNotNull(tcpStreamer1);
            Assert.IsFalse(tcpStreamer1.IsActive);
            Assert.IsFalse(tcpStreamer1.ReplyWaiting);
            Assert.IsFalse(tcpStreamer1.Connected);

            Assert.IsNotNull(tcpStreamer2);
            Assert.IsFalse(tcpStreamer2.IsActive);
            Assert.IsFalse(tcpStreamer2.ReplyWaiting);
            Assert.IsFalse(tcpStreamer2.Connected);

            tcpStreamer2.StartAsServer(endpoint1);
            Thread.Sleep(1000);
            tcpStreamer1.StartAsClient(endpoint1);
            Thread.Sleep(1000);

            Assert.IsTrue(tcpStreamer2.IsActive);
            Assert.IsFalse(tcpStreamer2.ReplyWaiting);
            Assert.IsTrue(tcpStreamer2.Connected);

            Assert.IsTrue(tcpStreamer1.IsActive);
            Assert.IsFalse(tcpStreamer1.ReplyWaiting);
            Assert.IsTrue(tcpStreamer1.Connected);


            AliveRequest request = new AliveRequest();
            LoginRequest request2 = new LoginRequest();
            AliveReply reply = new AliveReply();

            tcpStreamer1.Send(new Envelope(endpoint1, request));
            tcpStreamer1.Send(new Envelope(endpoint1, reply));
            tcpStreamer1.Send(new Envelope(endpoint1, request2));

            int retries = 20;
            while (!(tcpStreamer1.ReplyWaiting || tcpStreamer2.ReplyWaiting) && retries-- > 0 && wait(50)) { }

            Assert.IsTrue(tcpStreamer2.ReplyWaiting);
            Assert.IsTrue(tcpStreamer2.Receive(out Envelope requestEnvelope));
            Assert.IsNotNull(requestEnvelope);
            Assert.IsTrue(requestEnvelope.Message.GetType() == request.GetType());
            Assert.IsTrue(requestEnvelope.Message.MsgId == request.MsgId);
            Assert.IsTrue(requestEnvelope.Message.ConvId == request.ConvId);
            requestEnvelope = null;

            retries = 20;
            while (!(tcpStreamer1.ReplyWaiting || tcpStreamer2.ReplyWaiting) && retries-- > 0 && wait(50)) { }

            Assert.IsTrue(tcpStreamer2.Receive(out requestEnvelope));
            Assert.IsNotNull(requestEnvelope);
            Assert.IsTrue(requestEnvelope.Message.GetType() == reply.GetType());
            Assert.IsTrue(requestEnvelope.Message.MsgId == reply.MsgId);
            Assert.IsTrue(requestEnvelope.Message.ConvId == reply.ConvId);
            requestEnvelope = null;

            retries = 20;
            while (!(tcpStreamer1.ReplyWaiting || tcpStreamer2.ReplyWaiting) && retries-- > 0 && wait(50)) { }

            Assert.IsTrue(tcpStreamer2.Receive(out requestEnvelope));
            Assert.IsNotNull(requestEnvelope);
            Assert.IsTrue(requestEnvelope.Message.GetType() == request2.GetType());
            Assert.IsTrue(requestEnvelope.Message.MsgId == request2.MsgId);
            Assert.IsTrue(requestEnvelope.Message.ConvId == request2.ConvId);

            tcpStreamer2.Send(new Envelope(endpoint2, reply));

            retries = 20;
            while (!(tcpStreamer1.ReplyWaiting || tcpStreamer2.ReplyWaiting) && retries-- > 0 && wait(50)) { }

            Assert.IsTrue(tcpStreamer1.ReplyWaiting);
            Assert.IsTrue(tcpStreamer1.Receive(out Envelope replyEnvelope));
            Assert.IsNotNull(replyEnvelope);
            Assert.IsTrue(replyEnvelope.Message.GetType() == reply.GetType());
            Assert.IsTrue(replyEnvelope.Message.MsgId == reply.MsgId);
            Assert.IsTrue(replyEnvelope.Message.ConvId == reply.ConvId);

            tcpStreamer2.Stop();
            tcpStreamer1.Stop();

            Thread.Sleep(1500);

            Assert.IsFalse(tcpStreamer2.IsActive);
            Assert.IsFalse(tcpStreamer2.ReplyWaiting);
            Assert.IsFalse(tcpStreamer2.Connected);

            Assert.IsFalse(tcpStreamer1.IsActive);
            Assert.IsFalse(tcpStreamer1.ReplyWaiting);
            Assert.IsFalse(tcpStreamer1.Connected);
        }

        public bool wait(int timeout)
        {
            Thread.Sleep(timeout);
            return true;
        }
    }
}
