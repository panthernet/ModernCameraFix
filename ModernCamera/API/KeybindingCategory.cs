using ProjectM;
using Stunlock.Localization;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace ModernCamera.API;

public class KeybindingCategory
{
    public string Name { get; }
    public Dictionary<string, Keybinding.Data> Overrides = new();

    internal LocalizationKey NameKey;
    internal InputActionMap InputActionMap;

    internal Dictionary<string, Keybinding> KeybindingMap = new();
    internal Dictionary<ButtonInputAction, string> KeybindingFlags = new();

    public KeybindingCategory(string name, string desc)
    {
        Name = name;
        InputActionMap = new InputActionMap(Name);
        NameKey = LocalizationManager.CreateKey(desc);
    }

    public Keybinding AddKeyBinding(string name, string category, string description, UnityEngine.KeyCode keyCode)
    {
        bool hasKeyBinding = false;
        if (KeybindingMap.TryGetValue(name, out Keybinding binding))
        {
            hasKeyBinding = true;
            Bloodstone.API.KeybindManager.Unregister(binding.BloodstoneKeybinding);
        }

        var bloodstoneKeybinding = Bloodstone.API.KeybindManager.Register(new()
        {
            Id = name,
            Category = category,
            Name = description,
            DefaultKeybinding = keyCode
        });

        if(hasKeyBinding)
            KeybindingMap[name].BloodstoneKeybinding = bloodstoneKeybinding;
        else
        {
            var keybinding = new Keybinding(bloodstoneKeybinding);
            KeybindingMap.Add(name, keybinding);
            KeybindingFlags.Add(keybinding.InputFlag, name);
            return keybinding;
        }

        return KeybindingMap[name];
    }

    /*public Keybinding AddKeyBinding(string name, string defaultPrimary = null, string defaultSecondary = null)
    {
        if (KeybindingMap.TryGetValue(name, out Keybinding binding))
        {
            return binding;
        }

        var keybinding = new Keybinding(InputActionMap.AddAction(name), defaultPrimary, defaultSecondary);
        if (Overrides.TryGetValue(name, out var data))
        {
            if (!string.IsNullOrEmpty(data.PrimaryOverride)) keybinding.Override(true, data.PrimaryOverride);
            if (!string.IsNullOrEmpty(data.SecondaryOverride)) keybinding.Override(false, data.SecondaryOverride);
        }
        KeybindingMap.Add(name, keybinding);
        KeybindingFlags.Add(keybinding.InputFlag, name);
        return keybinding;
    }*/

    public Keybinding GetKeybinding(string id)
    {
        return KeybindingMap.GetValueOrDefault(id);
    }

    public Keybinding GetKeybinding(ButtonInputAction flag)
    {
        var id = KeybindingFlags.GetValueOrDefault(flag);
        return id == null ? default : GetKeybinding(id);
    }

    public bool HasKeybinding(string id)
    {
        return KeybindingMap.ContainsKey(id);
    }

    public bool HasKeybinding(ButtonInputAction flag)
    {
        return KeybindingFlags.ContainsKey(flag);
    }

    public void UpdateLocalization(string desc)
    {
        NameKey = LocalizationManager.UpdateKey(NameKey, desc);
    }
}
