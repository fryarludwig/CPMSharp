﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Common.Forms;

namespace CPMClient
{
    public partial class ClientWindow : BaseLoggingForm
    {
        public ClientWindow() : base("ClientUI", new CPMClient())
        {
            InitializeComponent();
            //ClientService.Registration_OnChange += ProcessRegistrationUpdate;
            Logger.Trace("Client window initialized successfully");
        }
        
        protected void InitializeService()
        {
            string address = addressInput.Text;
            int port = int.Parse(portInput.Text);
            ClientService.AuthenticatorEndpoint = new IPEndPoint(IPAddress.Parse(address), port);
        }

        private void Connect_Clicked(object sender, EventArgs e)
        {
            ConnectButton.Enabled = false;
            if (ConnectButton.Text == "Connect" && ValidateLoginInformation())
            {
                Logger.Trace("Login information passed basic validation");
                InitializeService();
                //ProcessInst
                //PerformLogin().ContinueWith((t) => UpdateLoginStatus(t.Result), TaskScheduler.FromCurrentSynchronizationContext());
            }
            else if (ConnectButton.Text == "Disconnect")
            {
                //PerformLogout().ContinueWith((t) => UpdateLoginStatus(t.Result), TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                Logger.Info("User entered invalid information");
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

            if (!int.TryParse(portInput.Text, out int throwaway))
            {
                validInformation = false;
                InputErrorProvider.SetError(portInput, "Please enter a valid port number");
            }

            return validInformation;
        }
        
        private void UpdateLoginStatus(bool loggedIn)
        {
            if (loggedIn)
            {
                Logger.Trace("User is logged in");
                ConnectButton.Text = "Disconnect";
            }
            else
            {
                Logger.Trace("User is logged out");
                ConnectButton.Text = "Connect";
            }

            ConnectButton.Enabled = true;
        }

        public void UpdateGuiStatus()
        {
            // Call a method to update the gui
        }

        protected CPMClient ClientService => (CPMClient)ProcessInstance;
        private void ClientWindow_Load(object sender, EventArgs e)
        {

        }

        private void Task_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Phase_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Contract_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ShowDebugInput_CheckedChanged_1(object sender, EventArgs e)
        {

        }
    }
}
