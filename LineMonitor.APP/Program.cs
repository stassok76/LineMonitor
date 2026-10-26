using LineMonitor.APP.Config;
using Serilog;

namespace LineMonitor.APP;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(
                Path.Combine(AppContext.BaseDirectory, "logs", "log-.txt"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

        Application.ThreadException += (s, e) =>
        {
            Log.Error(e.Exception, "界面线程未处理的异常");
            MessageBox.Show($"程序出错：{e.Exception.Message}\n详细信息已记录到日志。", "错误");
        };

        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            Log.Fatal(e.ExceptionObject as Exception, "未处理的异常，程序即将退出");
            Log.CloseAndFlush();
        };

        TaskScheduler.UnobservedTaskException += (s, e) =>
        {
            Log.Error(e.Exception, "后台任务未处理的异常");
            e.SetObserved();
        };

        try
        {
            Log.Information("程序启动");

            AppSettings settings;
            try
            {
                settings = SettingsLoader.Load(Path.Combine(AppContext.BaseDirectory, "appsettings.json"));
                Log.Information("配置已加载：PLC {Host}:{Port}，轮询间隔 {Interval} ms",
                    settings.Plc.Host, settings.Plc.Port, settings.PollIntervalMs);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "配置文件读取失败");
                MessageBox.Show($"配置文件读取失败：{ex.Message}", "启动失败");
                return;
            }

            Application.Run(new MainForm(settings));
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "程序异常退出");
            throw;
        }
        finally
        {
            Log.Information("程序退出");
            Log.CloseAndFlush();
        }
    }
}