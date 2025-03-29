using UnityEngine;

namespace ModernCamera.API;

public class SliderOption : Option<float>
{
    public float MinValue { get; internal set; }
    public float MaxValue { get; internal set; }
    public override float Value { get => Mathf.Clamp(base.Value, MinValue, MaxValue); internal set => base.Value = value; }
    public int Decimals { get; internal set; }
    public float StepValue { get; internal set; }

    public SliderOption(string text, string description, float minValue, float maxValue, float defaultvalue, int decimals = default, float stepValue = default) : base(text, description, defaultvalue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
        Value = Mathf.Clamp(Value, MinValue, MaxValue);
        Decimals = decimals;
        StepValue = stepValue;
    }

    public override void SetValue(float value)
    {
        base.SetValue(value);
        Value = Mathf.Clamp(Value, MinValue, MaxValue);
    }
}
