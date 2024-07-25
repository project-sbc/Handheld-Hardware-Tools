using Handheld_Hardware_Tools.AppWindows.OSK.Keyboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace Handheld_Hardware_Tools.Classes.MouseMode.Actions
{
    public class KeyboardClick : MouseAction
    {
        public KeyboardClick()
        {
            mouseActionName = "KeyboardClick";
        }
        public VirtualKeyCode virtualKeyCode { get; set; }
        public override void ClickEvent()
        {
            InputSimulator inputSimulator =Local_Object.Instance.GetMainWindowInputSimulator();
            if (inputSimulator != null)
            {
                inputSimulator.Keyboard.KeyPress(virtualKeyCode);
            }

        }
        public override string ReturnArgument()
        {
            return virtualKeyCode.ToString();
        }
    }
}
