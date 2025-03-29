using ModernCamera.Utils;
using ProjectM;
using Stunlock.Localization;

namespace ModernCamera.API;

public delegate void KeyEvent();

public class Keybinding
{
    public struct Data
    {
        public string Name;
        public string PrimaryDefault;
        public string PrimaryOverride;
        public string SecondaryDefault;
        public string SecondaryOverride;
    }

    public string Name { get => BloodstoneKeybinding.Description.Id; }
    public string Description { get; set; }
    public string Primary => BloodstoneKeybinding.Primary.ToString();
    public string Secondary => BloodstoneKeybinding.Secondary.ToString();
    public bool IsPressed => BloodstoneKeybinding.IsPressed;
    //public bool IsDown => BloodstoneKeybinding.IsPressed;
    //public bool IsUp => InputAction.WasReleasedThisFrame();

    internal event KeyEvent KeyPressed = delegate { };
    internal event KeyEvent KeyDown = delegate { };
    internal event KeyEvent KeyUp = delegate { };

    internal readonly ButtonInputAction InputFlag;
    internal LocalizationKey NameKey;
    //internal InputAction InputAction;
    internal string DefaultPrimary { get; set; }
    internal string DefaultSecondary { get; set; }
    internal string PrimaryName => BloodstoneKeybinding.Primary.ToString();
    internal string SecondaryName => BloodstoneKeybinding.Secondary.ToString();

   // public bool HasInputAction => InputAction != null;

    internal Bloodstone.API.Keybinding BloodstoneKeybinding;

    internal Keybinding(Bloodstone.API.Keybinding keybinding)
    {
        BloodstoneKeybinding = keybinding;
        Description = keybinding.Description.Name;
        InputFlag = (ButtonInputAction)HashUtils.Hash64(keybinding.Description.Name);
        NameKey = LocalizationManager.CreateKey(keybinding.Description.Name);
    }

    /*internal Keybinding(InputAction inputAction, string defaultPrimary = null, string defaultSecondary = null)
    {
        InputAction = inputAction;
        NameKey = LocalizationManager.CreateKey(InputAction.name);

        InputAction.AddBinding(defaultPrimary == null ? "" : defaultPrimary);
        DefaultPrimary = defaultPrimary == null ? "" : defaultPrimary;

        InputAction.AddBinding(defaultSecondary == null ? "" : defaultSecondary);
        DefaultSecondary = defaultSecondary == null ? "" : defaultSecondary;

        var flag = HashUtils.Hash64(InputAction.actionMap.name + "." + InputAction.name);
        do
        {
            InputFlag = (ButtonInputAction)flag;
        } while (Enum.IsDefined(typeof(ButtonInputAction), (ButtonInputAction)flag--));
    }*/

    /// <summary>
    /// Is called each frame the key is held down
    /// </summary>
    /// <param name="action"></param>
    public void AddKeyPressedListener(KeyEvent action)
    {
            KeyPressed += action;
    }

    /// <summary>
    /// Is called during the frame the key is pressed
    /// </summary>
    /// <param name="action"></param>
    public void AddKeyDownListener(KeyEvent action)
    {
            KeyDown += action;
    }

    /// <summary>
    /// Is called during the frame the key is released
    /// </summary>
    /// <param name="action"></param>
    public void AddKeyUpListener(KeyEvent action)
    {
            KeyUp += action;
    }

    public void Override(bool primary, string path)
    {
        //InputAction.ApplyBindingOverride(primary ? 0 : 1, path);
    }

    internal void OnKeyPressed() => KeyPressed();

    internal void OnKeyDown() => KeyDown();

    internal void OnKeyUp() => KeyUp();

    internal Data GetData()
    {
        return new Data
        {
            Name = Name,
            PrimaryDefault = DefaultPrimary,
            PrimaryOverride = BloodstoneKeybinding.Primary.ToString(),
            SecondaryDefault = DefaultSecondary,
            SecondaryOverride = BloodstoneKeybinding.Secondary.ToString(),
        };
    }
}