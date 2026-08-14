using SettingCompletedApp.Services;
using System.Windows.Forms;

namespace SettingCompletedApp;

internal static class Program
{
    private const string LogOnlyArgument = "/logonly";

    [STAThread]
    static void Main(string[] args)
    {
        try
        {
            if (args.Contains(LogOnlyArgument,
                  StringComparer.OrdinalIgnoreCase))
            {
                new LogService().CreateLog();
                return;
            }

            ApplicationConfiguration.Initialize();

            Application.Run(new Form1());
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.ToString(),
                "エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}