namespace ContractManager
{
    partial class ContractManagerGUI
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
            this.myPortInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.myAddressInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.authenticatorPortInput = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.authenticatorAddressInput = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
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
            this.ConnectButton.Location = new System.Drawing.Point(349, 75);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectButton.TabIndex = 10;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.Connect_Clicked);
            // 
            // myPortInput
            // 
            this.myPortInput.Location = new System.Drawing.Point(217, 30);
            this.myPortInput.Name = "myPortInput";
            this.myPortInput.Size = new System.Drawing.Size(100, 20);
            this.myPortInput.TabIndex = 9;
            this.myPortInput.Text = "5556";
            this.myPortInput.TextChanged += new System.EventHandler(this.portInput_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Port";
            // 
            // myAddressInput
            // 
            this.myAddressInput.Location = new System.Drawing.Point(63, 30);
            this.myAddressInput.Name = "myAddressInput";
            this.myAddressInput.Size = new System.Drawing.Size(100, 20);
            this.myAddressInput.TabIndex = 7;
            this.myAddressInput.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Address";
            // 
            // authenticatorPortInput
            // 
            this.authenticatorPortInput.Location = new System.Drawing.Point(217, 77);
            this.authenticatorPortInput.Name = "authenticatorPortInput";
            this.authenticatorPortInput.Size = new System.Drawing.Size(100, 20);
            this.authenticatorPortInput.TabIndex = 15;
            this.authenticatorPortInput.Text = "5555";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(185, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Port";
            // 
            // authenticatorAddressInput
            // 
            this.authenticatorAddressInput.Location = new System.Drawing.Point(63, 77);
            this.authenticatorAddressInput.Name = "authenticatorAddressInput";
            this.authenticatorAddressInput.Size = new System.Drawing.Size(100, 20);
            this.authenticatorAddressInput.TabIndex = 13;
            this.authenticatorAddressInput.Text = "127.0.0.1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Address";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Contract Manager";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 61);
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
            this.Controls.Add(this.label5);
            this.Controls.Add(this.authenticatorPortInput);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.authenticatorAddressInput);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.GuiLogOutput);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.myPortInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.myAddressInput);
            this.Controls.Add(this.label1);
            this.Name = "ContractManagerGUI";
            this.Text = "Contract Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox GuiLogOutput;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.TextBox myPortInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox myAddressInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox authenticatorPortInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox authenticatorAddressInput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}

