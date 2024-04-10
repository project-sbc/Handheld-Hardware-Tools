using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Handheld_Hardware_Tools.Classes.Actions.ActionClass
{
    public class Toggle_WinOSK : Action
    {
        public Toggle_WinOSK()
        {
            actionName = "Toggle_WinOSK";
            //arguments = new List<string>();
        }
        public override void OnActivate(string quickActionWheelParameter = "")
        {

            Process[] pname = Process.GetProcessesByName("TabTip");

            if (pname.Length == 0)
            {
                ProcessStartInfo psi = new ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles), "microsoft shared\\ink\\TabTip.exe")
                };
                System.Diagnostics.Process.Start(psi);

            }
            Thread.Sleep(300);

            pname = Process.GetProcessesByName("TabTip");
            if (pname.Length != 0)
            {
                //Thread tempThread = new Thread(OSKTablet.startOSK);
                //tempThread.SetApartmentState(ApartmentState.STA);

                //tempThread.Start();
                OSKTablet.ToggleOSK();
            }

          
        }
    }
    public static class OSKTablet
    {

        static ITipInvocation iti = (ITipInvocation)new UIHostNoLaunch();

        public static void ToggleOSK()
        {

            iti.Toggle(GetDesktopWindow());


        }

        [ComImport, Guid("4ce576fa-83dc-4F88-951c-9d0782b4e376")]
        class UIHostNoLaunch
        {
        }

        [ComImport, Guid("37c994e7-432b-4834-a2f7-dce1f13b834b")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface ITipInvocation
        {
            void Toggle(IntPtr hwnd);
        }

        [DllImport("user32.dll", SetLastError = false)]
        static extern IntPtr GetDesktopWindow();
    }
}
