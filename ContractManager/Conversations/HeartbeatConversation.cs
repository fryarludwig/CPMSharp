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

namespace ContractManager.Conversations
{
    public class HeartbeatConversation : RequestReplyResponder
    {
        public HeartbeatConversation() : base("Heartbeat")
        {

        }
                
        protected override void ProcessResponse(Envelope envelope)
        {
            if (envelope.Message.GetType() == typeof(AliveRequest))
            {
                AliveReply message = new AliveReply
                {
                    ConvId = envelope.ConvId,
                    MsgId = MessageNumber.Create(),
                    Success = true,
                    Note = "Ah, ha, ha, ha, stayin' alive, stayin' alive"
                };
                Envelope toSend = new Envelope(envelope.Address, message);
                WaitingForReply = false;
                SendMessage(toSend);
            }
            else
            {
                Logger.Info("Received unexpected message: " + envelope.Message.ToString());
            }
        }
    }
}
