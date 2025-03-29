﻿using ModernCamera.Enums;
using ModernCamera.Patches;
using ModernCamera.Utils;
using ProjectM;
using UnityEngine;

namespace ModernCamera.Behaviours;

internal abstract class CameraBehaviour
{
    internal BehaviourType BehaviourType;
    internal float DefaultMaxPitch;
    internal float DefaultMinPitch;
    internal bool Active;

    protected static float TargetZoom = Settings.MaxZoom / 2;
    protected static ZoomSettings BuildModeZoomSettings;
    protected static bool IsBuildSettingsSet;

    internal virtual void Activate(ref TopdownCameraState state)
    {
        Active = true;
    }

    internal virtual void Deactivate()
    {
        TargetZoom = Settings.MaxZoom / 2;
        Active = false;
    }

    internal virtual bool ShouldActivate(ref TopdownCameraState state) => false;

    internal virtual unsafe void HandleInput(ref InputState inputState)
    {
        if (!inputState.InputsPressed.IsCreated) return;

        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Escape))
        {
            if (EscapeMenuView_Patch.IsEscapeMenuOpen)
            {
                LogUtils.LogInfo("EscapeMenuView is open, closing it");
                ModernCameraState.IsMenuOpen = false;
                EscapeMenuView_Patch.IsEscapeMenuOpen = false;
            }
        }

        if (ModernCameraState.IsMouseLocked && !ModernCameraState.IsMenuOpen && !inputState.IsInputPressed(ButtonInputAction.RotateCamera))
        {
            inputState.InputsPressed.m_ListData->AddNoResize(ButtonInputAction.RotateCamera);
        }

        // Manually manage camera zoom
        var zoomVal = inputState.GetAnalogValue(AnalogInputAction.ZoomCamera);
        if (zoomVal != 0 && (!ModernCameraState.InBuildMode || !Settings.DefaultBuildMode))
        {
            // Consume zoom input for the camera
            var zoomAmount = Mathf.Lerp(.25f, 1.5f, Mathf.Max(0, TargetZoom - Settings.MinZoom) / Settings.MaxZoom);
            var zoomChange = inputState.GetAnalogValue(AnalogInputAction.ZoomCamera) > 0 ? zoomAmount : -zoomAmount;

            if ((TargetZoom > Settings.MinZoom && TargetZoom + zoomChange < Settings.MinZoom) || (ModernCameraState.IsFirstPerson && zoomChange > 0))
                TargetZoom = Settings.MinZoom;
            else
                TargetZoom = Mathf.Clamp(TargetZoom + zoomChange, Settings.FirstPersonEnabled ? 0 : Settings.MinZoom, Settings.MaxZoom);

            inputState.SetAnalogValue(AnalogInputAction.ZoomCamera, 0);
        }

        // Update zoom if MaxZoom is changed
        if (TargetZoom > Settings.MaxZoom)
            TargetZoom = Settings.MaxZoom;
    }

    internal virtual void UpdateCameraInputs(ref TopdownCameraState state, ref TopdownCamera data)
    {
        ModernCameraState.InBuildMode = state.InBuildMode;
        if (!IsBuildSettingsSet)
        {
            BuildModeZoomSettings = data.BuildModeZoomSettings;
            IsBuildSettingsSet = true;
        }

        // Set camera behaviour pitch settings
        state.ZoomSettings.MaxPitch = DefaultMaxPitch;
        state.ZoomSettings.MinPitch = DefaultMinPitch;

        // Manually set zoom if not in build mode
        if (!state.InBuildMode || !Settings.DefaultBuildMode)
        {
            data.BuildModeZoomSettings.MaxPitch = DefaultMaxPitch;
            data.BuildModeZoomSettings.MinPitch = DefaultMinPitch;
            state.LastTarget.Zoom = TargetZoom;
            state.Target.Zoom = TargetZoom;
        }

        // Use default build mode zoom
        if (state.InBuildMode && Settings.DefaultBuildMode)
        {
            data.BuildModeZoomSettings = BuildModeZoomSettings;
            state.LastTarget.Zoom = data.BuildModeZoomDistance;
            state.Target.Zoom = data.BuildModeZoomDistance;
        }
    }
}
