using BepInEx;
using System.Collections.Generic;
using ModernCamera.Utils;
using System.IO;
using Epic.OnlineServices.RTC;
using System;
using UnityEngine;
using static UnityEngine.UIElements.StylePropertyAnimationSystem;

namespace ModernCamera.API;

public static class OptionsManager
{
    internal static Dictionary<string, OptionCategory> Categories = new();
    private static readonly string OptionsFilename;

    static OptionsManager()
    {
        OptionsFilename = "options.json";
    }

    public static OptionCategory AddCategory(string name)
    {
        if (!Categories.ContainsKey(name))
            Categories.Add(name, new OptionCategory(name));

        return Categories[name];
    }

    public static ToggleOption GetToggle(string id)
    {
        foreach (var category in Categories.Values)
        {
            if (category.HasToggle(id))
                return category.GetToggle(id);
        }
        return default;
    }

    public static SliderOption GetSlider(string id)
    {
        foreach (var category in Categories.Values)
        {
            if (category.HasSlider(id))
                return category.GetSlider(id);
        }
        return default;
    }

    public static DropdownOption GetDropdown(string id)
    {
        foreach (var category in Categories.Values)
        {
            if (category.HasDropdown(id))
                return category.GetDropdown(id);
        }
        return default;
    }

    public static void Save()
    {
        FileUtils.WriteJson(OptionsFilename, Categories);
    }

    internal static void FullSave()
    {
        List<string> removeCategories = new();
        foreach (var category in Categories.Values)
        {
            category.Toggles.Clear();
            category.Sliders.Clear();
            category.Dropdowns.Clear();

            foreach (var toggle in category.ToggleOptions.Values)
                category.Toggles.Add(toggle.Name, toggle.Value);
            foreach (var slider in category.SliderOptions.Values)
                category.Sliders.Add(slider.Name, slider.Value);
            foreach (var dropdown in category.DropdownOptions.Values)
                category.Dropdowns.Add(dropdown.Name, dropdown.Values[dropdown.Value]);

            if (category.Toggles.Count + category.Sliders.Count + category.Dropdowns.Count == 0)
                removeCategories.Add(category.Name);
        }

        foreach (var name in removeCategories)
            Categories.Remove(name);

        Save();
    }

    internal static void Load()
    {
        if (!FileUtils.Exists(OptionsFilename))
        {
            Save();
            return;
        }

        var categories = FileUtils.ReadJson<Dictionary<string, OptionCategory>>(OptionsFilename);
        if (categories != null)
        {
            foreach (var category in Categories)
            {
                if(!categories.TryGetValue(category.Key, out var from))
                    continue;

                category.Value.Toggles = from.Toggles;
                foreach (KeyValuePair<string, bool> pair in category.Value.Toggles)
                {
                    if(category.Value.TryGetToggle(pair.Key, out var toggleOption))
                        toggleOption.SetValue(pair.Value);
                }
                category.Value.Dropdowns = from.Dropdowns;
                foreach (var pair in category.Value.Dropdowns)
                {
                    if (category.Value.TryGetDropdown(pair.Key, out var ddOption))
                        ddOption.SetValue(Mathf.Max(0, ddOption.Values.IndexOf(pair.Value)));
                }
                category.Value.Sliders = from.Sliders;
                foreach (var pair in category.Value.Sliders)
                {
                    if (category.Value.TryGetSlider(pair.Key, out var sOption))
                        sOption.SetValue(pair.Value);
                }
            }
        }
    }
    public static void Clear()
    {
        //Categories.Clear();
    }
}
