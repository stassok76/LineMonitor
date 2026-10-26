namespace LineMonitor.TcpTool
{
    partial class ClientForm
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
            txtHost = new TextBox();
            txtPort = new TextBox();
            btnConnect = new Button();
            txtLog = new TextBox();
            chkHexReceive = new CheckBox();
            txtSend = new TextBox();
            chkHexSend = new CheckBox();
            btnSend = new Button();
            btnClear = new Button();
            chkAutoReconnect = new CheckBox();
            SuspendLayout();
            // 
            // txtHost
            // 
            txtHost.Location = new Point(69, 21);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(200, 38);
            txtHost.TabIndex = 0;
            txtHost.Text = "127.0.0.1";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(307, 26);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(200, 38);
            txtPort.TabIndex = 1;
            txtPort.Text = "6000";
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(577, 21);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(150, 46);
            btnConnect.TabIndex = 2;
            btnConnect.Text = "连接";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(58, 91);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(222, 85);
            txtLog.TabIndex = 3;
            // 
            // chkHexReceive
            // 
            chkHexReceive.AutoSize = true;
            chkHexReceive.Location = new Point(58, 206);
            chkHexReceive.Name = "chkHexReceive";
            chkHexReceive.Size = new Size(190, 35);
            chkHexReceive.TabIndex = 4;
            chkHexReceive.Text = "十六进制显示";
            chkHexReceive.UseVisualStyleBackColor = true;
            // 
            // txtSend
            // 
            txtSend.Location = new Point(27, 287);
            txtSend.Name = "txtSend";
            txtSend.Size = new Size(200, 38);
            txtSend.TabIndex = 5;
            // 
            // chkHexSend
            // 
            chkHexSend.AutoSize = true;
            chkHexSend.Location = new Point(265, 206);
            chkHexSend.Name = "chkHexSend";
            chkHexSend.Size = new Size(190, 35);
            chkHexSend.TabIndex = 6;
            chkHexSend.Text = "十六进制发送";
            chkHexSend.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(243, 287);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(150, 46);
            btnSend.TabIndex = 7;
            btnSend.Text = "发送";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(345, 119);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(150, 46);
            btnClear.TabIndex = 8;
            btnClear.Text = "清空日志";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // chkAutoReconnect
            // 
            chkAutoReconnect.AutoSize = true;
            chkAutoReconnect.Location = new Point(620, 93);
            chkAutoReconnect.Name = "chkAutoReconnect";
            chkAutoReconnect.Size = new Size(142, 35);
            chkAutoReconnect.TabIndex = 9;
            chkAutoReconnect.Text = "自动重连";
            chkAutoReconnect.UseVisualStyleBackColor = true;
            // 
            // ClientForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(chkAutoReconnect);
            Controls.Add(btnClear);
            Controls.Add(btnSend);
            Controls.Add(chkHexSend);
            Controls.Add(txtSend);
            Controls.Add(chkHexReceive);
            Controls.Add(txtLog);
            Controls.Add(btnConnect);
            Controls.Add(txtPort);
            Controls.Add(txtHost);
            Name = "ClientForm";
            Text = "ClientForm";
            FormClosing += ClientForm_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtHost;
        private TextBox txtPort;
        private Button btnConnect;
        private TextBox txtLog;
        private CheckBox chkHexReceive;
        private TextBox txtSend;
        private CheckBox chkHexSend;
        private Button btnSend;
        private Button btnClear;
        private CheckBox chkAutoReconnect;
    }
}