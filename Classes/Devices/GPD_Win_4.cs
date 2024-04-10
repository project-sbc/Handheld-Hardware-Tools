using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Handheld_Hardware_Tools.Classes.Devices
{
    public class GPD_Win_4 : Device
    {
        public GPD_Win_4() 
        {
            deviceFanControlCapable = true;
            deviceGyroControlCapable = true;
            deviceName = "GPD Win 4";
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
