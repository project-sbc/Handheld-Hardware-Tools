using Everything_Handhelds_Tool.Classes.Devices;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Devices.Radios;

namespace Everything_Handhelds_Tool.Classes
{
    public class Wifi_BT_Management
    {
        private static Wifi_BT_Management _instance = null;
        private static readonly object lockObj = new object();
        private Wifi_BT_Management()
        {
        }
        public static Wifi_BT_Management Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Wifi_BT_Management();
                        }
                    }
                }
                return _instance;
            }
        }

        public async Task<bool> GetWifiIsEnabledAsync()
        {
            bool value = false;
            var radios = await Radio.GetRadiosAsync();
            var wifiRadio = radios.FirstOrDefault(radio => radio.Kind == RadioKind.WiFi);
            value = wifiRadio != null && wifiRadio.State == RadioState.On;
            wifiRadio = null;
            radios = null;

            return value;
        }
        public async Task ToggleWifi()
        {
            var radios = await Radio.GetRadiosAsync();
            var wifiRadio = radios.FirstOrDefault(radio => radio.Kind == RadioKind.WiFi);
            if (wifiRadio != null)
            {
                if (wifiRadio.State == RadioState.Off) { wifiRadio.SetStateAsync(RadioState.On); }
                if (wifiRadio.State == RadioState.On) { wifiRadio.SetStateAsync(RadioState.Off); }
            }
            wifiRadio = null;

        }
        public async Task<bool> GetBTIsEnabledAsync()
        {//error number BTT01
            try
            {
                bool value = false;
                var radios = await Radio.GetRadiosAsync();
                var btRadio = radios.FirstOrDefault(radio => radio.Kind == RadioKind.Bluetooth);
                value = btRadio != null && btRadio.State == RadioState.On;
                btRadio = null;
                radios = null;

                return value;
            }
            catch (Exception ex)
            {
                
                return false;
            }

        }
        public async Task ToggleBT()
        {
            var radios = await Radio.GetRadiosAsync();
            var btRadio = radios.FirstOrDefault(radio => radio.Kind == RadioKind.Bluetooth);
           

            if (btRadio != null)
            {
                Debug.WriteLine(btRadio.Name);
                if (btRadio.State == RadioState.Off) { btRadio.SetStateAsync(RadioState.On); }
                if (btRadio.State == RadioState.On) { btRadio.SetStateAsync(RadioState.Off); }
            }
            btRadio = null;

        }
    }

   
}
