using System.Text.Json;

namespace LineMonitor.APP.Config;

public static class SettingsLoader
{
    public static AppSettings Load(string path)
    {
        if (!File.Exists(path))
            return new AppSettings();      // 没有配置文件，就全部用默认值

        string json = File.ReadAllText(path);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,                 // 不区分大小写
            ReadCommentHandling = JsonCommentHandling.Skip,     // 允许写 // 注释
            AllowTrailingCommas = true                          // 允许最后多一个逗号
        };

        return JsonSerializer.Deserialize<AppSettings>(json, options) ?? new AppSettings();
    }
}
