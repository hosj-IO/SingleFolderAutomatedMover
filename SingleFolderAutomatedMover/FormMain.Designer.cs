namespace SingleFolderAutomatedMover
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.buttonStart = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.buttonStop = new System.Windows.Forms.Button();
            this.listBoxLogging = new System.Windows.Forms.ListBox();
            this.notifyIconMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.buttonConfiguration = new System.Windows.Forms.Button();
            this.labelConfInfoFrom = new System.Windows.Forms.Label();
            this.labelConfInfoTo = new System.Windows.Forms.Label();
            this.textBoxToInfo = new System.Windows.Forms.TextBox();
            this.textBoxFromInfo = new System.Windows.Forms.TextBox();
            this.textBoxUserInfo = new System.Windows.Forms.TextBox();
            this.labelConfUserForm = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(15, 12);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(125, 62);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(12, 77);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(40, 13);
            this.labelStatus.TabIndex = 2;
            this.labelStatus.Text = "Status:";
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(186, 12);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(112, 62);
            this.buttonStop.TabIndex = 4;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // listBoxLogging
            // 
            this.listBoxLogging.FormattingEnabled = true;
            this.listBoxLogging.Location = new System.Drawing.Point(12, 93);
            this.listBoxLogging.Name = "listBoxLogging";
            this.listBoxLogging.ScrollAlwaysVisible = true;
            this.listBoxLogging.Size = new System.Drawing.Size(881, 225);
            this.listBoxLogging.TabIndex = 5;
            // 
            // notifyIconMain
            // 
            this.notifyIconMain.Text = "notifyIcon1";
            this.notifyIconMain.Visible = true;
            this.notifyIconMain.DoubleClick += new System.EventHandler(this.notifyIconMain_DoubleClick);
            // 
            // buttonConfiguration
            // 
            this.buttonConfiguration.Location = new System.Drawing.Point(781, 12);
            this.buttonConfiguration.Name = "buttonConfiguration";
            this.buttonConfiguration.Size = new System.Drawing.Size(112, 62);
            this.buttonConfiguration.TabIndex = 6;
            this.buttonConfiguration.Text = "Configuration";
            this.buttonConfiguration.UseVisualStyleBackColor = true;
            this.buttonConfiguration.Click += new System.EventHandler(this.buttonConfiguration_Click);
            // 
            // labelConfInfoFrom
            // 
            this.labelConfInfoFrom.AutoSize = true;
            this.labelConfInfoFrom.Location = new System.Drawing.Point(304, 15);
            this.labelConfInfoFrom.Name = "labelConfInfoFrom";
            this.labelConfInfoFrom.Size = new System.Drawing.Size(33, 13);
            this.labelConfInfoFrom.TabIndex = 7;
            this.labelConfInfoFrom.Text = "From:";
            // 
            // labelConfInfoTo
            // 
            this.labelConfInfoTo.AutoSize = true;
            this.labelConfInfoTo.Location = new System.Drawing.Point(304, 41);
            this.labelConfInfoTo.Name = "labelConfInfoTo";
            this.labelConfInfoTo.Size = new System.Drawing.Size(23, 13);
            this.labelConfInfoTo.TabIndex = 8;
            this.labelConfInfoTo.Text = "To:";
            // 
            // textBoxToInfo
            // 
            this.textBoxToInfo.Enabled = false;
            this.textBoxToInfo.Location = new System.Drawing.Point(343, 38);
            this.textBoxToInfo.Name = "textBoxToInfo";
            this.textBoxToInfo.Size = new System.Drawing.Size(432, 20);
            this.textBoxToInfo.TabIndex = 10;
            // 
            // textBoxFromInfo
            // 
            this.textBoxFromInfo.Enabled = false;
            this.textBoxFromInfo.Location = new System.Drawing.Point(343, 12);
            this.textBoxFromInfo.Name = "textBoxFromInfo";
            this.textBoxFromInfo.Size = new System.Drawing.Size(432, 20);
            this.textBoxFromInfo.TabIndex = 11;
            // 
            // textBoxUserInfo
            // 
            this.textBoxUserInfo.Enabled = false;
            this.textBoxUserInfo.Location = new System.Drawing.Point(343, 64);
            this.textBoxUserInfo.Name = "textBoxUserInfo";
            this.textBoxUserInfo.Size = new System.Drawing.Size(432, 20);
            this.textBoxUserInfo.TabIndex = 13;
            // 
            // labelConfUserForm
            // 
            this.labelConfUserForm.AutoSize = true;
            this.labelConfUserForm.Location = new System.Drawing.Point(304, 67);
            this.labelConfUserForm.Name = "labelConfUserForm";
            this.labelConfUserForm.Size = new System.Drawing.Size(32, 13);
            this.labelConfUserForm.TabIndex = 12;
            this.labelConfUserForm.Text = "User:";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 334);
            this.Controls.Add(this.textBoxUserInfo);
            this.Controls.Add(this.labelConfUserForm);
            this.Controls.Add(this.textBoxFromInfo);
            this.Controls.Add(this.textBoxToInfo);
            this.Controls.Add(this.labelConfInfoTo);
            this.Controls.Add(this.labelConfInfoFrom);
            this.Controls.Add(this.buttonConfiguration);
            this.Controls.Add(this.listBoxLogging);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.buttonStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.ListBox listBoxLogging;
        private System.Windows.Forms.NotifyIcon notifyIconMain;
        private System.Windows.Forms.Button buttonConfiguration;
        private System.Windows.Forms.Label labelConfInfoFrom;
        private System.Windows.Forms.Label labelConfInfoTo;
        private System.Windows.Forms.TextBox textBoxToInfo;
        private System.Windows.Forms.TextBox textBoxFromInfo;
        private System.Windows.Forms.TextBox textBoxUserInfo;
        private System.Windows.Forms.Label labelConfUserForm;
    }
}

