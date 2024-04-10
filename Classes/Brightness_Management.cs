using Handheld_Hardware_Tools.Classes.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Handheld_Hardware_Tools.Classes
{
    public class Brightness_Management
    {
        private static Brightness_Management _instance = null;
        private static readonly object lockObj = new object();
        private Brightness_Management()
        {
        }
        public static Brightness_Management Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Brightness_Management();
                        }
                    }
                }
                return _instance;
            }
        }

        public int brightness = -1;

        public int ReturnBrightness()
        {
            GetBrightness();
            return brightness;
        }

        public bool IsDisplayBrightnessControllable()
        {
            //DEFAULT is -1, if there is error getting brightness then display is not adjustable and value stays -1
            //otherwise it will get a 0-100 value
            GetBrightness();
            if (brightness < 0) { return false; } else { return true;}
        }

        public void GetBrightness()
        {
            try
            {
                ManagementScope scope;
                SelectQuery query;

                scope = new ManagementScope("root\\WMI");
                query = new SelectQuery("SELECT * FROM WmiMonitorBrightness");

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                {
                    using (ManagementObjectCollection objectCollection = searcher.Get())
                    {
                        foreach (ManagementObject mObj in objectCollection)
                        {
                            Console.WriteLine(mObj.ClassPath);
                            foreach (var item in mObj.Properties)
                            {
                                Console.WriteLine(item.Name + " " + item.Value.ToString());
                                if (item.Name == "CurrentBrightness")
                                {
                                     brightness = Convert.ToInt32(item.Value); 
                                }
                            }
                        }
                    }
                }
            }
            catch { }

        }


      
        public void SetBrightness(int intBrightness)
        {

            try
            {


                ManagementClass mclass = new ManagementClass("WmiMonitorBrightnessMethods");
                mclass.Scope = new ManagementScope("\\\\.\\root\\wmi");

                ManagementObjectCollection instances = mclass.GetInstances();

                foreach (ManagementObject instance in instances)
                {
                    byte brightness = Convert.ToByte(intBrightness);
                    UInt64 timeout = 1;

                    object[] args = new object[] { timeout, brightness };

                    instance.InvokeMethod("WmiSetBrightness", args);
                }
                
            }
            catch
            {




            }







        }

    }
}
