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
            this.GuiLogOutput = new System.Windows.Forms.ListBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.portInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.addressInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // GuiLogOutput
            // 
            this.GuiLogOutput.FormattingEnabled = true;
            this.GuiLogOutput.Location = new System.Drawing.Point(80, 217);
            this.GuiLogOutput.Name = "GuiLogOutput";
            this.GuiLogOutput.Size = new System.Drawing.Size(541, 160);
            this.GuiLogOutput.TabIndex = 17;
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(421, 38);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectButton.TabIndex = 16;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.Connect_Clicked);
            // 
            // portInput
            // 
            this.portInput.Location = new System.Drawing.Point(285, 40);
            this.portInput.Name = "portInput";
            this.portInput.Size = new System.Drawing.Size(100, 20);
            this.portInput.TabIndex = 15;
            this.portInput.Text = "12034";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(253, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Port";
            // 
            // addressInput
            // 
            this.addressInput.Location = new System.Drawing.Point(131, 40);
            this.addressInput.Name = "addressInput";
            this.addressInput.Size = new System.Drawing.Size(100, 20);
            this.addressInput.TabIndex = 13;
            this.addressInput.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Address";
            // 
            // SharpMainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 415);
            this.Controls.Add(this.GuiLogOutput);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.portInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.addressInput);
            this.Controls.Add(this.label1);
            this.Name = "SharpMainWindow";
            this.Text = "CPM Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox GuiLogOutput;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.TextBox portInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox addressInput;
        private System.Windows.Forms.Label label1;
    }
}

