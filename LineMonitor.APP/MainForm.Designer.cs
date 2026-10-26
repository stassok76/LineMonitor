namespace LineMonitor.APP
{
    partial class MainForm
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
            lblConnection = new Label();
            lblRunning = new Label();
            lblAlarm = new Label();
            lblTemperature = new Label();
            lblCount = new Label();
            lblTime = new Label();
            lblError = new Label();
            btnStart = new Button();
            btnStop = new Button();
            lvAlarms = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            plotTemp = new ScottPlot.WinForms.FormsPlot();
            btnHistory = new Button();
            SuspendLayout();
            // 
            // lblConnection
            // 
            lblConnection.AutoSize = true;
            lblConnection.ForeColor = Color.Red;
            lblConnection.Location = new Point(12, 31);
            lblConnection.Name = "lblConnection";
            lblConnection.Size = new Size(109, 31);
            lblConnection.TabIndex = 0;
            lblConnection.Text = "● 未连接";
            // 
            // lblRunning
            // 
            lblRunning.AutoSize = true;
            lblRunning.Location = new Point(82, 137);
            lblRunning.Name = "lblRunning";
            lblRunning.Size = new Size(34, 31);
            lblRunning.TabIndex = 1;
            lblRunning.Text = "--";
            // 
            // lblAlarm
            // 
            lblAlarm.AutoSize = true;
            lblAlarm.Location = new Point(70, 207);
            lblAlarm.Name = "lblAlarm";
            lblAlarm.Size = new Size(34, 31);
            lblAlarm.TabIndex = 2;
            lblAlarm.Text = "--";
            // 
            // lblTemperature
            // 
            lblTemperature.AutoSize = true;
            lblTemperature.Location = new Point(128, 284);
            lblTemperature.Name = "lblTemperature";
            lblTemperature.Size = new Size(34, 31);
            lblTemperature.TabIndex = 3;
            lblTemperature.Text = "--";
            // 
            // lblCount
            // 
            lblCount.AutoSize = true;
            lblCount.Location = new Point(254, 137);
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(34, 31);
            lblCount.TabIndex = 4;
            lblCount.Text = "--";
            // 
            // lblTime
            // 
            lblTime.AutoSize = true;
            lblTime.Location = new Point(267, 216);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(34, 31);
            lblTime.TabIndex = 5;
            lblTime.Text = "--";
            // 
            // lblError
            // 
            lblError.AutoSize = true;
            lblError.Location = new Point(392, 331);
            lblError.Name = "lblError";
            lblError.Size = new Size(62, 31);
            lblError.TabIndex = 6;
            lblError.Text = "清空";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(610, 178);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(150, 46);
            btnStart.TabIndex = 7;
            btnStart.Text = "启动";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(625, 284);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(150, 46);
            btnStop.TabIndex = 8;
            btnStop.Text = "停止";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // lvAlarms
            // 
            lvAlarms.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            lvAlarms.FullRowSelect = true;
            lvAlarms.Location = new Point(350, 12);
            lvAlarms.Name = "lvAlarms";
            lvAlarms.Size = new Size(254, 194);
            lvAlarms.TabIndex = 9;
            lvAlarms.UseCompatibleStateImageBehavior = false;
            lvAlarms.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "时间";
            columnHeader1.Width = 160;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "事件";
            columnHeader2.Width = 260;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "温度";
            columnHeader3.Width = 80;
            // 
            // plotTemp
            // 
            plotTemp.Location = new Point(518, 374);
            plotTemp.Name = "plotTemp";
            plotTemp.Size = new Size(620, 343);
            plotTemp.TabIndex = 10;
            // 
            // btnHistory
            // 
            btnHistory.Location = new Point(930, 117);
            btnHistory.Name = "btnHistory";
            btnHistory.Size = new Size(150, 46);
            btnHistory.TabIndex = 11;
            btnHistory.Text = "历史查询";
            btnHistory.UseVisualStyleBackColor = true;
            btnHistory.Click += btnHistory_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1174, 729);
            Controls.Add(btnHistory);
            Controls.Add(plotTemp);
            Controls.Add(lvAlarms);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(lblError);
            Controls.Add(lblTime);
            Controls.Add(lblCount);
            Controls.Add(lblTemperature);
            Controls.Add(lblAlarm);
            Controls.Add(lblRunning);
            Controls.Add(lblConnection);
            Name = "MainForm";
            Text = "LineMonitor 产线监控";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblConnection;
        private Label lblRunning;
        private Label lblAlarm;
        private Label lblTemperature;
        private Label lblCount;
        private Label lblTime;
        private Label lblError;
        private Button btnStart;
        private Button btnStop;
        private ListView lvAlarms;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ScottPlot.WinForms.FormsPlot plotTemp;
        private Button btnHistory;
    }
}
