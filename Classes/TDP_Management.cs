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
        private bool unableToReadTDPDevice = false;
        public string cpuType;

        public int ReturnSustainedTDP()
        {
            //does not READ TDP just returns value
            return tdpSustained;
        }
        public int ReadAndReturnSustainedTDP()
        {
            ReadTDP("Sustained");
            return tdpSustained;
        }
        public int ReadAndReturnBoostTDP()
        {
            ReadTDP("Boost");
            return tdpBoost;
        }
        private void ReadTDP(string type)
        {
           
            if (cpuType == "AMD")
            {//always read both sustained and boost on AMD, its fast
                ReadAMDTDP();
            }
            if (cpuType == "Intel")
            {
                ReadIntelTDP(type);
            }
        }

        private void ReadIntelTDP(string type)
        {
            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");
            string mchBar = DeviceMCHBAR();
            if (settings.intelTDPType.Contains("MMIO"))
            {
                ReadIntelMMIOTDP(mchBar, type);
            }
            else
            {
                ReadIntelMSRTDP(type);
            }
        }
        private string ConvertTDPToHexMMIO(int tdp)
        {
            //Convert integer TDP value to Hex for rw.exe
            //Must use formula (TDP in watt   *1000/125) +32768 and convert to hex
            try
            {
                int newTDP = (tdp * 1000 / 125) + 32768;
                return newTDP.ToString("X");

            }
            catch (Exception ex)
            {

                return "Error";
            }
        }
        private void RunIntelTDPChangeMMIOKX(int pl1TDP, int pl2TDP, string mchBar)
        {
            string processKX = "";
            string hexPL1 = "";
            string hexPL2 = "";
            string commandArgumentsPL1 = "";
            string commandArgumentsPL2 = "";
            try
            {

                processKX = appDir + "\\Resources\\Intel\\KX\\KX.exe";
                hexPL1 = ConvertTDPToHexMMIO(pl1TDP);
                hexPL2 = ConvertTDPToHexMMIO(pl2TDP);
                //Log_Writer.writeLog("Change TDP MMIO processKX=" + processKX + "; Hex PL1 PL2=" + hexPL1 + "," + hexPL2 );
                if (hexPL1 != "Error" && hexPL2 != "Error" && mchBar != "")
                {
                    commandArgumentsPL1 = " /wrmem16 " + mchBar + "a0 0x" + hexPL1;
                    //Log_Writer.writeLog("Change TDP MMIO commandargumentPL1=" + commandArgumentsPL1);
                    Run_CLI.Instance.RunCommand(commandArgumentsPL1, true, processKX);
                    Task.Delay(500);
                    commandArgumentsPL2 = " /wrmem16 " + mchBar + "a4 0x" + hexPL2;
                    //Log_Writer.writeLog("Change TDP MMIO commandargumentPL2=" + commandArgumentsPL2);
                    Run_CLI.Instance.RunCommand(commandArgumentsPL2, true, processKX);
                    //Log_Writer.writeLog("Change TDP MMIO complete");
                    Task.Delay(100);
                }
            }
            catch (Exception ex)
            {
             

            }


        }
        private string ConvertTDPToHexMSR(int tdp)
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

        private void RunIntelTDPChangeMSR(int pl1TDP, int pl2TDP)
        {

            string processMSR = "";
            string hexPL1 = "";
            string hexPL2 = "";
            string commandArguments = "";
            try
            {

                hexPL1 = ConvertTDPToHexMSR(pl1TDP);
                hexPL2 = ConvertTDPToHexMSR(pl2TDP);

                if (hexPL1 != "Error" && hexPL2 != "Error")
                {
                    if (hexPL1.Length < 3)
                    {
                        if (hexPL1.Length == 1) { hexPL1 = "00" + hexPL1; }
                        if (hexPL1.Length == 2) { hexPL1 = "0" + hexPL1; }
                    }
                    if (hexPL2.Length < 3)
                    {
                        if (hexPL2.Length == 1) { hexPL2 = "00" + hexPL2; }
                        if (hexPL2.Length == 2) { hexPL2 = "0" + hexPL2; }
                    }

                    commandArguments = " -s write 0x610 0x00438" + hexPL2 + " 0x00dd8" + hexPL1;
                    processMSR = appDir + "\\Resources\\Intel\\MSR\\msr-cmd.exe";
                    //Log_Writer.writeLog("Change TDP MSR processMSR=" + processMSR + "; Hex PL1 PL2=" + hexPL1 + "," + hexPL2);
                    Run_CLI.Instance.RunCommand(commandArguments, false, processMSR);
                    //Log_Writer.writeLog("Change TDP MSR complete");
                    Task.Delay(100);
                }
            }
            catch (Exception ex)
            {


            }


        }

        private void ReadIntelMSRTDP(string type)
        {
            try
            {
                string processMSR = appDir + "\\Resources\\Intel\\MSR\\msr-cmd.exe";
                string commandArguments = " read 0x610";
                //Log_Writer.writeLog("Read TDP MSR processMSR=" + processMSR + "; commandarugmentPL1=" + commandArguments);
                string result = Run_CLI.Instance.RunCommand(commandArguments, true, processMSR);
                if (result != null)
                {
                    //MSR is FAST!! dont need to implement a sustained or boost read, both can be done at same time
                    double dblPL1 = Convert.ToDouble(ParseHexFromResultMSRConvertToTDP(result, true));
                    tdpSustained = (int)Math.Round(dblPL1, 0);

                    double dblPL2 = Convert.ToDouble(ParseHexFromResultMSRConvertToTDP(result, false));
                    tdpBoost = (int)Math.Round(dblPL2, 0);


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
        private void ReadIntelMMIOTDP(string mchBar, string type)
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
                    if (type == "Sustained" || type == "Both")
                    {
                        commandArgumentsPL1 = " /rdmem16 " + mchBar + "a0";

                        resultPL1 = Run_CLI.Instance.RunCommand(commandArgumentsPL1, true, processKX);

                        if (resultPL1 != null)
                        {

                            double dblPL1 = Convert.ToDouble(ParseHexFromResultMMIOConvertToTDPKX(resultPL1, true));
                            tdpSustained = (int)Math.Round(dblPL1, 0);

                        }
                    }
                    if (type == "Both") { Task.Delay(300); }
                  
                    if (type == "Boost" || type == "Both")
                    {
                        commandArgumentsPL2 = " /rdmem16 " + mchBar + "a4";

                        resultPL2 = Run_CLI.Instance.RunCommand(commandArgumentsPL2, true, processKX);
                        if (resultPL2 != null)
                        {

                            double dblPL2 = Convert.ToDouble(ParseHexFromResultMMIOConvertToTDPKX(resultPL2, false));
                            tdpBoost = (int)Math.Round(dblPL2, 0);
                        }
                    }
                   
                }
                else
                {
                    //if mchbar is null

                }
            }
            catch (Exception ex)
            {
              

            }
        }
        private string ParseHexFromResultMMIOConvertToTDPKX(string result, bool isPL1)
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
            QuickAccessMenu mw = Local_Object.Instance.GetQAMWindow();

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
                        return;
                    }
                }

                //THERE IS A RETURN IN THE SECOND SECTION THAT IF IT CAN READ THE TDP WONT COME TO THIS SECTIOn
                //THIS SECTION IS FOR DEVICES LIKE STEAM DECK AND AMD Z1E WHERE YOU CAN"T READ THE TDP
                unableToReadTDPDevice = true;
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


        private void ChangeIntelTDP(int tdp1, int tdp2)
        {
            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");

            if (settings.intelTDPType.Contains("MMIO"))
            {
                Task.Run(()=>RunIntelTDPChangeMMIOKX(tdp1, tdp2, DeviceMCHBAR()));
            }
            if (settings.intelTDPType.Contains("MSR"))
            {
                Task.Run(() => RunIntelTDPChangeMSR(tdp1, tdp2));
            }



        }


        public void ValidateTDPChange(int tdp1, int tdp2)
        {
            //this makes sure you dont exceed min or max values
            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");

            if (tdp1 > settings.maxTDP) { tdp1 = settings.maxTDP; }
            if (tdp1 < settings.minTDP) { tdp1 = settings.minTDP; }
            if (tdp2 > settings.maxTDP) { tdp2 = settings.maxTDP; }
            if (tdp2 < settings.minTDP) { tdp2 = settings.minTDP ; }

            //make sure to sync them regardless of what the value is if setting is set like that
            if (settings.syncSustainedBoostTDP)
            {
                tdp2 = tdp1;
            }

            ChangeTDP(tdp1, tdp2);
        }

        private void ChangeTDP(int tdp1, int tdp2)
        {
            ///keep below for the moment
           
          

            //set the values now
            tdpBoost = tdp1;
            tdpSustained = tdp2;
            if (cpuType == "AMD")
            {
                ChangeAMDTDP(tdp1, tdp2);
        
            }
            if (cpuType == "Intel")
            {
               ChangeIntelTDP(tdp1,tdp2);
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
