using HarmonyLib;
using ModernCamera.Utils;
using ProjectM.UI;

namespace ModernCamera.Patches;

[HarmonyPatch]
internal class EscapeMenuView_Patch
{
    internal static bool IsEscapeMenuOpen { get; set; } = false;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(EscapeMenuView), nameof(EscapeMenuView.OnEnable))]
    private static void OnEnable() => Enable();
    private static void Enable()
    {
        LogUtils.LogInfo("EscapeMenuView.OnEnable");
        if (!IsEscapeMenuOpen)
        {
            IsEscapeMenuOpen = true;
            ModernCameraState.IsMenuOpen = true;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(EscapeMenuView), nameof(EscapeMenuView.OnDestroy))]
    private static void OnDestroy() => Disable();
    private static void Disable()
    {
        LogUtils.LogInfo("EscapeMenuView.OnDisable");
        if (IsEscapeMenuOpen)
        {
            IsEscapeMenuOpen = false;
            ModernCameraState.IsMenuOpen = false;
        }
    }
}