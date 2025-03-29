using HarmonyLib;
using ProjectM.UI;
using UnityEngine;
using TMPro;
using Il2CppSystem;
using Stunlock.Localization;
using StunShared.UI;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using ModernCamera.API;
using ModernCamera.Hooks;
using System.Collections.Generic;
using System.Linq;

namespace Silkworm.Hooks;

[HarmonyPatch]
internal static class OptionsPanel_Interface_Hook
{
    private static OptionsPanel_Interface _instance;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(OptionsPanel_Interface), nameof(OptionsPanel_Interface.Start))]
    private static void Start(OptionsPanel_Interface __instance)
    {
        _controls.Clear();
        SettingsMenuHook.OnSettingsMenuOpened?.Invoke();
        OptionsManager.Load();

        _instance = __instance;

        foreach (var category in OptionsManager.Categories.Values)
        {
            if (category.Options.Count == 0)
            {
                continue;
            }

            __instance.AddHeader(category.LocalizationKey);
            
            foreach (var id in category.Options)
            {
                if (category.TryGetToggle(id, out var toggleOption))
                {
                    var checkbox = UIHelper.InstantiatePrefabUnderAnchor(__instance.CheckboxPrefab, __instance.ContentNode);
                    InitializeCheckBox(checkbox, toggleOption.DescKey, toggleOption.DescKey, toggleOption);
                    var checkboxEntry = (SettingsEntryBase)checkbox;
                    __instance.EntriesSelectionGroup.AddEntry(ref checkboxEntry);
                    _controls.Add(checkbox);
                }
                else if (category.TryGetSlider(id, out var sliderOption))
                {
                    var slider = UIHelper.InstantiatePrefabUnderAnchor(__instance.SliderPrefab, __instance.ContentNode);
                    InitializeSlider(slider, sliderOption.DescKey, sliderOption.DescKey, sliderOption);
                    var sliderEntry = slider as SettingsEntryBase;
                    __instance.EntriesSelectionGroup.AddEntry(ref sliderEntry);
                    _controls.Add(slider);
                }
                else if (category.TryGetDropdown(id, out var dropdownOption))
                {
                    try
                    {
                        var dropdown =
                            UIHelper.InstantiatePrefabUnderAnchor(__instance.DropdownPrefab, __instance.ContentNode);
                        InitializeDropDown(dropdown, dropdownOption.DescKey, dropdownOption.DescKey, dropdownOption);
                        var dropdownEntry = dropdown as SettingsEntryBase;
                        __instance.EntriesSelectionGroup.AddEntry(ref dropdownEntry);
                        _controls.Add(dropdown);
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
                else if (category.TryGetDivider(id, out var dividerText))
                {
                    var divider = CreateDivider(__instance.ContentNode, dividerText);
                }
            }
        }


    }

    private static readonly List<SettingsEntryBase> _controls = new();
    /// <summary>
    /// To update localization for controls we have to update all localization keys
    /// </summary>
    public static void UpdateOptionsLocalization()
    {
        if(_instance == null)
            return;

        foreach (var category in OptionsManager.Categories.Values)
        {
            if(category.Options.Count == 0)
                continue;

            var cKey = LocalizationManager.CreateKey(category.Name);
            category.UpdateLocalizationKey(cKey);
            _instance.HeaderPrefab.HeaderText.LocalizationKey = cKey;

            foreach (var id in category.Options)
            {
                if (category.TryGetToggle(id, out var toggleOption))
                {
                    var ctrl = _controls.FirstOrDefault(c => c.HeaderText.LocalizationKey.Key == toggleOption.DescKey.Key);
                    if (ctrl != null)
                    {
                        var newKey = LocalizationManager.CreateKey(toggleOption.Description);
                        toggleOption.UpdateDescKey(newKey);
                        InitializeCheckBox(ctrl as SettingsEntry_Checkbox, newKey, newKey, toggleOption); }
                }
                if (category.TryGetSlider(id, out var sliderOption))
                {
                    var ctrl = _controls.FirstOrDefault(c => c.HeaderText.LocalizationKey.Key == sliderOption.DescKey.Key);
                    if (ctrl != null)
                    {
                        var newKey = LocalizationManager.CreateKey(sliderOption.Description);
                        sliderOption.UpdateDescKey(newKey);
                        InitializeSlider(ctrl as SettingsEntry_Slider, newKey, newKey, sliderOption);
                    }
                }
                if (category.TryGetDropdown(id, out var ddOption))
                {
                    var ctrl = _controls.FirstOrDefault(c => c.HeaderText.LocalizationKey.Key == ddOption.DescKey.Key);
                    if (ctrl != null)
                    {
                        var newKey = LocalizationManager.CreateKey(ddOption.Description);
                        ddOption.UpdateDescKey(newKey);
                        InitializeDropDown(ctrl as SettingsEntry_Dropdown, newKey, newKey, ddOption);

                    }
                }
            }
        }
    }

    private static void InitializeDropDown(SettingsEntry_Dropdown dropdown, LocalizationKey nameKey, LocalizationKey descKey, DropdownOption dropdownOption)
    {
        dropdown.Initialize(
            nameKey,
            new Nullable_Unboxed<LocalizationKey>(descKey), //todo dropdownOption.DescKey
            new Il2CppReferenceArray<LocalizedKeyValue>(new LocalizedKeyValue[0]),
            dropdownOption.Values,
            dropdownOption.DefaultValue,
            dropdownOption.Value,
            OnChange(dropdownOption)
        );
    }

    private static void InitializeSlider(SettingsEntry_Slider slider, LocalizationKey nameKey, LocalizationKey descKey, SliderOption sliderOption)
    {
        slider.Initialize(
            nameKey,
            new Nullable_Unboxed<LocalizationKey>(descKey),
            sliderOption.MinValue,
            sliderOption.MaxValue,
            sliderOption.DefaultValue,
            sliderOption.Value,
            sliderOption.Decimals,
            sliderOption.Decimals == 0,
            OnChange(sliderOption),
            fixedStepValue: sliderOption.StepValue
        );
    }

    private static void InitializeCheckBox(SettingsEntry_Checkbox ctrl, LocalizationKey nameKey, LocalizationKey descKey, ToggleOption toggleOption)
    {
        if(ctrl == null)
            return;

        ctrl.Initialize(nameKey,
            new Nullable_Unboxed<LocalizationKey>(descKey), toggleOption.DefaultValue,
            toggleOption.Value, OnChange(toggleOption));
    }


    private static GameObject CreateDivider(Transform parent, string text)
    {
        var textComps = parent.GetComponentsInChildren<TextMeshProUGUI>();
        var divider = new GameObject("Divider");

        var dividerTransform = divider.AddComponent<RectTransform>();
        dividerTransform.SetParent(parent);
        dividerTransform.localScale = Vector3.one;
        dividerTransform.sizeDelta = new Vector2(0, 28);

        var dividerImage = divider.AddComponent<UnityEngine.UI.Image>();
        dividerImage.color = new Color(0.12f, 0.152f, 0.2f, 0.15f);

        var dividerLayout = divider.AddComponent<UnityEngine.UI.LayoutElement>();
        dividerLayout.preferredHeight = 28;

        var dividerTextObject = new GameObject("Text");
        var dividerTextTransform = dividerTextObject.AddComponent<RectTransform>();
        dividerTextTransform.SetParent(divider.transform);
        dividerTextTransform.localScale = Vector3.one;

        var dividerText = dividerTextObject.AddComponent<TextMeshProUGUI>();
        dividerText.alignment = TextAlignmentOptions.Center;
        dividerText.fontStyle = FontStyles.SmallCaps;
        dividerText.font = textComps[0].font;
        dividerText.fontSize = 20;
        if (text != null)
            dividerText.SetText(text);

        return divider;
    }

    private static Action<T> OnChange<T>(Option<T> option)
    {
        return (Action<T>)(value =>
        {
            option.SetValue(value);
            OptionsManager.FullSave();
        });
    }
}
