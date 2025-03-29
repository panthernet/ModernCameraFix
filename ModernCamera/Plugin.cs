﻿using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using ModernCamera.Hooks;
using ModernCamera.Utils;

namespace ModernCamera;

[BepInProcess("VRising.exe")]
[BepInDependency("gg.deca.Bloodstone")]
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    private static Harmony Harmony;

    public override void Load()
    {
        LogUtils.Init(Log);
        Settings.Init();

        AddComponent<ModernCamera>();

        TopdownCameraSystem_Hook.CreateAndApply();

        Harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        Harmony.PatchAll();

        LogUtils.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} v{PluginInfo.PLUGIN_VERSION} is loaded!");
    }

    public override bool Unload()
    {
        Harmony.UnpatchSelf();

        TopdownCameraSystem_Hook.Dispose();

        return true;
    }
}
