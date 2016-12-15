using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


using Common.Utilities;
using Common.Users;

namespace Common.Forms
{
    public class AbstractControlDescriptionProvider<TAbstract, TBase> : TypeDescriptionProvider
    {
        public AbstractControlDescriptionProvider()
            : base(TypeDescriptor.GetProvider(typeof(TAbstract)))
        {
        }

        public override Type GetReflectionType(Type objectType, object instance)
        {
            if (objectType == typeof(TAbstract))
                return typeof(TBase);

            return base.GetReflectionType(objectType, instance);
        }

        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            if (objectType == typeof(TAbstract))
                objectType = typeof(TBase);

            return base.CreateInstance(provider, objectType, argTypes, args);
        }
    }

    public abstract partial class BaseWindowForm : Form
    {
        public BaseWindowForm(string name, DistributedProcess process)
        {
            InitializeComponent();
            Logger = new LogUtility(name);
            Logger.RegisterGuiCallback(this);
            Logger.ConsoleOutput = true;
            Logger.GuiOutput = true;
            Logger.FileOutput = true;
            ProcessInstance = process;
            ProcessInstance.OnStatusChanged += ProcessStatusChanged;
        }

        protected Task<bool> StartConnection()
        {
            return Task.Factory.StartNew<bool>(ProcessInstance.InitializeConnection);
        }

        protected Task<bool> CloseConnection()
        {
            return Task.Factory.StartNew<bool>(ProcessInstance.ShutdownProcess);
        }

        public void PrintLogMessage(LogItem message)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new OnLogMessageReceived(PrintLogMessage), new object[] { message });
            }
            else if (LoggerOutput != null)
            {
                int NumberOfItems = LoggerOutput.ClientSize.Height / LoggerOutput.ItemHeight;
                if (Logger.CanPrintLevel(message.LogLevel))
                {
                    LoggerOutput.Items.Add(message);
                    if (LoggerOutput.TopIndex == LoggerOutput.Items.Count - NumberOfItems - 1)
                    {
                        LoggerOutput.TopIndex = LoggerOutput.Items.Count - NumberOfItems + 1;
                    }
                }
            }
        }

        //public void Logging_OnPrint(object sender, LogPrintEventArgs e)
        //{
        //    e.DrawEvent.DrawBackground();
        //    Graphics g = e.DrawEvent.Graphics;
        //    g.FillRectangle(new SolidBrush(LogLevelMapper.ColorFromLevel(e.LogMessage.LogLevel)), e.DrawEvent.Bounds);
        //    g.DrawString(e.LogMessage.LogMessage, e.DrawEvent.Font, new SolidBrush(e.DrawEvent.ForeColor), new PointF(e.DrawEvent.Bounds.X, e.DrawEvent.Bounds.Y));
        //    e.DrawEvent.DrawFocusRectangle();
        //}

        protected void LoggerOutput_DrawItem(object sender, DrawItemEventArgs e)
        {
            LogItem item = (LogItem)LoggerOutput.Items[e.Index];
            e.DrawBackground();
            Graphics g = e.Graphics;
            //g.FillRectangle(new SolidBrush(Color.Pink), e.Bounds);
            g.FillRectangle(new SolidBrush(LogLevelMapper.ColorFromLevel(item.LogLevel)), e.Bounds);
            g.DrawString(item.LogMessage, e.Font, new SolidBrush(e.ForeColor), new PointF(e.Bounds.X, e.Bounds.Y));
            e.DrawFocusRectangle();
        }


        protected virtual void PrepopulateProcessValues()
        {
            // Do nothing
        }

        protected abstract void ProcessStatusChanged(ProcessInfo processInfo);

        public delegate void OnLogMessageReceived(LogItem message);
        public delegate void OnProcessStatusChanged(ProcessInfo processInfo);

        public event EventHandler<LogPrintEventArgs> Logger_OnPrint;


        private ListBox _LoggerOutput;
        protected ListBox LoggerOutput
        {
            get
            {
                return _LoggerOutput;
            }
            set
            {
                _LoggerOutput = value;
                _LoggerOutput.DrawItem += LoggerOutput_DrawItem;
            }
        }

        protected LogUtility Logger { get; set; }
        protected DistributedProcess ProcessInstance { get; set; }
        protected ErrorProvider InputErrorProvider = new ErrorProvider();
    }

    public class LogPrintEventArgs : EventArgs
    {
        public LogItem LogMessage { get; set; }
        public DrawItemEventArgs DrawEvent { get; set; }
        public LogPrintEventArgs(LogItem logItem, DrawItemEventArgs e = null)
        {
            LogMessage = logItem;
            DrawEvent = e;
        }
    }
}
