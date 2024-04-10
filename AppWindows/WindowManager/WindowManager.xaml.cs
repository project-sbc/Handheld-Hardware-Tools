using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Handheld_Hardware_Tools.Classes.Devices;
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
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Handheld_Hardware_Tools.AppWindows.WindowManager
{
    /// <summary>
    /// Interaction logic for WindowManager.xaml
    /// </summary>
    public partial class WindowManager : ControllerWindow
    {
 
        List<UserControl> listviewList = new System.Collections.Generic.List<UserControl>();
       
        public WindowManager()
        {
            InitializeComponent();
            SubscribeControllerEvents();
            controllerNavigateWindow = false;
            commonFrame = frame;
        
           // instructionFrame = controllerFrame;
        }


        public override void HandleControllerInputTopLevel(object? sender, controllerInputEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                if (!controllerNavigateWindow)
                {//if not navigating at the window level pass input to page level
                    ControllerPage a = (ControllerPage)frame.Content;
                    a.HandleControllerInput(e.Action);
                }
                else
                {//send input to window handler
                    switch (e.Action)
                    {
                        case "B":
                            CloseWindow();
                            break;
                        default:
                            controllerNavigateWindow = true;
                            break;
                    }
                }
            }

          
        }

        private void CloseWindow()
        {
            UnsubscribeControllerEvents();
            this.Close();
        }

        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

      
    }
  
}
