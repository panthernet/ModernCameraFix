using ModernCamera.Utils;
using ProjectM;
using System.Collections.Generic;
using UnityEngine;

namespace ModernCamera.API;

/// <summary>
/// Primary and secondary keybinds use InputControlPaths, see Unity docs for more info:
/// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/api/UnityEngine.InputSystem.InputControlPath.html
/// </summary>
public static class KeybindingsManager
{
    internal static Dictionary<string, KeybindingCategory> Categories = new();
    internal static string ActionsFilename = "actions.json";
    internal static string KeyBindsFilename = "keybinds.json";


    internal class KeyCodeStorage
    {
        public KeyCode EnableKeyCode { get; set; } = KeyCode.Home;
        public KeyCode ActionModeKeyCode { get; set; } = KeyCode.PageUp;
        public KeyCode HideUIKeyCode { get; set; } = KeyCode.Slash;
    }

    internal static KeyCodeStorage KeyCodeStorageData { get; set; } = new();

    public static KeybindingCategory AddCategory(string name, string desc)
    {
        if (!Categories.ContainsKey(name))
        {
            var category = new KeybindingCategory(name, desc);
            Categories.Add(name, category);
        }

        Categories[name].UpdateLocalization(desc);
        return Categories[name];
    }

    public static void UpdateState(InputState inputState)
    {
        if (!inputState.InputsPressed.IsCreated || inputState.InputsPressed.IsEmpty)
        {
            LogUtils.LogInfo("No inputs pressed");
            return;
        }

        foreach (var category in Categories.Values)
        {
            LogUtils.LogInfo($"Checking category: {category.Name}");
            foreach (var keybinding in category.KeybindingMap.Values)
            {
                LogUtils.LogInfo($"Checking keybinding: {keybinding.Description}");
                if (keybinding.IsPressed)
                {
                    LogUtils.LogInfo($"Keybinding {keybinding.Description} is pressed");
                    keybinding.OnKeyPressed();
                }
            }
        }
    }

    /*public static Keybinding AddKeybinding(string category, string name, string desc, string defaultPrimary = null, string defaultSecondary = null)
    {
        return AddCategory(category).AddKeyBinding(name, defaultPrimary, defaultSecondary);
    }*/

    public static Keybinding GetKeybinding(string id)
    {
        foreach (var category in Categories.Values)
        {
            if (category.HasKeybinding(id))
            {
                return category.GetKeybinding(id);
            }
        }
        return default;
    }

    public static Keybinding GetKeybinding(ButtonInputAction flag)
    {
        foreach (var category in Categories.Values)
        {
            if (category.HasKeybinding(flag))
            {
                return category.GetKeybinding(flag);
            }
        }
        return default;
    }

    public static void Save()
    {
        FileUtils.WriteJson(ActionsFilename, Categories);
    }

    public static void FullSave()
    {
        List<string> removeCategories = new();
        foreach (var category in Categories.Values)
        {
            category.Overrides.Clear();
            foreach (var keybinding in category.KeybindingMap.Values)
            {
                category.Overrides.Add(keybinding.Name, keybinding.GetData());
            }

            if (category.Overrides.Count == 0)
            {
                removeCategories.Add(category.Name);
            }
        }

        foreach (var name in removeCategories)
        {
            Categories.Remove(name);
        }

        Save();
    }

    internal static void Load()
    {
        if (!FileUtils.Exists(ActionsFilename))
        {
            return;
        }

        var categories = FileUtils.ReadJson<Dictionary<string, KeybindingCategory>>(ActionsFilename);

        if (categories != null)
        {
            Categories = categories;
        }
    }

    public static void Clear()
    {
       // Categories.Clear();
    }

    public static void LoadKeyBinds()
    {
        if (!FileUtils.Exists(KeyBindsFilename))
        {
            FileUtils.WriteJson(KeyBindsFilename, KeyCodeStorageData, true);

            return;
        }

        KeyCodeStorageData = FileUtils.ReadJson<KeyCodeStorage>(KeyBindsFilename, true);
    }
}