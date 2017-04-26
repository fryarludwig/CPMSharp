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
    public class UdpTransport : NetworkClient
    {
        public UdpTransport() : base("UDP")
        {
            LocalEndpoint = new IPEndPoint(IPAddress.Any, 0);
        }

        public UdpTransport(int port) : base("UDP")
        {
            LocalEndpoint = new IPEndPoint(IPAddress.Any, port);
        }

        protected override void Run()
        {
            UdpClient socket = new UdpClient(LocalEndpoint);
            int port = ((IPEndPoint)socket.Client.LocalEndPoint).Port;
            socket.Client.ReceiveTimeout = 2000;
            Logger.Trace("Listening on port " + port.ToString());
            IPEndPoint recvEndpoint = null;
            State = (socket != null) ? STATE.READY : STATE.ERROR;
            while (ContinueThread)
            {
                try
                {
                    if (socket.Available > 0)
                    {
                        byte[] bytesReceived = socket.Receive(ref recvEndpoint);
                        if (bytesReceived.Length > 0)
                        {
                            IPEndPoint endpoint = new IPEndPoint(recvEndpoint.Address, recvEndpoint.Port);
                            Envelope tempEnvelope = new Envelope(endpoint, Message.Decode(bytesReceived));
                            if (tempEnvelope.Message != null)
                            {
                                Logger.Info("Received message, enqueuing.");
                                HandleReceivedMessage(tempEnvelope);
                            }
                        }
                    }
                    else if (!OutboundQueue.IsEmpty)
                    {
                        if (OutboundQueue.TryDequeue(out Envelope outboundEnvelope))
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
                    State = STATE.ERROR;
                    Logger.Error("UDP socket exception : " + exc.Message);
                }
            }

            socket.Close();
            Logger.Info("Closing down Network Manager");
        }
    }
}
