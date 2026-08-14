using System.Management;
using System.Net;
using System.Net.Sockets;
using SettingCompletedApp.Models;
using System.Net.NetworkInformation;
using System.Diagnostics;
using SettingCompletedApp.Models;
using System.Text.Json;

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
            MemorySize = GetTotalMemory(),
            Manufacturer = GetManufacturer(),
            ModelName = GetModelName(),
            BitLockerStatus = GetBitLockerStatus(),
            NetworkAdapters = GetNetworkAdapters()
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

    private string GetManufacturer()
    {
        try
        {
            using var searcher =
                new ManagementObjectSearcher(
                    "SELECT Manufacturer FROM Win32_ComputerSystem");

            foreach (ManagementObject obj in searcher.Get())
            {
                return obj["Manufacturer"]?.ToString() ?? "";
            }
        }
        catch { }

        return "取得失敗";
    }

    private string GetModelName()
    {
        try
        {
            using var searcher =
                new ManagementObjectSearcher(
                    "SELECT Model FROM Win32_ComputerSystem");

            foreach (ManagementObject obj in searcher.Get())
            {
                return obj["Model"]?.ToString() ?? "";
            }
        }
        catch { }

        return "取得失敗";
    }

    private List<NetworkAdapterInfo> GetNetworkAdapters()
    {
        List<NetworkAdapterInfo> adapters = new();

        foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
        {
            var info = new NetworkAdapterInfo
            {
                Name = nic.Name,
                Description = nic.Description,
                MacAddress = nic.GetPhysicalAddress().ToString()
            };

            foreach (UnicastIPAddressInformation ip
                     in nic.GetIPProperties().UnicastAddresses)
            {
                info.IpAddresses.Add(ip.Address.ToString());
            }

            adapters.Add(info);
        }

        return adapters;
    }

    private string GetBitLockerStatus()
    {
        try
        {
            ProcessStartInfo psi = new()
            {
                FileName = "powershell.exe",

                Arguments =
                    "-NoProfile " +
                    "\"Get-BitLockerVolume | " +
                    "Select MountPoint,VolumeStatus,ProtectionStatus | " +
                    "Format-Table -HideTableHeaders\"",

                RedirectStandardOutput = true,
                RedirectStandardError = true,

                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process process =
                Process.Start(psi)!;

            string output =
                process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            return output.Trim();
        }
        catch (Exception ex)
        {
            return $"取得失敗 : {ex.Message}";
        }
    }
}