using Everything_Handhelds_Tool.Classes.MouseMode.Actions;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace Everything_Handhelds_Tool.Classes.MouseMode
{
    public class MouseProfile
    {
        public Dictionary<GamepadButtonFlags, MouseAction> mouseActionList { get; set; } = new Dictionary<GamepadButtonFlags, MouseAction>
        {
            { GamepadButtonFlags.A, new MouseLeftClick() },
            { GamepadButtonFlags.B , new MouseRightClick() },
            { GamepadButtonFlags.DPadUp, new KeyboardClick(){virtualKeyCode = WindowsInput.Native.VirtualKeyCode.UP } },
            { GamepadButtonFlags.DPadDown, new KeyboardClick(){virtualKeyCode = WindowsInput.Native.VirtualKeyCode.DOWN } },
            { GamepadButtonFlags.DPadLeft, new KeyboardClick(){virtualKeyCode = WindowsInput.Native.VirtualKeyCode.LEFT } },
            { GamepadButtonFlags.DPadRight, new KeyboardClick(){virtualKeyCode = WindowsInput.Native.VirtualKeyCode.RIGHT } }
        };

        public bool rightScroll { get; set; } = true;
        public bool reverseVerticalScroll { get; set; } = false;
    }
}
