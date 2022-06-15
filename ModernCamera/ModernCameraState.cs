﻿using ModernCamera.Behaviours;
using ModernCamera.Enums;
using ProjectM;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ModernCamera;

internal static class ModernCameraState
{
    internal static IntPtr Gamehandle;
    internal static bool IsFirstPerson;
    internal static bool IsActionMode;
    internal static bool IsMouseLocked;
    internal static InputState GameplayInputState;
    internal static BehaviourType CurrentBehaviourType = BehaviourType.Default;
    internal static Dictionary<BehaviourType, CameraBehaviour> CameraBehaviours = new();

    internal static bool IsMenuOpen
    {
        get { return _menusOpen > 0; }
        set { _menusOpen = value ? _menusOpen + 1 : Mathf.Max(0, _menusOpen - 1); }
    }
    private static int _menusOpen;

    internal static CameraBehaviour CurrentCameraBehaviour
    {
        get
        {
            if (CameraBehaviours.ContainsKey(CurrentBehaviourType))
                return CameraBehaviours[CurrentBehaviourType];
            return null;
        }
    }

    internal static void RegisterCameraBehaviour(CameraBehaviour behaviour)
    {
        CameraBehaviours.Add(behaviour.BehaviourType, behaviour);
    }
}
