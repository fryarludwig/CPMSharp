namespace ContractManager
{
    partial class ContractWindow
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
            this.authenticatorPortInput = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.authenticatorAddressInput = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // GuiLogOutput
            // 
            this.GuiLogOutput.FormattingEnabled = true;
            this.GuiLogOutput.Location = new System.Drawing.Point(12, 189);
            this.GuiLogOutput.Name = "GuiLogOutput";
            this.GuiLogOutput.Size = new System.Drawing.Size(577, 160);
            this.GuiLogOutput.TabIndex = 11;
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(346, 23);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectButton.TabIndex = 10;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.Connect_Clicked);
            // 
            // authenticatorPortInput
            // 
            this.authenticatorPortInput.Location = new System.Drawing.Point(214, 25);
            this.authenticatorPortInput.Name = "authenticatorPortInput";
            this.authenticatorPortInput.Size = new System.Drawing.Size(100, 20);
            this.authenticatorPortInput.TabIndex = 15;
            this.authenticatorPortInput.Text = "12034";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(182, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Port";
            // 
            // authenticatorAddressInput
            // 
            this.authenticatorAddressInput.Location = new System.Drawing.Point(60, 25);
            this.authenticatorAddressInput.Name = "authenticatorAddressInput";
            this.authenticatorAddressInput.Size = new System.Drawing.Size(100, 20);
            this.authenticatorAddressInput.TabIndex = 13;
            this.authenticatorAddressInput.Text = "127.0.0.1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Address";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Authentication Manager";
            // 
            // ContractManagerGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 361);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.authenticatorPortInput);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.authenticatorAddressInput);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.GuiLogOutput);
            this.Controls.Add(this.ConnectButton);
            this.Name = "ContractManagerGUI";
            this.Text = "Contract Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox GuiLogOutput;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.TextBox authenticatorPortInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox authenticatorAddressInput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
    }
}

