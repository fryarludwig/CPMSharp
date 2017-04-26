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
        public TcpStreamer() : base("TCP Com")
        {
            LocalEndpoint = new IPEndPoint(IPAddress.Any, 0);
        }

        public TcpStreamer(int port) : base("TCP Com")
        {
            LocalEndpoint = new IPEndPoint(IPAddress.Any, port);
        }

        public string MessageToSend { get; set; }

        public int RepeatCount { get; set; }


        private bool _keepGoing;
        private readonly byte[] _buffer = new byte[256];

        public void RunReceive()
        {
            IPEndPoint myEp = new IPEndPoint(IPAddress.Any, 15000);
            TcpListener server = new TcpListener(myEp);
            server.Start();

            _keepGoing = true;
            while (_keepGoing)
            {
                TcpClient client = server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();

                ThreadPool.QueueUserWorkItem(ReceiveDataFromClient, stream);
            }
        }

        private void ReceiveDataFromClient(object myStream)
        {
            NetworkStream stream = myStream as NetworkStream;

            if (stream != null)
            {
                bool stayConnected = true;
                while (stayConnected)
                    stayConnected = ReadSomeData(stream);
            }
        }

        private bool ReadSomeData(NetworkStream stream)
        {
            bool stayConnected = true;
            try
            {
                int bytesRead = stream.Read(_buffer, 0, _buffer.Length);
                if (bytesRead > 0)
                {
                    string data = System.Text.Encoding.ASCII.GetString(_buffer, 0, bytesRead);
                    Console.WriteLine("Received {0} bytes: {1}", bytesRead, data);

                    if (data == "$" || data == "*")
                        stayConnected = false;

                    if (data == "$")
                        _keepGoing = false;
                }
            }
            catch (Exception err)
            {
                if (err is IOException && err.InnerException is SocketException &&
                    (err.InnerException as SocketException).SocketErrorCode == SocketError.ConnectionReset)
                    Console.WriteLine("Sender closed the connection");
                else
                    Console.WriteLine(err);
                stayConnected = false;
            }

            return stayConnected;
        }

        protected override void Run()
        {

            // Create a TcpClient
            TcpClient client = new TcpClient();

            // Connect the client to the server -- remember that TCP is connection orient
            client.Connect(LocalEndpoint);

            // Note that previous two statement can be combined using one of the
            // TcpClient constructs:
            //
            //  TcpClient client = new TcpClient("127.0.0.1", 15000);
            //
            // IMPORTANT: This constructor is NOT the same as one the takes an IPEndPoint
            // as a parameter.  That constructor binds the TcpClient to a local end point;
            // it does not connect the client to a remote end point.

            NetworkStream stream = client.GetStream();
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(MessageToSend);
            for (int i = 0; i < RepeatCount; i++)
                stream.Write(bytes, 0, bytes.Length);

            // Note that if we don't pause and thereby keep the channel open, it will
            // close the connection before the receiver has a chance to receive the data
            Console.WriteLine("Hit ENTER to exit");
            Console.ReadLine();

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
