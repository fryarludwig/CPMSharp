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
    public class LoginConversation : RequestReplyResponder
    {
        public LoginConversation() : base("Auth - Login Conv")
        {
            //DelegateList.Add(Updated);
        }

        protected override void ProcessMessage(Envelope envelope)
        {
            if (envelope.Message != null && envelope.Message.GetType() == typeof(LoginRequest))
            {
                LoginReply reply = new LoginReply();
                reply.ConvId = Id;
                reply.MsgId = MessageNumber.Create();
                reply.Note = "Granted!";
                reply.Success = true;
                reply.ProcessInfo = new ProcessInfo();
                reply.ProcessInfo.Status = ProcessInfo.StatusCode.Registered;
                reply.ProcessInfo.Type = ProcessInfo.ProcessType.ContractManager;

                Envelope env = new Envelope(envelope.Address, reply);
                SendMessage(env);
            }
            else
            {
                Logger.Info("Received unexpected message: " + envelope.Message);
            }
        }

        public delegate void LoginResponseEvent(string something);
        public event LoginResponseEvent Updated;

        //public delegate ProcessInfo LoginResponseEvent(object sender, EventArgs e);
        //public event LoginResponseEvent Updated;

        // A class that works just like ArrayList, but sends event
        // notifications whenever the list changes.
        // An event that clients can use to be notified whenever the
        // elements of the list change.

        // Invoke the Changed event; called whenever list changes
        public virtual void OnUpdate()
        {
            Console.WriteLine("InsideOnUpdate");
            Updated?.Invoke("party1");
        }
    }
}
