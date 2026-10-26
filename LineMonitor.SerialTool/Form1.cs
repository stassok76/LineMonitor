using System.IO.Ports;
using System.Text;

namespace LineMonitor.SerialTool
{
    public partial class Form1 : Form
    {
        // 串口对象：整个窗口只用这一个
        private readonly SerialPort _port = new SerialPort();

        public Form1()
        {
            InitializeComponent();

            // 1. 端口列表：读取电脑上所有的 COM 口
            cboPort.Items.AddRange(SerialPort.GetPortNames());
            if (cboPort.Items.Count > 0)
            {
                cboPort.SelectedIndex = 0;
            }

            // 2. 常用波特率
            cboBaud.Items.AddRange(new object[] { "9600", "19200", "38400", "57600", "115200" });
            cboBaud.SelectedItem = "9600";

            // 3. 校验位：直接用 SerialPort 自带的枚举
            cboParity.Items.AddRange(new object[] { Parity.None, Parity.Odd, Parity.Even });
            cboParity.SelectedIndex = 0;

            // 4. 订阅「收到数据」事件
            _port.DataReceived += Port_DataReceived;
        }

        // 打开 / 关闭串口
        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (_port.IsOpen)
                {
                    _port.Close();
                    btnOpen.Text = "打开串口";
                }
                else
                {
                    _port.PortName = cboPort.Text;
                    _port.BaudRate = int.Parse(cboBaud.Text);
                    _port.Parity = (Parity)cboParity.SelectedItem!;
                    _port.DataBits = 8;
                    _port.StopBits = StopBits.One;

                    _port.Open();
                    btnOpen.Text = "关闭串口";
                }

                // 串口打开后，不允许再修改参数
                cboPort.Enabled = !_port.IsOpen;
                cboBaud.Enabled = !_port.IsOpen;
                cboParity.Enabled = !_port.IsOpen;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败：{ex.Message}");
            }
        }

        // 发送
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!_port.IsOpen)
            {
                MessageBox.Show("请先打开串口");
                return;
            }

            byte[] data;
            try
            {
                data = chkHexSend.Checked
                    ? HexStringToBytes(txtSend.Text)            // 勾选了：按十六进制解析
                    : Encoding.UTF8.GetBytes(txtSend.Text);     // 没勾选：按文字编码
            }
            catch (Exception)
            {
                MessageBox.Show("十六进制格式不正确，请用空格分隔每个字节，例如：01 03 00 00 00 01");
                return;
            }

            _port.Write(data, 0, data.Length);
        }

        // 把字节数组转成十六进制字符串，例如 {1, 3, 132} → "01 03 84 "
        private static string ToHexString(byte[] data)
        {
            var sb = new StringBuilder();

            foreach (byte b in data)
            {
                sb.Append(b.ToString("X2"));   // 一个字节转成两位十六进制
                sb.Append(' ');                // 每个字节后面加一个空格
            }

            return sb.ToString();
        }

        // 把十六进制字符串转成字节数组，例如 "01 03 84" → {1, 3, 132}
        private static byte[] HexStringToBytes(string hex)
        {
            // 按空格切开，得到 ["01", "03", "84"]
            string[] parts = hex.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            byte[] result = new byte[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                result[i] = Convert.ToByte(parts[i], 16);   // 按十六进制解析成一个字节
            }
            return result;
        }

        // 收到数据（注意：这个方法在后台线程上执行！）
        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int count = _port.BytesToRead;           // 缓冲区里有多少个字节
            byte[] buffer = new byte[count];
            _port.Read(buffer, 0, count);            // 全部读出来

            // UI 线程：判断显示方式，并显示
            BeginInvoke(() =>
            {
                string text = chkHexReceive.Checked
                    ? ToHexString(buffer)                  // 勾选了：显示十六进制
                    : Encoding.UTF8.GetString(buffer);     // 没勾选：显示文字

                txtReceive.AppendText(text);
            });
        }

        // 关闭窗口时，记得关闭串口
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_port.IsOpen)
            {
                _port.Close();
            }
            base.OnFormClosing(e);
        }
    }
}