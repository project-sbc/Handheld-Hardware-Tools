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
    public class ScrollLeft : MouseAction
    {
        public int scrollSpeed { get; set; } = 5;
        public ScrollLeft()
        {
            mouseActionName = "ScrollLeft";
        }
        public override void ClickEvent()
        {
            InputSimulator inputSimulator =Local_Object.Instance.GetMainWindowInputSimulator();
            if (inputSimulator != null)
            {
                inputSimulator.Mouse.HorizontalScroll(scrollSpeed);
            }

        }
        public override string ReturnArgument()
        {
            return scrollSpeed.ToString();
        }
    }
}
