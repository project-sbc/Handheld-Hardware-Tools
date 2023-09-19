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
            //this makes sure you dont set tdp2 less than tdp1 and compares to settings and just makes the values line up
            if (tdp1 > tdp2) { tdp1 = tdpSustained; }
        }
        public void ChangeBoostTDP(int value)
        {//feeder routine for just boost tdp change
            ValidateTDPChange(tdpSustained, value);
        }
        public void ChangeSustainedBoostTDP(int tdp1, int tdp2)
        {//feeder routine for both sustained and boost tdp
            ValidateTDPChange(tdp1, tdp2);
        }

        public void ChangeSustainedTDP(int value)
        {//feeder routine for sustained tdp
            ValidateTDPChange(value, tdpBoost);

            ///keep below for the moment
            Device device = ((MainWindow)Application.Current.MainWindow).device;
            if (device.cpuType == "AMD")
            {
                ChangeAMDTDP();
            }

            tdpSustained = value;

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
