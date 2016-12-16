using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Common.Utilities;
using Common.Forms;
using System.Collections.Concurrent;
using System.Net;
using Common.Users;
using System.Timers;

namespace AuthenticationManager
{
    public partial class AuthenticatorGui : BaseWindowForm
    {
        public AuthenticatorGui() : base("AuthGui", new AuthManager())
        {
            InitializeComponent();
            LoggerOutput = GuiLogOutput;
            Authenticator.Registration_OnChange += ProcessRegistrationUpdate;
            StartButton.Text = START_TEXT;
            StatusDisplay.Text = "Not Started";
            Logger.Trace("Started Authentication Manager Interface");
        }

        protected void ProcessRegistrationUpdate(ProcessInfo processInfo)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new RegistrationChanged(ProcessRegistrationUpdate), new object[] { processInfo });
            }
            else
            {
                if (ProcessesDisplay.Items.ContainsKey(processInfo.LabelAndId))
                {
                    ProcessesDisplay.Items.RemoveByKey(processInfo.LabelAndId);
                }
                Logger.Trace($"Registration updated for process {processInfo.ToString()}");
                ListViewItem listItem = new ListViewItem(processInfo.ProcessId.ToString());
                listItem.Name = processInfo.LabelAndId;
                listItem.SubItems.Add(new ListViewItem.ListViewSubItem(listItem, (processInfo.TypeString)));
                listItem.SubItems.Add(new ListViewItem.ListViewSubItem(listItem, (processInfo.Label ?? "None")));
                listItem.SubItems.Add(new ListViewItem.ListViewSubItem(listItem, ((processInfo.EndPoint != null) ? processInfo.EndPoint.ToString() : "None")));
                listItem.SubItems.Add(new ListViewItem.ListViewSubItem(listItem, (processInfo.StatusString)));
                ProcessesDisplay.Items.Add(listItem);
            }
        }


        protected override void ProcessStatusChanged(ProcessInfo processInfo)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new OnProcessStatusChanged(ProcessStatusChanged), new object[] { processInfo });
            }
            else
            {
                StatusDisplay.Text = processInfo.StatusString;
                StartButton.Enabled = true;
                if (processInfo.Status == ProcessInfo.StatusCode.Registered)
                {
                    Logger.Trace("Server started successfull");
                    StartButton.Text = STOP_TEXT;
                }
                else
                {
                    Logger.Trace($"Server status is {processInfo.StatusString}");
                    StartButton.Text = START_TEXT;
                }
            }
        }

        protected override void PrepopulateProcessValues()
        {
            ProcessInstance.LocalEndpoint = new IPEndPoint(IPAddress.Any, int.Parse(portInput.Text));
            Authenticator.HeartbeatIntervalMs = int.Parse(intervalInput.Text);
        }
        protected void StartConnection(object sender, EventArgs e)
        {
            if (ValidateLoginInformation())
            {
                StartButton.Enabled = false;
                if (StartButton.Text == START_TEXT)
                {
                    PrepopulateProcessValues();
                    ProcessInstance.StartConnection();
                }
                else if (StartButton.Text == STOP_TEXT)
                {
                    Logger.Trace("Waiting for shutdown messages to propogate");
                    ProcessInstance.CloseConnection();
                }
            }
            else
            {
                Logger.Error("User entered invalid information");
                StartButton.Enabled = true;
            }
        }

        private bool ValidateLoginInformation()
        {
            bool validInformation = true;
            InputErrorProvider.Clear();

            IEnumerable<TextBox> textBoxes = Controls.OfType<TextBox>();
            foreach (TextBox box in textBoxes)
            {
                if (string.IsNullOrWhiteSpace(box.Text))
                {
                    InputErrorProvider.SetError(box, "Input cannot be blank");
                    validInformation = false;
                }
            }

            int throwaway = 0;
            if (!int.TryParse(portInput.Text, out throwaway))
            {
                validInformation = false;
                InputErrorProvider.SetError(portInput, "Please enter a valid port number");
            }

            if (!int.TryParse(intervalInput.Text, out throwaway))
            {
                validInformation = false;
                InputErrorProvider.SetError(intervalInput, "Please enter a valid number");
            }

            return validInformation;
        }

        private const string START_TEXT = "Start";
        private const string STOP_TEXT = "Stop";
        protected AuthManager Authenticator { get { return (AuthManager)ProcessInstance; } }
        public delegate void RegistrationChanged(ProcessInfo processInfo);
    }
}

