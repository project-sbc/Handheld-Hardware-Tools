using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Everything_Handhelds_Tool.Classes.Actions.ActionClass
{
    public class Toggle_WifiAP: Action
    {
        public Toggle_WifiAP()
        {
            actionName = "Toggle_WifiAP";
            //arguments = new List<string>();
        }
        public override void OnActivate(string quickActionWheelParameter = "")
        {
            //calls main window toggle window
            QuickAccessMenu mW = Local_Object.Instance.GetQAMWindow();

            if (mW.wifiAP == null)
            {
                Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");
               
                mW.wifiAP = new Wifi_AP.WiFiDirectHotspotManager(settings.ssidWifiAP,settings.passwordWifiAP);
                mW.wifiAP.Start();
            }
            else
            {
                if (mW.wifiAP.Status() == Windows.Devices.WiFiDirect.WiFiDirectAdvertisementPublisherStatus.Started)
                {
                    mW.wifiAP.Stop();
                    mW.wifiAP = null;
                }
               
            }
        }
    }
}
