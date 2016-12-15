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

    [TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider<BaseWindowForm, Form>))]
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

        public void PrintLogMessage(LogItem message)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new OnLogMessageReceived(PrintLogMessage), new object[] { message });
            }
            else if (LoggerOutput != null && Logger.CanPrintLevel(message.LogLevel))
            {
                int numVisible = LoggerOutput.ClientSize.Height / LoggerOutput.ItemHeight;
                LoggerOutput.Items.Add(message);
                LoggerOutput.TopIndex += (LoggerOutput.TopIndex == LoggerOutput.Items.Count - numVisible - 1) ? 1 : 0;
                //    if (LoggerOutput.TopIndex == LoggerOutput.Items.Count - numVisible - 1)
                //{
                //    LoggerOutput.TopIndex = LoggerOutput.Items.Count - numVisible + 1;
                //}
            }
        }

        protected void LoggerOutput_DrawItem(object sender, DrawItemEventArgs e)
        {
            LogItem item = (LogItem)LoggerOutput.Items[e.Index];
            e.DrawBackground();
            Graphics g = e.Graphics;
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
                if (_LoggerOutput != null)
                {
                    _LoggerOutput.DrawItem -= LoggerOutput_DrawItem;
                }

                _LoggerOutput = value;
                LoggerOutput.DrawMode = DrawMode.OwnerDrawVariable;
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
