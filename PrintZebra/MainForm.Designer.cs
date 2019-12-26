namespace PrintZebra
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.saveBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.server_url = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.printer = new System.Windows.Forms.ComboBox();
            this.editButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.Text = "PRINT ZEBRA";
            this.notifyIcon.Visible = true;
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(12, 131);
            this.saveBtn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(261, 31);
            this.saveBtn.TabIndex = 0;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server URL:";
            // 
            // server_url
            // 
            this.server_url.Location = new System.Drawing.Point(131, 36);
            this.server_url.Name = "server_url";
            this.server_url.Size = new System.Drawing.Size(421, 26);
            this.server_url.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Printer:";
            // 
            // printer
            // 
            this.printer.FormattingEnabled = true;
            this.printer.Location = new System.Drawing.Point(131, 81);
            this.printer.Name = "printer";
            this.printer.Size = new System.Drawing.Size(421, 28);
            this.printer.TabIndex = 5;
            // 
            // editButton
            // 
            this.editButton.Location = new System.Drawing.Point(302, 131);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(250, 30);
            this.editButton.TabIndex = 6;
            this.editButton.Text = "Edit";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 185);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.printer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.server_url);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.saveBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Zebra Printer Proxy";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox server_url;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox printer;
        private System.Windows.Forms.Button editButton;
    }
}

