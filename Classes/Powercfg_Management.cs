using Everything_Handhelds_Tool.Classes.Devices;
using Everything_Handhelds_Tool.Classes.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PowerManagerAPI;
using System.Diagnostics;
using SharpDX;

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

        //i havent finished commonizing everything, this can be a later to do, the idea is that I can do all powercfg stuff through these two routines
        //its a nice idea but needs further development. I am using parts of this already though

        private int PowercfgReadValueHandler(string queryString, string power = null)
        {
            //queryString is the command you would put into cmd powercfg tool i.e.  -Q SCHEME_CURRENT sub_processor CPMAXCORES
            //USE THIS WITH THE PowercfgResultHandler below to parse out the value you want
            //power is optional argument to specify AC vs DC power plan value. if null then it grabs current state
            string results = Run_CLI.Instance.RunCommand(queryString, true, "C:\\windows\\system32\\powercfg.exe", 1000).Trim();
            return PowercfgResultHandler(results, power);
        }


        private int PowercfgResultHandler(string result, string power = null)
        {
            //power is optional argument to specify AC vs DC power plan value. if null then it grabs current state
            if (power == null) { power = SystemParameters.PowerLineStatus.ToString(); }
            string[] resultArray = result.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string tempString = "";
            int value = -1;
            if (resultArray.Length > 2)
            {
                if (power == "Online")
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
                Run_CLI.Instance.RunCommand(" /setacvalueindex scheme_current sub_processor " + powercfgString + " " + value, true, "C:\\windows\\system32\\powercfg.exe", 3000, true);
            }
            else
            {
                Run_CLI.Instance.RunCommand(" /setdcvalueindex scheme_current sub_processor " + powercfgString + " " + value, true, "C:\\windows\\system32\\powercfg.exe", 3000, true);
            }
            Run_CLI.Instance.RunCommand(" /S scheme_current", true, "C:\\windows\\system32\\powercfg.exe", 3000, true);
        }

        public void UnhidePowerCfgSettings()
        {
            //unhides epp, active core, etc, otherwise these values can't be changed

         
            Run_CLI.Instance.RunCommand(" -attributes SUB_PROCESSOR PROCFREQMAX -ATTRIB_HIDE", false, "C:\\windows\\system32\\powercfg.exe", 3000);
            Run_CLI.Instance.RunCommand(" -attributes SUB_PROCESSOR PROCTHROTTLEMAX -ATTRIB_HIDE", false, "C:\\windows\\system32\\powercfg.exe", 3000);
            Run_CLI.Instance.RunCommand(" -attributes SUB_PROCESSOR CPMAXCORES -ATTRIB_HIDE", false, "C:\\windows\\system32\\powercfg.exe", 3000);
            Run_CLI.Instance.RunCommand(" -attributes SUB_PROCESSOR CPMINCORES -ATTRIB_HIDE", false, "C:\\windows\\system32\\powercfg.exe", 3000);
            Run_CLI.Instance.RunCommand(" -attributes SUB_PROCESSOR PERFEPP -ATTRIB_HIDE", false, "C:\\windows\\system32\\powercfg.exe", 3000);
            Run_CLI.Instance.RunCommand(" -attributes SUB_PROCESSOR PERFEPP1 -ATTRIB_HIDE", false, "C:\\windows\\system32\\powercfg.exe", 3000);

        }

        #endregion

        #region coreparking
        private int activeCores = 1;
        public int maxNumberCores = new ManagementObjectSearcher("Select * from Win32_Processor").Get().Cast<ManagementBaseObject>().Sum(item => int.Parse(item["NumberOfCores"].ToString()));

        public int ReturnMaxNumberActiveCores()
        {
            return maxNumberCores;
        }

        public int ReadAndReturnActiveCores()
        {
            ReadActiveCores();
            return activeCores;
        }

        private void ReadActiveCores()
        {
            //value is current active core count
            int value = PowercfgReadValueHandler(" -Q SCHEME_CURRENT sub_processor CPMAXCORES");

            //value is a percentage of total number active cores (i.e. 50% on a 8C CPU means 4C are active), so we need to convert back to a raw core number value
            double calculateCores = (value * maxNumberCores) / 100;
            calculateCores = Math.Round(calculateCores, 0);

            activeCores = (int)calculateCores;

        }

        public void ChangeActiveCores(int cores)
        {
            //convert core number to total percentage using CPU total # cores
         
            string CorePercentage = (Math.Round((double)(cores*100 / maxNumberCores), 0) ).ToString();
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
            int intEPP = PowercfgReadValueHandler(" -Q SCHEME_CURRENT SUB_PROCESSOR PERFEPP");

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
            return turboEnabled;
         
        }

        public int ReadAndReturnMaxCPUClock()
        {
            ReadMaxCPUClock();
            return maxCPUClock;
        }
        public int ReturnMaxCPUClock()
        {
            return maxCPUClock;
        }
        public bool ReturnMTurboEnabled()
        {
            return turboEnabled;
        }

        public bool ReadAndReturnCPUTurboStatus()
        {
            //use this for the TurboCPU_ToggleSwitch control

            ReadMaxCPUClock();
            return turboEnabled;

        }
        public void ToggleCPUTurboStatus(bool toggle)
        { //use this for the TurboCPU_ToggleSwitch control
       
            if (toggle) 
            {
                PowercfgChangeValueHandler("0", "PROCFREQMAX");
            }
            else
            {
                Device device = Local_Object.Instance.GetMainWindowDevice();
                if (device != null)
                {
                    if (device.maxNonTurboCPUFrequnecy > -1)
                    {
                        PowercfgChangeValueHandler(device.maxNonTurboCPUFrequnecy.ToString(), "PROCFREQMAX");
                    }
                }
                
            }
        }

        private void ReadMaxCPUClock()
        {
            string result = Run_CLI.Instance.RunCommand(" -Q SCHEME_CURRENT sub_processor PROCFREQMAX", true, "C:\\windows\\system32\\powercfg.exe", 1000).Trim();
            int value = PowercfgResultHandler(result);
            maxCPUClock = value;

            if (maxCPUClock != 0)
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
       

        #endregion


        #region set high perf/balanced/saver plan or get active plan
        public void SetHighPerformancePlan()
        {
            string result = Run_CLI.Instance.RunCommand(" -s 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c", true, "C:\\windows\\system32\\powercfg.exe", 1000);
        }
        public void SetBalancedPlan()
        {
            string result = Run_CLI.Instance.RunCommand(" -s 381b4222-f694-41f0-9685-ff5bb260df2e​", true, "C:\\windows\\system32\\powercfg.exe", 1000);
        }
        public void SetPowerSaver()
        {
            string result = Run_CLI.Instance.RunCommand(" -s a1841308-3541-4fab-bc81-f71556f20b4a", true, "C:\\windows\\system32\\powercfg.exe", 1000);
        }
        #endregion


        #region import/set hyatice plan
        public void SetHyaticePowerPlanModePowercfg()
        {

            importHyaticePowerPlan();

            string result = Run_CLI.Instance.RunCommand(" /l", true, "C:\\windows\\system32\\powercfg.exe", 1000);

            string[] array = result.Split('\n');

            foreach (string str in array)
            {
                if (str.Contains("Optimized Power Saver"))
                {

                    Run_CLI.Instance.RunCommand(" /s " + GetGUID(str), false, "C:\\windows\\system32\\powercfg.exe", 1000);
                }


            }

        }
        public string GetActiveScheme()
        {

            string activePlan = "";
            string result = Run_CLI.Instance.RunCommand(" getactivescheme", true, "C:\\windows\\system32\\powercfg.exe", 1000);

            string name = GetSchemeName(result);
            string GUID = GetGUID(result);
      
            switch (GUID)
            {
                case "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c":
                    return "High_Performance";
                    break;
                case "381b4222-f694-41f0-9685-ff5bb260df2e":
                    return "Balanced";
                    break;
                case "a1841308-3541-4fab-bc81-f71556f20b4a":
                    return "Power_Saver";
                    break;
                default:
                    if (name == "Optimized Power Saver")
                    {
                        return "Optimized_Power_Saver";
                    }
                    break;
            }

            return activePlan;

        }

        private string GetGUID(string input)
        {
            if (input.Contains(":"))
            {
                return input.Substring(input.IndexOf(":") + 1, 37).Trim();
            }
            else { return ""; }
        }
        private string GetSchemeName(string input)
        {
            if (input.Contains("(") && input.Contains(")"))
            {
                int startIndex = input.IndexOf("(") + 1;
                int numberOfCharacters = input.IndexOf(")") - startIndex;
                return input.Substring(startIndex, numberOfCharacters).Trim();
            }
            else { return ""; }
        }
        private bool HyaticePowerPlanInstalled()
        {
            string result = Run_CLI.Instance.RunCommand(" /l", true, "C:\\windows\\system32\\powercfg.exe", 1000);

            if (result.Contains("Optimized Power Saver"))
            {
                return true;
            }
            else { return false; }
        }

        public void importHyaticePowerPlan()
        {
            if (!HyaticePowerPlanInstalled())
            {
                string appDir = AppDomain.CurrentDomain.BaseDirectory;
                string directory = @Path.Combine(appDir + "Resources\\HyaticePowerPlan\\HyaticePowerPlan2.pow");

                string resultImport = Run_CLI.Instance.RunCommand(" -import " + directory, true, "C:\\windows\\system32\\powercfg.exe", 2000, true);
                string guid = GetGUID(resultImport);
                if (guid != "")
                {
                    string resultNameChange = Run_CLI.Instance.RunCommand(" -changename " + guid + " \"Optimized Power Saver\"", true, "C:\\windows\\system32\\powercfg.exe", 2000, true);
                    string resultSetPlan = Run_CLI.Instance.RunCommand(" -s " + guid, true, "C:\\windows\\system32\\powercfg.exe", 2000, true);

                }



            }

        }

        #endregion
    }
}
