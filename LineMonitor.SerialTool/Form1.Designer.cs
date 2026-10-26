namespace LineMonitor.SerialTool
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
            cboPort = new ComboBox();
            label2 = new Label();
            cboBaud = new ComboBox();
            label3 = new Label();
            cboParity = new ComboBox();
            btnOpen = new Button();
            txtReceive = new TextBox();
            txtSend = new TextBox();
            btnSend = new Button();
            chkHexReceive = new CheckBox();
            chkHexSend = new CheckBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(53, 61);
            label1.Name = "label1";
            label1.Size = new Size(62, 31);
            label1.TabIndex = 0;
            label1.Text = "端口";
            // 
            // cboPort
            // 
            cboPort.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPort.FormattingEnabled = true;
            cboPort.Location = new Point(12, 3);
            cboPort.Name = "cboPort";
            cboPort.Size = new Size(213, 39);
            cboPort.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(318, 45);
            label2.Name = "label2";
            label2.Size = new Size(86, 31);
            label2.TabIndex = 2;
            label2.Text = "波特率";
            // 
            // cboBaud
            // 
            cboBaud.DropDownStyle = ComboBoxStyle.DropDownList;
            cboBaud.FormattingEnabled = true;
            cboBaud.Location = new Point(246, 3);
            cboBaud.Name = "cboBaud";
            cboBaud.Size = new Size(242, 39);
            cboBaud.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(567, 45);
            label3.Name = "label3";
            label3.Size = new Size(86, 31);
            label3.TabIndex = 4;
            label3.Text = "校验位";
            // 
            // cboParity
            // 
            cboParity.DropDownStyle = ComboBoxStyle.DropDownList;
            cboParity.FormattingEnabled = true;
            cboParity.Location = new Point(504, 3);
            cboParity.Name = "cboParity";
            cboParity.Size = new Size(242, 39);
            cboParity.TabIndex = 5;
            // 
            // btnOpen
            // 
            btnOpen.Location = new Point(655, 86);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(150, 46);
            btnOpen.TabIndex = 6;
            btnOpen.Text = "打开串口";
            btnOpen.UseVisualStyleBackColor = true;
            btnOpen.Click += btnOpen_Click;
            // 
            // txtReceive
            // 
            txtReceive.Location = new Point(204, 171);
            txtReceive.Multiline = true;
            txtReceive.Name = "txtReceive";
            txtReceive.ReadOnly = true;
            txtReceive.ScrollBars = ScrollBars.Vertical;
            txtReceive.Size = new Size(200, 76);
            txtReceive.TabIndex = 7;
            // 
            // txtSend
            // 
            txtSend.Location = new Point(90, 324);
            txtSend.Name = "txtSend";
            txtSend.Size = new Size(200, 38);
            txtSend.TabIndex = 8;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(494, 302);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(150, 46);
            btnSend.TabIndex = 9;
            btnSend.Text = "发送";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // chkHexReceive
            // 
            chkHexReceive.AutoSize = true;
            chkHexReceive.Location = new Point(463, 212);
            chkHexReceive.Name = "chkHexReceive";
            chkHexReceive.Size = new Size(190, 35);
            chkHexReceive.TabIndex = 10;
            chkHexReceive.Text = "十六进制显示";
            chkHexReceive.UseVisualStyleBackColor = true;
            // 
            // chkHexSend
            // 
            chkHexSend.AutoSize = true;
            chkHexSend.Location = new Point(452, 153);
            chkHexSend.Name = "chkHexSend";
            chkHexSend.Size = new Size(190, 35);
            chkHexSend.TabIndex = 11;
            chkHexSend.Text = "十六进制发送";
            chkHexSend.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(chkHexSend);
            Controls.Add(chkHexReceive);
            Controls.Add(btnSend);
            Controls.Add(txtSend);
            Controls.Add(txtReceive);
            Controls.Add(btnOpen);
            Controls.Add(cboParity);
            Controls.Add(label3);
            Controls.Add(cboBaud);
            Controls.Add(label2);
            Controls.Add(cboPort);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cboPort;
        private Label label2;
        private ComboBox cboBaud;
        private Label label3;
        private ComboBox cboParity;
        private Button btnOpen;
        private TextBox txtReceive;
        private TextBox txtSend;
        private Button btnSend;
        private CheckBox chkHexReceive;
        private CheckBox chkHexSend;
    }
}
