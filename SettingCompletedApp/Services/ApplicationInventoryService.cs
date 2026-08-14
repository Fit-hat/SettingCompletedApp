using Microsoft.Win32;

namespace SettingCompletedApp.Services;

public class ApplicationInventoryService
{
    public List<string> GetWin32Applications()
    {
        List<string> applications = [];

        string[] uninstallKeys =
        {
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
            @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
        };

        foreach (string keyPath in uninstallKeys)
        {
            using RegistryKey? key =
                Registry.LocalMachine.OpenSubKey(keyPath);

            if (key == null)
            {
                continue;
            }

            foreach (string subkeyName in key.GetSubKeyNames())
            {
                using RegistryKey? subkey =
                    key.OpenSubKey(subkeyName);

                string? displayName =
                    subkey?.GetValue("DisplayName")?.ToString();

                if (!string.IsNullOrWhiteSpace(displayName))
                {
                    applications.Add(displayName);
                }
            }
        }

        return applications
            .Distinct()
            .OrderBy(x => x)
            .ToList();
    }
}