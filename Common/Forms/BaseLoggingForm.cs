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
    public partial class BaseLoggingForm : Form
    {
        public BaseLoggingForm()
        {
            InitializeComponent();
        }

        public BaseLoggingForm(string name, DistributedProcess process)
        {
            InitializeComponent();
            InitializeLogger(name);
            ProcessInstance = process;
            ProcessInstance.OnStatusChanged += ProcessStatusChanged;
        }

        protected void InitializeLogger(string name)
        {
            LoggerOutput = GuiLogOutput;
            LevelMap = new ConcurrentDictionary<Level, bool>
            {
                [Level.ERROR] = true,
                [Level.WARN] = true,
                [Level.INFO] = true,
                [Level.TRACE] = true,
                [Level.DEBUG] = true
            };
            Logger = new LogUtility(name)
            {
                ConsoleOutput = true,
                GuiOutput = true,
                FileOutput = true
            };

            Logger.RegisterGuiCallback(this);
            SetupLoggingCallbacks();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (ProcessInstance.MyProcessInfo.Status == ProcessInfo.StatusCode.Registered)
            {
                ProcessInstance.CloseConnection();
            }
            base.OnClosing(e);
        }

        protected void SetupLoggingCallbacks()
        {
            ShowInfoInput.CheckedChanged += new EventHandler(this.ShowInfoInput_CheckedChanged);
            ShowTraceInput.CheckedChanged += new EventHandler(this.ShowTraceInput_CheckedChanged);
            ShowWarningsInput.CheckedChanged += new EventHandler(this.ShowWarnings_CheckedChanged);
            ShowErrorsInput.CheckedChanged += new EventHandler(this.ShowErrorsInput_CheckedChanged); 
        }

        public void PrintLogMessage(LogItem message)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new OnLogMessageReceived(PrintLogMessage), new object[] { message });
            }
            else if (LoggerOutput != null && Logger.CanPrintLevel(message.LogLevel) && LevelMap[message.LogLevel])
            {
                int numVisible = LoggerOutput.ClientSize.Height / LoggerOutput.ItemHeight;
                LoggerOutput.Items.Add(message);
                LoggerOutput.TopIndex += (LoggerOutput.TopIndex == LoggerOutput.Items.Count - numVisible - 1) ? 1 : 0;
            }
        }

        protected void LoggerOutput_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e?.Index >= 0)
            {
                LogItem item = (LogItem)LoggerOutput.Items[e.Index];
                e.DrawBackground();
                Graphics g = e.Graphics;
                g.FillRectangle(new SolidBrush(LogLevelMapper.ColorFromLevel(item.LogLevel)), e.Bounds);
                g.DrawString(item.LogMessage, e.Font, new SolidBrush(e.ForeColor), new PointF(e.Bounds.X, e.Bounds.Y));
                e.DrawFocusRectangle();
            }
        }

        protected bool ValidateInteger(string value)
        {
            return int.TryParse(value, out int throwaway);
        }

        protected virtual void PrepopulateProcessValues()
        {
            // Do nothing
        }

        protected virtual void ProcessStatusChanged(ProcessInfo processInfo)
        {
            throw new Exception();
        }

        private ListBox _LoggerOutput;
        protected ListBox LoggerOutput
        {
            get => _LoggerOutput;
            set
            {
                if (_LoggerOutput != null)
                {
                    _LoggerOutput.DrawItem -= LoggerOutput_DrawItem;
                }

                _LoggerOutput = value;
                _LoggerOutput.DrawMode = DrawMode.OwnerDrawVariable;
                _LoggerOutput.DrawItem += LoggerOutput_DrawItem;
            }
        }

        public delegate void OnLogMessageReceived(LogItem message);
        public delegate void OnProcessStatusChanged(ProcessInfo processInfo);

        protected LogUtility Logger { get; set; }
        protected DistributedProcess ProcessInstance { get; set; }
        protected ErrorProvider InputErrorProvider = new ErrorProvider();
        protected ConcurrentDictionary<Level, bool> LevelMap;
        
        protected void ShowErrorsInput_CheckedChanged(object sender, EventArgs e)
        {
            LevelMap[Level.ERROR] = ShowErrorsInput.Checked;
        }

        protected void ShowWarnings_CheckedChanged(object sender, EventArgs e)
        {
            LevelMap[Level.WARN] = ShowWarningsInput.Checked;
        }

        protected void ShowInfoInput_CheckedChanged(object sender, EventArgs e)
        {
            LevelMap[Level.INFO] = ShowInfoInput.Checked;
        }

        protected void ShowTraceInput_CheckedChanged(object sender, EventArgs e)
        {
            LevelMap[Level.TRACE] = ShowTraceInput.Checked;
        }
        protected void ShowDebugInput_CheckedChanged(object sender, EventArgs e)
        {
            LevelMap[Level.DEBUG] = ShowTraceInput.Checked;
        }
    }
}
