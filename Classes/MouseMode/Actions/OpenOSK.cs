using Handheld_Hardware_Tools.AppWindows.OSK;
using Handheld_Hardware_Tools.AppWindows.OSK.Keyboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WindowsInput;
using WindowsInput.Native;

namespace Handheld_Hardware_Tools.Classes.MouseMode.Actions
{
    public class OpenOSK : MouseAction
    {

        public override void ClickEvent()
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                if (!Application.Current.Windows.OfType<OSK>().Any())
                {
                    OSK osk = new OSK();
                    osk.Show();
                }

            });
        }
    }
}
