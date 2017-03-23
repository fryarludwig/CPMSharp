namespace Common.Forms
{
    partial class LoggingUtilityForm
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
            this.ParentProcessLabel = new System.Windows.Forms.Label();
            this.ShowErrorsInput = new System.Windows.Forms.CheckBox();
            this.ShowWarningsInput = new System.Windows.Forms.CheckBox();
            this.ShowTraceInput = new System.Windows.Forms.CheckBox();
            this.ShowInfoInput = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // GuiLogOutput
            // 
            this.GuiLogOutput.FormattingEnabled = true;
            this.GuiLogOutput.Location = new System.Drawing.Point(12, 51);
            this.GuiLogOutput.Name = "GuiLogOutput";
            this.GuiLogOutput.Size = new System.Drawing.Size(705, 368);
            this.GuiLogOutput.TabIndex = 6;
            // 
            // ParentProcessLabel
            // 
            this.ParentProcessLabel.AutoSize = true;
            this.ParentProcessLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ParentProcessLabel.Location = new System.Drawing.Point(12, 19);
            this.ParentProcessLabel.Name = "ParentProcessLabel";
            this.ParentProcessLabel.Size = new System.Drawing.Size(234, 20);
            this.ParentProcessLabel.TabIndex = 7;
            this.ParentProcessLabel.Text = "PARENT_PROCESS_NAME";
            // 
            // ShowErrorsInput
            // 
            this.ShowErrorsInput.AutoSize = true;
            this.ShowErrorsInput.Checked = true;
            this.ShowErrorsInput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowErrorsInput.Location = new System.Drawing.Point(501, 23);
            this.ShowErrorsInput.Name = "ShowErrorsInput";
            this.ShowErrorsInput.Size = new System.Drawing.Size(48, 17);
            this.ShowErrorsInput.TabIndex = 8;
            this.ShowErrorsInput.Text = "Error";
            this.ShowErrorsInput.UseVisualStyleBackColor = true;
            this.ShowErrorsInput.CheckedChanged += new System.EventHandler(this.ShowErrorsInput_CheckedChanged);
            // 
            // ShowWarningsInput
            // 
            this.ShowWarningsInput.AutoSize = true;
            this.ShowWarningsInput.Checked = true;
            this.ShowWarningsInput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowWarningsInput.Location = new System.Drawing.Point(555, 24);
            this.ShowWarningsInput.Name = "ShowWarningsInput";
            this.ShowWarningsInput.Size = new System.Drawing.Size(52, 17);
            this.ShowWarningsInput.TabIndex = 9;
            this.ShowWarningsInput.Text = "Warn";
            this.ShowWarningsInput.UseVisualStyleBackColor = true;
            this.ShowWarningsInput.CheckedChanged += new System.EventHandler(this.ShowWarnings_CheckedChanged);
            // 
            // ShowTraceInput
            // 
            this.ShowTraceInput.AutoSize = true;
            this.ShowTraceInput.Checked = true;
            this.ShowTraceInput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowTraceInput.Location = new System.Drawing.Point(663, 24);
            this.ShowTraceInput.Name = "ShowTraceInput";
            this.ShowTraceInput.Size = new System.Drawing.Size(54, 17);
            this.ShowTraceInput.TabIndex = 11;
            this.ShowTraceInput.Text = "Trace";
            this.ShowTraceInput.UseVisualStyleBackColor = true;
            this.ShowTraceInput.CheckedChanged += new System.EventHandler(this.ShowTraceInput_CheckedChanged);
            // 
            // ShowInfoInput
            // 
            this.ShowInfoInput.AutoSize = true;
            this.ShowInfoInput.Checked = true;
            this.ShowInfoInput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowInfoInput.Location = new System.Drawing.Point(613, 24);
            this.ShowInfoInput.Name = "ShowInfoInput";
            this.ShowInfoInput.Size = new System.Drawing.Size(44, 17);
            this.ShowInfoInput.TabIndex = 10;
            this.ShowInfoInput.Text = "Info";
            this.ShowInfoInput.UseVisualStyleBackColor = true;
            this.ShowInfoInput.CheckedChanged += new System.EventHandler(this.ShowInfoInput_CheckedChanged);
            // 
            // LoggingUtilityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 431);
            this.Controls.Add(this.ShowTraceInput);
            this.Controls.Add(this.ShowInfoInput);
            this.Controls.Add(this.ShowWarningsInput);
            this.Controls.Add(this.ShowErrorsInput);
            this.Controls.Add(this.ParentProcessLabel);
            this.Controls.Add(this.GuiLogOutput);
            this.Name = "LoggingUtilityForm";
            this.Text = "LoggingUtilityForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox GuiLogOutput;
        private System.Windows.Forms.Label ParentProcessLabel;
        private System.Windows.Forms.CheckBox ShowErrorsInput;
        private System.Windows.Forms.CheckBox ShowWarningsInput;
        private System.Windows.Forms.CheckBox ShowTraceInput;
        private System.Windows.Forms.CheckBox ShowInfoInput;
    }
}