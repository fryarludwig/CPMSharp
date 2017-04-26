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
        public void TcpListenerConstructor()
        {
            int testerPort1 = 15549;
            string testerAddress = "127.0.0.1";
            TcpReceiver tcpTester1 = new TcpReceiver(testerPort1);

            

        }
    }
}
