using Handheld_Hardware_Tools.AppWindows.OSK;
using Handheld_Hardware_Tools.AppWindows.OSK.Keyboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace Handheld_Hardware_Tools.Classes.MouseMode.Actions
{
    public class MouseRightClick : MouseAction
    {
        public MouseRightClick()
        {
            mouseActionName = "MouseRightClick";
        }
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
