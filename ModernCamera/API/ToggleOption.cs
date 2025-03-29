namespace ModernCamera.API;

public class ToggleOption : Option<bool>
{
    public ToggleOption(string name, string description, bool defaultvalue) : base(name, description, defaultvalue)
    {
    }
}
