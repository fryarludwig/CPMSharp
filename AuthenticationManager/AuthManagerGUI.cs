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
using System.Collections.Concurrent;

namespace AuthenticationManager
{
    public partial class AuthManagerGUI : Form
    {
        public AuthManagerGUI()
        {
            InitializeComponent();

            Logger.ConsoleOutput = true;
            Logger.GuiOutput = true;
            Logger.FileOutput = true;
            Task.Factory.StartNew(RunLoop);
        }

        public void KillChildren()
        {
            AuthenticationService.Stop();
        }

        private void StartServer_Clicked(object sender, EventArgs e)
        {
            StartButton.Enabled = false;
            if (StartButton.Text == "Login" && ValidateLoginInformation())
            {
                Logger.Trace("Login information passed basic validation");
                //InitializePlayerInformation();
                PerformLogin().ContinueWith((t) => UpdateLoginStatus(t.Result), TaskScheduler.FromCurrentSynchronizationContext());
            }
            else if (StartButton.Text == "Logout")
            {
                PerformLogout().ContinueWith((t) => UpdateLoginStatus(t.Result), TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                Logger.Info("User entered invalid information");
                StartButton.Enabled = true;
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
            if (!int.TryParse(portInput.Text, out throwaway))
            {
                validInformation = false;
                InputErrorProvider.SetError(portInput, "Please enter a valid port number");
            }

            return validInformation;
        }
        
        private Task<bool> PerformLogin()
        {
            Logger.Trace("Calling Login function for player");

            return Task.Factory.StartNew<bool>(AuthenticationService.LoginHelper);
        }

        private Task<bool> PerformLogout()
        {
            Logger.Trace("Requesting user log out");

            return Task.Factory.StartNew<bool>(AuthenticationService.LogoutHelper);
        }

        private void UpdateLoginStatus(bool loggedIn)
        {
            if (loggedIn)
            {
                Logger.Trace("Player is logged in");
                StartButton.Text = "Logout";
            }
            else
            {
                Logger.Trace("Player is logged out");
                StartButton.Text = "Login";
            }

            StartButton.Enabled = true;
        }
        
        public void UpdateGuiStatus()
        {
            // Call a method to update the gui
        }

        protected ErrorProvider InputErrorProvider = new ErrorProvider();
        protected LogUtility Logger = new LogUtility("DSoak GUI");
        protected AuthManager AuthenticationService { get; }
        public static ConcurrentQueue<LogItem> GuiLogQueue = new ConcurrentQueue<LogItem>();
        public static ConcurrentDictionary<Level, bool> LogPrintDictionary = new ConcurrentDictionary<Level, bool>();


    }
}

