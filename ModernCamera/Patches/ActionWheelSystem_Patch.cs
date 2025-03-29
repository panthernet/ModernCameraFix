using HarmonyLib;
using ModernCamera.Utils;
using ProjectM.UI;

namespace ModernCamera.Patches;

[HarmonyPatch]
internal static class ActionWheelSystem_Patch
{
    internal static bool WheelVisible;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ActionWheelSystem), nameof(ActionWheelSystem.OnUpdate))]
    private static void OnUpdate(ActionWheelSystem __instance)
    {
        if (__instance == null)
        {
            return;
        }

        if (WheelVisible)
        {
            if (__instance._CurrentActiveWheel != null && !__instance._CurrentActiveWheel.IsVisible())
            {
                LogUtils.LogInfo("No wheel visible");
                ModernCameraState.IsMenuOpen = false;
                WheelVisible = false;
            }
            else if (__instance._CurrentActiveWheel == null)
            {
                LogUtils.LogInfo("Wheel is null");
                ModernCameraState.IsMenuOpen = false;
                WheelVisible = false;
            }
        }
        else if (__instance._CurrentActiveWheel != null && __instance._CurrentActiveWheel.IsVisible())
        {
            LogUtils.LogInfo("CurrentActiveWheel is visible");
            WheelVisible = true;
            ModernCameraState.IsMenuOpen = true;
        }
    }
}