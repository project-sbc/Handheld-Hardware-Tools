using Handheld_Hardware_Tools.AppWindows.OSK;
using Handheld_Hardware_Tools.AppWindows.OSK.Keyboards;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Handheld_Hardware_Tools.Classes.Devices;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WindowsInput;

namespace Handheld_Hardware_Tools.Classes
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

        public Window GetGeneralWindow(DependencyObject page)
        {

            //use this code to work up the visual tree until you hit a controllerpage
            DependencyObject parent = System.Windows.Media.VisualTreeHelper.GetParent(page);

            // Traverse up the visual tree until we find a page
            while (parent != null && !(parent is Window))
            {
                parent = System.Windows.Media.VisualTreeHelper.GetParent(parent);
            }

            if (parent != null)
            {
                if (parent is Window)
                {
                    return (Window)parent;
                }
            }

            return null;

        }

        public Page GetMainWindowFramePage()
        {
            return (Page)((QuickAccessMenu)Application.Current.MainWindow).frame.Content;
        }

        public string DeviceCPUType()
        {
            string cpuType = "";
            cpuType = ((App)Application.Current).device.cpuType;
            return cpuType;
        }

        public Device GetMainWindowDevice()
        {
            return ((App)Application.Current).device;
           
        }
        public OSK GetOSKWindow()
        {//used in 
            OSK osk = (OSK)Application.Current.Windows.OfType<OSK>().First();
            if (osk != null)
            {
                return osk;
            }
            return null;
        }

        public KeyboardPage GetOSKKeyboard()
        {
            OSK osk = GetOSKWindow();
            if (osk != null)
            {
                if (osk.frame.Content != null)
                {
                    return (KeyboardPage)osk.frame.Content;
                }
            }
            return null;
        }
        public QuickAccessMenu GetQAMWindow()
        {
            return ((QuickAccessMenu)Application.Current.MainWindow);

        }
        public ControllerInput GetMainWindowControllerInput()
        {
            QuickAccessMenu qam = GetQAMWindow();
            if (qam != null)
            {
                if (qam.controllerInput != null)
                {
                    return qam.controllerInput;
                }
            }
            return null;
        }
        public MouseMode.MouseMode GetMainWindowMouseMode()
        {
            QuickAccessMenu qam = GetQAMWindow();
            if (qam != null)
            {
                if (qam.mouseMode != null)
                {
                    return qam.mouseMode;
                }
            }
            return null;
        }
        public InputSimulator GetMainWindowInputSimulator()
        {
            QuickAccessMenu qam = GetQAMWindow();
            if (qam != null)
            {
                if (qam.inputSimulator != null)
                {
                    return qam.inputSimulator;
                }
            }
            return null;
        }
        public Controller GetMainWindowController()
        {
            QuickAccessMenu qam = GetQAMWindow();
            if (qam != null)
            {
                if (qam.controllerInput != null)
                {
                    if (qam.controllerInput.controller != null)
                    {
                        return qam.controllerInput.controller;
                    }
                }
            }
            return null;
        }


    }
}
