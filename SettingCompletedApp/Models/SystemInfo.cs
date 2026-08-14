namespace SettingCompletedApp.Models;

public class SystemInfo
{
    public string HostName { get; set; } = "";

    public string IpAddress { get; set; } = "";

    public string OsVersion { get; set; } = "";

    public string UserName { get; set; } = "";

    public string DomainName { get; set; } = "";

    public string LogonUser { get; set; } = "";

    public string CpuName { get; set; } = "";

    public string MemorySize { get; set; } = "";

    public string Manufacturer { get; set; } = "";

    public string ModelName { get; set; } = "";

    public List<NetworkAdapterInfo> NetworkAdapters { get; set; }
    = new();

    public string BitLockerStatus { get; set; } = "";
}