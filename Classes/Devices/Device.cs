using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everything_Handhelds_Tool.Classes.Devices
{
    public class Device
    {
        //Base class for the device constructor


        public bool deviceFanControlCapable { get; set; } = false;
        public bool deviceGyroControlCapable { get; set; } = false;
        public string deviceName { get; set; } = string.Empty;
        public string cpuName { get; set; } = string.Empty;
        public string cpuType { get; set; } = string.Empty;
        public string mchBar { get; set; } = string.Empty;
        public virtual bool ManualFanControlEnabled() 
        {
            //This will return false for any device that doesnt have fan control
            //I will override this routine on devices with fan control so this only applies
            //to devices without fan control capability
            return false;
        }
        public virtual void EnableManualFanControl() { }
        public virtual void EnableAutomaticFanControl() { }


    }

    
}
