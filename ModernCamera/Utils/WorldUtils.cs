using Unity.Entities;
using System;

namespace ModernCamera.Utils;

#nullable enable
public static class WorldUtils
{
    private static readonly string _ClientWorldName = "Client_0";
    private static readonly string _ServerWorldName = "Server";
    private static World? _ClientWorld;
    private static World? _ServerWorld;

    public static bool ClientWorldExists => WorldExists(_ClientWorldName);
    public static bool ServerWorldExists => WorldExists(_ServerWorldName);

    public static World DefaultWorld => World.DefaultGameObjectInjectionWorld;
    public static World ClientWorld
    {
        get
        {
            if (_ClientWorld == null || !_ClientWorld.IsCreated)
                _ClientWorld = FindWorld(_ClientWorldName) ?? throw new Exception("Client world does not exist yet");
            return _ClientWorld;
        }
    }
    public static World ServerWorld
    {
        get
        {
            if (_ServerWorld == null || !_ServerWorld.IsCreated)
                _ServerWorld = FindWorld(_ServerWorldName) ?? throw new Exception("Server world does not exist yet");
            return _ServerWorld;
        }
    }

    public static bool WorldExists(string name)
    {
        foreach (var world in World.All)
            if (world.Name == name)
                return true;
        return false;
    }

    public static World? FindWorld(string name)
    {
        foreach (var world in World.All)
            if (world.Name == name)
                return world;
        return null;
    }
}
