using Handheld_Hardware_Tools.Classes.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PowerManagerAPI;
using System.Diagnostics;
using SharpDX;
using Notification.Wpf;
using System.Windows.Forms;

namespace Handheld_Hardware_Tools.Classes
{
    public class Notification_Management
    {
        private static Notification_Management _instance = null;
        private static readonly object lockObj = new object();
        private Notification_Management()
        {
        }
        public static Notification_Management Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Notification_Management();
                        }
                    }
                }
                return _instance;
            }
        }

       

        public void TaskbarNotification(string message)
        {
            
            NotificationManager __NotificationManager = new();
            __NotificationManager.Show("Handheld Control Panel", message);
            __NotificationManager = null;

        }
        public void AppWindowNotification (string message)
        {

        }
    }
}
