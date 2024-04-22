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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace Handheld_Hardware_Tools.AppWindows.WindowManager
{
    /// <summary>
    /// Interaction logic for WindowManagerPage.xaml
    /// </summary>
    public partial class WindowManagerPage : ControllerPage
    {
        Process selectedProcess;

        public WindowManagerPage()
        {
            InitializeComponent();
            virtualStackPanel = stackPanel;

          

          

        }
        private void SetLargeViewerToFirstListViewItem()
        {
            if (userControls.Count > 0)
            {
                if (userControls[0] != null)
                {
                    Window_UserControl wuc = userControls[0] as Window_UserControl;
                    UpdateProcessLargeViewer(wuc);
                }
            }
        }

        #region button routines
        private void maximizeButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleWindowState();
        }
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            CloseProgram();
        }

        private void endTaskButton_Click(object sender, RoutedEventArgs e)
        {

        }
        #region move window to different monitor

        public async Task MoveWindowToNextMonitorAsync()
        {
            if (selectedProcess != null)
            {
                if (selectedProcess.MainWindowHandle != IntPtr.Zero)
                {
                   


                }
            }
           
        }

        public async Task MoveWindowToPreviousMonitorAsync()
        {
            if (selectedProcess != null)
            {
                if (selectedProcess.MainWindowHandle != IntPtr.Zero)
                {
                    ScreenProgram_Management.MoveProgramToPreviousMonitor(selectedProcess.MainWindowHandle);

                }
            }

        }
        #endregion

        public async void CloseProgram()
        {
            if (highlightedUserControl > -1)
            {
                Window_UserControl wuc = (Window_UserControl)userControls[highlightedUserControl];
                string processName = wuc.process.ProcessName;
                if (wuc.process != null)
                {//double check process is running
                    if (Process.GetProcessesByName(wuc.process.ProcessName).Length > 0)
                    {
                        wuc.process.CloseMainWindow();
                        wuc.process.Close();
                    }
                   
                }


                int x = 0;

                while(Process.GetProcessesByName(processName).Length > 0 && x < 20)
                {

                    x++;
                    await Task.Delay(100);
                }
                if (Process.GetProcessesByName(processName).Length == 0)
                {

                    userControls.Remove(wuc);
                    stackPanel.Children.Remove(wuc);
                    ReturnControlToPage();
                    if (userControls.Count > 1)
                    {
                       
                        highlightedUserControl = highlightedUserControl - 1;
                        HighlightUserControl();
                    }
                    else
                    {
                        highlightedUserControl = -1;
                    }
                }
         


            }
        }

        #region handle window state toggle
        public void ToggleWindowState()
        {
            if (selectedProcess != null)
            {
                if (selectedProcess.MainWindowHandle != IntPtr.Zero)
                {
                    var placement = ScreenProgram_Management.GetPlacement(selectedProcess.MainWindowHandle);

                    WindowState windowState = ScreenProgram_Management.GetWindowState(selectedProcess.MainWindowHandle);
                    
                    if (windowState == WindowState.Maximized)
                    {
                        ScreenProgram_Management.SetWindowState(selectedProcess.MainWindowHandle, WindowState.Minimized);
                    }
                    else
                    {
                        ScreenProgram_Management.SetWindowState(selectedProcess.MainWindowHandle, WindowState.Maximized);
                        ScreenProgram_Management.SetForegroundWindow(selectedProcess.MainWindowHandle);
                    }
                }
            }
        }

      

    
        #endregion
        #endregion





        private void PopulateStackPanelWithProcesses()
        {
            //we start by getting a list of windows open in top most z order (the most visible window)
            //we can use this list to organize our processes by window order starting with the topmost window
            List<IntPtr> listWindowZOrder = new List<IntPtr>();

            IntPtr windowIntPtr = ScreenProgram_Management.GetTopWindow(IntPtr.Zero);
            while (windowIntPtr != IntPtr.Zero)
            {
                listWindowZOrder.Add(windowIntPtr);
                windowIntPtr = ScreenProgram_Management.GetWindow(windowIntPtr, ScreenProgram_Management.GW_HWNDNEXT);
            }


            //now lets get processes and compare the maindinowhandle to the list generated above

            Process[] processes = Process.GetProcesses().Where(p => p.MainWindowHandle != IntPtr.Zero).ToArray();

            List<Process> finalProcessList = new List<Process>();

            foreach (Process process in processes)
            {
                if (!ExclusionProcessList.Contains(process.ProcessName))
                {
                    //Okay here's my logic, find what order the window is and insert the process
                    //into the final list at that index location IF the final list count is greater than the index - 1 value
                    //otherwise it doesn't have enough objects and we just add it to the end.

                    //This is OK because as more items get added, any item that should have gone before this added item will
                    //fall into the insert part
                    int indexMainWindowHandleInZOrder = listWindowZOrder.IndexOf(process.MainWindowHandle);

                    if (indexMainWindowHandleInZOrder > -1)
                    {
                        if (finalProcessList.Count > indexMainWindowHandleInZOrder - 1)
                        {
                            finalProcessList.Insert(indexMainWindowHandleInZOrder, process);
                        }
                        else
                        {
                            finalProcessList.Add(process);
                        }
                    }
                    else
                    {
                        finalProcessList.Add(process);
                    }

                }

            }


            foreach(Process process in finalProcessList)
            {
                Window_UserControl wuc = new Window_UserControl(process);
                stackPanel.Children.Add(wuc);
                userControls.Add(wuc);
            }

        }

        public override void HandleUserControlNavigation(string action)
        {
            //run base routine, because it handles usercontrol highlighting and indexing for the movement
            base.HandleUserControlNavigation(action);

            //here we will handle updating the main image and allow end task, move window, etc
            if (highlightedUserControl > -1) //make sure a user control is highlighted
            {
                if (userControls[highlightedUserControl] != null)
                {
                    Window_UserControl wuc = userControls[highlightedUserControl] as Window_UserControl;
                    UpdateProcessLargeViewer(wuc);
                }

            }

        }

        public void HandleMouseTouchSelection(Window_UserControl wuc)
        {
            if (wuc != null)
            {
                //lets make sure the wuc selected is in the list of usercontrols
                int index = userControls.IndexOf(wuc);
                if (index != -1)
                {
                    //if in the list we can continue
                    //lets compare to the current selected one

                    UnhighlightUserControl();
                    highlightedUserControl = index;
                    SelectUserControl();
                    UpdateProcessLargeViewer(wuc);

                }
                              

            }
        }

        public void UpdateProcessLargeViewer(Window_UserControl wuc)
        {
            //updates the large image, text etc to reflect the selected process
            largeImage.Source = wuc.image.Source;

            Process process = wuc.process;
            if (process != null)
            {
                selectedProcess = process;
                if (!process.HasExited)
                {
                    textProcessName.Text = process.ProcessName;
                    textProcessTitle.Text = process.MainWindowTitle;

                }
            }
            else
            {//the way i am going to handle this is that I always want to account for the selected process whether its controller or mouse input
             //I need to set the selectedProcess to null when something goes wrong so we don't inadverently end the wrong process when requested
                selectedProcess = null;
            }
            
            
        }

        //this is a list used to get rid of the few weird processes that have windows but we don't want to include in the list
        private List<string> ExclusionProcessList = new List<string>()
        {
           {"TextInputHost"},
           {"steamwebhelper"},
           {"svchost"},
           {"Taskmgr"},
           {"explorer"},
           {"ApplicationFrameHost"},
           {"Everything Handhelds Tool"},
           {"ShellExperienceHost"},

         };

        private void ControllerPage_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateStackPanelWithProcesses();

            SetLargeViewerToFirstListViewItem();

        }

      
    }

   
   

}
