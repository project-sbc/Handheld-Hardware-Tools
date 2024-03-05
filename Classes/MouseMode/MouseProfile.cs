using Everything_Handhelds_Tool.Classes.MouseMode.Actions;
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
        public Dictionary<string, MouseAction> mouseActionList { get; set; } = new Dictionary<string, MouseAction>
        {
            { "A", new MouseLeftClick() },
            { "B" , new MouseRightClick() },
            { "DPadUp", new KeyboardClick(){virtualKeyCode = WindowsInput.Native.VirtualKeyCode.UP } },
            { "DPadDown", new KeyboardClick(){virtualKeyCode = WindowsInput.Native.VirtualKeyCode.DOWN } },
            { "DPadLeft", new KeyboardClick(){virtualKeyCode = WindowsInput.Native.VirtualKeyCode.LEFT } },
            { "DPadRight", new KeyboardClick(){virtualKeyCode = WindowsInput.Native.VirtualKeyCode.RIGHT } }
        };

        public bool rightScroll { get; set; } = true;
        public bool reverseVerticalScroll { get; set; } = false;


    }
}
