namespace VersionSelector
{
    partial class Form1
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
            this.uxRbGui = new System.Windows.Forms.RadioButton();
            this.uxRbConsole = new System.Windows.Forms.RadioButton();
            this.uxBtnStart = new System.Windows.Forms.Button();
            this.uxLblSelect = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // uxRbGui
            // 
            this.uxRbGui.AutoSize = true;
            this.uxRbGui.Location = new System.Drawing.Point(25, 49);
            this.uxRbGui.Name = "uxRbGui";
            this.uxRbGui.Size = new System.Drawing.Size(64, 24);
            this.uxRbGui.TabIndex = 0;
            this.uxRbGui.TabStop = true;
            this.uxRbGui.Text = "GUI";
            this.uxRbGui.UseVisualStyleBackColor = true;
            // 
            // uxRbConsole
            // 
            this.uxRbConsole.AutoSize = true;
            this.uxRbConsole.Location = new System.Drawing.Point(25, 88);
            this.uxRbConsole.Name = "uxRbConsole";
            this.uxRbConsole.Size = new System.Drawing.Size(92, 24);
            this.uxRbConsole.TabIndex = 1;
            this.uxRbConsole.TabStop = true;
            this.uxRbConsole.Text = "Console";
            this.uxRbConsole.UseVisualStyleBackColor = true;
            // 
            // uxBtnStart
            // 
            this.uxBtnStart.Location = new System.Drawing.Point(25, 132);
            this.uxBtnStart.Name = "uxBtnStart";
            this.uxBtnStart.Size = new System.Drawing.Size(291, 51);
            this.uxBtnStart.TabIndex = 2;
            this.uxBtnStart.Text = "Start";
            this.uxBtnStart.UseVisualStyleBackColor = true;
            this.uxBtnStart.Click += new System.EventHandler(this.uxBtnStart_Click);
            // 
            // uxLblSelect
            // 
            this.uxLblSelect.AutoSize = true;
            this.uxLblSelect.Location = new System.Drawing.Point(25, 13);
            this.uxLblSelect.Name = "uxLblSelect";
            this.uxLblSelect.Size = new System.Drawing.Size(176, 20);
            this.uxLblSelect.TabIndex = 3;
            this.uxLblSelect.Text = "Select Program Version";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 195);
            this.Controls.Add(this.uxLblSelect);
            this.Controls.Add(this.uxBtnStart);
            this.Controls.Add(this.uxRbConsole);
            this.Controls.Add(this.uxRbGui);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Version Selector";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton uxRbGui;
        private System.Windows.Forms.RadioButton uxRbConsole;
        private System.Windows.Forms.Button uxBtnStart;
        private System.Windows.Forms.Label uxLblSelect;
    }
}

