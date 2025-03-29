using HarmonyLib;
using ModernCamera.API;
using ModernCamera.Utils;
using ProjectM.UI;
using Stunlock.Localization;
using StunShared.UI;
using UnityEngine.InputSystem;

namespace ModernCamera.Hooks;

[HarmonyPatch]
internal class RebindingMenu_Hook
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(RebindingMenu), nameof(RebindingMenu.Start))]
    private static void StartPostfix(RebindingMenu __instance)
    {
        /*LogUtils.LogInfo("KBMData:");
        foreach (var data in __instance._InputActionSystem._ControlsVisualMapping.KBMData)
        {
            LogUtils.LogInfo("\t" + Localization.Get(data.LocalizationKey) + ": " + data.ControlPath);
        }

        LogUtils.LogInfo("PSData:");
        foreach (var data in __instance._InputActionSystem._ControlsVisualMapping.PSData)
        {
            LogUtils.LogInfo("\t" + Localization.Get(data.LocalizationKey) + ": " + data.ControlPath);
        }

        LogUtils.LogInfo("XboxData:");
        foreach (var data in __instance._InputActionSystem._ControlsVisualMapping.XboxData)
        {
            LogUtils.LogInfo("\t" + Localization.Get(data.LocalizationKey) + ": " + data.ControlPath);
        }

        LogUtils.LogInfo("ButtonInputActionData:");
        foreach (var data in __instance._InputActionSystem._ControlsVisualMapping.ButtonInputActionData)
        {
            LogUtils.LogInfo("\t" + data.Input + " " + Localization.Get(data.ButtonInputActionKey) + ": " + data.ButtonInputActionKeyDesc);
        }*/

        if (__instance._BindingTypeToDisplay != ProjectM.ControllerType.KeyboardAndMouse)
        {
            return;
        }

        foreach (var category in KeybindingsManager.Categories.Values)
        {
            __instance.AddHeader(category.NameKey);

            foreach (var keybinding in category.KeybindingMap.Values)
            {
                var binding = UIHelper.InstantiatePrefabUnderAnchor(__instance.ControlsInputEntryPrefab, __instance.ContentNode);
                binding.Initialize(
                    ProjectM.ControllerType.KeyboardAndMouse,
                    keybinding.InputFlag,
                    ProjectM.AnalogInputAction.None,
                    true,
                    onClick: (Il2CppSystem.Action<SettingsEntry_Binding, bool, ProjectM.ButtonInputAction, ProjectM.AnalogInputAction, bool>)__instance.OnEntryButtonClicked,
                    onClear: (Il2CppSystem.Action<SettingsEntry_Binding, ProjectM.ButtonInputAction>)__instance.OnEntryCleared
                );
                binding.SetInputInfo(keybinding.NameKey, keybinding.NameKey);
                binding.SetPrimary(keybinding.PrimaryName);
                binding.SetSecondary(keybinding.SecondaryName);

                var keybindEntry = binding as SettingsEntryBase;
                __instance.EntriesSelectionGroup.AddEntry(ref keybindEntry);
            }
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(RebindingMenu), nameof(RebindingMenu.OnClick_ResetButton))]
    private static void OnClick_ResetButton()
    {
        foreach (var category in KeybindingsManager.Categories.Values)
        {
            foreach (var action in category.InputActionMap.actions)
            {
                action.RemoveAllBindingOverrides();
            }
        }

        KeybindingsManager.FullSave();
    }
}
