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
using System.Windows;
using System.Windows.Controls;
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





        // Import the necessary Windows APIs


        [DllImport("user32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetTopWindow(IntPtr hWnd);


        private const uint GW_HWNDNEXT = 2;

        private void PopulateStackPanelWithProcesses()
        {
            //we start by getting a list of windows open in top most z order (the most visible window)
            //we can use this list to organize our processes by window order starting with the topmost window
            List<IntPtr> listWindowZOrder = new List<IntPtr>();

            IntPtr windowIntPtr = GetTopWindow(IntPtr.Zero);
            while (windowIntPtr != IntPtr.Zero)
            {
                listWindowZOrder.Add(windowIntPtr);
                windowIntPtr = GetWindow(windowIntPtr, GW_HWNDNEXT);
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


        private List<string> ExclusionProcessList = new List<string>()
        {
           {"TextInputHost"},
           {"steamwebhelper"},
           {"svchost"},
           {"Taskmgr"},
           {"explorer"},
           {"ApplicationFrameHost"},

         };


        System.Windows.Media.Imaging.BitmapImage BitmapToImageSource(Bitmap bitmap)
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
