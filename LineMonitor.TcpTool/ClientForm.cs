using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineMonitor.TcpTool
{
    public partial class ClientForm : Form
    {

        private readonly TcpClientService _client = new();

        public ClientForm()
        {
            InitializeComponent();

            _client.DataReceived += OnDataReceived;
            _client.Disconnected += OnDisconnected;
            _client.Reconnecting += OnReconnecting;
            _client.Reconnected += OnReconnected;
        }

        private void OnDataReceived(byte[] data)
        {
            RunOnUi(() =>
            {
                string text = HexHelper.ToText(data, chkHexReceive.Checked);
                AppendLog($"收到：{text}");
            });
        }

        private void OnDisconnected(string reason)
        {
            RunOnUi(() =>
            {
                AppendLog($"已断开（{reason}）");
                if (!_client.IsActive) SetActiveUi(false);
            });
        }
        private void OnReconnecting(int attempt)
        {
            RunOnUi(() => AppendLog($"{_client.ReconnectSeconds} 秒后第 {attempt} 次尝试重连……"));
        }

        private void OnReconnected()
        {
            RunOnUi(() => AppendLog("重连成功"));
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

        
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (_client.IsActive)
            {
                _client.Disconnect();
                SetActiveUi(false);
                AppendLog("已停止连接");
                return;
            }

            btnConnect.Enabled = false;
            AppendLog($"正在连接 {txtHost.Text}:{txtPort.Text} ...");
            try
            {
                _client.AutoReconnect = chkAutoReconnect.Checked;
                await _client.ConnectAsync(txtHost.Text, int.Parse(txtPort.Text));
                AppendLog("连接成功");
                SetActiveUi(true);
            }
            catch (Exception ex)
            {
                AppendLog("连接失败：" + ex.Message);
            }
            finally
            {
                btnConnect.Enabled = true;
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                await _client.SendAsync(HexHelper.ToBytes(txtSend.Text, chkHexSend.Checked));
                AppendLog($"发送：{txtSend.Text}");
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _client.Disconnect();
        }
        private void SetActiveUi(bool active)
        {
            btnConnect.Text = active ? "断开" : "连接";
            txtHost.Enabled = !active;
            txtPort.Enabled = !active;
            chkAutoReconnect.Enabled = !active;
        }

    }
}
