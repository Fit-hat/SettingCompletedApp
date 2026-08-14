using System.Management;

namespace SettingCompletedApp.Services;

public class WindowsUpdateService
{
    public List<string> GetInstalledUpdates()
    {
        List<string> result = new();

        try
        {
            using var searcher =
                new ManagementObjectSearcher(
                    "SELECT * FROM Win32_QuickFixEngineering");

            foreach (ManagementObject obj in searcher.Get())
            {
                result.Add(
                    $"{obj["InstalledOn"]}  {obj["HotFixID"]}");
            }
        }
        catch
        {
        }

        return result;
    }
}