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

        public override void ClickEvent()
        {
            InputSimulator inputSimulator = Local_Object.Instance.GetMainWindowInputSimulator();
            if (inputSimulator != null)
            {
                inputSimulator.Mouse.RightButtonClick();
            }
        }
    }
}
