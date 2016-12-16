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
        public Threaded(string name)
        {
            ContinueThread = false;
            Logger = (name != "LogHelper") ? new LogUtility(name) : null;
            ActiveThread = new Thread(Run);
            ActiveThread.IsBackground = true;
            try { ActiveThread.Name = name; }
            catch (InvalidOperationException e) { Logger?.Warn($"Unable to name thread due to exception {e.ToString()}"); }
        }

        ~Threaded()
        {
            Stop();
        }

        public void Start()
        {
            if (!ActiveThread.IsAlive)
            {
                Setup();
                ContinueThread = true;
                ActiveThread = new Thread(Run);
                ActiveThread.IsBackground = true;
                ActiveThread.Start();
            }
            else
            {
                Logger?.Warn("Cannot intiate thread, it is already active");
            }
        }

        public void Stop()
        {
            if (ActiveThread.IsAlive)
            {
                Logger?.Info("Closing Thread");
                DerivedStop();
                ContinueThread = false;
                ActiveThread.Join(2000);
                Cleanup();
            }
            else
            {
                ContinueThread = false;
                Logger?.Warn("Current thread is not running, cannot stop");
            }
        }

        protected virtual void Setup() { }
        protected virtual void Cleanup() { }

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
