using Common.Utilities;
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

using Common.Forms;
using Common.Users;

namespace ContractManager
{
    public partial class ContractWindow : BaseWindowForm
    {
        public ContractWindow() : base("ContractGui", new ContractManager())
        {
            InitializeComponent();
            LoggerOutput = GuiLogOutput;
            ProcessInstance.OnStatusChanged += ProcessStatusChanged;
            Contractor.Registration_OnChange += ProcessStatusChanged;
            StartButton.Text = START_TEXT;
        }

        public void KillChildren()
        {
            //ContractService.Stop();
        }

        protected override void ProcessStatusChanged(ProcessInfo processInfo)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new StatusChanged_Update(ProcessStatusChanged), new object[] { processInfo });
            }
            else
            {
                StartButton.Enabled = true;
                if (processInfo.Status == ProcessInfo.StatusCode.Registered)
                {
                    Logger.Trace("Contractor started successfull");
                    StartButton.Text = STOP_TEXT;
                }
                else
                {
                    Logger.Trace($"Contractor status is {processInfo.StatusString}");
                    StartButton.Text = START_TEXT;
                }
            }
        }

        protected override void PrepopulateProcessValues()
        {
            ProcessInstance.LocalEndpoint = new IPEndPoint(IPAddress.Any, 12099);
            ProcessInstance.AuthenticatorEndpoint = new IPEndPoint(IPAddress.Parse(authenticatorAddressInput.Text), int.Parse(authenticatorPortInput.Text));
        }

        private void Connect_Clicked(object sender, EventArgs e)
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
            
            if (!ValidateInteger(authenticatorPortInput.Text))
            {
                validInformation = false;
                InputErrorProvider.SetError(authenticatorPortInput, "Please enter a valid port number");
            }

            return validInformation;
        }

        private const string START_TEXT = "Connect";
        private const string STOP_TEXT = "Disconnect";
        public ContractManager Contractor { get { return (ContractManager)ProcessInstance; } }
        public delegate void StatusChanged_Update(ProcessInfo processInfo);
    }
}
