using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Everything_Handhelds_Tool.Classes.Devices
{
    public class ON_One_X_Player_2 : Device
    {
        public ON_One_X_Player_2() 
        {
            deviceFanControlCapable = true;
            deviceGyroControlCapable = true;
            deviceName = "One X Player 2";
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
