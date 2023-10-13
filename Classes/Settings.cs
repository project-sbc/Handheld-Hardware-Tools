using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Everything_Handhelds_Tool.Classes
{

   
    public class Settings
    {
        public string language { get; set; } = "English";
        public int defaultTDP { get; set; } = 15;
        public int maxTDP { get; set; } = 30;
        public int minTDP { get; set; } = 5;




        public string ssidWifiAP { get; set; } = "EverythingHandheldsWifi";
        public string passwordWifiAP { get; set; } = "everythinghandhelds";
    }
   
}
