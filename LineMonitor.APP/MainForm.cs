using LineMonitor.APP.Config;
using LineMonitor.APP.Data;
using LineMonitor.APP.Models;
using LineMonitor.APP.Services;
using LineMonitor.Modbus;
using Serilog;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LineMonitor.APP;

public partial class MainForm : Form
{
    private readonly ModbusTcpMaster _master;
    private readonly PlcService _plc;
    private readonly PollingService _polling;
    private readonly AlarmService _alarm;
    private readonly Database _db;
    private readonly AlarmRepository _alarmRepo;
    private readonly HistoryRecorder _recorder;
    private readonly ScottPlot.Plottables.DataLogger _tempLogger;
    private readonly HistoryRepository _historyRepo;

    public MainForm(AppSettings settings)
    {
        InitializeComponent();
        _db = new Database(Path.Combine(AppContext.BaseDirectory, settings.DatabaseFile));
        _db.CreateTables();

        _tempLogger = plotTemp.Plot.Add.DataLogger();
        InitChart();

        _alarmRepo = new AlarmRepository(_db);


        _master = new ModbusTcpMaster(settings.Plc.Host, settings.Plc.Port)
        {
            UnitId = settings.Plc.UnitId,
            TimeoutMs = settings.Plc.TimeoutMs
        };
        _plc = new PlcService(_master);
        _polling = new PollingService(_plc)
        {
            IntervalMs = settings.PollIntervalMs
        };

        _polling.StatusUpdated += OnStatusUpdated;
        _polling.ConnectionChanged += OnConnectionChanged;
        _polling.ErrorOccurred += OnError;

        _alarm = new AlarmService(_polling);
        _alarm.AlarmChanged += OnAlarmChanged;
        _alarm.AlarmChanged += _alarmRepo.Insert;

        _historyRepo = new HistoryRepository(_db);
        _recorder = new HistoryRecorder(_polling, _historyRepo)
        {
            Interval = TimeSpan.FromSeconds(settings.HistoryIntervalSeconds)
        };
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        foreach (AlarmEvent a in _alarmRepo.GetRecent(100))
            lvAlarms.Items.Add(CreateAlarmItem(a));

        _polling.Start();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _polling.Stop();
        _master.Dispose();
        base.OnFormClosing(e);
    }
    private void OnStatusUpdated(LineStatus s)
    {
        if (IsDisposed) return;
        BeginInvoke(() => ShowStatus(s));
    }

    private void ShowStatus(LineStatus s)
    {
        lblRunning.Text = s.Running ? "运行中" : "已停止";
        lblRunning.ForeColor = s.Running ? Color.Green : Color.Gray;

        lblAlarm.Text = s.Alarm ? "超温报警！" : "正常";
        lblAlarm.ForeColor = s.Alarm ? Color.Red : Color.Green;

        lblTemperature.Text = $"{s.Temperature:F1} ℃";
        lblCount.Text = s.Count.ToString();
        lblTime.Text = s.Time.ToString("HH:mm:ss");
        lblError.Text = "";

        UpdateChart(s);
    }

    private void UpdateChart(LineStatus s)
    {
        double x = s.Time.ToOADate();
        _tempLogger.Add(x, s.Temperature);

        plotTemp.Plot.Axes.SetLimitsX(s.Time.AddMinutes(-2).ToOADate(), x);
        plotTemp.Refresh();
    }

    private void OnConnectionChanged(bool connected)
    {
        if (IsDisposed) return;
        BeginInvoke(() =>
        {
            lblConnection.Text = connected ? "● 已连接" : "● 未连接";
            lblConnection.ForeColor = connected ? Color.Green : Color.Red;
        });
    }

    private void OnError(string message)
    {
        if (IsDisposed) return;
        BeginInvoke(() => lblError.Text = $"{DateTime.Now:HH:mm:ss}  {message}");
    }

    private async void btnStart_Click(object sender, EventArgs e)
    {
        Log.Information("用户操作：启动产线");
        btnStart.Enabled = false;
        try
        {
            await _plc.StartAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "启动命令发送失败");
            MessageBox.Show($"启动失败：{ex.Message}");
        }
        finally
        {
            btnStart.Enabled = true;
        }
    }

    private async void btnStop_Click(object sender, EventArgs e)
    {
        btnStop.Enabled = false;
        try
        {
            await _plc.StopAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"停止失败：{ex.Message}");
        }
        finally
        {
            btnStop.Enabled = true;
        }
    }

    private void OnAlarmChanged(AlarmEvent a)
    {
        if (IsDisposed) return;
        BeginInvoke(() => lvAlarms.Items.Insert(0, CreateAlarmItem(a)));
    }

    private static ListViewItem CreateAlarmItem(AlarmEvent a)
    {
        var item = new ListViewItem(a.Time.ToString("yyyy-MM-dd HH:mm:ss"));
        item.SubItems.Add(a.Message);
        item.SubItems.Add($"{a.Temperature:F1} ℃");
        item.ForeColor = a.IsActive ? Color.Red : Color.Green;
        return item;
    }

    private void InitChart()
    {
        plotTemp.Plot.Font.Set("Microsoft YaHei");   // 微软雅黑，Windows 自带            // 自动选字体，让中文能正常显示
        plotTemp.Plot.Title("实时温度");
        plotTemp.Plot.YLabel("温度 (℃)");
        plotTemp.Plot.Axes.DateTimeTicksBottom();    // 横轴显示成时间

        _tempLogger.ManageAxisLimits = false;        // 坐标范围由我们自己控制
        plotTemp.Plot.Axes.SetLimitsY(0, 100);       // 纵轴固定 0~100℃

        var alarmLine = plotTemp.Plot.Add.HorizontalLine(80);
        alarmLine.Color = ScottPlot.Colors.Red;
        alarmLine.LinePattern = ScottPlot.LinePattern.Dashed;
    }

    private void btnHistory_Click(object sender, EventArgs e)
    {
        var form = new HistoryForm(_historyRepo);
        form.Show();
    }
}