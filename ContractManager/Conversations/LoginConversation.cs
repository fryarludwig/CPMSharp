using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Communication;
using Common.Messages.Replies;
using Common.Messages.Requests;
using Common.Messages;
using Common.Users;
using Common.Utilities;
using System.Threading;

namespace ContractManager.Conversations
{
    public class LoginConversation : Conversation
    {
        public LoginConversation() : base("Login Conv")
        {
            MsgNum = MessageNumber.Create();
            ConvId = MessageNumber.Create();
        }

        protected override void Run()
        {
            UInt32 availableRetries = MaxRetries;
            Envelope tempEnvelope;

            while (ContinueThread && Process.Status != ProcessInfo.StatusCode.Registered)
            {
                if (!NewMessages.IsEmpty)
                {
                    Logger.Info("Received some kind of response");
                    if (NewMessages.TryDequeue(out tempEnvelope))
                    {
                        if (tempEnvelope.Message.GetType() == typeof(LoginReply))
                        {
                            LoginReply replyMessage = (LoginReply)tempEnvelope.Message;
                            Logger.Info("Received Login response: " + replyMessage.Note + Properties.Process.StatusString);
                            
                            Properties.Process = replyMessage.ProcessInfo;
                        }
                        else
                        {
                            Logger.Info("Received unexpected message: " + tempEnvelope.Message.ToString());
                        }
                    }
                }
                else if (availableRetries-- > 0)
                {
                    Logger.Info($"Sending login request to {Properties.AuthenticatorEndpoint.ToString()}");
                    Envelope toSend = new Envelope(Properties.AuthenticatorEndpoint, CreateMessage());
                    Communicator.Send(PopulateEnvelope());
                    Thread.Sleep(Timeout);
                }
                else
                {
                    Logger.Warn("Failed to log in");
                    Properties.Process.Status = ProcessInfo.StatusCode.NotInitialized;
                    Stop();
                }
            }

            Logger.Info("Ending conversation");
        }


        protected override Message CreateMessage()
        {
            LoginRequest request = new LoginRequest();
            request.ConvId = ConvId;
            request.MsgId = MsgNum;
            request.IdentityInfo = IdentityInfo;
            request.ProcessLabel = Process.Label;
            request.ProcessType = Process.Type;
            request.PublicKey = new PublicKey();

            return request;
        }

        protected MessageNumber MsgNum { get; set; }
        protected MessageNumber ConvId { get; set; }
    }
}
