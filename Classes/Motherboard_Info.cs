using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Handheld_Hardware_Tools.Classes.Motherboard_Info
{
    public class Motherboard_Info
    {
        ManagementObjectSearcher baseboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
        ManagementObjectSearcher motherboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_MotherboardDevice");
        object processorNameRegistry = Registry.GetValue("HKEY_LOCAL_MACHINE\\hardware\\description\\system\\centralprocessor\\0", "ProcessorNameString", null);


        public string Manufacturer()
        {
            try
            {
                foreach (ManagementObject queryObj in baseboardSearcher.Get())
                {
                    return queryObj["Manufacturer"].ToString();
                }
                return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public int MaxNonTurboCPUFrequency()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");

                foreach (ManagementObject obj in searcher.Get())
                {
                    // The MaxClockSpeed property represents the maximum frequency of the processor in MHz.
                    uint value = (uint)obj["MaxClockSpeed"];

                    //make sure it rounds to nearest 100
                    int finalValueRounded = (int)(Math.Round((decimal)value,0)/100)*100;

                    return (int)finalValueRounded;
   
                }
                return -1;
            }
            catch (Exception e)
            {
                return -1;
            }
           
        }

        public string Product()
        {
            try
            {
                foreach (ManagementObject queryObj in baseboardSearcher.Get())
                {
                    return queryObj["Product"].ToString();
                }
                return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public string CPUType()
        {
            if (processorNameRegistry != null)
            {
                //If not null, find intel or AMD string and clarify type. For Intel determine MCHBAR for rw.exe
                string processorName = processorNameRegistry.ToString();
                if (processorName.IndexOf("Intel") >= 0) { return "Intel"; }
                if (processorName.IndexOf("AMD") >= 0) { return "AMD"; }
            }
            return "";
        }
        public string CPUName()
        {
            if (processorNameRegistry != null)
            {
                //return cpu name like AMD 6800U
                return processorNameRegistry.ToString();
            }
            return "";
        }
        public string MCHBar()
        {
            if (processorNameRegistry != null)
            {
                //If not null, find intel or AMD string and clarify type. For Intel determine MCHBAR for rw.exe
                string processorName = processorNameRegistry.ToString();
                if (processorName.IndexOf("Intel") >= 0)
                {
                    object processorModelRegistry = Registry.GetValue("HKEY_LOCAL_MACHINE\\hardware\\description\\system\\centralprocessor\\0", "Identifier", null);
                    string processorModel = null;
                    if (processorModelRegistry != null)
                    {
                        //If not null, convert to string and determine MCHBAR for rw.exe
                        processorModel = processorModelRegistry.ToString();
                        if (processorModel.IndexOf("Model 140") >= 0) { return "0xFEDC59"; } else { return "0xFED159"; };
                    }
                }

            }
            return "";
        }

        public string SystemName()
        {
            try
            {
                foreach (ManagementObject queryObj in motherboardSearcher.Get())
                {
                    return queryObj["SystemName"].ToString();
                }
                return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public string Version()
        {
            try
            {
                foreach (ManagementObject queryObj in baseboardSearcher.Get())
                {
                    return queryObj["Version"].ToString();
                }
                return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }


    }
}
