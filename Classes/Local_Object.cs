using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Everything_Handhelds_Tool.Classes
{
    public class Local_Object
    {

        private static Local_Object _instance = null;
        private static readonly object lockObj = new object();
        private Local_Object()
        {
        }


        public static Local_Object Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Local_Object();
                        }
                    }
                }
                return _instance;
            }
        }

        public ControllerPage GetGeneralWindowPage(DependencyObject page)
        {
            
            //use this code to work up the visual tree until you hit a controllerpage
            DependencyObject parent = System.Windows.Media.VisualTreeHelper.GetParent(page);

            // Traverse up the visual tree until we find a page
            while (parent != null && !(parent is ControllerPage))
            {
                parent = System.Windows.Media.VisualTreeHelper.GetParent(parent);
            }

            if (parent != null)
            {
                if (parent is ControllerPage)
                {
                    return (ControllerPage)parent;
                }
            }

            return null;

        }


        public Page GetMainWindowFramePage()
        {
            return (Page)((MainWindow)Application.Current.MainWindow).frame.Content;
        }

        public string DeviceCPUType()
        {
            string cpuType = "";
            cpuType = ((MainWindow)Application.Current.MainWindow).device.cpuType;
            return cpuType;
        }

        public Device GetMainWindowDevice()
        {
            MainWindow mw = GetMainWindow();
            return mw.device;
           
        }

        public MainWindow GetMainWindow()
        {
            return ((MainWindow)Application.Current.MainWindow);

        }



    }
}
