using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messages
{
    public class Envelope
    {
        public Envelope()
        {
            Address = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0);
            Message = new Message();
        }

        public Envelope(Message tempMessage)
        {
            Address = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0);
            Message = tempMessage;
        }

        public Envelope(IPEndPoint tempAddress, Message tempMessage)
        {
            Address = tempAddress;
            Message = tempMessage;
        }

        public MessageNumber ConvId => Message?.ConvId;
        public IPEndPoint Address { get; set; }
        public Message Message { get; set; }
    }
}

