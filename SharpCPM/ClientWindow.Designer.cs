namespace SharpCPM
{
    partial class ClientWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ConnectButton = new System.Windows.Forms.Button();
            this.portInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.addressInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.usernameInput = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ContractListBox = new System.Windows.Forms.ListBox();
            this.PhaseListBox = new System.Windows.Forms.ListBox();
            this.TaskListBox = new System.Windows.Forms.ListBox();
            this.ItemSelectionBox = new System.Windows.Forms.GroupBox();
            this.detailsGroupBox = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.InputErrorProvider)).BeginInit();
            this.ItemSelectionBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ShowTraceInput
            // 
            this.ShowTraceInput.Location = new System.Drawing.Point(1246, 8);
            // 
            // ShowInfoInput
            // 
            this.ShowInfoInput.Location = new System.Drawing.Point(1196, 8);
            // 
            // ShowWarningsInput
            // 
            this.ShowWarningsInput.Location = new System.Drawing.Point(1138, 8);
            // 
            // ShowErrorsInput
            // 
            this.ShowErrorsInput.Location = new System.Drawing.Point(1084, 7);
            // 
            // GuiLogOutput
            // 
            this.GuiLogOutput.Location = new System.Drawing.Point(803, 31);
            this.GuiLogOutput.Size = new System.Drawing.Size(497, 641);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(683, 20);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectButton.TabIndex = 16;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.Connect_Clicked);
            // 
            // portInput
            // 
            this.portInput.Location = new System.Drawing.Point(210, 23);
            this.portInput.Name = "portInput";
            this.portInput.Size = new System.Drawing.Size(62, 20);
            this.portInput.TabIndex = 15;
            this.portInput.Text = "12034";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(178, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Port";
            // 
            // addressInput
            // 
            this.addressInput.Location = new System.Drawing.Point(68, 23);
            this.addressInput.Name = "addressInput";
            this.addressInput.Size = new System.Drawing.Size(100, 20);
            this.addressInput.TabIndex = 13;
            this.addressInput.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Address";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(542, 23);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(123, 20);
            this.textBox1.TabIndex = 33;
            this.textBox1.Text = "password_is_password";
            this.textBox1.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(485, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "Password";
            // 
            // usernameInput
            // 
            this.usernameInput.Location = new System.Drawing.Point(360, 23);
            this.usernameInput.Name = "usernameInput";
            this.usernameInput.Size = new System.Drawing.Size(119, 20);
            this.usernameInput.TabIndex = 31;
            this.usernameInput.Text = "fryarludwig";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(299, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Username";
            // 
            // ContractListBox
            // 
            this.ContractListBox.FormattingEnabled = true;
            this.ContractListBox.Location = new System.Drawing.Point(9, 19);
            this.ContractListBox.Name = "ContractListBox";
            this.ContractListBox.Size = new System.Drawing.Size(237, 199);
            this.ContractListBox.TabIndex = 34;
            this.ContractListBox.SelectedIndexChanged += new System.EventHandler(this.Contract_SelectedIndexChanged);
            // 
            // PhaseListBox
            // 
            this.PhaseListBox.FormattingEnabled = true;
            this.PhaseListBox.Location = new System.Drawing.Point(261, 19);
            this.PhaseListBox.Name = "PhaseListBox";
            this.PhaseListBox.Size = new System.Drawing.Size(237, 199);
            this.PhaseListBox.TabIndex = 35;
            this.PhaseListBox.SelectedIndexChanged += new System.EventHandler(this.Phase_SelectedIndexChanged);
            // 
            // TaskListBox
            // 
            this.TaskListBox.FormattingEnabled = true;
            this.TaskListBox.Location = new System.Drawing.Point(514, 19);
            this.TaskListBox.Name = "TaskListBox";
            this.TaskListBox.Size = new System.Drawing.Size(237, 199);
            this.TaskListBox.TabIndex = 36;
            this.TaskListBox.SelectedIndexChanged += new System.EventHandler(this.Task_SelectedIndexChanged);
            // 
            // ItemSelectionBox
            // 
            this.ItemSelectionBox.Controls.Add(this.PhaseListBox);
            this.ItemSelectionBox.Controls.Add(this.TaskListBox);
            this.ItemSelectionBox.Controls.Add(this.ContractListBox);
            this.ItemSelectionBox.Location = new System.Drawing.Point(12, 73);
            this.ItemSelectionBox.Name = "ItemSelectionBox";
            this.ItemSelectionBox.Size = new System.Drawing.Size(775, 228);
            this.ItemSelectionBox.TabIndex = 37;
            this.ItemSelectionBox.TabStop = false;
            this.ItemSelectionBox.Text = "Make a selection";
            // 
            // detailsGroupBox
            // 
            this.detailsGroupBox.Location = new System.Drawing.Point(12, 308);
            this.detailsGroupBox.Name = "detailsGroupBox";
            this.detailsGroupBox.Size = new System.Drawing.Size(775, 369);
            this.detailsGroupBox.TabIndex = 38;
            this.detailsGroupBox.TabStop = false;
            this.detailsGroupBox.Text = "Details";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.addressInput);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.portInput);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.ConnectButton);
            this.groupBox1.Controls.Add(this.usernameInput);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(775, 59);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection Details";
            // 
            // ClientWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 689);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.detailsGroupBox);
            this.Controls.Add(this.ItemSelectionBox);
            this.Name = "ClientWindow";
            this.Text = "CPM Client";
            this.Load += new System.EventHandler(this.ClientWindow_Load);
            this.Controls.SetChildIndex(this.ShowInfoInput, 0);
            this.Controls.SetChildIndex(this.ShowWarningsInput, 0);
            this.Controls.SetChildIndex(this.ShowErrorsInput, 0);
            this.Controls.SetChildIndex(this.ShowTraceInput, 0);
            this.Controls.SetChildIndex(this.GuiLogOutput, 0);
            this.Controls.SetChildIndex(this.ItemSelectionBox, 0);
            this.Controls.SetChildIndex(this.detailsGroupBox, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.InputErrorProvider)).EndInit();
            this.ItemSelectionBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.TextBox portInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox addressInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox usernameInput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox ContractListBox;
        private System.Windows.Forms.ListBox PhaseListBox;
        private System.Windows.Forms.ListBox TaskListBox;
        private System.Windows.Forms.GroupBox ItemSelectionBox;
        private System.Windows.Forms.GroupBox detailsGroupBox;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

