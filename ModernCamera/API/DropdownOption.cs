using Il2CppSystem.Collections.Generic;
using System;

namespace ModernCamera.API;

public class DropdownOption : Option<int>
{
    internal List<string> Values;

    public DropdownOption(string name, string description, int defaultValue, string[] values) : base(name, description, defaultValue)
    {
        Values = new List<string>();
        foreach (var v in values)
        {
            Values.Add(v);
        }
    }

    public T GetEnumValue<T>()
    {
        return (T)Enum.Parse(typeof(T), Values[Value]);
    }
}