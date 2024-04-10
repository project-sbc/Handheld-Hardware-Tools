using Handheld_Hardware_Tools.Classes.Devices;
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

namespace Handheld_Hardware_Tools.Classes
{
    public class GPUClock_Management
    {

        private static GPUClock_Management _instance = null;
        private static readonly object lockObj = new object();
        private GPUClock_Management()
        {
        }
        public static GPUClock_Management Instance
        {
            get
            {
                if (_instance == null )
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new GPUClock_Management();
                        }
                    }
                }
                return _instance;
            }
        }



        //Start the actual code and not just singleton stuff
        private string appDir = AppDomain.CurrentDomain.BaseDirectory;
        private int gpuClock = -1;
    

        public int ReturnGPUClock()
        {
            //does not READ TDP just returns value
            return gpuClock;
        }
       

     
        public void ChangeGPUClock(int gpuCLK)
        {
            string processRyzenAdj = "";
            string result = "";
            string commandArguments = "";
            try
            {//small fail safe to prevent an accidental way overclock
                if (gpuCLK > 3000) { gpuCLK = 3000; }


                processRyzenAdj = appDir + "\\Resources\\AMD\\RyzenAdj\\ryzenadj.exe";

                //set the limits one at a time to prevent crash or glitches, put 30 ms delay to prevent errors
                commandArguments = "  --gfx-clk=" + (gpuCLK).ToString();
                result = Run_CLI.Instance.RunCommand(commandArguments, true, processRyzenAdj);
                Thread.Sleep(30);
                gpuClock = gpuCLK;
            }
            catch (Exception ex)
            {

            }
        }
       
      
    }
}
