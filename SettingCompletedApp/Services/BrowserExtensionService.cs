using SettingCompletedApp.Models;
using System.Text.Json;


namespace SettingCompletedApp.Services;

public class BrowserExtensionService
{
    public List<BrowserExtensionInfo> GetEdgeExtensions()
    {
        string path =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                @"Microsoft\Edge\User Data\Default\Extensions");

        return GetExtensions(path);
    }

    public List<BrowserExtensionInfo> GetChromeExtensions()
    {
        string path =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                @"Google\Chrome\User Data\Default\Extensions");

        return GetExtensions(path);
    }

    private List<BrowserExtensionInfo> GetExtensions(string rootPath)
    {
        List<BrowserExtensionInfo> result = new();

        if (!Directory.Exists(rootPath))
        {
            return result;
        }

        foreach (string extDir in Directory.GetDirectories(rootPath))
        {
            string extensionId =
                Path.GetFileName(extDir);

            foreach (string versionDir in Directory.GetDirectories(extDir))
            {
                string manifestPath =
                    Path.Combine(versionDir, "manifest.json");

                if (!File.Exists(manifestPath))
                {
                    continue;
                }

                try
                {
                    string json =
                        File.ReadAllText(manifestPath);

                    using JsonDocument doc =
                        JsonDocument.Parse(json);

                    string name = "";
                    string version = "";

                    if (doc.RootElement.TryGetProperty(
                        "name",
                        out JsonElement nameElement))
                    {
                        name = nameElement.GetString() ?? "";
                    }

                    if (doc.RootElement.TryGetProperty(
                        "version",
                        out JsonElement versionElement))
                    {
                        version =
                            versionElement.GetString() ?? "";
                    }

                    name =
                        ResolveMessageName(
                            name,
                            versionDir);

                    result.Add(
                        new BrowserExtensionInfo
                        {
                            Id = extensionId,
                            Name = name,
                            Version = version
                        });
                }
                catch
                {
                }
            }
        }

        return result
            .OrderBy(x => x.Name)
            .ToList();
    }
    private string ResolveMessageName(
    string manifestName,
    string versionPath)
    {
        if (!manifestName.StartsWith("__MSG_"))
        {
            return manifestName;
        }

        string key =
            manifestName
                .Replace("__MSG_", "")
                .Replace("__", "");

        string localeFile =
            Path.Combine(
                versionPath,
                "_locales",
                "ja",
                "messages.json");

        if (!File.Exists(localeFile))
        {
            localeFile =
                Path.Combine(
                    versionPath,
                    "_locales",
                    "en",
                    "messages.json");
        }

        if (!File.Exists(localeFile))
        {
            return manifestName;
        }

        try
        {
            string json =
                File.ReadAllText(localeFile);

            using JsonDocument doc =
                JsonDocument.Parse(json);

            if (doc.RootElement.TryGetProperty(
                key,
                out JsonElement element))
            {
                if (element.TryGetProperty(
                    "message",
                    out JsonElement message))
                {
                    return message.GetString()
                        ?? manifestName;
                }
            }
        }
        catch
        {
        }

        return manifestName;
    }
}