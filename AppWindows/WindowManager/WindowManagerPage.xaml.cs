using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.Devices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace Everything_Handhelds_Tool.AppWindows.WindowManager
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

            PopulateStackPanelWithProcesses();

            SetLargeViewerToFirstListViewItem();
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

        #region move window to different monitor

        public async Task MoveWindowToNextMonitorAsync()
        {
            if (selectedProcess != null)
            {
                if (selectedProcess.MainWindowHandle != IntPtr.Zero)
                {
                    //WE NEED TO GET CURRENT STATE OF WINDOW BECAUSE MOVING IT WHILE NOT OPEN MAKES IT NOT WORK THE FIRST TIME
                    var placement = WindowStateHandler.GetPlacement(selectedProcess.MainWindowHandle);

                    WindowState windowState = GetWindowState(placement.showCmd.ToString());

                    if (windowState != WindowState.Maximized)
                    {
                        WindowStateHandler.SetWindowState(selectedProcess.MainWindowHandle, WindowState.Normal);
                        await Task.Delay(200);
                    }



                    List<Screen> screens = Screen.AllScreens.ToList();

                    // Get the position and size of the window
                    MoveWindowToMonitor.RECT windowRect;
                    MoveWindowToMonitor.GetWindowRect(selectedProcess.MainWindowHandle, out windowRect);


                    // Get the screen  of the winodw
                    Screen windowScreen = Screen.FromHandle(selectedProcess.MainWindowHandle);

                    int indexScreen = screens.IndexOf(windowScreen);

          

                    int newIndex = 0;
                    if (indexScreen != -1 && indexScreen < (screens.Count-1))
                    {
                        newIndex = indexScreen + 1;
                    }

                    Screen targetScreen = screens[newIndex];
                                        // Calculate the new position of the window relative to the target monitor
                    int newX = targetScreen.Bounds.Left + 40; // Example: 100 pixels from the left edge
                    int newY = targetScreen.Bounds.Top + 40; // Example: 100 pixels from the top edge


                    Debug.WriteLine(indexScreen.ToString() + " newX " + newX.ToString() + "; newY " + newY.ToString());

                    // Move the window to the new position
                    MoveWindowToMonitor.SetWindowPos(selectedProcess.MainWindowHandle, IntPtr.Zero, newX, newY, windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top, MoveWindowToMonitor.SWP_SHOWWINDOW | MoveWindowToMonitor.SWP_NOZORDER);

                    await Task.Delay(200);
                    //maximize window and set to front
   
                    WindowStateHandler.SetForegroundWindow(selectedProcess.MainWindowHandle);
                    await Task.Delay(200);
                    WindowStateHandler.SetWindowState(selectedProcess.MainWindowHandle, WindowState.Maximized);

                }
            }
           
        }

        public async Task MoveWindowToPreviousMonitorAsync()
        {
            if (selectedProcess != null)
            {
                if (selectedProcess.MainWindowHandle != IntPtr.Zero)
                {
                    //WE NEED TO GET CURRENT STATE OF WINDOW BECAUSE MOVING IT WHILE NOT OPEN MAKES IT NOT WORK THE FIRST TIME
                    var placement = WindowStateHandler.GetPlacement(selectedProcess.MainWindowHandle);

                    WindowState windowState = GetWindowState(placement.showCmd.ToString());

                    if (windowState != WindowState.Maximized)
                    {
                        WindowStateHandler.SetWindowState(selectedProcess.MainWindowHandle, WindowState.Maximized);
                        await Task.Delay(500);
                    }



                    List<Screen> screens = Screen.AllScreens.ToList();

                    // Get the position and size of the window
                    MoveWindowToMonitor.RECT windowRect;
                    MoveWindowToMonitor.GetWindowRect(selectedProcess.MainWindowHandle, out windowRect);


                    // Get the screen  of the winodw
                    Screen windowScreen = Screen.FromHandle(selectedProcess.MainWindowHandle);

                    int indexScreen = screens.IndexOf(windowScreen);



                    int newIndex = screens.Count-1;
                    if (indexScreen > 0)
                    {
                        newIndex = indexScreen - 1;
                    }

                    Screen targetScreen = screens[newIndex];
                    // Calculate the new position of the window relative to the target monitor
                    int newX = targetScreen.Bounds.Left + 40; // Example: 100 pixels from the left edge
                    int newY = targetScreen.Bounds.Top + 40; // Example: 100 pixels from the top edge


                    Debug.WriteLine(indexScreen.ToString() + " newX " + newX.ToString() + "; newY " + newY.ToString());

                    // Move the window to the new position
                    MoveWindowToMonitor.SetWindowPos(selectedProcess.MainWindowHandle, IntPtr.Zero, newX, newY, windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top, MoveWindowToMonitor.SWP_SHOWWINDOW | MoveWindowToMonitor.SWP_NOZORDER);

                    await Task.Delay(400);
                    //maximize window and set to front

                    WindowStateHandler.SetForegroundWindow(selectedProcess.MainWindowHandle);
                    await Task.Delay(400);
                    WindowStateHandler.SetWindowState(selectedProcess.MainWindowHandle, WindowState.Maximized);

                }
            }

        }
        #endregion



        #region handle window state toggle
        public void ToggleWindowState()
        {
            if (selectedProcess != null)
            {
                if (selectedProcess.MainWindowHandle != IntPtr.Zero)
                {
                    var placement = WindowStateHandler.GetPlacement(selectedProcess.MainWindowHandle);

                    WindowState windowState = GetWindowState(placement.showCmd.ToString());
                    
                    if (windowState == WindowState.Maximized)
                    {
                        WindowStateHandler.SetWindowState(selectedProcess.MainWindowHandle, WindowState.Minimized);
                    }
                    else
                    {
                        WindowStateHandler.SetWindowState(selectedProcess.MainWindowHandle, WindowState.Maximized);
                        WindowStateHandler.SetForegroundWindow(selectedProcess.MainWindowHandle);
                    }
                }
            }
        }

        private WindowState GetWindowState(string cmdPlacement)
        {
            switch (cmdPlacement)
            {
                case "Normal":
                    return WindowState.Normal;
                case "Minimized":
                    return WindowState.Minimized;
                case "Maximized":
                    return WindowState.Maximized;
                default:
                    return WindowState.Normal;
            }

        }

    
        #endregion
        #endregion



        // Import the necessary Windows APIs



        private void PopulateStackPanelWithProcesses()
        {
            //we start by getting a list of windows open in top most z order (the most visible window)
            //we can use this list to organize our processes by window order starting with the topmost window
            List<IntPtr> listWindowZOrder = new List<IntPtr>();

            IntPtr windowIntPtr = ZOrderFunctions.GetTopWindow(IntPtr.Zero);
            while (windowIntPtr != IntPtr.Zero)
            {
                listWindowZOrder.Add(windowIntPtr);
                windowIntPtr = ZOrderFunctions.GetWindow(windowIntPtr, ZOrderFunctions.GW_HWNDNEXT);
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

        private void UpdateProcessLargeViewer(Window_UserControl wuc)
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

         };

     
    }

    public static class WindowStateHandler
    {

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        public static WINDOWPLACEMENT GetPlacement(IntPtr hwnd)
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(hwnd, ref placement);
            return placement;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(
            IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public ShowWindowCommands showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        public enum ShowWindowCommands : int
        {
            Hide = 0,
            Normal = 1,
            Minimized = 2,
            Maximized = 3,
        }


        public static void SetWindowState(IntPtr hWnd, WindowState state)
        {
            int nCmdShow = 0;
            switch (state)
            {
                case WindowState.Normal:
                    nCmdShow = SW_SHOWNORMAL;
                    break;
                case WindowState.Minimized:
                    nCmdShow = SW_SHOWMINIMIZED;
                    break;
                case WindowState.Maximized:
                    nCmdShow = SW_SHOWMAXIMIZED;
                    break;
                default:
                    break;
            }

            // Call ShowWindow with the appropriate window handle and command
            ShowWindow(hWnd, nCmdShow);
        }

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // Define constants for window states
        public const int SW_HIDE = 0;
        public const int SW_SHOWNORMAL = 1;
        public const int SW_SHOWMINIMIZED = 2;
        public const int SW_SHOWMAXIMIZED = 3;
        public const int SW_SHOWNOACTIVATE = 4;
        public const int SW_RESTORE = 9;
        public const int SW_SHOWDEFAULT = 10;
    }

    public static class MoveWindowToMonitor
    {

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        // Constants for window positioning
        public const uint SWP_SHOWWINDOW = 0x0040;
        public const uint SWP_NOZORDER = 0x0004;


        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }

    public static class ZOrderFunctions
    {
        //all of these are used to get the z order of windows for populating the windows manager

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetTopWindow(IntPtr hWnd);


        public const uint GW_HWNDNEXT = 2;
        public static System.Windows.Media.Imaging.BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                System.Windows.Media.Imaging.BitmapImage bitmapimage = new System.Windows.Media.Imaging.BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }


        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        public static Bitmap PrintWindow(IntPtr hwnd)
        {
            RECT rc;
            GetWindowRect(hwnd, out rc);

            Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            PrintWindow(hwnd, hdcBitmap, 2);

            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();

            return bmp;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            private int _Left;
            private int _Top;
            private int _Right;
            private int _Bottom;

            public RECT(RECT Rectangle) : this(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom)
            {
            }
            public RECT(int Left, int Top, int Right, int Bottom)
            {
                _Left = Left;
                _Top = Top;
                _Right = Right;
                _Bottom = Bottom;
            }

            public int X
            {
                get { return _Left; }
                set { _Left = value; }
            }
            public int Y
            {
                get { return _Top; }
                set { _Top = value; }
            }
            public int Left
            {
                get { return _Left; }
                set { _Left = value; }
            }
            public int Top
            {
                get { return _Top; }
                set { _Top = value; }
            }
            public int Right
            {
                get { return _Right; }
                set { _Right = value; }
            }
            public int Bottom
            {
                get { return _Bottom; }
                set { _Bottom = value; }
            }
            public int Height
            {
                get { return _Bottom - _Top; }
                set { _Bottom = value + _Top; }
            }
            public int Width
            {
                get { return _Right - _Left; }
                set { _Right = value + _Left; }
            }
            public System.Drawing.Point Location
            {
                get { return new System.Drawing.Point(Left, Top); }
                set
                {
                    _Left = value.X;
                    _Top = value.Y;
                }
            }
            public System.Drawing.Size Size
            {
                get { return new System.Drawing.Size(Width, Height); }
                set
                {
                    _Right = value.Width + _Left;
                    _Bottom = value.Height + _Top;
                }
            }

            public static implicit operator Rectangle(RECT Rectangle)
            {
                return new Rectangle(Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height);
            }
            public static implicit operator RECT(Rectangle Rectangle)
            {
                return new RECT(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom);
            }
            public static bool operator ==(RECT Rectangle1, RECT Rectangle2)
            {
                return Rectangle1.Equals(Rectangle2);
            }
            public static bool operator !=(RECT Rectangle1, RECT Rectangle2)
            {
                return !Rectangle1.Equals(Rectangle2);
            }

            public override string ToString()
            {
                return "{Left: " + _Left + "; " + "Top: " + _Top + "; Right: " + _Right + "; Bottom: " + _Bottom + "}";
            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }

            public bool Equals(RECT Rectangle)
            {
                return Rectangle.Left == _Left && Rectangle.Top == _Top && Rectangle.Right == _Right && Rectangle.Bottom == _Bottom;
            }

            public override bool Equals(object Object)
            {
                if (Object is RECT)
                {
                    return Equals((RECT)Object);
                }
                else if (Object is Rectangle)
                {
                    return Equals(new RECT((Rectangle)Object));
                }

                return false;
            }
        }
    }
}
