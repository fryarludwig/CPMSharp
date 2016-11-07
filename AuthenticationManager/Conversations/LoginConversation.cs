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
using System.Net;

namespace AuthenticationManager.Conversations
{
    public class LoginConversation : Conversation
    {
        public LoginConversation() : base("Login Conv")
        {
        }

        protected override void Run()
        {
            UInt32 availableRetries = MaxRetries;
            Envelope tempEnvelope;

            while (ContinueThread)
            {
                Logger.Trace("Conversation is active");

                if (!NewMessages.IsEmpty)
                {
                    Logger.Info("Received some kind of response");
                    if (NewMessages.TryDequeue(out tempEnvelope))
                    {
                        if (tempEnvelope.Message.GetType() == typeof(LoginRequest))
                        {
                            LoginRequest request = (LoginRequest)tempEnvelope.Message;
                            Logger.Info("Received Login response");
                            Logger.Info($"Sender: {request.ProcessLabel}");
                            Envelope env = PopulateEnvelope();
                            env.Address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5556);
                            Communicator.Send(env);
                            Stop();
                        }
                        else
                        {
                            Logger.Info("Received unexpected message: " + tempEnvelope.Message.ToString());
                        }
                    }
                }
                else if (availableRetries-- > 0)
                {
                    Logger.Info("Sending login request");
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
            LoginReply reply = new LoginReply();
            reply.ConvId = Id;
            reply.MsgId = Id;
            reply.Note = "Granted!";
            reply.Success = true;
            reply.ProcessInfo = new ProcessInfo();
            reply.ProcessInfo.Status = ProcessInfo.StatusCode.Registered;
            reply.ProcessInfo.Type = ProcessInfo.ProcessType.ContractManager;
            return reply;
        }
    }
}
