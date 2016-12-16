//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using Common.Communication;
//using Common.Messages.Replies;
//using Common.Messages.Requests;
//using Common.Messages;
//using Common.Users;
//using Common.Utilities;
//using System.Threading;
//using System.Net;

//namespace SharpCPM.ClientConversation
//{
//    public class ShutdownConversation : RequestReplyInitiator
//    {
//        public ShutdownConversation() : base("ShutdownConv")
//        {
//            WaitingForReply = true;
//            AllowRepeats = false;
//            CallbacksRegistered = false;
//        }

//        public override void RegisterConversationCallbacks(DistributedProcess process)
//        {
//            if (!CallbacksRegistered && process.GetType() == typeof(ContractManager))
//            {
//                ContractManager manager = (ContractManager)process;
//                OnLoginUpdated += manager.HandleLoginUpdated;
//                CallbacksRegistered = true;
//                Logger.Info("Callbacks are now registered");
//            }
//            else if (CallbacksRegistered)
//            {
//                Logger.Info("Callbacks have already been registered");
//            }
//            else
//            {
//                Logger.Warn("Unable to assign unknown dist process to event");
//            }
//        }

//        public ShutdownConversation(IPEndPoint target) : base("ShutdownConv", target)
//        {
//            WaitingForReply = true;
//            AllowRepeats = false;
//            CallbacksRegistered = false;
//        }

//        protected override void BeginConversation()
//        {
//            ShutdownRequest request = new ShutdownRequest();


//        }

//        protected override void ProcessResponse(Envelope envelope)
//        {
//            throw new NotImplementedException();
//        }

//    }
//}
