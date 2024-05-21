using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Handheld_Hardware_Tools.Classes.Motherboard_Info;

namespace Handheld_Hardware_Tools.Classes.Devices
{
    public class Device_Management
    {
        public Device device;

        public Device_Management()
        {
            Motherboard_Info.Motherboard_Info motherboard_Info = new Motherboard_Info.Motherboard_Info();

            string manufacturer = motherboard_Info.Manufacturer().ToUpper();
            string product = motherboard_Info.Product().ToUpper();
            string cpuType = motherboard_Info.CPUType();
            int maxNonTurboCPUFrequency = motherboard_Info.MaxNonTurboCPUFrequency();

            try
            {//do this so when people ask about new devices i can ask them for log file
                Log_Writer.Instance.writeLog("Manufacturer:" + manufacturer + ", product: " + product + ", cpu: " + motherboard_Info.CPUName());
            }
            catch { }

            Device handheldDevice = null;
            if (cpuType == "Intel")
            {
                handheldDevice = new Intel_Generic();
            }
            if (cpuType == "AMD")
            {
                handheldDevice = new AMD_Generic();
            }


            switch (manufacturer)
            {
                case "GPD":
                    switch (product)
                    {
                        case "G1618-04":
                            //win 4 6800U
                            handheldDevice = new GPD_Win_4();
                            break;
                        case "G1619-04":
                            //win max 2 6800u
                            handheldDevice = new GPD_Win_Max_2();
                            break;
                        case "G1619-03":
                            //win max 2 intel
                            handheldDevice.deviceName = "GPD Win Max 2";
                            break;
                    }



                    break;
                case "ONE-NETBOOK TECHNOLOGY CO., LTD.":
                case "ONE-NETBOOK":
                    switch (product)
                    {
                        case "ONEXPLAYER 2 ARP23":
                            handheldDevice = new ON_One_X_Player_2();
                            break;
                        case "V01":
                            //handheldDevice = new OneXPlayerMiniAMD();
                            handheldDevice.deviceName = "One X Player Mini";
                            break;
                        case "1002-C":
                            //handheldDevice = new OneXPlayerMiniIntel();
                            handheldDevice.deviceName = "One X Player Mini";
                            break;
                        case "V03":
                            //handheldDevice = new OneXPlayerMiniPro();
                            handheldDevice.deviceName = "One X Player Mini";
                            break;
                    }


                    break;
                    //REMEMBER I CHANGED AYANEO TO STOP BECAUSE I NEVER MADE CLASSES FOR ALL AYA NEO PRODUCTS. LIGHTEN WAS TESTING FOR ME
                case "AYANEO_CHANGEBACK":
                    switch (product)
                    {
                        case "AIR":
                            handheldDevice.deviceName = "Aya Neo Air";
                            break;
                        case "AIR Pro":
                            handheldDevice.deviceName = "Aya Neo Air Pro";
                            break;
                        case "AIR Lite":
                            handheldDevice.deviceName = "Aya Neo Air Lite";
                            break;
                        case "AYA NEO FOUNDER":
                        case "AYANEO 2021":
                            handheldDevice.deviceName = "Aya Neo";
                            break;
                        case "AYANEO 2021 Pro":
                        case "AYANEO 2021 Pro Retro Power":
                            handheldDevice.deviceName = "Aya Neo Pro";
                            break;
                        case "NEXT Pro":
                        case "NEXT Advance":
                        case "NEXT":
                            handheldDevice.deviceName = "Aya Neo NEXT";
                            break;
                        case "AYANEO 2":
                            handheldDevice.deviceName = "Aya Neo 2";
                            break;
                        case "GEEK":
                            //handheldDevice = new AyaNeo2();
                            handheldDevice.deviceName = "Aya Neo Geek";
                            break;
                        case "AYANEOAir":

                            break;
                    }
                    break;
                    //minisforum
                case "SHENZHEN MEIGAO ELECTRONIC EQUIPMENT CO.,LTD":
                    switch (product)
                    {
                        case "HPPAC": //V3 tablet
                            handheldDevice.deviceName = "Minisforum V3";
                            break;
                    }
                    break;
                case "AOKZOE":
                    switch (product)
                    {
                        case "AOKZOE A1 AR07":
                            //handheldDevice = new AOKZOEA1();
                            handheldDevice.deviceName = "AOKZOE A1";
                            break;
                        default: break;

                    }
                    break;
                default:

                    break;
            }
            

            handheldDevice.cpuType = cpuType;
            TDP_Management.Instance.cpuType = cpuType;
            handheldDevice.mchBar = motherboard_Info.MCHBar();
            handheldDevice.cpuName = motherboard_Info.CPUName();
            handheldDevice.maxNonTurboCPUFrequnecy = maxNonTurboCPUFrequency;

            device = handheldDevice;
        }
    }
}
