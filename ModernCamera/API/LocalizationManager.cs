using Il2CppSystem;
using Stunlock.Core;
using Stunlock.Localization;
using System.Collections.Generic;

namespace ModernCamera.API;

public static class LocalizationManager
{
    private static readonly Dictionary<AssetGuid, string> guids;

    static LocalizationManager()
    {
        guids = new();
    }

    public static LocalizationKey CreateKey(string value)
    {
        var key = new LocalizationKey(AssetGuid.FromGuid(Guid.NewGuid()));
        guids.Add(key.GetGuid(), value);
        return key;
    }

    public static Nullable_Unboxed<LocalizationKey> CreateNullableKey(string value)
    {
        return new Nullable_Unboxed<LocalizationKey>(CreateKey(value));
    }

    internal static string GetKey(AssetGuid guid)
    {
        return guids[guid];
    }

    internal static string GetKey(LocalizationKey key)
    {
        return GetKey(key.GetGuid());
    }

    internal static bool HasKey(AssetGuid guid)
    {
        return guids.ContainsKey(guid);
    }

    internal static bool HasKey(LocalizationKey key)
    {
        return guids.ContainsKey(key.GetGuid());
    }

    public static LocalizationKey UpdateKey(LocalizationKey nameKey, string desc)
    {
        guids.Remove(nameKey.Key);
        return CreateKey(desc);
    }
}