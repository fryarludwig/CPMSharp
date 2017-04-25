namespace Common.Forms
{
    public partial class BaseLoggingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        public System.ComponentModel.IContainer components = null;

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
        public void InitializeComponent()
        {
            this.ShowDebugInput = new System.Windows.Forms.CheckBox();
            this.ShowTraceInput = new System.Windows.Forms.CheckBox();
            this.ShowInfoInput = new System.Windows.Forms.CheckBox();
            this.ShowWarningsInput = new System.Windows.Forms.CheckBox();
            this.ShowErrorsInput = new System.Windows.Forms.CheckBox();
            this.GuiLogOutput = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // ShowDebugInput
            // 
            this.ShowDebugInput.AutoSize = true;
            this.ShowDebugInput.Checked = true;
            this.ShowDebugInput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowDebugInput.Location = new System.Drawing.Point(719, 323);
            this.ShowDebugInput.Name = "ShowDebugInput";
            this.ShowDebugInput.Size = new System.Drawing.Size(58, 17);
            this.ShowDebugInput.TabIndex = 30;
            this.ShowDebugInput.Text = "Debug";
            this.ShowDebugInput.UseVisualStyleBackColor = true;
            this.ShowDebugInput.CheckedChanged += new System.EventHandler(this.ShowDebugInput_CheckedChanged);
            // 
            // ShowTraceInput
            // 
            this.ShowTraceInput.AutoSize = true;
            this.ShowTraceInput.Checked = true;
            this.ShowTraceInput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowTraceInput.Location = new System.Drawing.Point(659, 323);
            this.ShowTraceInput.Name = "ShowTraceInput";
            this.ShowTraceInput.Size = new System.Drawing.Size(54, 17);
            this.ShowTraceInput.TabIndex = 29;
            this.ShowTraceInput.Text = "Trace";
            this.ShowTraceInput.UseVisualStyleBackColor = true;
            // 
            // ShowInfoInput
            // 
            this.ShowInfoInput.AutoSize = true;
            this.ShowInfoInput.Checked = true;
            this.ShowInfoInput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowInfoInput.Location = new System.Drawing.Point(609, 323);
            this.ShowInfoInput.Name = "ShowInfoInput";
            this.ShowInfoInput.Size = new System.Drawing.Size(44, 17);
            this.ShowInfoInput.TabIndex = 28;
            this.ShowInfoInput.Text = "Info";
            this.ShowInfoInput.UseVisualStyleBackColor = true;
            // 
            // ShowWarningsInput
            // 
            this.ShowWarningsInput.AutoSize = true;
            this.ShowWarningsInput.Checked = true;
            this.ShowWarningsInput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowWarningsInput.Location = new System.Drawing.Point(551, 323);
            this.ShowWarningsInput.Name = "ShowWarningsInput";
            this.ShowWarningsInput.Size = new System.Drawing.Size(52, 17);
            this.ShowWarningsInput.TabIndex = 27;
            this.ShowWarningsInput.Text = "Warn";
            this.ShowWarningsInput.UseVisualStyleBackColor = true;
            // 
            // ShowErrorsInput
            // 
            this.ShowErrorsInput.AutoSize = true;
            this.ShowErrorsInput.Checked = true;
            this.ShowErrorsInput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowErrorsInput.Location = new System.Drawing.Point(497, 322);
            this.ShowErrorsInput.Name = "ShowErrorsInput";
            this.ShowErrorsInput.Size = new System.Drawing.Size(48, 17);
            this.ShowErrorsInput.TabIndex = 26;
            this.ShowErrorsInput.Text = "Error";
            this.ShowErrorsInput.UseVisualStyleBackColor = true;
            // 
            // GuiLogOutput
            // 
            this.GuiLogOutput.FormattingEnabled = true;
            this.GuiLogOutput.Location = new System.Drawing.Point(238, 346);
            this.GuiLogOutput.Name = "GuiLogOutput";
            this.GuiLogOutput.Size = new System.Drawing.Size(541, 160);
            this.GuiLogOutput.TabIndex = 25;
            // 
            // BaseLoggingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 518);
            this.Controls.Add(this.ShowDebugInput);
            this.Controls.Add(this.GuiLogOutput);
            this.Controls.Add(this.ShowTraceInput);
            this.Controls.Add(this.ShowErrorsInput);
            this.Controls.Add(this.ShowWarningsInput);
            this.Controls.Add(this.ShowInfoInput);
            this.Name = "BaseLoggingForm";
            this.Text = "BaseLoggingForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.CheckBox ShowDebugInput;
        public System.Windows.Forms.CheckBox ShowTraceInput;
        public System.Windows.Forms.CheckBox ShowInfoInput;
        public System.Windows.Forms.CheckBox ShowWarningsInput;
        public System.Windows.Forms.CheckBox ShowErrorsInput;
        public System.Windows.Forms.ListBox GuiLogOutput;
    }
}