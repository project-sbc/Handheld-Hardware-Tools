using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Everything_Handhelds_Tool.Classes
{
    public class Wifi_Management
    {

        private static Wifi_Management _instance = null;
        private static readonly object lockObj = new object();
        private Wifi_Management()
        {
        }
        public static Wifi_Management Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Wifi_Management();
                        }
                    }
                }
                return _instance;
            }
        }


        public Process wifiDirectProcess = null;
        private string appDir = AppDomain.CurrentDomain.BaseDirectory;
        public void StartWifi()
        {
            CheckAPExeIsRunning();
            if (wifiDirectProcess == null)
            {
                StartWifiExe();
            }
            else
            {
                if (!CheckAPExeStatus())
                {
                    StartWifiExe();
                }
            }
         
        }

        public void StopWifi() 
        {
            if (wifiDirectProcess != null)
            {
                SendConsoleCommand("stop");
                SendConsoleCommand("exit");
            }

        }

        private void StartWifiExe()
        {
            //If wifiDirectProcess is null use this to start the wifi process
            if (wifiDirectProcess == null)
            {
                ProcessStartInfo psf = new System.Diagnostics.ProcessStartInfo();
                psf.FileName = appDir + "\\Resources\\WifiDirect\\WifiDirectLegacyAP.exe";
                psf.WindowStyle = ProcessWindowStyle.Hidden;
                psf.CreateNoWindow = true;
                psf.UseShellExecute = false;
                psf.RedirectStandardInput = true;
                wifiDirectProcess = new Process();
                wifiDirectProcess.StartInfo = psf;
                wifiDirectProcess.Start();
            }
            SendConsoleCommand("ssid EverythingHandheldsWifi");
            SendConsoleCommand("password everythinghandhelds");
            SendConsoleCommand("start");
        }

        public bool IsWifiRunning()
        {
            CheckAPExeIsRunning();
            if (wifiDirectProcess != null) { return true; } else { return false; }
        }
        private void CheckAPExeIsRunning()
        {
            if (wifiDirectProcess == null) 
            {
                Process[] pname = Process.GetProcessesByName("WiFiDirectLegacyAP");
                if (pname.Length > 0)
                {
                    foreach( Process proc in pname)
                    {
                        proc.Kill();
                    }
                }

            }
           
        }

        private bool CheckAPExeStatus()
        {
            if (SendAndReadConsoleCommand("status") == "True")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SendConsoleCommand(string message)
        {
            wifiDirectProcess.StandardInput.WriteLine(message);
        }
        private string SendAndReadConsoleCommand(string message)
        {
            wifiDirectProcess.StandardInput.WriteLine(message);
            return wifiDirectProcess.StandardOutput.ReadToEnd();
        }

        [DllImport("kernel32.dll")]
        public static extern bool AttachConsole(int processID);
       
    }
}
