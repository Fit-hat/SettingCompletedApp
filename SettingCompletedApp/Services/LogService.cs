using System.Reflection;
using System.Text;
using SettingCompletedApp.Models;

namespace SettingCompletedApp.Services;

public class LogService
{
    private readonly SystemInfoService _systemInfoService;
    private readonly ApplicationInventoryService _applicationInventoryService;

    public LogService()
    {
        _systemInfoService = new SystemInfoService();
        _applicationInventoryService =
            new ApplicationInventoryService();
    }

    public void CreateLog()
    {
        SystemInfo info =
            _systemInfoService.GetSystemInfo();

        string logFilePath = GetLogFilePath();

        StringBuilder sb = new();

        sb.AppendLine();
        sb.AppendLine(new string('=', 80));
        sb.AppendLine($"実行日時 : {DateTime.Now:yyyy/MM/dd HH:mm:ss}");
        sb.AppendLine(new string('=', 80));
        sb.AppendLine();

        sb.AppendLine("【基本情報】");
        sb.AppendLine($"HOST名         : {info.HostName}");
        sb.AppendLine($"IPアドレス     : {info.IpAddress}");
        sb.AppendLine($"OSバージョン   : {info.OsVersion}");
        sb.AppendLine($"ユーザー名     : {info.UserName}");
        sb.AppendLine($"ドメイン名     : {info.DomainName}");
        sb.AppendLine($"ログオンユーザ : {info.LogonUser}");
        sb.AppendLine($"CPU名          : {info.CpuName}");
        sb.AppendLine($"メモリ容量     : {info.MemorySize}");

        sb.AppendLine();
        sb.AppendLine("【Win32アプリ】");

        foreach (string app
            in _applicationInventoryService.GetWin32Applications())
        {
            sb.AppendLine(app);
        }

        File.AppendAllText(
            logFilePath,
            sb.ToString(),
            Encoding.UTF8);
    }

    private string GetLogFilePath()
    {
        string exePath =
            Assembly.GetExecutingAssembly().Location;

        return Path.ChangeExtension(exePath, ".log");
    }
}