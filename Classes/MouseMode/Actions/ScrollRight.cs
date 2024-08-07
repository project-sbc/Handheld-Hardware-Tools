﻿using Handheld_Hardware_Tools.AppWindows.OSK.Keyboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace Handheld_Hardware_Tools.Classes.MouseMode.Actions
{
    public class ScrollRight : MouseAction
    {
        public int scrollSpeed { get; set; } = -5;
        public ScrollRight()
        {
            mouseActionName = "ScrollRight";
        }
        public override void ClickEvent()
        {
            InputSimulator inputSimulator =Local_Object.Instance.GetMainWindowInputSimulator();
            if (inputSimulator != null)
            {
                inputSimulator.Mouse.VerticalScroll(scrollSpeed);
            }

        }
        public override string ReturnArgument()
        {
            return scrollSpeed.ToString();
        }
    }
}
