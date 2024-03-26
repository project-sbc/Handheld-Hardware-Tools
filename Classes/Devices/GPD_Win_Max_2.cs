using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Everything_Handhelds_Tool.Classes.Devices
{
    public class GPD_Win_Max_2 : Device
    {
        public GPD_Win_Max_2() 
        {
            deviceFanControlCapable = true;
            deviceGyroControlCapable = true;
            deviceName = "GPD Win Max 2";
            maxGPUFrequency = 2200;
        }

        public override void EnableAutomaticFanControl()
        {
            
        }
        public override void EnableManualFanControl()
        {
            
        }

    }
}
