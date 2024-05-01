using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handheld_Hardware_Tools.Classes
{
    public class Steam_Management
    {

        private static Steam_Management _instance = null;
        private static readonly object lockObj = new object();
        private Steam_Management()
        {
        }
        public static Steam_Management Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Steam_Management();
                        }
                    }
                }
                return _instance;
            }
        }

        private bool steamRunning()
        {
            Process[] pname = Process.GetProcessesByName("Steam");
            if (pname.Length != 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public void openSteamBigPicture()
        {

            string steamInstallPath = GetSteamDirectory();

            if (steamInstallPath != "")
            {
                if (steamRunning())
                {
                    Run_CLI.Instance.RunCommand(" \"steam://open/bigpicture\"", false, steamInstallPath + "\\Steam.exe", 6000, false);

                }
                else
                {
                    Run_CLI.Instance.RunCommand(" -bigpicture", false, steamInstallPath + "\\Steam.exe", 6000, false);
                }
            }



        }
        public string GetSteamDirectory()
        {
            try
            {
                //32 and 64 bit install locations in registry
                string steam32 = "SOFTWARE\\VALVE\\STEAM";
                string steam64 = "SOFTWARE\\Wow6432Node\\Valve\\STEAM";
                string installPath = "";
                RegistryKey keyReg = Registry.LocalMachine.OpenSubKey(steam64);
                if (keyReg != null)
                {
                    if (keyReg.GetValue("InstallPath") != null)
                    {
                        return keyReg.GetValue("InstallPath").ToString();
        
                    }
                    else
                    {
                        //if null might be 32 bit
                        keyReg = Registry.LocalMachine.OpenSubKey(steam32);
                        if (keyReg != null)
                        {
                            if (keyReg.GetValue("InstallPath") != null)
                            {
                                return keyReg.GetValue("InstallPath").ToString();
                
                            }
                        }


                    }
                }
            }
            catch { }

            return "";

        }



    }
}
