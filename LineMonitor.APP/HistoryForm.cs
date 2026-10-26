using LineMonitor.APP.Data;
using LineMonitor.APP.Models;

namespace LineMonitor.APP;

public partial class HistoryForm : Form
{
    private readonly HistoryRepository _repo;

    public HistoryForm(HistoryRepository repo)
    {
        InitializeComponent();
        _repo = repo;

        dtpTo.Value = DateTime.Now;
        dtpFrom.Value = DateTime.Now.AddHours(-1);    // 默认查最近 1 小时

        plotHistory.Plot.Axes.DateTimeTicksBottom();
    }

    private void btnQuery_Click(object sender, EventArgs e)
    {
        DateTime from = dtpFrom.Value;
        DateTime to = dtpTo.Value;
        if (from >= to)
        {
            MessageBox.Show("开始时间必须早于结束时间");
            return;
        }

        List<LineStatus> data = _repo.GetRange(from, to);

        plotHistory.Plot.Clear();
        if (data.Count == 0)
        {
            lblInfo.Text = "这段时间没有数据";
            plotHistory.Refresh();
            return;
        }

        double[] xs = data.Select(d => d.Time.ToOADate()).ToArray();
        double[] ys = data.Select(d => d.Temperature).ToArray();

        var line = plotHistory.Plot.Add.Scatter(xs, ys);
        line.MarkerSize = 0;                 // 只画线，不画点

        plotHistory.Plot.Axes.AutoScale();
        plotHistory.Refresh();

        lblInfo.Text = $"共 {data.Count} 条　最高 {ys.Max():F1} ℃　最低 {ys.Min():F1} ℃";
    }
}