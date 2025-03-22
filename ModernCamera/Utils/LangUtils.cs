using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ModernCamera.Utils
{
    internal static class LangUtils
    {
	    private static readonly string PLUGINS_PATH = BepInEx.Paths.PluginPath;
	    private static readonly string CONFIG_PATH = Path.Combine(BepInEx.Paths.ConfigPath, "ModernCamera");
        private static Dictionary<string, string> Language = new();
        private static readonly string GAME_SETTINGS_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "LocalLow","Stunlock Studios\\VRising\\Settings\\v3");

        public static void LoadLanguage(string language)
        {
            LogUtils.LogInfo($"Loading {language}...");
            var name = Path.Combine(CONFIG_PATH, $"ModernCamera.{language}.json");
            var defaultName = Path.Combine(CONFIG_PATH, $"ModernCamera.en.json");
            if (!File.Exists(name))
            {
                LogUtils.LogInfo($"Language {language} not found! Using default...");
                if (!File.Exists(defaultName))
                    return;
                Language = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(defaultName));
                return;
            }

            Language = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(name));
            LogUtils.LogInfo($"Language {language} is loaded");
        }

        public static string Get(string key)
        {
            return Language.GetValueOrDefault(key, key);
        }
    }
}
