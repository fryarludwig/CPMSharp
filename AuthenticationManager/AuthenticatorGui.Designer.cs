namespace AuthenticationManager
{
    partial class AuthenticatorGui
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
            this.portInput = new System.Windows.Forms.TextBox();
            this.intervalInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.StatusDisplay = new System.Windows.Forms.Label();
            this.ProcessesDisplay = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.InputErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // ShowTraceInput
            // 
            this.ShowTraceInput.Location = new System.Drawing.Point(470, 207);
            // 
            // ShowInfoInput
            // 
            this.ShowInfoInput.Location = new System.Drawing.Point(420, 207);
            // 
            // ShowWarningsInput
            // 
            this.ShowWarningsInput.Location = new System.Drawing.Point(362, 207);
            // 
            // ShowErrorsInput
            // 
            this.ShowErrorsInput.Location = new System.Drawing.Point(308, 206);
            // 
            // GuiLogOutput
            // 
            this.GuiLogOutput.Location = new System.Drawing.Point(12, 230);
            // 
            // portInput
            // 
            this.portInput.Location = new System.Drawing.Point(67, 18);
            this.portInput.Name = "portInput";
            this.portInput.Size = new System.Drawing.Size(52, 20);
            this.portInput.TabIndex = 3;
            this.portInput.Text = "12034";
            // 
            // intervalInput
            // 
            this.intervalInput.Location = new System.Drawing.Point(267, 18);
            this.intervalInput.Name = "intervalInput";
            this.intervalInput.Size = new System.Drawing.Size(52, 20);
            this.intervalInput.TabIndex = 20;
            this.intervalInput.Text = "30000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(136, 16);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(73, 23);
            this.StartButton.TabIndex = 4;
            this.StartButton.Text = "Start Server";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartConnection);
            // 
            // StatusDisplay
            // 
            this.StatusDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusDisplay.AutoSize = true;
            this.StatusDisplay.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.StatusDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StatusDisplay.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusDisplay.Location = new System.Drawing.Point(441, 19);
            this.StatusDisplay.Margin = new System.Windows.Forms.Padding(5);
            this.StatusDisplay.Name = "StatusDisplay";
            this.StatusDisplay.Size = new System.Drawing.Size(117, 24);
            this.StatusDisplay.TabIndex = 8;
            this.StatusDisplay.Text = "Not Started";
            this.StatusDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.StatusDisplay.UseCompatibleTextRendering = true;
            // 
            // ProcessesDisplay
            // 
            this.ProcessesDisplay.CheckBoxes = true;
            this.ProcessesDisplay.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.ProcessesDisplay.Location = new System.Drawing.Point(12, 51);
            this.ProcessesDisplay.Name = "ProcessesDisplay";
            this.ProcessesDisplay.Size = new System.Drawing.Size(540, 145);
            this.ProcessesDisplay.TabIndex = 9;
            this.ProcessesDisplay.UseCompatibleStateImageBehavior = false;
            this.ProcessesDisplay.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 40;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 140;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Label";
            this.columnHeader3.Width = 140;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Address";
            this.columnHeader4.Width = 95;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Status";
            this.columnHeader5.Width = 90;
            // 
            // AuthenticatorGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 401);
            this.Controls.Add(this.ProcessesDisplay);
            this.Controls.Add(this.StatusDisplay);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.portInput);
            this.Controls.Add(this.intervalInput);
            this.Controls.Add(this.label2);
            this.Name = "AuthenticatorGui";
            this.Text = "Authentication Manager";
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.intervalInput, 0);
            this.Controls.SetChildIndex(this.portInput, 0);
            this.Controls.SetChildIndex(this.StartButton, 0);
            this.Controls.SetChildIndex(this.GuiLogOutput, 0);
            this.Controls.SetChildIndex(this.StatusDisplay, 0);
            this.Controls.SetChildIndex(this.ProcessesDisplay, 0);
            this.Controls.SetChildIndex(this.ShowErrorsInput, 0);
            this.Controls.SetChildIndex(this.ShowWarningsInput, 0);
            this.Controls.SetChildIndex(this.ShowInfoInput, 0);
            this.Controls.SetChildIndex(this.ShowTraceInput, 0);
            ((System.ComponentModel.ISupportInitialize)(this.InputErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox portInput;
        private System.Windows.Forms.TextBox intervalInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label StatusDisplay;
        private System.Windows.Forms.ListView ProcessesDisplay;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}

