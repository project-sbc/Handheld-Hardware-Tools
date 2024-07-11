using Handheld_Hardware_Tools.Classes.Devices;
using Nefarius.Utilities.DeviceManagement.Extensions;
using Nefarius.Utilities.DeviceManagement.PnP;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Handheld_Hardware_Tools.Classes
{
    public class ControllerStatus_Management
    {

        private static ControllerStatus_Management _instance = null;
        private static readonly object lockObj = new object();
        private ControllerStatus_Management()
        {
        }
        public static ControllerStatus_Management Instance
        {
            get
            {
                if (_instance == null )
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new ControllerStatus_Management();
                        }
                    }
                }
                return _instance;
            }
        }


        private static string controllerInstanceID = "";
        private static string controllerGUID = ""; 
   

        public static void GetControllerGUIDInstanceID(bool checkNewController = false)
        {
            //checkNewController will overwrite the settings guid and controller
            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");
            if (settings.controllerGUID == "" || checkNewController)
            {
                string[] guidInstanceID = GetControllerGUIDFromDeviceManager();
                if (settings.controllerGUID != guidInstanceID[0] && guidInstanceID[0] != "")
                {
                    settings.controllerGUID = guidInstanceID[0];
                    settings.controllerInstanceID = guidInstanceID[1];
                    settings.SaveToXML();
                }
   
              
            }
            controllerGUID = settings.controllerGUID;
            controllerInstanceID = settings.controllerInstanceID;
        }

        private static string[] GetControllerGUIDFromDeviceManager()
        {
            string[] returnValues = new string[3];
            returnValues[0] = "";
            returnValues[1] = "";


            var instance = 0;


            // enumerate all devices that export the GUID_DEVINTERFACE_USB_DEVICE interface
            while (Devcon.FindByInterfaceGuid(DeviceInterfaceIds.XUsbDevice, out var path,
                       out var instanceId, instance++))
            {

                var usbDevice = PnPDevice
                    .GetDeviceByInterfaceId(path)
                    .ToUsbPnPDevice();



                //We want the device that has our VID and PID value, the variable strDevInstPth that should look like this  VID_045E&PID_028E
                if (path != null)
                {
                    returnValues[0] = usbDevice.GetProperty<Guid>(DevicePropertyKey.Device_ClassGuid).ToString();
                    returnValues[1] = usbDevice.InstanceId;
                   
                    break;
                    
                }
            }

            return returnValues;
        }




        public static bool BuiltInControllerStatus()
        {

            bool controllerEnabled = false;
            try
            {
                if (controllerGUID == "" || controllerInstanceID == "")
                {
                    GetControllerGUIDInstanceID();
                }


                var instance = 0;

                while (Devcon.FindByInterfaceGuid(DeviceInterfaceIds.XUsbDevice, out var path,
                           out var instanceId, instance++))
                {

                    var usbDevice = PnPDevice
                        .GetDeviceByInterfaceId(path)
                        .ToUsbPnPDevice();

                    //We want the device that has our VID and PID value, the variable strDevInstPth that should look like this  VID_045E&PID_028E


                    if (usbDevice.InstanceId == controllerInstanceID)
                    {

                        controllerEnabled = true;
                        break;
                    }
                }
                return controllerEnabled;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public static bool ToggleEnableDisableController()
        {
            //error number CM01
            try
            {
                //make this negative because we want to do the opposite of its current state (so if enabled lets disable)
                bool deviceEnable = !BuiltInControllerStatus();

                if (controllerGUID == "" || controllerInstanceID == "")
                {
                    GetControllerGUIDInstanceID();
                }

                if (controllerGUID != "" && controllerInstanceID != "")
                {
         



                    Guid guid = new Guid(controllerGUID);
                  
                    enabledevice.DeviceHelper.SetDeviceEnabled(guid, controllerInstanceID, deviceEnable);


                    if (!deviceEnable)
                    {
                        Task.Delay(2000);
                        powerCycleController();
                    }


                    return deviceEnable;
                }
                else
                {
                    return !deviceEnable;
                }
            }
            catch (Exception ex)
            {
               
                return false;
            }




        }


        public static void powerCycleController()
        {

            //error number CM02
            try
            {
                var instance = 0;

                while (Devcon.FindByInterfaceGuid(DeviceInterfaceIds.XUsbDevice, out var path,
                           out var instanceId, instance++))
                {

                    var usbDevice = PnPDevice
                        .GetDeviceByInterfaceId(path)
                        .ToUsbPnPDevice();

                    //We want the device that has our VID and PID value, the variable strDevInstPth that should look like this  VID_045E&PID_028E
                    if (usbDevice.InstanceId == controllerInstanceID)
                    {

                        //Apply power port cycle to finish disable
                        usbDevice.CyclePort();
                    }
                }
            }
            catch (Exception ex)
            {
               

            }

        }

    }
}
