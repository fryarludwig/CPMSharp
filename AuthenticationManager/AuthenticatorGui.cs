﻿using System;
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

namespace AuthenticationManager
{
    public partial class AuthenticatorGui : BaseWindowForm
    {
        public AuthenticatorGui() : base("AuthGui", new AuthManager())
        {
            InitializeComponent();
            LoggerOutput = GuiLogOutput;
            StartButton.Text = START_TEXT;
            Logger.Trace("Started Authentication Manager Interface");
        }

        public void KillChildren()
        {
            //AuthenticationService();
        }

        protected override void ProcessStatusChanged(ProcessInfo processInfo)
        {
            Logger.Trace(processInfo.StatusString);

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

            StartButton.Enabled = true;
        }

        protected override void PrepopulateProcessValues()
        {
            ProcessInstance.LocalEndpoint = new IPEndPoint(IPAddress.Any, int.Parse(portInput.Text));
        }

        private void StartServer_Clicked(object sender, EventArgs e)
        {
            StartButton.Enabled = false;
            if (StartButton.Text == START_TEXT && ValidateLoginInformation())
            {
                PrepopulateProcessValues();
                ProcessInstance.StartConnection();
            }
            else if (StartButton.Text == STOP_TEXT)
            {
                Logger.Trace("Waiting for shutdown messages to propogate");
                ProcessInstance.CloseConnection();
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

            return validInformation;
        }

        private const string START_TEXT = "Start";
        private const string STOP_TEXT = "Stop";

        //public void OnMatchFound_Trigger(MatchResult result)
        //{
        //    Logger.Info("Match found!");

        //    if (InvokeRequired)
        //    {
        //        this.BeginInvoke(new OnMatchFound_GuiUpdate(OnMatchFound_Trigger), new object[] { result });
        //    }
        //    else
        //    {
        //        MatchResultsListBox.Items.Add("     " + result.ToString());
        //    }
        //}

        //public delegate void OnMatchFound_GuiUpdate(MatchResult result);
    }
}

