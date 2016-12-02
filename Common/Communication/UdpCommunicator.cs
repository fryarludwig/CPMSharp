using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Common.Messages;
using Common.Utilities;

namespace Common.Communication
{
    public class UdpCommunicator : BaseCommunicator
    {
        public UdpCommunicator() : base("UDP Com")
        {
            // Do nothing
        }
        
        protected override void Run()
        {
            UdpClient socket = new UdpClient(LocalEndpoint);
            IPEndPoint recvEndpoint = null;
            socket.Client.ReceiveTimeout = 2000;

            while (ContinueThread)
            {
                try
                {
                    if (socket.Available > 0)
                    {
                        //Logger.Info("Information is available on the socket");

                        byte[] bytesReceived = socket.Receive(ref recvEndpoint);
                        if (bytesReceived.Length > 0)
                        {
                            IPEndPoint endpoint = new IPEndPoint(recvEndpoint.Address, recvEndpoint.Port);
                            Envelope tempEnvelope = new Envelope(endpoint, Message.Decode(bytesReceived));
                            if (tempEnvelope.Message != null)
                            {
                                Logger.Info("Received message, enqueuing.");
                                InboundQueue.Enqueue(tempEnvelope);
                            }
                        }
                    }
                    else if (!OutboundQueue.IsEmpty)
                    {
                        Envelope outboundEnvelope;
                        if (OutboundQueue.TryDequeue(out outboundEnvelope))
                        {
                            byte[] bytesToSend = outboundEnvelope.Message.Encode();
                            Logger.Info($"Sending outbound message of length {bytesToSend.Length} to {outboundEnvelope.Address.ToString()}");
                            if (bytesToSend.Length > 0)
                            {
                                socket.Send(bytesToSend, bytesToSend.Length, outboundEnvelope.Address);
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
                catch (Exception exc)
                {
                    Logger.Error("UDP socket exception : " + exc.Message);
                }
            }

            socket.Close();
            Logger.Info("Closing down Network Manager");
        }
    }
}
