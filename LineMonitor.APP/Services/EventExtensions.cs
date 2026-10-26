using Serilog;

namespace LineMonitor.APP.Services;

public static class EventExtensions
{
    public static void SafeInvoke<T>(this Action<T>? handler, T arg)
    {
        if (handler == null) return;

        foreach (Action<T> subscriber in handler.GetInvocationList())
        {
            try
            {
                subscriber(arg);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "事件订阅者 {Method} 出错", subscriber.Method.Name);
            }
        }
    }
}