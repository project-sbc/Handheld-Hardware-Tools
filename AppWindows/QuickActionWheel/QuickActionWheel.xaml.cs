using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.Devices;
using RadialMenu.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Everything_Handhelds_Tool.AppWindows.QuickActionWheel
{
    /// <summary>
    /// Interaction logic for WindowManager.xaml
    /// </summary>
    public partial class QuickActionWheel : Window
    {
        RadialMenuItem highlightedItem = null;
        RadialMenuCentralItem highlightCentralItem = null;
        System.Windows.Media.Brush defaultBrush = null;


        List<RadialMenuItem> mainLevelItems = null;
        List<RadialMenuItem> subLevelItems = null;


        public QuickActionWheel()
        {
            InitializeComponent();

        }
               
        private void SubscribeEvents()
        {
            ControllerInput controllerInput = Local_Object.Instance.GetMainWindowControllerInput();
            if (controllerInput != null )
            {
                controllerInput.joystickEvent.controllerJoystickEvent += JoystickEvent_controllerJoystickEvent;
                controllerInput.buttonPressEvent.controllerInputEvent += ButtonPressEvent_controllerInputEvent; ;
            }
        }

        private void ButtonPressEvent_controllerInputEvent(object? sender, controllerInputEventArgs e)
        {
            
        }

        private void JoystickEvent_controllerJoystickEvent(object? sender, controllerJoystickEventArgs e)
        {
            HandleJoystickInput(e.lx, -e.ly);
        }

        private void HandleJoystickInput(double x, double y)
        {
            //from 0 to 360
            double angle = 0;
            //from 0 to 100
            double radius = 0;

            ConvertJoystickToPolarCoordinates(x/33000,y/33000,out angle, out radius);

            Debug.WriteLine(angle);
            if (radius > 0.4)
            {
                HighlightRadialMenuItem(angle);
            }
            else
            {
                HighlightCentralMenuItem(centralItem);
            }

        }

        private void HighlightRadialMenuItem(double angle)
        {
            foreach (RadialMenuItem rmi in radialMenu.Items)
            {
                double endAngle = rmi.StartAngle + rmi.AngleDelta;

                if (angle >= rmi.StartAngle && angle <= endAngle && rmi != highlightedItem)
                {

                    UnhighlightMenuItem(highlightedItem);
                    HighlightMenuItem(rmi);
                    break;
                }
            }
        }

        private void UnhighlightMenuItem(RadialMenuItem rmi)
        {
            if (rmi != null)
            {
                rmi.Background = defaultBrush;
                highlightedItem = null;
            }
            
        }
        private void UnhighlightCentralMenuItem()
        {
            highlightCentralItem.Background = defaultBrush;
            highlightCentralItem = null;

        }
        private void HighlightCentralMenuItem(RadialMenuCentralItem rmci)
        {
            UnhighlightMenuItem(highlightedItem);
            rmci.Background = System.Windows.Media.Brushes.DimGray;
            highlightCentralItem = rmci;

        }
        private void HighlightMenuItem(RadialMenuItem rmi)
        {
            if (rmi != null)
            {
                highlightedItem = rmi;
                rmi.Background = System.Windows.Media.Brushes.DimGray;
                if (highlightCentralItem != null)
                {
                    UnhighlightCentralMenuItem();
                }
            }
            
        }

        private void ConvertJoystickToPolarCoordinates(double x, double y, out double angle, out double radius)
        {
            // Calculate radius
            radius = Math.Sqrt(x * x + y * y);

            // Calculate angle with 0 degree mark on the positive y-axis
            angle = Math.Atan2(x, -y);

            // Convert angle to degrees
            angle = angle * 180 / Math.PI;

            // Ensure angle is within [0, 360) range
            angle = (angle < 0) ? angle + 360 : angle;
        }

        private void CloseWindow()
        {
         
            this.Close();
        }

        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //set default color so we can set it later
            defaultBrush = radialMenu.Items[0].Background;

            SubscribeEvents();
        }
    }
  
}
