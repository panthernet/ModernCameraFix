using ModernCamera.Enums;
using ModernCamera.Utils;
using ProjectM.UI;
using Silkworm.API;
using Silkworm.Core.KeyBinding;
using Silkworm.Core.Options;
using Stunlock.Localization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ModernCamera
{
    internal static class Settings
    {
        internal static bool Enabled { get => EnabledOption.Value; set => EnabledOption.SetValue(value); }
        internal static bool FirstPersonEnabled { get => FirstPersonEnabledOption.Value; set => FirstPersonEnabledOption.SetValue(value); }
        internal static bool DefaultBuildMode { get => DefaultBuildModeOption.Value; set => DefaultBuildModeOption.SetValue(value); }
        internal static bool AlwaysShowCrosshair { get => AlwaysShowCrosshairOption.Value; set => AlwaysShowCrosshairOption.SetValue(value); }
        internal static bool ActionModeCrosshair { get => ActionModeCrosshairOption.Value; set => ActionModeCrosshairOption.SetValue(value); }
        internal static float FieldOfView { get => FieldOfViewOption.Value; set => FieldOfViewOption.SetValue(value); }

        internal static int AimOffsetX { get => (int)(Screen.width * (AimOffsetXOption.Value / 100)); set => AimOffsetXOption.SetValue(Mathf.Clamp(value / Screen.width, -25, 25)); }
        internal static int AimOffsetY { get => (int)(Screen.height * (AimOffsetYOption.Value / 100)); set => AimOffsetYOption.SetValue(Mathf.Clamp(value / Screen.width, -25, 25)); }
        internal static CameraAimMode CameraAimMode { get => CameraAimModeOption.GetEnumValue<CameraAimMode>(); set => CameraAimModeOption.SetValue((int)value); }

        internal static bool LockZoom { get => LockCameraZoomOption.Value; set => LockCameraZoomOption.SetValue(value); }
        internal static float LockZoomDistance { get => LockCameraZoomDistanceOption.Value; set => LockCameraZoomDistanceOption.SetValue(value); }
        internal static float MinZoom { get => MinZoomOption.Value; set => MinZoomOption.SetValue(value); }
        internal static float MaxZoom { get => MaxZoomOption.Value; set => MaxZoomOption.SetValue(value); }

        internal static bool LockPitch { get => LockCameraPitchOption.Value; set => LockCameraPitchOption.SetValue(value); }
        internal static float LockPitchAngle { get => LockCameraPitchAngleOption.Value * Mathf.Deg2Rad; set => LockCameraPitchAngleOption.SetValue(Mathf.Clamp(value * Mathf.Rad2Deg, 0, 90)); }
        internal static float MinPitch { get => MinPitchOption.Value * Mathf.Deg2Rad; set => MinPitchOption.SetValue(Mathf.Clamp(value * Mathf.Rad2Deg, 0, 90)); }
        internal static float MaxPitch { get => MaxPitchOption.Value * Mathf.Deg2Rad; set => MaxPitchOption.SetValue(Mathf.Clamp(value * Mathf.Rad2Deg, 0, 90)); }

        internal static bool OverTheShoulder { get => OverTheShoulderOption.Value; set => OverTheShoulderOption.SetValue(value); }
        internal static float OverTheShoulderX { get => OverTheShoulderXOption.Value; set => OverTheShoulderXOption.SetValue(value); }
        internal static float OverTheShoulderY { get => OverTheShoulderYOption.Value; set => OverTheShoulderYOption.SetValue(value); }

        internal static float FirstPersonForwardOffset = 1.65f;
        internal static float MountedOffset = 1.6f;
        internal static float HeadHeightOffset = 1.05f;
        internal static float ShoulderRightOffset = 0.8f;
        internal static Dictionary<string, Vector2> FirstPersonShapeshiftOffsets = new Dictionary<string, Vector2>
        {
            { "AB_Shapeshift_Bat_Buff", new Vector2(0, 2.5f) },
            { "AB_Shapeshift_Bear_Buff", new Vector2(0.25f, 5f) },
            { "AB_Shapeshift_Bear_Skin01_Buff", new Vector2(0.25f, 5f) },
            { "AB_Shapeshift_Human_Grandma_Skin01_Buff", new Vector2(-0.1f, 1.55f) },
            { "AB_Shapeshift_Human_Buff", new Vector2(0.5f, 1.4f) },
            { "AB_Shapeshift_Rat_Buff", new Vector2(-1.85f, 2f) },
            { "AB_Shapeshift_Toad_Buff", new Vector2(-0.6f, 4.2f) },
            { "AB_Shapeshift_Wolf_Buff", new Vector2(-0.25f, 4.3f) },
            { "AB_Shapeshift_Wolf_Skin01_Buff", new Vector2(-0.25f, 4.3f) }
        };

        private static float ZoomOffset = 2;

        private static ToggleOption EnabledOption;
        private static SliderOption FieldOfViewOption;
        private static ToggleOption AlwaysShowCrosshairOption;
        private static ToggleOption ActionModeCrosshairOption;
        private static ToggleOption FirstPersonEnabledOption;
        private static ToggleOption DefaultBuildModeOption;

        private static DropdownOption CameraAimModeOption;
        private static SliderOption AimOffsetXOption;
        private static SliderOption AimOffsetYOption;

        private static ToggleOption LockCameraZoomOption;
        private static SliderOption LockCameraZoomDistanceOption;
        private static SliderOption MinZoomOption;
        private static SliderOption MaxZoomOption;

        private static ToggleOption LockCameraPitchOption;
        private static SliderOption LockCameraPitchAngleOption;
        private static SliderOption MinPitchOption;
        private static SliderOption MaxPitchOption;

        private static ToggleOption OverTheShoulderOption;
        private static SliderOption OverTheShoulderXOption;
        private static SliderOption OverTheShoulderYOption;

        private static Keybinding EnabledKeybind;
        private static Keybinding ActionModeKeybind;
        private static Keybinding HideUIKeybind;

        internal static void Init()
        {
            SetupOptions();
            SetupKeybinds();

            //a bit hacky way to update the settings menu when it's opened
            SettingsMenuHook.OnSettingsMenuLanguageChanged += (language) =>
            {
                LangUtils.LoadLanguage(Localization.CurrentLanguage ?? language);
                SetupOptions();
                SetupKeybinds();
            };
        }

        internal static void AddEnabledListener(OnChange<bool> action) => EnabledOption.AddListener(action);
        internal static void AddFieldOfViewListener(OnChange<float> action) => FieldOfViewOption.AddListener(action);
        internal static void AddHideUIListener(KeyEvent action) => HideUIKeybind.AddKeyDownListener(action);

        private static void SetupOptions()
        {
            OptionsManager.Clear();
            var category = OptionsManager.AddCategory(LangUtils.Get("moderncamera.category"));
            EnabledOption = category.AddToggle("moderncamera.enabled", LangUtils.Get("moderncamera.enabled"), true);
            FirstPersonEnabledOption = category.AddToggle("moderncamera.firstperson", LangUtils.Get("moderncamera.firstperson"), true);
            DefaultBuildModeOption = category.AddToggle("moderncamera.defaultbuildmode", LangUtils.Get("moderncamera.defaultbuildmode"), true);
            AlwaysShowCrosshairOption = category.AddToggle("moderncamera.alwaysshowcrosshair", LangUtils.Get("moderncamera.alwaysshowcrosshair"), false);
            ActionModeCrosshairOption = category.AddToggle("moderncamera.actionmodecrosshair", LangUtils.Get("moderncamera.actionmodecrosshair"), false);
            FieldOfViewOption = category.AddSlider("moderncamera.fieldofview", LangUtils.Get("moderncamera.fieldofview"), 50, 90, 60);

            category.AddDivider(LangUtils.Get("moderncamera.divider.thirdpersonaiming"));
            CameraAimModeOption = category.AddDropdown("moderncamera.aimmode", LangUtils.Get("moderncamera.aimmode"), (int)CameraAimMode.Default, Enum.GetNames(typeof(CameraAimMode)));
            AimOffsetXOption = category.AddSlider("moderncamera.aimoffsetx", LangUtils.Get("moderncamera.aimoffsetx"), -25, 25, 0);
            AimOffsetYOption = category.AddSlider("moderncamera.aimoffsety", LangUtils.Get("moderncamera.aimoffsety"), -25, 25, 0);

            category.AddDivider(LangUtils.Get("moderncamera.divider.thirdpersonzoom"));
            MinZoomOption = category.AddSlider("moderncamera.minzoom", LangUtils.Get("moderncamera.minzoom"), 1, 18, 2);
            MaxZoomOption = category.AddSlider("moderncamera.maxzoom", LangUtils.Get("moderncamera.maxzoom"), 3, 20, 18);
            LockCameraZoomOption = category.AddToggle("moderncamera.lockzoom", LangUtils.Get("moderncamera.lockzoom"), false);
            LockCameraZoomDistanceOption = category.AddSlider("moderncamera.lockzoomdistance", LangUtils.Get("moderncamera.lockzoomdistance"), 6, 20, 15);

            category.AddDivider(LangUtils.Get("moderncamera.divider.thirdpersonpitch"));
            MinPitchOption = category.AddSlider("moderncamera.minpitch", LangUtils.Get("moderncamera.minpitch"), 0, 90, 9);
            MaxPitchOption = category.AddSlider("moderncamera.maxpitch", LangUtils.Get("moderncamera.maxpitch"), 0, 90, 90);
            LockCameraPitchOption = category.AddToggle("moderncamera.lockpitch", LangUtils.Get("moderncamera.lockpitch"), false);
            LockCameraPitchAngleOption = category.AddSlider("moderncamera.lockpitchangle", LangUtils.Get("moderncamera.lockpitchangle"), 0, 90, 60);

            category.AddDivider(LangUtils.Get("moderncamera.divider.overtheshoulder"));
            OverTheShoulderOption = category.AddToggle("moderncamera.overtheshoulder", LangUtils.Get("moderncamera.overtheshoulder"), false);
            OverTheShoulderXOption = category.AddSlider("moderncamera.overtheshoulderx", LangUtils.Get("moderncamera.overtheshoulderx"), 0.5f, 4, 1);
            OverTheShoulderYOption = category.AddSlider("moderncamera.overtheshouldery", LangUtils.Get("moderncamera.overtheshouldery"), 1, 8, 1);

            MinZoomOption.AddListener(value =>
            {
                if (value + ZoomOffset > MaxZoom && value + ZoomOffset < MaxZoomOption.MaxValue)
                    MaxZoomOption.SetValue(value + ZoomOffset);
                else if (value + ZoomOffset > MaxZoomOption.MaxValue)
                    MinZoomOption.SetValue(MaxZoomOption.MaxValue - ZoomOffset);
            });

            MaxZoomOption.AddListener(value =>
            {
                if (value - ZoomOffset < MinZoom && value - ZoomOffset > MinZoomOption.MinValue)
                    MinZoomOption.SetValue(value - ZoomOffset);
                else if (value - ZoomOffset < MinZoomOption.MinValue)
                    MaxZoomOption.SetValue(MinZoomOption.MinValue + ZoomOffset);
            });

            MinPitchOption.AddListener(value =>
            {
                if (value > MaxPitchOption.Value && value < MaxPitchOption.MaxValue)
                    MaxPitchOption.SetValue(value);
                else if (value > MaxPitchOption.MaxValue)
                    MinPitchOption.SetValue(MaxPitchOption.MaxValue);
            });

            MaxPitchOption.AddListener(value =>
            {
                if (value < MinPitchOption.Value && value > MinPitchOption.MinValue)
                    MinPitchOption.SetValue(value);
                else if (value < MinPitchOption.MinValue)
                    MaxPitchOption.SetValue(MinPitchOption.MinValue);
            });
        }

        private static void SetupKeybinds()
        {
            KeybindingsManager.Clear();
            var category = KeybindingsManager.AddCategory(LangUtils.Get("moderncamera.category"));

            EnabledKeybind = category.AddKeyBinding("moderncamera.enabled", LangUtils.Get("moderncamera.enabled"));
            EnabledKeybind.AddKeyDownListener(() => EnabledOption.SetValue(!Enabled));

            ActionModeKeybind = category.AddKeyBinding("moderncamera.actionmode", LangUtils.Get("moderncamera.actionmode"));
            ActionModeKeybind.AddKeyDownListener(() =>
            {
                if (Settings.Enabled && !ModernCameraState.IsFirstPerson)
                {
                    ModernCameraState.IsMouseLocked = !ModernCameraState.IsMouseLocked;
                    ModernCameraState.IsActionMode = !ModernCameraState.IsActionMode;
                }
            });

            HideUIKeybind = category.AddKeyBinding("moderncamera.hideui", LangUtils.Get("moderncamera.hideui"));
        }
    }
}
