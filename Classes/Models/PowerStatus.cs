using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Handheld_Hardware_Tools.Classes.Models
{
    public class PowerStatus
    {
        public string batteryLevel = "";
        public string powerStatus = "AC";
        public PowerStatus() 
        {
            try
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_Battery");
                foreach (ManagementObject mo in mos.Get())
                {
                    batteryLevel = mo["EstimatedChargeRemaining"].ToString();
                    powerStatus = SystemParameters.PowerLineStatus.ToString();
                    return;
                }
            }
            catch(Exception ex)
            {
                Log_Writer.Instance.writeLog("Error in get power status; " + ex.Message, "PS01");
            }
           
        }

    }
}
