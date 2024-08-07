﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Handheld_Hardware_Tools.Classes
{

   
    public class Settings
    {
        public string language { get; set; } = "English";
        public int defaultTDP { get; set; } = 15;
        public int maxTDP { get; set; } = 30;
        public int minTDP { get; set; } = 5;
        public int maxGPUClock { get; set; } = -1;
        public string intelTDPType { get; set; } = "MSR";

        public bool syncSustainedBoostTDP = true;

        public string ssidWifiAP { get; set; } = "EverythingHandheldsWifi";
        public string passwordWifiAP { get; set; } = "eh123456";

        public bool hideSplashScreen { get; set; } = false;
        public bool launchLargeWindow { get; set; } = false;

        public bool qamOnRightSide { get; set; } = true;

        public bool checkUpdatesAtStart { get; set; } = false;
        public string lastUpdateCheck { get; set; } = "";
        public string lastUpdateCheckVersion { get; set; } = "";

        public string controllerGUID = "";
        public string controllerInstanceID = "";

        public void SaveToXML()
        {
            XML_Management.Instance.SaveXML("Settings", this);
        }

    }

}
