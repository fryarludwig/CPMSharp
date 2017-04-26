namespace ContractManager
{
    partial class ContractWindow
    {

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
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
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        {
            this.StartButton = new System.Windows.Forms.Button();
            this.authenticatorPortInput = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.authenticatorAddressInput = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.StatusDisplay = new System.Windows.Forms.Label();
            this.workItemsDisplay = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.InputErrorProvider)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ShowDebugInput
            // 
            this.ShowDebugInput.Location = new System.Drawing.Point(494, 204);
            // 
            // ShowTraceInput
            // 
            this.ShowTraceInput.Location = new System.Drawing.Point(434, 204);
            // 
            // ShowInfoInput
            // 
            this.ShowInfoInput.Location = new System.Drawing.Point(384, 205);
            // 
            // ShowWarningsInput
            // 
            this.ShowWarningsInput.Location = new System.Drawing.Point(326, 205);
            // 
            // ShowErrorsInput
            // 
            this.ShowErrorsInput.Location = new System.Drawing.Point(272, 204);
            // 
            // GuiLogOutput
            // 
            this.GuiLogOutput.Location = new System.Drawing.Point(12, 228);
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(329, 20);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 10;
            this.StartButton.Text = "Connect";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.Connect_Clicked);
            // 
            // authenticatorPortInput
            // 
            this.authenticatorPortInput.Location = new System.Drawing.Point(212, 22);
            this.authenticatorPortInput.Name = "authenticatorPortInput";
            this.authenticatorPortInput.Size = new System.Drawing.Size(100, 20);
            this.authenticatorPortInput.TabIndex = 15;
            this.authenticatorPortInput.Text = "12034";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(180, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Port";
            // 
            // authenticatorAddressInput
            // 
            this.authenticatorAddressInput.Location = new System.Drawing.Point(58, 22);
            this.authenticatorAddressInput.Name = "authenticatorAddressInput";
            this.authenticatorAddressInput.Size = new System.Drawing.Size(100, 20);
            this.authenticatorAddressInput.TabIndex = 13;
            this.authenticatorAddressInput.Text = "127.0.0.1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Address";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.StatusDisplay);
            this.groupBox1.Controls.Add(this.StartButton);
            this.groupBox1.Controls.Add(this.authenticatorPortInput);
            this.groupBox1.Controls.Add(this.authenticatorAddressInput);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(541, 57);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Authenticator Connection Details";
            // 
            // StatusDisplay
            // 
            this.StatusDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusDisplay.AutoSize = true;
            this.StatusDisplay.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.StatusDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StatusDisplay.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusDisplay.Location = new System.Drawing.Point(416, 20);
            this.StatusDisplay.Margin = new System.Windows.Forms.Padding(5);
            this.StatusDisplay.Name = "StatusDisplay";
            this.StatusDisplay.Size = new System.Drawing.Size(117, 24);
            this.StatusDisplay.TabIndex = 31;
            this.StatusDisplay.Text = "Not Started";
            this.StatusDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.StatusDisplay.UseCompatibleTextRendering = true;
            // 
            // workItemsDisplay
            // 
            this.workItemsDisplay.FormattingEnabled = true;
            this.workItemsDisplay.Location = new System.Drawing.Point(12, 76);
            this.workItemsDisplay.Name = "workItemsDisplay";
            this.workItemsDisplay.Size = new System.Drawing.Size(541, 121);
            this.workItemsDisplay.TabIndex = 31;
            // 
            // ContractWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 401);
            this.Controls.Add(this.workItemsDisplay);
            this.Controls.Add(this.groupBox1);
            this.Name = "ContractWindow";
            this.Text = "Contract Manager";
            this.Controls.SetChildIndex(this.ShowDebugInput, 0);
            this.Controls.SetChildIndex(this.GuiLogOutput, 0);
            this.Controls.SetChildIndex(this.ShowErrorsInput, 0);
            this.Controls.SetChildIndex(this.ShowWarningsInput, 0);
            this.Controls.SetChildIndex(this.ShowInfoInput, 0);
            this.Controls.SetChildIndex(this.ShowTraceInput, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.workItemsDisplay, 0);
            ((System.ComponentModel.ISupportInitialize)(this.InputErrorProvider)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.TextBox authenticatorPortInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox authenticatorAddressInput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label StatusDisplay;
        private System.Windows.Forms.ListBox workItemsDisplay;
    }
}

