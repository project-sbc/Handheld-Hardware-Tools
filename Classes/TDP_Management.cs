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
        }

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

      
      
    }
}
