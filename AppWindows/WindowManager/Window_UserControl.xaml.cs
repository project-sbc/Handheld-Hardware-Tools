using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Actions;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.Devices;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using static Everything_Handhelds_Tool.Classes.ScreenProgram_Management;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;


namespace Everything_Handhelds_Tool.AppWindows.WindowManager
{
    /// <summary>
    /// Interaction logic for
    /// </summary>
    public partial class Window_UserControl : ControllerUserControl
    {
        public Process process;
        IntPtr windowHandle;

        bool usedExeIcon = false;
        public Window_UserControl(Process proc)
        {
            InitializeComponent();
            //set variables
            process = proc;
            windowHandle = proc.MainWindowHandle;

            //set virtual border
            borderControl = border;


            //set control
            ConfigureControl();
        }
        private void ConfigureControl()
        {

            //handle text
           if (process.MainWindowTitle != null && process.MainWindowTitle != "")
            {
                ProcessName.Text = process.ProcessName;
            }
            else
            {
                ProcessName.Text = process.MainWindowTitle;
            }

           //handle image
           CreateImageOfWindow();
        }

        private void CreateImageOfWindow()
        {

            if (ScreenProgram_Management.GetWindowState(process.MainWindowHandle)== WindowState.Minimized)
            {
                usedExeIcon = true;
                using (Icon ico = Icon.ExtractAssociatedIcon(process.MainModule.FileName))
                {
                    image.Source = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
            }
            else
            {
                usedExeIcon = false;
                System.Windows.Media.Imaging.BitmapImage bitmap =ScreenProgram_Management.BitmapToImageSource(ScreenProgram_Management.ScreenshotWindow(windowHandle));
                image.Source = bitmap;
            }

           

        }

        public override void HandleControllerInput(string action)
        {
            //we will completely override base handler here
            if (action == "B")
            {
                ReturnControlToPage();
                return;
            }

            ControllerPage page = Local_Object.Instance.GetGeneralWindowPage(this);

            // Check if we found a page
            if (page != null)
            {
                if (page is WindowManagerPage)
                {
                    WindowManagerPage pageManager = (WindowManagerPage)page;

                    switch (action)
                    {
                        case "A":
                            pageManager.ToggleWindowState();
                            break;
                        case "RightShoulder":
                            pageManager.MoveWindowToNextMonitorAsync();
                            break;
                        case "LeftShoulder":
                            pageManager.MoveWindowToPreviousMonitorAsync();
                            break;
                        default:
                            MessageBox.Show("FINIUSH THIS HERE");
                            break;
                    }
                }
            }
            
        }

     
      

        public override void ChangeMainWindowControllerInstructionPage()
        {
            //General_Functions.ChangeControllerInstructionPage("ChangeBack");
        }



        public override void ControlChangeValueHandler()
        {
          
        }

        private void HandleMouseTouchWindow_UserControlSelect()
        {
            //use this to select the object on the window manager page level when NOT using the controller but using mouse or touch

            ControllerPage page = Local_Object.Instance.GetGeneralWindowPage(this);

            // Check if we found a page
            if (page != null)
            {
                if (page is WindowManagerPage)
                {
                    WindowManagerPage pageManager = (WindowManagerPage)page;
                    pageManager.HandleMouseTouchSelection(this);
                }
            }
        }

        private void ControllerUserControl_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
           HandleMouseTouchWindow_UserControlSelect();
        }
    }
}
