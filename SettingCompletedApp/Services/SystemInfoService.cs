using System.Management;
using System.Net;
using System.Net.Sockets;
using SettingCompletedApp.Models;

namespace SettingCompletedApp.Services;

public class SystemInfoService
{
    public SystemInfo GetSystemInfo()
    {
        return new SystemInfo
        {
            HostName = GetHostName(),
            IpAddress = GetIPv4Address(),
            OsVersion = Environment.OSVersion.VersionString,
            UserName = Environment.UserName,
            DomainName = Environment.UserDomainName,
            LogonUser = $"{Environment.UserDomainName}\\{Environment.UserName}",
            CpuName = GetCpuName(),
            MemorySize = GetTotalMemory()
        };
    }

    private string GetHostName()
    {
        return Dns.GetHostName();
    }

    private string GetIPv4Address()
    {
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
        }
        catch
        {
        }

        return "取得失敗";
    }

    private string GetCpuName()
    {
        try
        {
            using var searcher =
                new ManagementObjectSearcher(
                    "SELECT Name FROM Win32_Processor");

            foreach (ManagementObject obj in searcher.Get())
            {
                return obj["Name"]?.ToString() ?? "";
            }
        }
        catch
        {
        }

        return "取得失敗";
    }

    private string GetTotalMemory()
    {
        try
        {
            using var searcher =
                new ManagementObjectSearcher(
                    "SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");

            foreach (ManagementObject obj in searcher.Get())
            {
                ulong memory =
                    Convert.ToUInt64(obj["TotalPhysicalMemory"]);

                return $"{memory / 1024 / 1024 / 1024:N0} GB";
            }
        }
        catch
        {
        }

        return "取得失敗";
    }
}