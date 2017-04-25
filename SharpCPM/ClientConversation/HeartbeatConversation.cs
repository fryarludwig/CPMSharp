using Common.Communication;
using Common.Messages;
using Common.Messages.Replies;
using Common.Messages.Requests;

namespace CPMClient.ClientConversation
{
    public class HeartbeatConversation : RequestReplyResponder
    {
        public HeartbeatConversation() : base("Heartbeat Conversation")
        {

        }

        protected override void ProcessResponse(Envelope envelope)
        {
            if (envelope.Message.GetType() == typeof(AliveRequest))
            {
                AliveReply message = new AliveReply();
                message.ConvId = Id;
                message.MsgId = MessageNumber.Create();
                message.Success = true;
                message.Note = "Ah, ha, ha, ha, stayin' alive, stayin' alive";
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
