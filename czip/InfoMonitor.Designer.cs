namespace czip
{
    partial class InfoMonitor
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
            this.rtb_printArea = new System.Windows.Forms.RichTextBox();
            this.cb_message = new System.Windows.Forms.CheckBox();
            this.cb_info = new System.Windows.Forms.CheckBox();
            this.cb_warning = new System.Windows.Forms.CheckBox();
            this.cb_error = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // rtb_printArea
            // 
            this.rtb_printArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb_printArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtb_printArea.Location = new System.Drawing.Point(0, 0);
            this.rtb_printArea.Name = "rtb_printArea";
            this.rtb_printArea.ReadOnly = true;
            this.rtb_printArea.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.rtb_printArea.Size = new System.Drawing.Size(584, 326);
            this.rtb_printArea.TabIndex = 0;
            this.rtb_printArea.Text = "";
            // 
            // cb_message
            // 
            this.cb_message.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_message.AutoSize = true;
            this.cb_message.Checked = true;
            this.cb_message.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_message.Location = new System.Drawing.Point(13, 332);
            this.cb_message.Name = "cb_message";
            this.cb_message.Size = new System.Drawing.Size(74, 17);
            this.cb_message.TabIndex = 1;
            this.cb_message.Text = "Messages";
            this.cb_message.UseVisualStyleBackColor = true;
            // 
            // cb_info
            // 
            this.cb_info.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_info.AutoSize = true;
            this.cb_info.Checked = true;
            this.cb_info.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_info.Location = new System.Drawing.Point(93, 332);
            this.cb_info.Name = "cb_info";
            this.cb_info.Size = new System.Drawing.Size(44, 17);
            this.cb_info.TabIndex = 2;
            this.cb_info.Text = "Info";
            this.cb_info.UseVisualStyleBackColor = true;
            // 
            // cb_warning
            // 
            this.cb_warning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_warning.AutoSize = true;
            this.cb_warning.Checked = true;
            this.cb_warning.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_warning.Location = new System.Drawing.Point(143, 332);
            this.cb_warning.Name = "cb_warning";
            this.cb_warning.Size = new System.Drawing.Size(71, 17);
            this.cb_warning.TabIndex = 3;
            this.cb_warning.Text = "Warnings";
            this.cb_warning.UseVisualStyleBackColor = true;
            // 
            // cb_error
            // 
            this.cb_error.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cb_error.AutoSize = true;
            this.cb_error.Checked = true;
            this.cb_error.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_error.Location = new System.Drawing.Point(220, 332);
            this.cb_error.Name = "cb_error";
            this.cb_error.Size = new System.Drawing.Size(53, 17);
            this.cb_error.TabIndex = 4;
            this.cb_error.Text = "Errors";
            this.cb_error.UseVisualStyleBackColor = true;
            // 
            // InfoMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.cb_error);
            this.Controls.Add(this.cb_warning);
            this.Controls.Add(this.cb_info);
            this.Controls.Add(this.cb_message);
            this.Controls.Add(this.rtb_printArea);
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "InfoMonitor";
            this.Text = "Info Monitor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtb_printArea;
        private System.Windows.Forms.CheckBox cb_message;
        private System.Windows.Forms.CheckBox cb_info;
        private System.Windows.Forms.CheckBox cb_warning;
        private System.Windows.Forms.CheckBox cb_error;
    }
}