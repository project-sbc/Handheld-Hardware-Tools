using Everything_Handhelds_Tool.Classes.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Everything_Handhelds_Tool.Classes
{
    public class Powercfg_Management
    {
        private static Powercfg_Management _instance = null;
        private static readonly object lockObj = new object();
        private Powercfg_Management()
        {
        }
        public static Powercfg_Management Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Powercfg_Management();
                        }
                    }
                }
                return _instance;
            }
        }

        #region common powercfg handler
        private int PowercfgResultHandler(string result)
        {
            string Power = SystemParameters.PowerLineStatus.ToString();
            string[] resultArray = result.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string tempString = "";
            int value = -1;
            if (resultArray.Length > 2)
            {
                if (Power == "Online")
                {
                    tempString = resultArray[resultArray.Length - 2];
                }
                else
                {
                    tempString = resultArray[resultArray.Length - 1];
                }
                tempString = tempString.Replace("\r", "");
                tempString = tempString.Replace(" ", "");
                tempString = tempString.Substring(tempString.IndexOf(":") + 1, 10).Trim();
                value = Convert.ToInt32(tempString, 16);
            }
            return value;

        }

        private void PowercfgChangeValueHandler(string value, string powercfgString)
        {
            string Power = SystemParameters.PowerLineStatus.ToString();
            if (Power == "Online")
            {
                Run_CLI.Instance.RunCommand(" /setacvalueindex scheme_current sub_processor " + powercfgString + " " + value, false, "C:\\windows\\system32\\powercfg.exe", 1000);
            }
            else
            {
                Run_CLI.Instance.RunCommand(" /setdcvalueindex scheme_current sub_processor " + powercfgString + " " + value, false, "C:\\windows\\system32\\powercfg.exe", 1000);
            }
            Run_CLI.Instance.RunCommand(" /S scheme_current", false, "C:\\windows\\system32\\powercfg.exe", 1000);
        }

        #endregion

        #region coreparking
        private int activeCores = 1;
        public int maxNumberCores = new ManagementObjectSearcher("Select * from Win32_Processor").Get().Cast<ManagementBaseObject>().Sum(item => int.Parse(item["NumberOfCores"].ToString()));


        public int ReadAndReturnActiveCores()
        {
            ReadActiveCores();
            return activeCores;
        }

        private void ReadActiveCores()
        {
            string Power = SystemParameters.PowerLineStatus.ToString();
            //call common string handler for powercfg
            string result = Run_CLI.Instance.RunCommand(" -Q SCHEME_CURRENT sub_processor CPMAXCORES", true, "C:\\windows\\system32\\powercfg.exe", 1000).Trim();
            //value is current active core count
            int value = PowercfgResultHandler(result);
            double calculateCores = (value * maxNumberCores) / 100;
            calculateCores = Math.Round(calculateCores, 0);

            activeCores = (int)calculateCores;

        }

        public void ChangeActiveCores(int cores)
        {
            string CorePercentage = (Math.Round((double)(cores / maxNumberCores), 2) * 100).ToString();
            PowercfgChangeValueHandler(CorePercentage, "CPMAXCORES");
            PowercfgChangeValueHandler(CorePercentage, "CPMINCORES");      
            
            activeCores = cores;
        }

        #endregion

        #region EPP
        private int EPP = 0;
     
        public int ReadAndReturnEPP()
        {
            ReadEPP();
            return EPP;
        }

        private void ReadEPP()
        {
            string Power = SystemParameters.PowerLineStatus.ToString();
            string result = Run_CLI.Instance.RunCommand(" -Q SCHEME_CURRENT SUB_PROCESSOR PERFEPP", true, "C:\\windows\\system32\\powercfg.exe", 1000).Trim();

            int intEPP = PowercfgResultHandler(result);
       
            EPP = intEPP;
        }
  
        public void ChangeEPP(int epp)
        {
            PowercfgChangeValueHandler(epp.ToString(), "PERFEPP");
            PowercfgChangeValueHandler(epp.ToString(), "PERFEPP1");
            EPP = epp;
        }

        #endregion

        #region cpu clock speed set
        private int maxCPUClock = 0;
        private bool turboEnabled = true;
        public bool ReadAndReturnCPUClockStatus()
        {
            //this routine checks max cpu clock and cpu processor speed to determine if max cpu is
            //unlimited and if turbo is enabled or disabled

            ReadMaxCPUClock();
            ReadTurboEnabled();
            if (turboEnabled && maxCPUClock == 0) { return true; }
            else { return false; }
        }

        public int ReadAndReturnMaxCPUClock()
        {
            ReadMaxCPUClock();
            return maxCPUClock;
        }

        private void ReadMaxCPUClock()
        {
            string result = Run_CLI.Instance.RunCommand(" -Q SCHEME_CURRENT sub_processor PROCFREQMAX", true, "C:\\windows\\system32\\powercfg.exe", 1000).Trim();
            int value = PowercfgResultHandler(result);
            maxCPUClock = value;
        }
        private void ReadTurboEnabled()
        {
            string result = Run_CLI.Instance.RunCommand(" -Q SCHEME_CURRENT sub_processor PROCTHROTTLEMAX", true, "C:\\windows\\system32\\powercfg.exe", 1000).Trim();
            int value = PowercfgResultHandler(result);
            if (value < 100)
            {
                turboEnabled = false;
            }
            else
            {
                turboEnabled = true;
            }
        }
        public void ChangeMaxCPUClock(int value)
        {
            //when setting the max cpu clock, make sure the procthrottle max isn't 99% or less
            //this will mess with clock limits above the base cpu frequency
            //so call both a max cpu clock AND set procthrottlemax to 100% so that doesnt accidentally limit us

            PowercfgChangeValueHandler("100", "PROCTHROTTLEMAX");
            PowercfgChangeValueHandler(value.ToString(), "PROCFREQMAX");
            maxCPUClock = value;
        }
        public void ChangeTurboEnabledState(bool value)
        {
            //Convert the bool value of enabling turbo to either 100% state for allow turbo
            //or 99% state to disable turbo
            //MAKE SURE PROCFREQMAX IS 0!!!! changing processor max frequency is one step below enable disable turbo
            //That will be a separate call the user will make afterward
            //setting procthrottlemax to 99 will disable turbo regardless of max proc freq so just leave max proc freq out
            //by setting to 0
            int sendValue = 100;
            if (!value) { sendValue = 99; }
            PowercfgChangeValueHandler("0", "PROCFREQMAX");
            PowercfgChangeValueHandler(sendValue.ToString(), "PROCTHROTTLEMAX");
            turboEnabled = value;
        }

        #endregion


        #region set high perf/balanced/saver plan
        public void SetHighPerformancePlan()
        {
            string result = Run_CLI.Instance.RunCommand(" -s 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c", true, "C:\\windows\\system32\\powercfg.exe", 1000);
        }
        public void SetBalancedPlan()
        {
            Run_CLI.Instance.RunCommand(" -s 381b4222-f694-41f0-9685-ff5bb260df2e​", false, "C:\\windows\\system32\\powercfg.exe", 1000);
        }
        public void SetBatterySaver()
        {
            string result = Run_CLI.Instance.RunCommand(" -s a1841308-3541-4fab-bc81-f71556f20b4a", true, "C:\\windows\\system32\\powercfg.exe", 1000);
        }
        #endregion

    }
}
