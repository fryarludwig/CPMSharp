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
            //GameInfoList = new List<GameInfo>();
            IdentityInfo = new Identity();
            MyProcess = new ProcessInfo();
            MyProcess.ProcessId = 0;
            MyProcess.Status = ProcessInfo.StatusCode.Unknown;
            MyProcess.AliveReties = 5;
            MyProcess.AliveTimestamp = DateTime.Now;
            MyProcess.EndPoint = new IPEndPoint(IPAddress.Any, 0);
            MyProcess.Type = ProcessInfo.ProcessType.Player;
            MyProcess.Label = "Some Label, yeah?";
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
        private static object IdentityLock = new object();
        private static object RegistryEndpointLock = new object();
        private static object GameInfoListLock = new object();
        private static volatile SharedProperties instance;

        private IPEndPoint MyRegistryEndpoint;
        private Identity MyIdentity;
        private ProcessInfo MyProcess;
        //private List<GameInfo> MyList;

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
                    MyProcess.AliveReties = value.AliveReties;
                    MyProcess.AliveTimestamp = value.AliveTimestamp;
                    MyProcess.EndPoint = value.EndPoint;
                }
            }
        }
        public Identity IdentityInfo
        {
            get
            {
                lock (IdentityLock)
                {
                    return MyIdentity;
                }
            }
            set
            {
                lock (IdentityLock)
                {
                    MyIdentity = value;
                }
            }
        }
        public IPEndPoint RegistryEndpoint
        {
            get
            {
                lock (RegistryEndpointLock)
                {
                    return MyRegistryEndpoint;
                }
            }
            set
            {
                lock (RegistryEndpointLock)
                {
                    MyRegistryEndpoint = value;
                }
            }
        }
        //public List<GameInfo> GameInfoList
        //{
        //    get
        //    {
        //        lock (GameInfoListLock)
        //        {
        //            return MyList;
        //        }
        //    }
        //    set
        //    {
        //        lock (GameInfoListLock)
        //        {
        //            MyList = value;
        //        }
        //    }
        //}
    }
}
