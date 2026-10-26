namespace LineMonitor.APP
{
    partial class HistoryForm
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
            label1 = new Label();
            dtpFrom = new DateTimePicker();
            label2 = new Label();
            dtpTo = new DateTimePicker();
            btnQuery = new Button();
            lblInfo = new Label();
            plotHistory = new ScottPlot.WinForms.FormsPlot();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(58, 63);
            label1.Name = "label1";
            label1.Size = new Size(86, 31);
            label1.TabIndex = 0;
            label1.Text = "开始：";
            // 
            // dtpFrom
            // 
            dtpFrom.CustomFormat = "yyyy-MM-dd HH:mm";
            dtpFrom.Format = DateTimePickerFormat.Custom;
            dtpFrom.Location = new Point(215, 63);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(400, 38);
            dtpFrom.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(43, 226);
            label2.Name = "label2";
            label2.Size = new Size(86, 31);
            label2.TabIndex = 2;
            label2.Text = "结束：";
            // 
            // dtpTo
            // 
            dtpTo.CustomFormat = "yyyy-MM-dd HH:mm";
            dtpTo.Format = DateTimePickerFormat.Custom;
            dtpTo.Location = new Point(215, 206);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(400, 38);
            dtpTo.TabIndex = 3;
            // 
            // btnQuery
            // 
            btnQuery.Location = new Point(820, 92);
            btnQuery.Name = "btnQuery";
            btnQuery.Size = new Size(150, 46);
            btnQuery.TabIndex = 4;
            btnQuery.Text = "查询";
            btnQuery.UseVisualStyleBackColor = true;
            btnQuery.Click += btnQuery_Click;
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Location = new Point(972, 206);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(62, 31);
            lblInfo.TabIndex = 5;
            lblInfo.Text = "清空";
            // 
            // plotHistory
            // 
            plotHistory.Location = new Point(76, 348);
            plotHistory.Name = "plotHistory";
            plotHistory.Size = new Size(1001, 354);
            plotHistory.TabIndex = 6;
            // 
            // HistoryForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1174, 729);
            Controls.Add(plotHistory);
            Controls.Add(lblInfo);
            Controls.Add(btnQuery);
            Controls.Add(dtpTo);
            Controls.Add(label2);
            Controls.Add(dtpFrom);
            Controls.Add(label1);
            Name = "HistoryForm";
            Text = "HistoryForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private DateTimePicker dtpFrom;
        private Label label2;
        private DateTimePicker dtpTo;
        private Button btnQuery;
        private Label lblInfo;
        private ScottPlot.WinForms.FormsPlot plotHistory;
    }
}