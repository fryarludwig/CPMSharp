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
    public class TestRequestReplyConversation
    {
        [TestMethod]
        public void ReqRepBasic()
        {
            //ConversationFactory.PrimaryCommunicator.SetLocalEndpoint()

            TestRequestReplyConv initiator = new TestRequestReplyConv();


            //testCommunicator1.LocalPort = 12345;

            //if (!testCommunicator1.IsActive())
            //{
            //    testCommunicator1.Start();
            //}

            //Assert.IsFalse(testCommunicator1.ReplyWaiting);

            //TestSocket testSocket = new TestSocket(54321);
            //testSocket.Start();

            //int testRetries = 50;

            //while (!testSocket.IsActive() && !testCommunicator1.IsActive() && testRetries-- > 0)
            //{
            //    Thread.Sleep(100);
            //}

            //Assert.IsTrue(testSocket.IsActive());
            //Assert.IsTrue(testCommunicator1.IsActive());

            //LoginRequest loginMessage = new LoginRequest();
            //Assert.AreEqual("", loginMessage.FirstName);
            //Assert.AreEqual("", loginMessage.ANumber);
            //Assert.AreEqual("", loginMessage.LastName);
            //Assert.AreEqual(1, LoginRequest.MsgType);

            //loginMessage.ANumber = "A01284981";
            //loginMessage.LastName = "FryarLudwig";
            //loginMessage.FirstName = "Kenny";

            //IPEndPoint toComm1 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            //IPEndPoint toComm2 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 54321);

            //testCommunicator1.Send(new Envelope(toComm2, loginMessage));

            //testRetries = 50;
            //while (!testSocket.ReplyWaiting && testRetries-- > 0)
            //{
            //    Thread.Sleep(100);
            //}

            //Assert.IsTrue(testSocket.IsActive());
            //Assert.IsTrue(testCommunicator1.IsActive());

            //Assert.IsTrue(testSocket.ReplyWaiting);
            //Envelope recvdEnv = null;
            //Assert.IsTrue(testSocket.Receive(out recvdEnv));
            //Message recvdMessage = recvdEnv.Message;
            //Assert.IsTrue(typeof(LoginRequest) == recvdMessage.GetType());

            //Assert.AreEqual(loginMessage.ANumber, ((LoginRequest)recvdMessage).ANumber);
            //Assert.AreEqual(loginMessage.FirstName, ((LoginRequest)recvdMessage).FirstName);
            //Assert.AreEqual(loginMessage.LastName, ((LoginRequest)recvdMessage).LastName);

            //loginMessage.ANumber = "ANumber";
            //loginMessage.LastName = "LastName";
            //loginMessage.FirstName = "FirstNAme";

            //testSocket.Send(new Envelope(toComm1, loginMessage));

            //testRetries = 50;
            //while (!testCommunicator1.ReplyWaiting && testRetries-- > 0)
            //{
            //    Thread.Sleep(100);
            //}

            //Assert.IsTrue(testCommunicator1.ReplyWaiting);
            //recvdEnv = null;
            //Assert.IsTrue(testCommunicator1.Receive(out recvdEnv));
            //recvdMessage = recvdEnv.Message;
            //Assert.IsTrue(typeof(LoginRequest) == recvdMessage.GetType());

            //Assert.AreEqual(loginMessage.ANumber, ((LoginRequest)recvdMessage).ANumber);
            //Assert.AreEqual(loginMessage.FirstName, ((LoginRequest)recvdMessage).FirstName);
            //Assert.AreEqual(loginMessage.LastName, ((LoginRequest)recvdMessage).LastName);

            //testSocket.Stop();

            //testRetries = 50;
            //while (testCommunicator1.IsActive() && testSocket.IsActive() && testRetries-- > 0)
            //{
            //    Thread.Sleep(100);
            //}

            //Assert.IsFalse(testCommunicator1.IsActive());
            //Assert.IsFalse(testSocket.IsActive());
        }
    }
}
