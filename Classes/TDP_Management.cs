using Everything_Handhelds_Tool.Classes.Devices;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        public int[] ReadTDP(bool read)
        {
            int[] tdps = new int[2];
            Device device = ((MainWindow)Application.Current.MainWindow).device;
            if (device.cpuType == "AMD")
            {
                return ReadAMDTDP();
            }

            return tdps;
        }

        private int[] ReadAMDTDP()
        {
            int[] tdps = new int[2];
            string ryzenAdj = appDir + "\\Resources\\AMD\\RyzenAdj\\ryzenadj.exe";
            string command = " -i";

            string result = (new Run_CLI()).RunCommand(command, true, ryzenAdj);
            return AMDParseTDP(result);
        }

        private int[] AMDParseTDP(string result)
        {
            int[] tdps = new int[2];
            tdps[0] = -1;
            tdps[1] = -1;
            using (StringReader reader = new StringReader(result))
            {
                string line;
                string tdp;
                while ((line = reader.ReadLine()) != null)
                {
                    double n = 0;
                    if (line.Contains("STAPM LIMIT"))
                    {

                        tdp = line;
                        tdp = tdp.Replace("|", "");
                        tdp = tdp.Replace("STAPM LIMIT", "");
                        tdp = tdp.Replace(" ", "");
                        tdp = tdp.Replace("stapm-limit", "");
                        tdp = tdp.Substring(0, tdp.IndexOf("."));
                        tdps[0] = (int)Math.Round(Convert.ToDouble(tdp), 0);

                    }
                    if (line.Contains("PPT LIMIT SLOW"))
                    {
                        tdp = line;
                        tdp = tdp.Replace("|", "");
                        tdp = tdp.Replace("PPT LIMIT SLOW", "");
                        tdp = tdp.Replace(" ", "");
                        tdp = tdp.Replace("slow-limit", "");
                        tdp = tdp.Substring(0, tdp.IndexOf("."));
                        tdps[1] = (int)Math.Round(Convert.ToDouble(tdp), 0);
                        return tdps;
                    }
                }
            }

            return tdps;
        } 

        public void changeTDP(int value)
        {


            tdp = value;

        }
    }
}
