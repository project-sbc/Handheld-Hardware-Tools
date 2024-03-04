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
           
        };

        public bool rightScroll { get; set; } = true;
        public bool reverseVerticalScroll { get; set; } = false;
    }
}
