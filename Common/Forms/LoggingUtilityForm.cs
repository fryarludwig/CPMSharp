using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Common.Utilities;

namespace Common.Forms
{
    public partial class LoggingUtilityForm : Form
    {
        public LoggingUtilityForm(string mainProcessName)
        {
            InitializeComponent();
            LevelMap = new ConcurrentDictionary<Level, bool>();
            LevelMap[Level.ERROR] = true;
            LevelMap[Level.WARN] = true;
            LevelMap[Level.INFO] = true;
            LevelMap[Level.TRACE] = true;
            Logger = new LogUtility(mainProcessName)
            {
                ConsoleOutput = true,
                GuiOutput = true,
                FileOutput = true
            };
            LoggerOutput = GuiLogOutput;
            Logger.RegisterFloatingGuiCallback(this);
            ParentProcessLabel.Text = mainProcessName;
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
            if (e != null && e.Index >= 0)
            {
                LogItem item = (LogItem)LoggerOutput.Items[e.Index];
                e.DrawBackground();
                Graphics g = e.Graphics;
                g.FillRectangle(new SolidBrush(LogLevelMapper.ColorFromLevel(item.LogLevel)), e.Bounds);
                g.DrawString(item.LogMessage, e.Font, new SolidBrush(e.ForeColor), new PointF(e.Bounds.X, e.Bounds.Y));
                e.DrawFocusRectangle();
            }
        }
       
        public delegate void OnLogMessageReceived(LogItem message);

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
                _LoggerOutput.DrawMode = DrawMode.OwnerDrawVariable;
                _LoggerOutput.DrawItem += LoggerOutput_DrawItem;
            }
        }

        protected LogUtility Logger { get; set; }
        protected DistributedProcess ProcessInstance { get; set; }
        protected ErrorProvider InputErrorProvider = new ErrorProvider();
        protected ConcurrentQueue<LogItem> LogMessages;
        protected ConcurrentDictionary<Level, bool> LevelMap;

        private void ShowErrorsInput_CheckedChanged(object sender, EventArgs e)
        {
            LevelMap[Level.ERROR] = ShowErrorsInput.Checked;
        }

        private void ShowWarnings_CheckedChanged(object sender, EventArgs e)
        {
            LevelMap[Level.WARN] = ShowWarningsInput.Checked;
        }

        private void ShowInfoInput_CheckedChanged(object sender, EventArgs e)
        {
            LevelMap[Level.INFO] = ShowInfoInput.Checked;
        }

        private void ShowTraceInput_CheckedChanged(object sender, EventArgs e)
        {
            LevelMap[Level.TRACE] = ShowTraceInput.Checked;
        }
    }
}
