using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Users
{
    public sealed class SharedProperties
    {
        private SharedProperties()
        {
            MyProcess = new ProcessInfo();
            MyProcess.ProcessId = 0;
            MyProcess.Status = ProcessInfo.StatusCode.Unknown;
            MyProcess.AliveRetries = 5;
            MyProcess.AliveTimestamp = DateTime.Now;
            MyProcess.EndPoint = new IPEndPoint(IPAddress.Any, 0);
            MyProcess.Label = "DEFAULT_LABEL";
        }

        public static SharedProperties Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (InstanceLock)
                    {
                        if (instance == null)
                            instance = new SharedProperties();
                    }
                }

                return instance;
            }
        }

        private static object InstanceLock = new object();
        private static object ProcessLock = new object();
        private static object AuthenticatorEndpointLock = new object();
        private static object LocalEndpointLock = new object();
        private static volatile SharedProperties instance;

        private IPEndPoint MyAuthenticatorEndpoint;
        private IPEndPoint MyLocalEndpoint;
        private ProcessInfo MyProcess;

        public ProcessInfo Process
        {
            get
            {
                lock (ProcessLock)
                {
                    return MyProcess;
                }
            }
            set
            {
                lock (ProcessLock)
                {
                    MyProcess.Label = value.Label;
                    MyProcess.ProcessId = value.ProcessId;
                    MyProcess.Status = value.Status;
                    MyProcess.Type = value.Type;
                    MyProcess.AliveRetries = value.AliveRetries;
                    MyProcess.AliveTimestamp = value.AliveTimestamp;
                    MyProcess.EndPoint = value.EndPoint;
                }
            }
        }

        public IPEndPoint AuthenticatorEndpoint
        {
            get
            {
                lock (AuthenticatorEndpointLock)
                {
                    return MyAuthenticatorEndpoint;
                }
            }
            set
            {
                lock (AuthenticatorEndpointLock)
                {
                    MyAuthenticatorEndpoint = value;
                }
            }
        }

        public IPEndPoint LocalEndpoint
        {
            get
            {
                lock (LocalEndpointLock)
                {
                    return MyLocalEndpoint;
                }
            }
            set
            {
                lock (LocalEndpointLock)
                {
                    MyProcess.EndPoint = value;
                    MyLocalEndpoint = value;
                }
            }
        }
    }
}
