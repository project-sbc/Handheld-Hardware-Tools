using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Handheld_Hardware_Tools.Classes.Devices
{
    public class AyaNeo_Flip_DS : Device
    {
        public AyaNeo_Flip_DS() 
        {
            deviceFanControlCapable = false;
            deviceGyroControlCapable = false;
            deviceName = "Aya Neo Flip DS";
            maxGPUFrequency = 2200;


            ScreenProgram_Management.OpenAyaFlipDSWindowOnSpecificMonitor(960,640);
        }

      

    }
}
