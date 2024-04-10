using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Handheld_Hardware_Tools.Classes.Devices
{
    public class AMD_Generic : Device
    {
        public AMD_Generic() 
        {

            deviceFanControlCapable = false;
            deviceGyroControlCapable = false;
            deviceName = "AMD Generic Device";
            maxGPUFrequency = 2200;
        }

     

    }
}
