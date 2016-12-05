using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public abstract class Threaded
    {
        public Threaded(string loggerName)
        {
            Logger = new LogUtility(loggerName);
            ActiveThread = new Thread(Run);
            ActiveThread.IsBackground = true;
            ContinueThread = false;
        }
        ~Threaded()
        {
            Stop();
        }

        public void Start()
        {
            if (!ActiveThread.IsAlive)
            {
                ContinueThread = true;
                ActiveThread = new Thread(Run);
                ActiveThread.IsBackground = true;
                ActiveThread.Start();
            }
            else
            {
                Logger.Warn("Cannot intiate thread, it is already active");
            }
        }

        public void Stop()
        {
            if (ActiveThread.IsAlive)
            {
                Logger.Info("Closing Thread");
                DerivedStop();
                ContinueThread = false;
                ActiveThread.Join(2000);
            }
            else
            {
                ContinueThread = false;
                Logger.Warn("Current thread is not running, cannot stop");
            }
        }

        protected abstract void Run();

        protected virtual void DerivedStop() { }

        public bool IsActive()
        {
            return ContinueThread;
        }

        protected Thread ActiveThread;
        protected volatile bool ContinueThread;
        protected LogUtility Logger;
    }
}
