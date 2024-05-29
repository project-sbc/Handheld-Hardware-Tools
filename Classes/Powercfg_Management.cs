using Handheld_Hardware_Tools.Classes.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PowerManagerAPI;

using System.Runtime.InteropServices;
using Windows.System.Power;
using AudioSwitcher.AudioApi;

namespace Handheld_Hardware_Tools.Classes
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


        public int GetPlanSetting(Guid activeplan, SettingSubgroup settingSubgroup, Setting setting, PowerMode powerMode)
        {
            //main routine to get power plan settings
            //if Guid is empty then get active plan (in case there is a specific plan to change)
            //if powermode is None then get the powermode by power line status

            if (activeplan == Guid.Empty)
            {
                activeplan = PowerplanHelper.GetActiveScheme();
            }
           
            if (powerMode == PowerMode.None)
            {
                if (SystemParameters.PowerLineStatus == PowerLineStatus.Online)
                {
                    powerMode = PowerMode.AC;
                }
                else
                {
                    powerMode = PowerMode.DC;
                }
            }
           
            return (int)PowerManager.GetPlanSetting(activeplan, settingSubgroup, setting, powerMode);

        }
        public void SetPlanSetting(Guid activeplan, SettingSubgroup settingSubgroup, Setting setting, PowerMode powerMode, uint value)
        {
            //main routine to sets power plan settings
            //if Guid is empty then get active plan (in case there is a specific plan to change)
            //if powermode is None then get the powermode by power line status
            if (activeplan == Guid.Empty)
            {
                activeplan = PowerplanHelper.GetActiveScheme();
            }

            if (powerMode == PowerMode.None)
            {
                if (SystemParameters.PowerLineStatus == PowerLineStatus.Online)
                {
                    powerMode = PowerMode.AC;
                }
                else
                {
                    powerMode = PowerMode.DC;
                }
            }

            PowerManager.SetPlanSetting(activeplan, settingSubgroup, setting, powerMode, value);

            PowerManager.SetActivePlan(activeplan);
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
            int value = GetPlanSetting(Guid.Empty, SettingSubgroup.PROCESSOR_SETTINGS_SUBGROUP, Setting.MAXCORES, PowerMode.None);

            //value is a percentage of total number active cores (i.e. 50% on a 8C CPU means 4C are active), so we need to convert back to a raw core number value
            double calculateCores = (value * maxNumberCores) / 100;
            calculateCores = Math.Round(calculateCores, 0);

            activeCores = (int)calculateCores;

        }

        public void ChangeActiveCores(int cores)
        {
            //convert core number to total percentage using CPU total # cores
         
            uint CorePercentage = ((uint)Math.Round((double)(cores*100 / maxNumberCores), 0) );

            SetPlanSetting(Guid.Empty, SettingSubgroup.PROCESSOR_SETTINGS_SUBGROUP, Setting.MAXCORES, PowerMode.None, CorePercentage);
            SetPlanSetting(Guid.Empty, SettingSubgroup.PROCESSOR_SETTINGS_SUBGROUP, Setting.MINCORES, PowerMode.None, CorePercentage);
                       
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
            int value = GetPlanSetting(Guid.Empty, SettingSubgroup.PROCESSOR_SETTINGS_SUBGROUP, Setting.EPP, PowerMode.None);

            EPP = value;
        }
  
        public void ChangeEPP(int epp)
        {
            SetPlanSetting(Guid.Empty, SettingSubgroup.PROCESSOR_SETTINGS_SUBGROUP, Setting.EPP, PowerMode.None, (uint)epp);
            SetPlanSetting(Guid.Empty, SettingSubgroup.PROCESSOR_SETTINGS_SUBGROUP, Setting.EPP1, PowerMode.None, (uint)epp);

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
                SetPlanSetting(Guid.Empty, SettingSubgroup.PROCESSOR_SETTINGS_SUBGROUP, Setting.PROCFREQMAX, PowerMode.None, 0);
            }
            else
            {
                Devices.Device device = Local_Object.Instance.GetMainWindowDevice();
                if (device != null)
                {
                    if (device.maxNonTurboCPUFrequnecy > -1)
                    {
                        SetPlanSetting(Guid.Empty, SettingSubgroup.PROCESSOR_SETTINGS_SUBGROUP, Setting.PROCFREQMAX, PowerMode.None, (uint)device.maxNonTurboCPUFrequnecy);
                      
                    }
                }
                
            }
        }

        private void ReadMaxCPUClock()
        {
            int value = GetPlanSetting(Guid.Empty, SettingSubgroup.PROCESSOR_SETTINGS_SUBGROUP, Setting.PROCFREQMAX, PowerMode.None);
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
            SetPlanSetting(Guid.Empty, SettingSubgroup.PROCESSOR_SETTINGS_SUBGROUP, Setting.PROCFREQMAX, PowerMode.None, (uint)value);
            SetPlanSetting(Guid.Empty, SettingSubgroup.PROCESSOR_SETTINGS_SUBGROUP, Setting.PROCTHROTTLEMAX, PowerMode.None, 100);

            maxCPUClock = value;
        }


        #endregion


        #region set high perf/balanced/saver plan or get active plan

        #endregion


        #region import/set hyatice plan



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
                string guid = PowerplanHelper.GetGUID(resultImport);
                if (guid != "")
                {
                    string resultNameChange = Run_CLI.Instance.RunCommand(" -changename " + guid + " \"Optimized Power Saver\"", true, "C:\\windows\\system32\\powercfg.exe", 2000, true);
                    string resultSetPlan = Run_CLI.Instance.RunCommand(" -s " + guid, true, "C:\\windows\\system32\\powercfg.exe", 2000, true);

                }



            }

        }

        #endregion
    }
    public static class PowerplanHelper
    {
        //guid values for diff power parameters used in reading/changing value
        public static Guid GUID_MIN_CORE_PARKING = new Guid("0cc5b647-c1df-4637-891a-dec35c318583");
        public static Guid GUID_MAX_CORE_PARKING = new Guid("0012ee47-9041-4b5d-9b77-535fba8b1442");
        public static Guid GUID_EPP1 = new Guid("4faab71a-8bfb-4828-855e-c05f3c977f9a");
        public static Guid GUID_MAX_PROC_FREQ = new Guid("893dee8e-2bef-41e0-89c6-b55d0929964b");


        public static Guid PROC_SUB_GROUP = new Guid("{54533251-82be-4824-96c1-47b60b740d00}");




        //used to change AC values of power plan
        [DllImport("powrprof.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern uint PowerWriteACValueIndex(IntPtr RootPowerKey, ref Guid SchemeGuid, ref Guid SubGroupOfPowerSetting, ref Guid PowerSettingGuid, uint AcValueIndex);
        //used to change DC values of power plan
        [DllImport("powrprof.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern uint PowerWriteDCValueIndex(IntPtr RootPowerKey, ref Guid SchemeGuid, ref Guid SubGroupOfPowerSetting, ref Guid PowerSettingGuid, uint AcValueIndex);


        //used to read AC values of power plan
        [DllImport("powrprof.dll")]
        public static extern uint PowerReadACValue(IntPtr RootPowerKey,ref Guid SchemeGuid,ref Guid SubGroupOfPowerSettingGuid, ref Guid PowerSettingGuid, ref int Type, ref int Buffer, ref uint BufferSize);

        //used to read DC values of power plan
        [DllImport("powrprof.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern uint PowerReadDCValueIndex(IntPtr RootPowerKey, ref Guid SchemeGuid, ref Guid SubGroupOfPowerSetting, ref Guid PowerSettingGuid, out uint AcValueIndex);







        [DllImport("powrprof.dll", CharSet = CharSet.Unicode)]
        public static extern uint PowerGetActiveScheme(IntPtr UserPowerKey, ref Guid SchemeGuid);

        [DllImport("powrprof.dll", CharSet = CharSet.Unicode)]
        public static extern uint PowerEnumerate(IntPtr RootPowerKey, ref Guid SchemeGuid, ref Guid SubGroupOfPowerSettingsGuid, uint AccessFlags, uint Index, ref Guid Buffer, ref uint BufferSize);

        [DllImport("powrprof.dll", CharSet = CharSet.Unicode)]
        public static extern uint PowerReadFriendlyName(IntPtr RootPowerKey, ref Guid SchemeGuid, IntPtr SubGroupOfPowerSettingsGuid, IntPtr PowerSettingGuid, IntPtr Buffer, ref uint BufferSize);

        [DllImport("powrprof.dll", CharSet = CharSet.Unicode)]
        public static extern uint PowerSetActiveScheme(IntPtr UserPowerKey, ref Guid SchemeGuid);

        public static bool SetActivePowerScheme(Guid schemeGuid)
        {
            uint result = PowerSetActiveScheme(IntPtr.Zero, ref schemeGuid);
            return result == 0; // 0 indicates success
        }
        public static Dictionary<Guid, string> GetPowerSchemes()
        {
            Powercfg_Management.Instance.importHyaticePowerPlan();

            Dictionary<Guid, string> schemes = new Dictionary<Guid, string>();
            uint index = 0;
            uint bufferSize = 128; // Start with a small buffer size

            // Loop through all power schemes
            while (true)
            {   
                Guid schemeGuid = Guid.Empty;
                Guid emptyGuid = Guid.Empty;
                Guid emptyGuid2 = Guid.Empty;

                uint result = PowerEnumerate(IntPtr.Zero, ref emptyGuid, ref emptyGuid2, 16 /*ACCESS_SCHEME*/, index, ref schemeGuid, ref bufferSize);


                if (result != 0)
                {
                    //lets check for saver, balanced, and performance the default plans


                    Guid powerSaver = new Guid("a1841308-3541-4fab-bc81-f71556f20b4a");
                    Guid balanced = new Guid("381b4222-f694-41f0-9685-ff5bb260df2e");
                    Guid highPerformance = new Guid("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c");

                    if (!schemes.ContainsKey(powerSaver))
                    {
                        string keyName = GetPowerSchemeName(powerSaver);
                        schemes.Add(powerSaver, keyName);
                    }
                    if (!schemes.ContainsKey(balanced))
                    {
                        string keyName = GetPowerSchemeName(balanced);
                        schemes.Add(balanced, keyName);
                    }
                    if (!schemes.ContainsKey(highPerformance))
                    {
                        string keyName = GetPowerSchemeName(highPerformance);
                        schemes.Add(highPerformance, keyName);
                    }
                  

                    if (result == 2) // No more power schemes available
                        break;
                    else
                    {
                        Console.WriteLine($"Failed to enumerate power schemes. Error code: {result}");
                        return schemes;
                    }
                }

                string schemeName = GetPowerSchemeName(schemeGuid);
                schemes.Add(schemeGuid, schemeName);
                index++;
            }



            return schemes;

        }
        public static string GetActivePowerSchemeName()
        {
            //i need to use the powercfg command to get the active plan name.
            //see comment in GetActiveScheme for reason why
            Guid activeScheme = GetActiveScheme();

            return GetPowerSchemeName(activeScheme);
        }

        public static Guid GetActiveScheme()
        {
            //KEEP THIS, i dont have a good way to get active GUID when the device has a power plan POLICY in place
            //this seems crazy but on the legion go using win api to get active plan GUID returns a power policy
            //i can't actually convert the power plan policy to the active scheme GUID
            string activePlan = "";
            string result = Run_CLI.Instance.RunCommand(" getactivescheme", true, "C:\\windows\\system32\\powercfg.exe", 1000);


            string GUID = GetGUID(result);


            return new Guid(GUID);

        }
        public static string GetGUID(string input)
        {//used to get GUID from GetActiveScheme
            if (input.Contains(":"))
            {
                return input.Substring(input.IndexOf(":") + 1, 37).Trim();
            }
            else { return ""; }
        }

        public static string GetPowerSchemeName(Guid schemeGuid)
        {
            uint bufferSize = 128;

            // Retrieve the size of the buffer required to hold the scheme name
            uint result = PowerReadFriendlyName(IntPtr.Zero, ref schemeGuid, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, ref bufferSize);
            if (result != 0)
                return "Unknown";

            // Allocate memory for the scheme name
            IntPtr bufferPtr = Marshal.AllocHGlobal((int)bufferSize);

            // Retrieve the friendly name of the power scheme
            result = PowerReadFriendlyName(IntPtr.Zero, ref schemeGuid, IntPtr.Zero, IntPtr.Zero, bufferPtr, ref bufferSize);
            if (result != 0)
            {
                Marshal.FreeHGlobal(bufferPtr);
                return "Unknown";
            }

            // Convert the buffer to a string
            string schemeName = Marshal.PtrToStringUni(bufferPtr);

            // Free allocated memory
            Marshal.FreeHGlobal(bufferPtr);

            return schemeName;
        }
    }
}
