using Everything_Handhelds_Tool.Classes.Devices;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Everything_Handhelds_Tool.Classes
{
    public class PowerBalance_Management
    {

        private static PowerBalance_Management _instance = null;
        private static readonly object lockObj = new object();
        private PowerBalance_Management()
        {
        }
        public static PowerBalance_Management Instance
        {
            get
            {
                if (_instance == null )
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new PowerBalance_Management();
                        }
                    }
                }
                return _instance;
            }
        }



        //Start the actual code and not just singleton stuff
        private string appDir = AppDomain.CurrentDomain.BaseDirectory;
        private int powerBalanceCPU = 0;
        private int powerBalanceGPU = 0;

        public string cpuType;

        public int ReturnCpuPowerBalance()
        {
            //does not READ cpu power bal just returns value
            return powerBalanceCPU;
        }
        public int ReturnGpuPowerBalance()
        {
            //does not READ Gpu power bal just returns value
            return powerBalanceGPU;
        }

        public int ReadAndReturnCpuPowerBalance()
        {
            ReadIntelPowerBalanceCPU();
            return powerBalanceCPU;
        }

        public int ReadAndReturnGpuPowerBalance()
        {
            ReadIntelPowerBalanceGPU();
            return powerBalanceGPU;
        }
       
        private string ConvertHexToInt(string result)
        {
            //Convert integer TDP value to Hex for rw.exe
            //Must use formula (TDP in watt   *1000/125) +32768 and convert to hex
            try
            {
               
                return result;

            }
            catch (Exception ex)
            {

                return "Error";
            }
        }
      
        private string ConvertPowerToHexMSR(int tdp)
        {
            //Convert integer TDP value to Hex for rw.exe
            //Must use formula (TDP in watt   *1000/125) +32768 and convert to hex
            try
            {
                int newTDP = (tdp * 8);
                return newTDP.ToString("X");

            }
            catch (Exception ex)
            {

                return "Error";
            }
        }

        public void ChangePowerBalance(int value, string cpuOrGPU)
        {
            if (cpuOrGPU != null && value >=0 && value <= 31)
            {
                if (cpuOrGPU == "CPU")
                {
                    changePowerBalance(value, "0x0000063a 0x00000000");
                }
                if (cpuOrGPU == "GPU")
                {
                    changePowerBalance(value, "0x00000642 0x00000000");
                }
            }
        }

        private void changePowerBalance(int value, string addressCPUorGPU)
        {

            string processMSR = "";
            string commandArguments = "";
            try
            {
                string hexvalue;
                hexvalue = "0x" + value.ToString("X");


                commandArguments = " -s write " + addressCPUorGPU + " " + hexvalue;
                processMSR = appDir + "\\Resources\\Intel\\MSR\\msr-cmd.exe";
                //Log_Writer.writeLog("Change TDP MSR processMSR=" + processMSR + "; Hex PL1 PL2=" + hexPL1 + "," + hexPL2);
                Run_CLI.Instance.RunCommand(commandArguments, false, processMSR);
                //Log_Writer.writeLog("Change TDP MSR complete");
                Task.Delay(100);
            }
            catch (Exception ex)
            {


            }


        }
        private void ReadIntelPowerBalanceGPU()
        {
            try
            {
                string processMSR = appDir + "\\Resources\\Intel\\MSR\\msr-cmd.exe";
                string commandArguments = " read 0x642 0x00000000";
                //Log_Writer.writeLog("Read TDP MSR processMSR=" + processMSR + "; commandarugmentPL1=" + commandArguments);
                string result = Run_CLI.Instance.RunCommand(commandArguments, true, processMSR);
                if (result != null)
                {
                    string findstring = "0x00000642 0x00000000 ";
                    int index = result.IndexOf(findstring)+findstring.Length+2;
                    result = result.Substring(index, 8);

                    int value = Convert.ToInt32(result, 16);

                    powerBalanceGPU = value;

                }

            }
            catch (Exception ex)
            {

            }
        }

        private void ReadIntelPowerBalanceCPU()
        {
            try
            {
                string processMSR = appDir + "\\Resources\\Intel\\MSR\\msr-cmd.exe";
                string commandArguments = " read 0x63a 0x00000000";
                //Log_Writer.writeLog("Read TDP MSR processMSR=" + processMSR + "; commandarugmentPL1=" + commandArguments);
                string result = Run_CLI.Instance.RunCommand(commandArguments, true, processMSR);
                if (result != null)
                {
                    //MessageBox.Show("ADD THE POWER BALANCE READ STUFF HERE YOU DUMMY + " + result);
                    string findstring = "0x0000063a 0x00000000 ";
                    int index = result.IndexOf(findstring) + findstring.Length+2;
                    result = result.Substring(index, 8);

                    int value = Convert.ToInt32(result,16);

                    powerBalanceCPU = value;

                }

            }
            catch (Exception ex)
            {
            
            }
        }
        private string ParseHexFromResultMSRConvertToTDP(string result, bool isPL1)
        {
            int FindString = -1;
            string hexResult = "";
            try
            {

                float intResult;
                if (isPL1)
                {
                    FindString = result.IndexOf("0x00000610") + 29;
                    hexResult = result.Substring(FindString, 3).Trim();
                    intResult = (Convert.ToInt32(hexResult, 16)) / 8;
                    return intResult.ToString();

                }
                else
                {
                    FindString = result.IndexOf("0x00000610") + 18;
                    hexResult = result.Substring(FindString, 3).Trim();
                    intResult = (Convert.ToInt32(hexResult, 16)) / 8;
                    return intResult.ToString();
                }
            }
            catch (Exception ex)
            {
              
                return "Error";
            }



        }
     
      

      




      

       
      
      
      
    }
}
