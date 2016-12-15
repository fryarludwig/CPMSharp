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

namespace ContractManager
{
    public partial class ContractWindow : Form
    {
        public ContractWindow()
        {
            InitializeComponent();
            //PersonMatcher.OnMatchFound += new MatchMaker.MatchEvent(OnMatchFound_Trigger);
            //Logger.RegisterGuiCallback(this);
            Logger.ConsoleOutput = true;
            Logger.GuiOutput = true;
            Logger.FileOutput = true;
            Task.Factory.StartNew(RunLoop);
            ContractService = new ContractManager();
        }
        
        public void KillChildren()
        {
            //ContractService.Stop();
        }

        protected void InitializeService()
        {
            ContractService.AuthenticatorEndpoint = new IPEndPoint(IPAddress.Parse(authenticatorAddressInput.Text), int.Parse(authenticatorPortInput.Text));
        }

        private void Connect_Clicked(object sender, EventArgs e)
        {
            ConnectButton.Enabled = false;
            if (ConnectButton.Text == "Connect" && ValidateLoginInformation())
            {
                Logger.Trace("Login information passed basic validation");
                InitializeService();
                PerformLogin().ContinueWith((t) => UpdateLoginStatus(t.Result), TaskScheduler.FromCurrentSynchronizationContext());
            }
            else if (ConnectButton.Text == "Disconnect")
            {
                PerformLogout().ContinueWith((t) => UpdateLoginStatus(t.Result), TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                Logger.Info("User entered invalid information");
                ConnectButton.Enabled = true;
            }
        }


        protected void RunLoop()
        {
            int NumberOfItems = GuiLogOutput.ClientSize.Height / GuiLogOutput.ItemHeight;
            bool keepGoing = true;
            LogItem message;

            while (keepGoing)
            {
                if (!GuiLogQueue.IsEmpty && GuiLogQueue.TryDequeue(out message))
                {
                    Invoke((MethodInvoker)delegate
                    {
                        try
                        {
                            if (LogPrintDictionary[message.LogLevel])
                            {
                                GuiLogOutput.Items.Add(message.LogMessage);
                                if (GuiLogOutput.TopIndex == GuiLogOutput.Items.Count - NumberOfItems - 1)
                                {
                                    //The item at the top when you can just see the bottom item
                                    GuiLogOutput.TopIndex = GuiLogOutput.Items.Count - NumberOfItems + 1;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.GuiOutput = false;
                            Logger.ConsoleOutput = true;
                            Logger.Error(e.Message);
                            keepGoing = false;
                        }
                    });
                }

                Thread.Sleep(100);
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

        private Task<bool> PerformLogin()
        {
            return Task.Factory.StartNew<bool>(ContractService.InitializeConnection);
        }

        private Task<bool> PerformLogout()
        {
            return Task.Factory.StartNew<bool>(ContractService.ShutdownProcess);
        }

        private void UpdateLoginStatus(bool loggedIn)
        {
            if (loggedIn)
            {
                ConnectButton.Text = "Disconnect";
            }
            else
            {
                ConnectButton.Text = "Connect";
            }

            ConnectButton.Enabled = true;
        }

        public void UpdateGuiStatus()
        {
            // Call a method to update the gui
        }

        protected ErrorProvider InputErrorProvider = new ErrorProvider();
        protected LogUtility Logger = new LogUtility("Contract GUI");
        protected ContractManager ContractService { get; }
        public static ConcurrentQueue<LogItem> GuiLogQueue = new ConcurrentQueue<LogItem>();
        public static ConcurrentDictionary<Level, bool> LogPrintDictionary = new ConcurrentDictionary<Level, bool>();

        private void portInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
