using System.Reflection;
using System.Text;
using SettingCompletedApp.Models;

namespace SettingCompletedApp.Services;

public class LogService
{
    private readonly SystemInfoService _systemInfoService;
    private readonly ApplicationInventoryService _applicationInventoryService;

    private readonly BrowserExtensionService _browserExtensionService;

    private readonly WindowsUpdateService  _windowsUpdateService;

    private readonly PrinterService  _printerService;

    public LogService()
    {
        _systemInfoService = new SystemInfoService();
        
        _applicationInventoryService =
            new ApplicationInventoryService();

        _browserExtensionService =
            new BrowserExtensionService();

        _windowsUpdateService =
            new WindowsUpdateService();

        _printerService =
            new PrinterService();
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
        sb.AppendLine($"製造元         : {info.Manufacturer}");
        sb.AppendLine($"モデル名       : {info.ModelName}");

        sb.AppendLine();
        sb.AppendLine("【BitLocker】");
        sb.AppendLine(info.BitLockerStatus);

        sb.AppendLine();
        sb.AppendLine("【ネットワーク】");

        foreach (var nic in info.NetworkAdapters)
        {
            sb.AppendLine();

            sb.AppendLine($"名前 : {nic.Name}");
            sb.AppendLine($"説明 : {nic.Description}");
            sb.AppendLine($"MAC  : {nic.MacAddress}");

            foreach (string ip in nic.IpAddresses)
            {
                sb.AppendLine($"IP   : {ip}");
            }
        }

        sb.AppendLine();
        sb.AppendLine("【Microsoft Edge拡張】");

        foreach (var ext
                 in _browserExtensionService
                 .GetEdgeExtensions())
        {
            sb.AppendLine($"ID      : {ext.Id}");
            sb.AppendLine($"名称    : {ext.Name}");
            sb.AppendLine($"Version : {ext.Version}");
            sb.AppendLine();
        }

        sb.AppendLine();
        sb.AppendLine("【Google Chrome拡張】");

        foreach (var ext
                 in _browserExtensionService
                 .GetChromeExtensions())
        {
            sb.AppendLine($"ID      : {ext.Id}");
            sb.AppendLine($"名称    : {ext.Name}");
            sb.AppendLine($"Version : {ext.Version}");
            sb.AppendLine();
        }
        
        sb.AppendLine("【Windows Update履歴】");

        foreach (var update
                 in _windowsUpdateService
                 .GetInstalledUpdates())
        {
            sb.AppendLine(update);
        }

        sb.AppendLine();
        sb.AppendLine("【プリンター一覧】");

        foreach (var printer
                 in _printerService.GetPrinters())
        {
            sb.AppendLine(printer);
        }

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
            Application.ExecutablePath;

        return Path.ChangeExtension(exePath, ".log");
    }
    private string TranslateBitLockerStatus(string value)
    {
        return value switch
        {
            "FullyEncrypted" => "暗号化済み",
            "FullyDecrypted" => "未暗号化",
            "EncryptionInProgress" => "暗号化中",
            "DecryptionInProgress" => "復号中",
            _ => value
        };
    }

    private string TranslateProtectionStatus(string value)
    {
        return value switch
        {
            "On" => "有効",
            "Off" => "無効",
            _ => value
        };
    }
}