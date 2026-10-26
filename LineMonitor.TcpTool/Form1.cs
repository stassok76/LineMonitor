namespace LineMonitor.TcpTool

{
    using System.Text;

    public partial class Form1 : Form
    {

        private readonly TcpServer _server = new();

        public Form1()
        {
            InitializeComponent();

            _server.ClientConnected += OnClientConnected;
            _server.ClientDisconnected += OnClientDisconnected;
            _server.DataReceived += OnDataReceived;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {

        }
        private void OnClientConnected(string id)
        {
            RunOnUi(() =>
            {
                lstClients.Items.Add(id);
                AppendLog($"{id} 上线");
            });
        }

        private void OnClientDisconnected(string id, string reason)
        {
            RunOnUi(() =>
            {
                lstClients.Items.Remove(id);
                AppendLog($"{id} 下线（{reason}）");
            });
        }

        private void OnDataReceived(string id, byte[] data)
        {
            RunOnUi(() =>
            {
                string text = HexHelper.ToText(data, chkHexReceive.Checked);
                AppendLog($"收到 {id}：{text}");
            });
        }
        private void RunOnUi(Action action)
        {
            if (IsDisposed) return;
            BeginInvoke(action);
        }

        private void AppendLog(string message)
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_server.IsRunning)
            {
                _server.Stop();
                btnStart.Text = "启动";
                txtPort.Enabled = true;
                AppendLog("服务器已停止");
            }
            else
            {
                try
                {
                    _server.TimeoutSeconds = (int)numTimeout.Value;
                    _server.Start(int.Parse(txtPort.Text));
                    btnStart.Text = "停止";
                    txtPort.Enabled = false;
                    AppendLog($"服务器已启动，端口 {txtPort.Text}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("启动失败：" + ex.Message);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _server.Stop();
        }

        

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (lstClients.SelectedItem is not string id)
            {
                MessageBox.Show("请先在左边列表选中一个客户端");
                return;
            }

            try
            {
                await _server.SendAsync(id, HexHelper.ToBytes(txtSend.Text, chkHexSend.Checked));
                AppendLog($"发给 {id}：{txtSend.Text}");
            }
            catch (FormatException)
            {
                MessageBox.Show("十六进制格式不对，例如：01 03 00 00 00 0A");
            }
            catch (Exception ex)
            {
                MessageBox.Show("发送失败：" + ex.Message);
            }
        }

        private async void btnBroadcast_Click(object sender, EventArgs e)
        {
            if (lstClients.Items.Count == 0)
            {
                MessageBox.Show("当前没有在线客户端");
                return;
            }

            try
            {
                await _server.BroadcastAsync(HexHelper.ToBytes(txtSend.Text, chkHexSend.Checked));
                AppendLog($"广播给 {lstClients.Items.Count} 个客户端：{txtSend.Text}");
            }
            catch (FormatException)
            {
                MessageBox.Show("十六进制格式不对，例如：01 03 00 00 00 0A");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }

        private void btnOpenClient_Click(object sender, EventArgs e)
        {
            new ClientForm().Show();
        }
    }
}
