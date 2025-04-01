using BepInEx;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModernCamera.Utils;

#nullable enable
public static class FileUtils
{
    public static bool Exists(string filename)
    {
        return File.Exists(Path.Join(Paths.ConfigPath, PluginInfo.PLUGIN_NAME, filename));
    }

    public static void WriteJson(string filename, object? data, bool hasEnums = false)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,

            };
            if(hasEnums)
                options.Converters.Add(new JsonStringEnumConverter());
            var serialized = JsonSerializer.Serialize(data, options);
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

    public static T? ReadJson<T>(string filename, bool hasEnums = false)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,

            };
            if (hasEnums)
                options.Converters.Add(new JsonStringEnumConverter());

            var content = File.ReadAllText(Path.Join(Paths.ConfigPath, PluginInfo.PLUGIN_NAME, filename));
            var deserialized = JsonSerializer.Deserialize<T>(content, options);
            return deserialized;
        }
        catch (Exception ex)
        {
            LogUtils.LogWarning($"Error reading {filename}: {ex.Message}");
            return default;
        }
    }
}