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
            { "Y" , new OpenOSK() },
            { "DPadUp", new KeyboardClick(){virtualKeyCode = WindowsInput.Native.VirtualKeyCode.UP } },
            { "DPadDown", new KeyboardClick(){virtualKeyCode = WindowsInput.Native.VirtualKeyCode.DOWN } },
            { "DPadLeft", new KeyboardClick(){virtualKeyCode = WindowsInput.Native.VirtualKeyCode.LEFT } },
            { "DPadRight", new KeyboardClick(){virtualKeyCode = WindowsInput.Native.VirtualKeyCode.RIGHT } },
            { "X", new AltTab() }
        };

        public bool rightScroll { get; set; } = true;
        public bool reverseVerticalScroll { get; set; } = false;

 
        public double sensitivityMouseValue { get; set; } = 15;
        public double sensitivityScrollValue { get; set; } = 3;
        //read as percentage of total joystick range (i.e. 4 = 4% which is 0.04*32600 for maximum joystick output)
        public double deadZone { get; set; } = 4;

        //these are in percentage, so make sure to divide by 100
       
        public double[] yValuesMouse { get; set; } = { 3, 5, 10, 15, 20, 25, 30, 35, 45, 55 };
        public double[] yValuesScroll { get; set; } = { 3, 5, 10, 15, 20, 25, 30, 35, 45, 55 };
    }
}
