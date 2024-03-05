using Everything_Handhelds_Tool.AppWindows.OSK.Keyboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace Everything_Handhelds_Tool.Classes.MouseMode.Actions
{
    public class KeyboardClick : MouseAction
    {
        public VirtualKeyCode virtualKeyCode { get; set; }
        public override void ClickEvent(bool pressed)
        {
            KeyboardPage keyboardPage = Local_Object.Instance.GetOSKKeyboard();
            if (keyboardPage != null)
            {
                if (pressed)
                {
                    keyboardPage.inputSimulator.Keyboard.KeyPress(virtualKeyCode);
                }
                
            }
        }
    }
}
