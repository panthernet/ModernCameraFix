using HarmonyLib;
using ProjectM.UI;

namespace ModernCamera.Patches;

[HarmonyPatch]
internal static class ActionWheelSystem_Patch
{
    private static bool WheelVisible;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ActionWheelSystem), nameof(ActionWheelSystem.OnUpdate))]
    private static void OnUpdate(ActionWheelSystem __instance)
    {
        if (!WheelVisible && ((__instance?._CurrentActiveWheel?.IsVisible() ?? false) ||
                              (__instance?._EmotesWheel?.IsVisible() ?? false)))
        {
            ModernCameraState.IsMenuOpen = true;
            WheelVisible = true;
        }
        else if (WheelVisible && !(__instance?._CurrentActiveWheel?.IsVisible() ?? false) && !(__instance?._EmotesWheel?.IsVisible() ?? false))
        {
            ModernCameraState.IsMenuOpen = false;
            WheelVisible = false;
        }
    }
}
