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


namespace AuthenticationManager.Conversations
{
    public class KillProcessConversation : RequestReplyInitiator
    {
        public KillProcessConversation(int id, IPEndPoint destination) : base ("KillProc", destination)
        {
            ProcToEnd = id;
        }

        protected override void BeginConversation()
        {
            ShutdownRequest request = new ShutdownRequest(ProcToEnd);
            SendMessage(new Envelope(Destination, request));
            WaitingForReply = false;
        }

        protected override void ProcessResponse(Envelope envelope)
        {
            Logger.Warn("Unexpected message received from " + envelope.Address.ToString() + ". Dead processes don't talk");
            WaitingForReply = false;
        }

        private int ProcToEnd { get; set; }
    }
}
