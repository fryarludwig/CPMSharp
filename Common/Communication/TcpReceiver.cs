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
    public class TcpReceiver : NetworkClient
    {
        public TcpReceiver() : base("TCP Com")
        {
            LocalEndpoint = new IPEndPoint(IPAddress.Any, 0);
        }

        public TcpReceiver(int port) : base("TCP Com")
        {
            LocalEndpoint = new IPEndPoint(IPAddress.Any, port);
        }

        private readonly byte[] _buffer = new byte[256];

        protected override void Run()
        {
            TcpListener listener = new TcpListener(LocalEndpoint);
            listener.Start();

            while (ContinueThread)
            {
                TcpClient client = listener.AcceptTcpClient();
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
                {
                    stayConnected = GetStreamDAta(stream);
                }
            }
        }

        private bool GetStreamDAta(NetworkStream stream)
        {
            bool stayConnected = true;
            try
            {
                int bytesRead = stream.Read(_buffer, 0, _buffer.Length);
                if (bytesRead > 0)
                {
                    string data = System.Text.Encoding.ASCII.GetString(_buffer, 0, bytesRead);
                    Logger.Trace("Received " + bytesRead + " bytes: " + data);

                    if (data == "$" || data == "*") { Stop(); }

                    if (data == "$") { Stop(); }
                }
            }
            catch (Exception err)
            {
                if (err is IOException && err.InnerException is SocketException &&
                    (err.InnerException as SocketException).SocketErrorCode == SocketError.ConnectionReset)
                {
                    Logger.Warn("Sender closed the connection");
                }
                else
                {
                    Logger.Error(err.ToString());
                }
                
                Stop();
            }

            return IsActive;
        }
    }
}
