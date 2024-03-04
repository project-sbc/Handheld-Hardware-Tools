using Everything_Handhelds_Tool.AppWindows.OSK;
using Everything_Handhelds_Tool.AppWindows.OSK.Keyboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace Everything_Handhelds_Tool.Classes.MouseMode
{
    public class MouseLeftClick : MouseAction
    {
        
        public override void ClickEvent()
        {
            KeyboardPage keyboardPage = Local_Object.Instance.GetOSKKeyboard();
            if (keyboardPage != null )
            {
                keyboardPage.inputSimulator.Mouse.LeftButtonClick();
            }
        }
    }
}
