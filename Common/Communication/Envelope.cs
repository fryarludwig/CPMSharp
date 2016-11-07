using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Common.Messages;

namespace Common.Communication
{
    public class Envelope
    {
        public Envelope()
        {
            Address = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0);
            Message = new Message();
            Unread = false;
        }

        public Envelope(Message tempMessage)
        {
            Address = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0);
            Message = tempMessage;
            Unread = true;
        }

        public Envelope(IPEndPoint tempAddress, Message tempMessage)
        {
            Address = tempAddress;
            Message = tempMessage;
            Unread = true;
        }

        public MessageNumber ConvId
        {
            get
            {
                return Message.ConvId;
            }
        }
        public bool Unread { get; set; }
        public IPEndPoint Address { get; set; }
        public Message Message { get; set; }
    }


}

