﻿using Everything_Handhelds_Tool.Classes.Devices;
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
    }
}