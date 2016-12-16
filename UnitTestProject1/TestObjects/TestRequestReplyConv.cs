using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Communication;
using Common.Messages;
using Common.Messages.Replies;
using Common.Messages.Requests;
using Common.Utilities;
using System.Threading;
using System.Net;

namespace TestCommon.TestObjects
{
    public class TestRequestReplyConv : RequestReplyInitiator
    {
        public TestRequestReplyConv() : base("TestReqReplyInit")
        {

        }

        public void BeginHelper(IPEndPoint remoteEndpoint, Message message)
        {
            MessageToSend = new Envelope(remoteEndpoint, message);
        }

        protected override void BeginConversation()
        {
            SendMessage(MessageToSend);
            WaitingForReply = true;
        }

        protected override void ProcessResponse(Envelope envelope)
        {
            ReceivedMessage = envelope;
        }

        public Envelope MessageToSend { get; set; }
        public Envelope ReceivedMessage { get; set; }
    }
}
