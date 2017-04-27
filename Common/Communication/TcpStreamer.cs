using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Common.Messages;
using Common.Utilities;

namespace Common.Communication
{
    public class TcpStreamer : NetworkClient
    {
        public TcpStreamer(string name = "") : base("TCP " + name)
        {
            LocalEndpoint = new IPEndPoint(IPAddress.Any, 0);
            TargetEndpoint = new IPEndPoint(IPAddress.Any, 0);
            RunAsServer = true;
        }

        //public TcpStreamer(IPEndPoint localEndPoint, IPEndPoint targetEndpoint) : base("TCP Out")
        //{
        //    LocalEndpoint = localEndPoint ?? new IPEndPoint(IPAddress.Any, 0);
        //    TargetEndpoint = targetEndpoint ?? new IPEndPoint(IPAddress.Any, 0);
        //}

        public bool StartAsServer(IPEndPoint localEndpoint = null)
        {
            if (IsActive)
            {
                Logger.Warn("Network port is currently running - stop the TcpTransport before proceeding");
                return false;
            }
            else
            {
                LocalEndpoint = localEndpoint ?? new IPEndPoint(IPAddress.Any, 0);
                RunAsServer = true;
                Start();
                return true;
            }
        }

        public bool StartAsClient(IPEndPoint remoteEndpoint = null)
        {
            if (IsActive)
            {
                Logger.Warn("Network port is currently running - stop the TcpTransport before proceeding");
                return false;
            }
            else
            {
                TargetEndpoint = remoteEndpoint ?? new IPEndPoint(IPAddress.Any, 0);
                RunAsServer = false;
                Start();
                return true;
            }
        }
        
        protected override void DerivedStop()
        {
            socket?.Dispose();
        }

        protected override void Run()
        {
            try
            {
                if (RunAsServer)
                {
                    Logger.Trace("Starting TCP connection as server");
                    listener = new TcpListener(LocalEndpoint);
                    listener.Start();
                    State = STATE.Listening;
                    socket = listener.AcceptTcpClient();
                }
                else
                {
                    Logger.Trace("Starting TCP connection as client");
                    State = STATE.Listening;
                    socket = new TcpClient();
                    socket.Connect(TargetEndpoint);
                }

                NetworkStream stream = socket.GetStream();
                State = STATE.Ready;
                Logger.Trace("Connection made - beginning to stream");

                while (ContinueThread && socket.Connected)
                {
                    if (!OutboundQueue.IsEmpty && OutboundQueue.TryDequeue(out Envelope outboundEnvelope))
                    {
                        byte[] bytesToSend = outboundEnvelope.Message.Encode();
                        Logger.Info($"Sending outbound message of length {bytesToSend.Length} to {outboundEnvelope.Address.ToString()}");
                        if (bytesToSend.Length > 0)
                        {
                            stream.Write(bytesToSend, 0, bytesToSend.Length);
                            Thread.Sleep(50);
                        }
                    }
                    else if (stream.DataAvailable)
                    {
                        State = STATE.Busy;
                        int bytesRead = stream.Read(_buffer, 0, _buffer.Length);
                        Logger.Trace("Received " + bytesRead + " bytes from " + TargetEndpoint);
                        if (bytesRead > 0)
                        {
                            byte[] toDecode = new byte[bytesRead];
                            Array.Copy(_buffer, toDecode, bytesRead);
                            Envelope tempEnvelope = new Envelope(TargetEndpoint, Message.Decode(toDecode));
                            if (tempEnvelope.Message != null)
                            {
                                Logger.Info("Received message, enqueuing.");
                                HandleReceivedMessage(tempEnvelope);
                            }
                        }

                        State = STATE.Ready;
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
            }
            catch (SocketException e)
            {
                State = STATE.Stopped;
                Logger.Info("TCP streaming was stopped: " + e.ToString());
            }
            catch (Exception e)
            {
                State = STATE.Error;
                Logger.Error("TCP error occurred in run loop: " + e.ToString());
            }

            State = STATE.Stopped;
            Logger.Trace("TCP Connection shutting down");
            socket?.Dispose();
        }

        private bool RunAsServer { get; set; }
        private TcpClient socket { get; set; }
        private TcpListener listener { get; set; }

        private readonly byte[] _buffer = new byte[256];
        public IPEndPoint TargetEndpoint { get; set; }
        public bool Connected => State == STATE.Busy || State == STATE.Ready;
    }
}
