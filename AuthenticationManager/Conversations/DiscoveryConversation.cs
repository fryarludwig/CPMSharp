using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Communication;
using System.Threading;
using Common.Messages;
using Common.Messages.Requests;
using Common.Messages.Replies;

namespace AuthenticationManager.Conversations
{
    class DiscoveryConversation : Conversation
    {
        public DiscoveryConversation() : base("ConvDiscovery")
        {
        }

        protected override void Run()
        {
            Envelope tempEnvelope;

            while (ContinueThread)
            {
                if (!NewMessages.IsEmpty)
                {
                    Logger.Info("Received some kind of response");
                    if (NewMessages.TryDequeue(out tempEnvelope))
                    {
                        Logger.Info("Received unexpected message: " + tempEnvelope.Message.ToString());
                    }
                }

                Thread.Sleep(500);
            }

            Logger.Info("Conversation: Ending a conversation");
        }

        protected override Message CreateMessage()
        {
            AliveReply message = new AliveReply();
            message.ConvId = Id;
            message.MsgId = MessageNumber.Create();
            message.Success = true;
            message.Note = "Ah, ha, ha, ha, stayin' alive, stayin' alive";

            return message;
        }
    }
}
