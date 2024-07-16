using Handheld_Hardware_Tools.AppWindows.OSK;
using Handheld_Hardware_Tools.AppWindows.OSK.Keyboards;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace Handheld_Hardware_Tools.Classes.MouseMode.Actions
{
    public class MouseLeftClick : MouseAction
    {
        public MouseLeftClick()
        {
            mouseActionName = "MouseLeftClick";
        }
        public override void ClickEvent()
        {
            InputSimulator inputSimulator = Local_Object.Instance.GetMainWindowInputSimulator();
            if (inputSimulator != null)
            {
                inputSimulator.Mouse.LeftButtonDown();
            }
          
        }

        public void ReleaseClick()
        {
            InputSimulator inputSimulator = Local_Object.Instance.GetMainWindowInputSimulator();
            if (inputSimulator != null)
            {
                inputSimulator.Mouse.LeftButtonUp();
            }

        }
    }
}
