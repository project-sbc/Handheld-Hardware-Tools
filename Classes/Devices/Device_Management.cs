using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everything_Handhelds_Tool.Classes.Motherboard_Info;

namespace Everything_Handhelds_Tool.Classes.Devices
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

            Device handheldDevice = null;

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
                            break;
                        case "1002-C":
                            //handheldDevice = new OneXPlayerMiniIntel();
                            break;
                        case "V03":
                            //handheldDevice = new OneXPlayerMiniPro();
                            break;
                    }


                    break;
                    //REMEMBER I CHANGED AYANEO TO STOP BECAUSE I NEVER MADE CLASSES FOR ALL AYA NEO PRODUCTS. LIGHTEN WAS TESTING FOR ME
                case "AYANEO_CHANGEBACK":
                    switch (product)
                    {
                        case "AIR":

                            break;
                        case "AIR Pro":

                            break;
                        case "AIR Lite":

                            break;
                        case "AYA NEO FOUNDER":
                        case "AYANEO 2021":

                            break;
                        case "AYANEO 2021 Pro":
                        case "AYANEO 2021 Pro Retro Power":

                            break;
                        case "NEXT Pro":
                        case "NEXT Advance":
                        case "NEXT":

                            break;
                        case "AYANEO 2":
                        case "GEEK":
                            //handheldDevice = new AyaNeo2();
                            break;
                        case "AYANEOAir":

                            break;
                    }
                    break;
                case "AOKZOE":
                    switch (product)
                    {
                        case "AOKZOE A1 AR07":
                            //handheldDevice = new AOKZOEA1();
                            break;
                        default: break;

                    }
                    break;
                default:

                    break;
            }
            if (handheldDevice == null)
            {
                if (cpuType == "Intel")
                {
                    handheldDevice = new Intel_Generic();
                }
                if (cpuType == "AMD")
                {
                    handheldDevice = new AMD_Generic();
                }
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
