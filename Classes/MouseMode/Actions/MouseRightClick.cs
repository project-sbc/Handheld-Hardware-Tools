using Everything_Handhelds_Tool.AppWindows.OSK;
using Everything_Handhelds_Tool.AppWindows.OSK.Keyboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace Everything_Handhelds_Tool.Classes.MouseMode.Actions
{
    public class MouseRightClick : MouseAction
    {

        public override void ClickEvent(bool pressed)
        {
            KeyboardPage keyboardPage = Local_Object.Instance.GetOSKKeyboard();
            if (keyboardPage != null)
            {
                if (pressed)
                {
                    keyboardPage.inputSimulator.Mouse.RightButtonDown();
                }
                else
                {
                    keyboardPage.inputSimulator.Mouse.RightButtonUp();
                }
            }
        }
    }
}
