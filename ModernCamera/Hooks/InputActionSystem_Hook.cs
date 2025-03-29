using HarmonyLib;
using Il2CppSystem;
using ModernCamera.API;
using ModernCamera.Utils;
using ProjectM;
using UnityEngine.InputSystem;
using static ProjectM.InputActionSystem;

namespace ModernCamera.Hooks;

[HarmonyPatch]
internal static class InputActionSystem_Hook
{
    private static InputActionMap MC_InputActionMap;
    private static InputAction ActionModeInputAction;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(InputActionSystem), nameof(InputActionSystem.OnCreate))]
    private static void OnCreate(InputActionSystem __instance)
    {
        __instance._LoadedInputActions.Disable();
        foreach (var category in KeybindingsManager.Categories.Values)
        {
            __instance._LoadedInputActions.AddActionMap(category.InputActionMap);
        }
        __instance._LoadedInputActions.Enable();

        // TODO: Load __instance.._ControlsVisualMapping.ButtonInputActionData
        // TODO: Load __instance._ControlsVisualMapping.AnalogInputActionData
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InputActionSystem), nameof(InputActionSystem.ModifyInputActionBinding), typeof(ButtonInputAction), typeof(bool), typeof(Action<bool>), typeof(Action<bool, bool>), typeof(OnRebindCollision), typeof(Nullable_Unboxed<ControllerType>))]
    private static void ModifyKeyInputSetting(ButtonInputAction buttonInput, bool modifyPrimary, ref Action<bool> onComplete, ref Action<bool, bool> onCancel, OnRebindCollision onCollision, Nullable_Unboxed<ControllerType> overrideControllerType)
    {
        LogUtils.LogInfo("ModifyKeyInputSetting Primary" +
            "\n\tButtonInput: " + buttonInput +
            "\n\tPrimary: " + modifyPrimary +
            "\n\tOnCollision: " + onCollision +
            "\n\tOverrideControllerType: " + overrideControllerType
        );
        /*
         * TODO: Why is this canceling immediately without a popup
         */
        onComplete += (Action<bool>)(b1 =>
        {
            LogUtils.LogInfo("onComplete " + b1);
        });
        onCancel += (Action<bool, bool>)((b1, b2) =>
        {
            LogUtils.LogInfo("onCancel " + b1 + ", " + b2);
        });
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InputActionSystem), nameof(InputActionSystem.OnUpdate))]
    private static void OnUpdate()
    {
        foreach (var category in KeybindingsManager.Categories.Values)
        {
            foreach (var keybinding in category.KeybindingMap.Values)
            {
                if (keybinding.IsPressed)
                    keybinding.OnKeyPressed();
            }
        }
    }
}
