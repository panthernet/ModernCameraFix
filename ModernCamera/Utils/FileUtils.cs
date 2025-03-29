using BepInEx;
using System;
using System.IO;
using System.Text.Json;

namespace ModernCamera.Utils;

#nullable enable
public static class FileUtils
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        IncludeFields = true
    };

    public static bool Exists(string filename)
    {
        return File.Exists(Path.Join(Paths.ConfigPath, PluginInfo.PLUGIN_NAME, filename));
    }

    public static void WriteJson(string filename, object? data)
    {
        try
        {
            var serialized = JsonSerializer.Serialize(data, jsonSerializerOptions);
            var dir = Path.Join(Paths.ConfigPath, PluginInfo.PLUGIN_NAME);
            if(!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllText(Path.Join(dir, filename), serialized);
        }
        catch (Exception ex)
        {
            LogUtils.LogWarning($"Error saving {filename}: {ex.Message}");
        }
    }

    public static T? ReadJson<T>(string filename)
    {
        try
        {
            var content = File.ReadAllText(Path.Join(Paths.ConfigPath, PluginInfo.PLUGIN_NAME, filename));
            var deserialized = JsonSerializer.Deserialize<T>(content, jsonSerializerOptions);
            return deserialized;
        }
        catch (Exception ex)
        {
            LogUtils.LogWarning($"Error reading {filename}: {ex.Message}");
            return default;
        }
    }
}