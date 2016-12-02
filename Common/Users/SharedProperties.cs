using Common.Utilities;
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
            // Do nothing
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
        private static object DistProcessLock = new object();
        private static volatile SharedProperties instance;
        private static volatile DistributedProcess process;

        public DistributedProcess DistInstance
        {
            get
            {
                lock (DistProcessLock)
                {
                    return process;
                }
            }
            set
            {
                lock (DistProcessLock)
                {
                    process = value;
                }
            }
        }
    }
}
