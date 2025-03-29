using Stunlock.Localization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ModernCamera.API;

public class OptionCategory
{
    public string Name { get; internal set; }
    public Dictionary<string, bool> Toggles = new();
    public Dictionary<string, float> Sliders = new();
    public Dictionary<string, string> Dropdowns = new();

    internal LocalizationKey LocalizationKey { get; private set; }

    internal List<string> Options = new();
    internal Dictionary<string, ToggleOption> ToggleOptions = new();
    internal Dictionary<string, SliderOption> SliderOptions = new();
    internal Dictionary<string, DropdownOption> DropdownOptions = new();
    internal Dictionary<string, string> Dividers = new();

    public OptionCategory(string name)
    {
        Name = name;
        LocalizationKey = LocalizationManager.CreateKey(name);
    }

    public void UpdateLocalizationKey(LocalizationKey key)
    {
        LocalizationKey = key;
    }

    public ToggleOption AddToggle(string name, string description, bool defaultValue)
    {
        if (ToggleOptions.TryGetValue(name, out ToggleOption addToggle))
        {
            addToggle.Description = description;
            return addToggle;
        }

        var option = new ToggleOption(name, description, defaultValue);
        if (Toggles.TryGetValue(name, out bool toggle))
            option.Value = toggle;

        ToggleOptions.Add(name, option);
        Options.Add(option.Name);

        return option;
    }

    public SliderOption AddSlider(string name, string description, float minValue, float maxValue, float defaultValue, int decimals = default, float stepValue = default)
    {
        if (SliderOptions.TryGetValue(name, out SliderOption slider))
        {
            slider.Description = description;
            return slider;
        }

        var option = new SliderOption(name, description, minValue, maxValue, defaultValue, decimals);
        if (Sliders.ContainsKey(name))
            option.Value = Mathf.Clamp(Sliders[name], minValue, maxValue);

        SliderOptions.Add(name, option);
        Options.Add(option.Name);

        return option;
    }

    public DropdownOption AddDropdown(string name, string description, int defaultValue, string[] values)
    {
        if (DropdownOptions.TryGetValue(name, out DropdownOption dropdown))
        {
            dropdown.Description = description;
            return dropdown;
        }

        var option = new DropdownOption(name, description, defaultValue, values);
        if (Dropdowns.ContainsKey(name))
            option.Value = Mathf.Max(0, Array.IndexOf(values, Dropdowns[name]));


        DropdownOptions.Add(name, option);
        Options.Add(option.Name);

        return option;
    }

    public void AddDivider(string name, string desc)
    {
        if(!Dividers.TryAdd(name, desc))
            return;
        Options.Add(name);
    }

    public ToggleOption GetToggle(string id)
    {
        return ToggleOptions.GetValueOrDefault(id);
    }

    public SliderOption GetSlider(string id)
    {
        return SliderOptions.GetValueOrDefault(id);
    }

    public DropdownOption GetDropdown(string id)
    {
        return DropdownOptions.GetValueOrDefault(id);
    }

    public bool HasToggle(string id)
    {
        return ToggleOptions.ContainsKey(id);
    }

    public bool HasSlider(string id)
    {
        return SliderOptions.ContainsKey(id);
    }

    public bool HasDropdown(string id)
    {
        return DropdownOptions.ContainsKey(id);
    }

    public bool TryGetToggle(string id, out ToggleOption option)
    {
        if (!ToggleOptions.TryGetValue(id, out ToggleOption toggleOption))
        {
            option = null;
            return false;
        }

        option = toggleOption;
        return true;
    }

    public bool TryGetSlider(string id, out SliderOption option)
    {
        if (!SliderOptions.TryGetValue(id, out SliderOption sliderOption))
        {
            option = null;
            return false;
        }

        option = sliderOption;
        return true;
    }

    public bool TryGetDropdown(string id, out DropdownOption option)
    {
        if (!DropdownOptions.TryGetValue(id, out DropdownOption dropdownOption))
        {
            option = null;
            return false;
        }

        option = dropdownOption;
        return true;
    }

    public bool TryGetDivider(string id, out string text)
    {
        if (!Dividers.TryGetValue(id, out string divider))
        {
            text = null;
            return false;
        }

        text = divider;
        return true;
    }
}