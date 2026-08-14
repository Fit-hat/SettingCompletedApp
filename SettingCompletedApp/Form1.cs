using System.Drawing;
using System.Net;
using System.Net.Sockets;
using SettingCompletedApp.Services;

namespace SettingCompletedApp;

public partial class Form1 : Form
{
    private readonly LogService _logService;
    private readonly DateTime _startTime;

    private Label centerLabel = null!;
    private Label dateTimeLabel = null!;

    private Label hostNameLabel = null!;
    private Label ipAddressLabel = null!;
    private Label osVersionLabel = null!;
    private Label userNameLabel = null!;
    private Label domainNameLabel = null!;

    public Form1()
    {
        InitializeComponent();

        _startTime = DateTime.Now;

        _logService = new LogService();

        _logService.CreateLog();

        SetupForm();
        CreateControls();
    }

    private void SetupForm()
    {
        Text = "設定完了";

        Width = 800;
        Height = 500;

        StartPosition = FormStartPosition.CenterScreen;

        TopMost = true;

        FormBorderStyle = FormBorderStyle.FixedSingle;

        MaximizeBox = false;

        ShowInTaskbar = true;

        //BackColor = vividColors[0];
        BackColor = Color.FromArgb(0, 120, 215);
    }

    private void CreateControls()
    {
        centerLabel = new Label();
        centerLabel.Text = "設定完了";
        centerLabel.AutoSize = true;
        centerLabel.Font = new Font("Meiryo", 32, FontStyle.Bold);
        centerLabel.ForeColor = Color.White;
        Controls.Add(centerLabel);

        dateTimeLabel = new Label();
        dateTimeLabel.AutoSize = true;
        dateTimeLabel.Font = new Font("Meiryo", 10, FontStyle.Bold);
        dateTimeLabel.ForeColor = Color.White;
        Controls.Add(dateTimeLabel);

        hostNameLabel = new Label();
        hostNameLabel.AutoSize = true;
        hostNameLabel.Font = new Font("Meiryo", 10);
        hostNameLabel.ForeColor = Color.White;
        Controls.Add(hostNameLabel);

        ipAddressLabel = new Label();
        ipAddressLabel.AutoSize = true;
        ipAddressLabel.Font = new Font("Meiryo", 10);
        ipAddressLabel.ForeColor = Color.White;
        Controls.Add(ipAddressLabel);

        osVersionLabel = new Label();
        osVersionLabel.AutoSize = true;
        osVersionLabel.Font = new Font("Meiryo", 10);
        osVersionLabel.ForeColor = Color.White;
        Controls.Add(osVersionLabel);

        userNameLabel = new Label();
        userNameLabel.AutoSize = true;
        userNameLabel.Font = new Font("Meiryo", 10);
        userNameLabel.ForeColor = Color.White;
        Controls.Add(userNameLabel);

        domainNameLabel = new Label();
        domainNameLabel.AutoSize = true;
        domainNameLabel.Font = new Font("Meiryo", 10);
        domainNameLabel.ForeColor = Color.White;
        Controls.Add(domainNameLabel);

        dateTimeLabel.Text =
            $"実行日時 : {_startTime:yyyy/MM/dd HH:mm:ss}";

        hostNameLabel.Text =
            $"HOST : {GetHostName()}";

        ipAddressLabel.Text =
            $"IP : {GetIPv4Address()}";

        osVersionLabel.Text =
            $"OS : {Environment.OSVersion.VersionString}";

        userNameLabel.Text =
            $"USER : {Environment.UserName}";

        domainNameLabel.Text =
            $"DOMAIN : {Environment.UserDomainName}";

        Resize += Form1_Resize;

        PositionControls();
    }


    private void Form1_Resize(object? sender, EventArgs e)
    {
        PositionControls();
    }

    private void PositionControls()
    {
        centerLabel.Left =
            (ClientSize.Width - centerLabel.Width) / 2;

        centerLabel.Top =
            (ClientSize.Height - centerLabel.Height) / 2;

        dateTimeLabel.Left =
            ClientSize.Width -
            dateTimeLabel.Width - 10;

        dateTimeLabel.Top =
            ClientSize.Height -
            dateTimeLabel.Height - 10;

        hostNameLabel.Left = 10;
        hostNameLabel.Top = ClientSize.Height - 135;

        ipAddressLabel.Left = 10;
        ipAddressLabel.Top = ClientSize.Height - 110;

        osVersionLabel.Left = 10;
        osVersionLabel.Top = ClientSize.Height - 85;

        userNameLabel.Left = 10;
        userNameLabel.Top = ClientSize.Height - 60;

        domainNameLabel.Left = 10;
        domainNameLabel.Top = ClientSize.Height - 35;
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
                if (ip.AddressFamily ==
                    AddressFamily.InterNetwork)
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
}