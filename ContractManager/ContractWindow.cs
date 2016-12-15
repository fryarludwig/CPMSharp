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
            ConnectButton.Text = START_TEXT;
        }
        
        public void KillChildren()
        {
            //ContractService.Stop();
        }

        protected override void ProcessStatusChanged(ProcessInfo processInfo)
        {
            Logger.Trace(processInfo.StatusString);

            if (processInfo.Status == ProcessInfo.StatusCode.Registered)
            {
                Logger.Trace("Server started successfull");
                ConnectButton.Text = STOP_TEXT;
            }
            else
            {
                Logger.Trace($"Server status is {processInfo.StatusString}");
                ConnectButton.Text = START_TEXT;
            }
        }

        protected void InitializeService()
        {
            ProcessInstance.AuthenticatorEndpoint = new IPEndPoint(IPAddress.Parse(authenticatorAddressInput.Text), int.Parse(authenticatorPortInput.Text));
        }

        private void Connect_Clicked(object sender, EventArgs e)
        {
            ConnectButton.Enabled = false;
            if (ConnectButton.Text == START_TEXT && ValidateLoginInformation())
            {
                PrepopulateProcessValues();
                ProcessInstance.StartConnection();
            }
            else if (ConnectButton.Text == STOP_TEXT)
            {
                Logger.Trace("Waiting for shutdown messages to propogate");
                ProcessInstance.CloseConnection();
            }
            else
            {
                Logger.Error("User entered invalid information");
                ConnectButton.Enabled = true;
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
            if (!int.TryParse(authenticatorPortInput.Text, out throwaway))
            {
                validInformation = false;
                InputErrorProvider.SetError(authenticatorPortInput, "Please enter a valid port number");
            }

            return validInformation;
        }
        

        public void UpdateGuiStatus()
        {
            // Call a method to update the gui
        }
        

        private const string START_TEXT = "Connect";
        private const string STOP_TEXT = "Disconnect";
    }
}
