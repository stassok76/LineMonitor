namespace LineMonitor.TcpTool
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txtPort = new TextBox();
            label2 = new Label();
            numTimeout = new NumericUpDown();
            btnStart = new Button();
            label3 = new Label();
            lstClients = new ListBox();
            txtLog = new TextBox();
            chkHexReceive = new CheckBox();
            txtSend = new TextBox();
            chkHexSend = new CheckBox();
            btnSend = new Button();
            btnBroadcast = new Button();
            btnClear = new Button();
            btnOpenClient = new Button();
            ((System.ComponentModel.ISupportInitialize)numTimeout).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(62, 31);
            label1.TabIndex = 0;
            label1.Text = "端口";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(80, 9);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(200, 38);
            txtPort.TabIndex = 1;
            txtPort.Text = "6000";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(303, 14);
            label2.Name = "label2";
            label2.Size = new Size(62, 31);
            label2.TabIndex = 2;
            label2.Text = "超时";
            // 
            // numTimeout
            // 
            numTimeout.Location = new Point(385, 12);
            numTimeout.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            numTimeout.Name = "numTimeout";
            numTimeout.Size = new Size(240, 38);
            numTimeout.TabIndex = 3;
            numTimeout.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(631, 6);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(150, 46);
            btnStart.TabIndex = 4;
            btnStart.Text = "启动";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 127);
            label3.Name = "label3";
            label3.Size = new Size(134, 31);
            label3.TabIndex = 5;
            label3.Text = "在线客户端";
            // 
            // lstClients
            // 
            lstClients.FormattingEnabled = true;
            lstClients.Location = new Point(21, 193);
            lstClients.Name = "lstClients";
            lstClients.Size = new Size(240, 190);
            lstClients.TabIndex = 6;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(287, 75);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(324, 219);
            txtLog.TabIndex = 7;
            txtLog.TextChanged += txtLog_TextChanged;
            // 
            // chkHexReceive
            // 
            chkHexReceive.AutoSize = true;
            chkHexReceive.Location = new Point(303, 319);
            chkHexReceive.Name = "chkHexReceive";
            chkHexReceive.Size = new Size(190, 35);
            chkHexReceive.TabIndex = 8;
            chkHexReceive.Text = "十六进制显示";
            chkHexReceive.UseVisualStyleBackColor = true;
            // 
            // txtSend
            // 
            txtSend.Location = new Point(287, 382);
            txtSend.Name = "txtSend";
            txtSend.Size = new Size(200, 38);
            txtSend.TabIndex = 9;
            // 
            // chkHexSend
            // 
            chkHexSend.AutoSize = true;
            chkHexSend.Location = new Point(521, 300);
            chkHexSend.Name = "chkHexSend";
            chkHexSend.Size = new Size(190, 35);
            chkHexSend.TabIndex = 10;
            chkHexSend.Text = "十六进制发送";
            chkHexSend.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(521, 341);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(150, 46);
            btnSend.TabIndex = 11;
            btnSend.Text = "发给选中";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // btnBroadcast
            // 
            btnBroadcast.Location = new Point(521, 412);
            btnBroadcast.Name = "btnBroadcast";
            btnBroadcast.Size = new Size(150, 46);
            btnBroadcast.TabIndex = 12;
            btnBroadcast.Text = "广播";
            btnBroadcast.UseVisualStyleBackColor = true;
            btnBroadcast.Click += btnBroadcast_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(677, 360);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(150, 46);
            btnClear.TabIndex = 13;
            btnClear.Text = "清空日志";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // btnOpenClient
            // 
            btnOpenClient.Location = new Point(660, 222);
            btnOpenClient.Name = "btnOpenClient";
            btnOpenClient.Size = new Size(150, 46);
            btnOpenClient.TabIndex = 14;
            btnOpenClient.Text = "打开客户端窗口";
            btnOpenClient.UseVisualStyleBackColor = true;
            btnOpenClient.Click += btnOpenClient_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnOpenClient);
            Controls.Add(btnClear);
            Controls.Add(btnBroadcast);
            Controls.Add(btnSend);
            Controls.Add(chkHexSend);
            Controls.Add(txtSend);
            Controls.Add(chkHexReceive);
            Controls.Add(txtLog);
            Controls.Add(lstClients);
            Controls.Add(label3);
            Controls.Add(btnStart);
            Controls.Add(numTimeout);
            Controls.Add(label2);
            Controls.Add(txtPort);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            ((System.ComponentModel.ISupportInitialize)numTimeout).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtPort;
        private Label label2;
        private NumericUpDown numTimeout;
        private Button btnStart;
        private Label label3;
        private ListBox lstClients;
        private TextBox txtLog;
        private CheckBox chkHexReceive;
        private TextBox txtSend;
        private CheckBox chkHexSend;
        private Button btnSend;
        private Button btnBroadcast;
        private Button btnClear;
        private Button btnOpenClient;
    }
}
