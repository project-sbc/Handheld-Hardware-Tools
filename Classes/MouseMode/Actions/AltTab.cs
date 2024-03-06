using Everything_Handhelds_Tool.AppWindows.OSK.Keyboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace Everything_Handhelds_Tool.Classes.MouseMode.Actions
{
    public class AltTab : MouseAction
    {
        DispatcherTimer unpressAltTimer = null;
        public override void ClickEvent()
        {
            
            InputSimulator inputSimulator =Local_Object.Instance.GetMainWindowInputSimulator();
            if (inputSimulator != null)
            {
                if (!Keyboard.IsKeyDown(Key.LeftAlt))
                {
                    inputSimulator.Keyboard.KeyDown(VirtualKeyCode.LMENU);
                }
                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.TAB);

                if (unpressAltTimer == null)
                {
                    unpressAltTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
                    unpressAltTimer.Tick += UnpressAltTimer_Tick;
                    unpressAltTimer.Start();
                }
                else
                {//reset everytime tab is clicked
                    unpressAltTimer.Stop();
                    unpressAltTimer.Start();
            
                }


            }
           

        }

        private void UnpressAltTimer_Tick(object? sender, EventArgs e)
        {
            
            InputSimulator inputSimulator = Local_Object.Instance.GetMainWindowInputSimulator();
            if (inputSimulator != null)
            {
                if (Keyboard.IsKeyDown(Key.LeftAlt))
                {
                    inputSimulator.Keyboard.KeyUp(VirtualKeyCode.LMENU);
                }
                
            }
            unpressAltTimer.Stop();
            unpressAltTimer.Tick -= UnpressAltTimer_Tick;
            unpressAltTimer = null;
        }
    }
}
