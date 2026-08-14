using System.Drawing.Printing;

namespace SettingCompletedApp.Services;

public class PrinterService
{
    public List<string> GetPrinters()
    {
        List<string> result = new();

        foreach (string printer
                 in PrinterSettings.InstalledPrinters)
        {
            result.Add(printer);
        }

        return result;
    }
}