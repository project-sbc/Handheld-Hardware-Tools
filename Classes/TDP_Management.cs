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
    public class TDP_Management
    {

        private static TDP_Management _instance = null;
        private static readonly object lockObj = new object();
        private TDP_Management()
        {
        }
        public static TDP_Management Instance
        {
            get
            {
                if (_instance == null )
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new TDP_Management();
                        }
                    }
                }
                return _instance;
            }
        }



        //Start the actual code and not just singleton stuff
        private string appDir = AppDomain.CurrentDomain.BaseDirectory;
        private int tdpSustained = 15;
        private int tdpBoost = 15;



        public int ReturnSustainedTDP()
        {
            //does not READ TDP just returns value
            return tdpSustained;
        }

        public int ReadAndReturnSustainedTDP()
        {
            ReadTDP();
            return tdpSustained;
        }
        public int ReadAndReturnBoostTDP()
        {
            ReadTDP();
            return tdpBoost;
        }
        private void ReadTDP()
        {
            Device device = ((MainWindow)Application.Current.MainWindow).device;
            if (device.cpuType == "AMD")
            {
                ReadAMDTDP();
            }
            if (device.cpuType == "Intel")
            {
                ReadIntelTDP();
            }
        }

        private void ReadIntelTDP()
        {
            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");
            string mchBar = DeviceMCHBAR();
            if (settings.intelTDPType.Contains("MMIO"))
            {
                ReadIntelMMIOTDP(mchBar);
            }
            else
            {
                ReadIntelMSRTDP();
            }
        }

        private void ReadIntelMSRTDP()
        {
            try
            {
                string processMSR = appDir + "\\Resources\\Intel\\MSR\\msr-cmd.exe";
                string commandArguments = " read 0x610";
                //Log_Writer.writeLog("Read TDP MSR processMSR=" + processMSR + "; commandarugmentPL1=" + commandArguments);
                string result = Run_CLI.Instance.RunCommand(commandArguments, true, processMSR);
                if (result != null)
                {
                    //Log_Writer.writeLog("Read TDP MSR result=" + result);
                    double dblPL1 = Convert.ToDouble(parseHexFromResultMSRConvertToTDP(result, true));
                    tdpSustained = (int)Math.Round(dblPL1,0);

                    double dblPL2 = Convert.ToDouble(parseHexFromResultMSRConvertToTDP(result, false));
                    tdpBoost = (int)Math.Round(dblPL2, 0);
                    //Log_Writer.writeLog("Read TDP MSR PL1=" + dblPL1.ToString() + "; PL1=" + dblPL2.ToString());
                }

            }
            catch (Exception ex)
            {
            
            }
        }
        private string parseHexFromResultMSRConvertToTDP(string result, bool isPL1)
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

        private void ReadIntelMMIOTDP(string mchBar)
        {
            string processKX = "";
            string commandArgumentsPL1 = "";
            string resultPL1 = "";
            string commandArgumentsPL2 = "";
            string resultPL2 = "";

            try
            {
                processKX = appDir + "\\Resources\\Intel\\KX\\KX.exe";
                if (mchBar != "")
                {
                    commandArgumentsPL1 = " /rdmem16 " + mchBar + "a0";
                    //Log_Writer.writeLog("Read TDP MMIO processKX=" + processKX + "; commandarugmentPL1=" + commandArgumentsPL1 );
                    resultPL1 = Run_CLI.Instance.RunCommand(commandArgumentsPL1, true, processKX);

                    if (resultPL1 != null)
                    {
                        //Log_Writer.writeLog("Read TDP MMIO resultpl1=" + resultPL1);
                        double dblPL1 = Convert.ToDouble(parseHexFromResultMMIOConvertToTDPKX(resultPL1, true));
                        tdpSustained = (int)Math.Round(dblPL1,0);
                        //Log_Writer.writeLog("Read TDP MMIO pl1=" + dblPL1.ToString());
                    }
                    Task.Delay(300);
                    commandArgumentsPL2 = " /rdmem16 " + mchBar + "a4";
                    //Log_Writer.writeLog("Read TDP MMIO processKX=" + processKX + "; commandarugmentPL2=" + commandArgumentsPL2);
              
                    resultPL2 = Run_CLI.Instance.RunCommand(commandArgumentsPL2, true, processKX);
                    if (resultPL2 != null)
                    {
                        //Log_Writer.writeLog("Read TDP MMIO resultpl2=" + resultPL2);
                        double dblPL2 = Convert.ToDouble(parseHexFromResultMMIOConvertToTDPKX(resultPL2, false));
                        tdpBoost = (int)Math.Round(dblPL2, 0);
                        //Log_Writer.writeLog("Read TDP MMIO pl2=" + dblPL2.ToString());
                    }
                }
                else
                {
                    

                }
            }
            catch (Exception ex)
            {
              

            }
        }
        private string parseHexFromResultMMIOConvertToTDPKX(string result, bool isPL1)
        {
            try
            {
                int FindString;
                string hexResult;
                float intResult;
                if (isPL1)
                {
                    FindString = result.IndexOf("Memory Data") + 22;
                    hexResult = result.Substring(FindString, 7).Trim();
                    intResult = (Convert.ToInt32(hexResult, 16) - 32768) / 8;
                    return intResult.ToString();

                }
                else
                {
                    FindString = result.IndexOf("Memory Data") + 22;
                    hexResult = result.Substring(FindString, 7).Trim();
                    intResult = (Convert.ToInt32(hexResult, 16) - 32768) / 8;
                    return intResult.ToString();
                }
            }
            catch (Exception ex)
            {
                return "Error";
            }



        }

        private string DeviceMCHBAR()
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;

            if (mw!= null)
            {
                if (mw.device.mchBar != null)
                {
                    return mw.device.mchBar;
                }
            }

            return "";
        }


        #region read change AMD TDP

        private void ReadAMDTDP()
        {
            string result = Run_CLI.Instance.RunCommand(" -i", true, appDir + "\\Resources\\AMD\\RyzenAdj\\ryzenadj.exe");
            AMDParseTDP(result);
        }

        private void AMDParseTDP(string result)
        {
            using (StringReader reader = new StringReader(result))
            {
                string line;
                string tdp;
                while ((line = reader.ReadLine()) != null)
                {
                    double n = 0;
                    if (line.Contains("STAPM LIMIT"))
                    {
                        tdp = line.Replace("|", "").Replace("STAPM LIMIT", "").Replace(" ", "").Replace("stapm-limit", "");
                        tdp = tdp.Substring(0, tdp.IndexOf("."));
                        tdpSustained = (int)Math.Round(Convert.ToDouble(tdp), 0);
                    }
                    if (line.Contains("PPT LIMIT SLOW"))
                    {
                        tdp = line.Replace("|", "").Replace("PPT LIMIT SLOW", "").Replace(" ", "").Replace("slow-limit", "");
                        tdp = tdp.Substring(0, tdp.IndexOf("."));
                        tdpBoost = (int)Math.Round(Convert.ToDouble(tdp), 0);
                    }
                }
            }
        }
        private void ChangeAMDTDP(int pl1TDP, int pl2TDP)
        {
            string processRyzenAdj = "";
            string result = "";
            string commandArguments = "";
            try
            {
                processRyzenAdj = appDir + "\\Resources\\AMD\\RyzenAdj\\ryzenadj.exe";

                //set the limits one at a time to prevent crash or glitches, put 30 ms delay to prevent errors
                commandArguments = " --stapm-limit=" + (pl1TDP * 1000).ToString();
                result = Run_CLI.Instance.RunCommand(commandArguments, true, processRyzenAdj);
                Thread.Sleep(30);
                commandArguments = " --slow-limit=" + (pl2TDP * 1000).ToString();
                result = Run_CLI.Instance.RunCommand(commandArguments, true, processRyzenAdj);
                Thread.Sleep(30);
                commandArguments = " --fast-limit=" + (pl2TDP * 1000).ToString();
                result = Run_CLI.Instance.RunCommand(commandArguments, true, processRyzenAdj);
                Thread.Sleep(30);
                commandArguments = " --apu-slow-limit=" + (pl1TDP * 1000).ToString();
                result = Run_CLI.Instance.RunCommand(commandArguments, true, processRyzenAdj);
                Thread.Sleep(30);


            }
            catch (Exception ex)
            {

            }
        }
        #endregion





        public void ValidateTDPChange(int tdp1, int tdp2)
        {
            //this makes sure you dont exceed min or max values
            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");

            if (tdp1 > settings.maxTDP) { tdp1 = settings.maxTDP; }
            if (tdp1 < settings.minTDP) { tdp1 = settings.minTDP; }
            if (tdp2 > settings.maxTDP) { tdp2 = settings.maxTDP; }
            if (tdp2 < settings.minTDP) { tdp2 = settings.minTDP ; }

            ChangeTDP(tdp1, tdp2);
        }

        private void ChangeTDP(int tdp1, int tdp2)
        {
            ///keep below for the moment
            Device device = ((MainWindow)Application.Current.MainWindow).device;
            if (device.cpuType == "AMD")
            {
                ChangeAMDTDP(tdp1,tdp2);
            }
            if (device.cpuType == "Intel")
            {

            }

        }

        public void ChangeBoostTDP(int value)
        {//feeder routine for just boost tdp change
            if (tdpSustained > value) { tdpSustained = value; }
            ValidateTDPChange(tdpSustained, value);
        }
        public void ChangeSustainedBoostTDP(int tdp1, int tdp2)
        {//feeder routine for both sustained and boost tdp
            if (tdp1 > tdp2) { tdp1 = tdp2; }
            ValidateTDPChange(tdp1, tdp2);
        }

        public void ChangeSustainedTDP(int value)
        {//feeder routine for sustained tdp
            if (value > tdpBoost) { tdpBoost = value; }
            ValidateTDPChange(value, tdpBoost);
        }

      
      
      
    }
}
