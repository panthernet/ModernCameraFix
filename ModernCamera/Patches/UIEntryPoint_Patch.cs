﻿using HarmonyLib;
using ProjectM.UI;

namespace ModernCamera.Patches
{
    [HarmonyPatch]
    internal class UIEntryPoint_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIEntryPoint), nameof(UIEntryPoint.Awake))]
        private static void AwakePostfix()
        {
            ModernCameraState.Reset();
        }
    }
}
