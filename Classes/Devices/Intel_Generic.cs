using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Handheld_Hardware_Tools.Classes.Devices
{
    public class Intel_Generic : Device
    {
        public Intel_Generic() 
        {
            deviceFanControlCapable = false;
            deviceGyroControlCapable = false;
            deviceName = "Intel Generic Device";
            
        }

    }
}
