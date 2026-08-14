namespace SettingCompletedApp.Models;

public class NetworkAdapterInfo
{
    public string Name { get; set; } = "";

    public string Description { get; set; } = "";

    public string MacAddress { get; set; } = "";

    public List<string> IpAddresses { get; set; }
        = new();
}
