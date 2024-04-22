using Handheld_Hardware_Tools.AppWindows.AyaNeoFlipDSApp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using static Handheld_Hardware_Tools.Classes.DisplayHelper;

namespace Handheld_Hardware_Tools.Classes
{
    public static class ScreenProgram_Management
    {

        //This list is used to remove certain processes that appear to be full screen but aren't, when i detect if a game is running
        //i dont want to accidentally pick these processes
        public static List<string> ExcludeFullScreenProcessList = new List<string>()
        {
           {"TextInputHost"},
           {"steamwebhelper"},

         };


        //RECT is used to map the location of a program window. USED A LOT
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
        //RECT is the win api used to get the window rectangle for finding location, making a snapshot of the window
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(HandleRef hWnd, [In, Out] ref RECT rect);


        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);
        //returns the foreground window handle
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        //sets the foreground window handle, used a lot in the window manager app
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


        //checks to see if foreground app is FULL SCREEN
        public static bool IsForegroundFullScreen(HandleRef hWnd, System.Windows.Forms.Screen screen)
        {
            if (screen == null)
            {
                screen = System.Windows.Forms.Screen.PrimaryScreen;
            }
            RECT rect = new RECT();
            GetWindowRect(hWnd, ref rect);


            return new System.Drawing.Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top).Contains(screen.Bounds);
        }

        //support function for one of the other ones above
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

        //GetWindow and GetTopWindow are used to determine the order of processes in the window manager app
        //it was a nice idea to sort them in z order
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetTopWindow(IntPtr hWnd);

        public enum ShowWindowCommands : int
        {
            Hide = 0,
            Normal = 1,
            Minimized = 2,
            Maximized = 3,
        }

        //USED FOR making the cute snapshot of the window for the window manager app
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

        //same as above, snapshot window manager app
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        //same as above, snapshot window manager app
     
        //same as above, snapshot window manager app
        public static Bitmap ScreenshotWindow(IntPtr hwnd)
        {
            RECT rc;
            GetWindowRect(hwnd, out rc);
            if (rc.Width > 0 && rc.Height > 0)
            {
                Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
                Graphics gfxBmp = Graphics.FromImage(bmp);
                IntPtr hdcBitmap = gfxBmp.GetHdc();

                PrintWindow(hwnd, hdcBitmap, 2);

                gfxBmp.ReleaseHdc(hdcBitmap);
                gfxBmp.Dispose();

                return bmp;
            }
            return null;
        }



        //used for moving windows from one screen to another
        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);
        //used to actually move the app window from one monitor to another
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        // Constants for window positioning
        public const uint SWP_SHOWWINDOW = 0x0040;
        public const uint SWP_NOZORDER = 0x0004;



        //sets a window state by handle, used a bunch in window manager app
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

        public static void MoveProgramToPreviousMonitor(IntPtr handle)
        {
            //This moves the program window handle from one monitor to the next


            //WE NEED TO GET CURRENT STATE OF WINDOW BECAUSE MOVING IT WHILE NOT OPEN MAKES IT NOT WORK THE FIRST TIME

            WindowState windowState = ScreenProgram_Management.GetWindowState(handle);


            //if (windowState != WindowState.Maximized)
            //{
            ScreenProgram_Management.SetWindowState(handle, WindowState.Normal);
            Thread.Sleep(200);
            //}



            List<Screen> screens = Screen.AllScreens.ToList();

            // Get the position and size of the window
            ScreenProgram_Management.RECT windowRect;
            ScreenProgram_Management.GetWindowRect(handle, out windowRect);


            // Get the screen  of the winodw
            Screen windowScreen = Screen.FromHandle(handle);

            int indexScreen = screens.IndexOf(windowScreen);



            int newIndex = screens.Count - 1;
            if (indexScreen > 0)
            {
                newIndex = indexScreen - 1;
            }

            Screen targetScreen = screens[newIndex];
            // Calculate the new position of the window relative to the target monitor
            int newX = targetScreen.Bounds.Left; // Example: 100 pixels from the left edge
            int newY = targetScreen.Bounds.Top; // Example: 100 pixels from the top edge


            Debug.WriteLine(indexScreen.ToString() + " newX " + newX.ToString() + "; newY " + newY.ToString());

            // Move the window to the new position
            ScreenProgram_Management.SetWindowPos(handle, IntPtr.Zero, newX, newY, windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top, ScreenProgram_Management.SWP_SHOWWINDOW | ScreenProgram_Management.SWP_NOZORDER);

            Thread.Sleep(400);
            ScreenProgram_Management.SetWindowState(handle, WindowState.Maximized);
            Thread.Sleep(400);
            //maximize window and set to front

            ScreenProgram_Management.SetForegroundWindow(handle);


        }

        public static void MoveProgramToNextMonitor(IntPtr handle)
        {
            //This moves the program window handle from one monitor to the next


            //WE NEED TO GET CURRENT STATE OF WINDOW BECAUSE MOVING IT WHILE NOT OPEN MAKES IT NOT WORK THE FIRST TIME


            WindowState windowState = ScreenProgram_Management.GetWindowState(handle);


            //if (windowState != WindowState.Maximized)
            //{
            ScreenProgram_Management.SetWindowState(handle, WindowState.Normal);
            Thread.Sleep(200);
            //}



            List<Screen> screens = Screen.AllScreens.ToList();

            // Get the position and size of the window
            ScreenProgram_Management.RECT windowRect;
            ScreenProgram_Management.GetWindowRect(handle, out windowRect);


            // Get the screen  of the winodw
            Screen windowScreen = Screen.FromHandle(handle);

            int indexScreen = screens.IndexOf(windowScreen);



            int newIndex = 0;
            if (indexScreen != -1 && indexScreen < (screens.Count - 1))
            {
                newIndex = indexScreen + 1;
            }

            Screen targetScreen = screens[newIndex];
            // Calculate the new position of the window relative to the target monitor
            int newX = targetScreen.Bounds.Left; // Example: 100 pixels from the left edge
            int newY = targetScreen.Bounds.Top; // Example: 100 pixels from the top edge


            Debug.WriteLine(indexScreen.ToString() + " newX " + newX.ToString() + "; newY " + newY.ToString());

            // Move the window to the new position
            ScreenProgram_Management.SetWindowPos(handle, IntPtr.Zero, newX, newY, windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top, ScreenProgram_Management.SWP_SHOWWINDOW | ScreenProgram_Management.SWP_NOZORDER);

            Thread.Sleep(400);
            ScreenProgram_Management.SetWindowState(handle, WindowState.Maximized);
            Thread.Sleep(400);
            //maximize window and set to front

            ScreenProgram_Management.SetForegroundWindow(handle);


        }


        public static WindowState GetWindowState(IntPtr handle)
        {
            if (handle != null)
            {
                var placement = ScreenProgram_Management.GetPlacement(handle);


                switch (placement.showCmd.ToString())
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

            return WindowState.Normal;

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

        [DllImport("user32.dll")]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lpRect, MonitorEnumProc callback, IntPtr dwData);
        public delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, IntPtr dwData);

        [StructLayout(LayoutKind.Sequential)]
        private struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

    
      
        public static void OpenAyaFlipDSWindowOnSpecificMonitor(int desiredWidth, int desiredHeight)
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                var dm = new DEVMODE();
                dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
                EnumDisplaySettings(screen.DeviceName, ENUM_CURRENT_SETTINGS, ref dm);

                if (dm.dmPelsWidth == desiredWidth && dm.dmPelsHeight == desiredHeight)
                {
                    Window window = new AyaNeoFlipDSApp
                    {
                        Width = screen.Bounds.Width,
                        Height = screen.Bounds.Height,

                        Left = screen.Bounds.Left,
                        Top = screen.Bounds.Top,
                        //WindowState = WindowState.Maximized
                    };
           
                    // Show the window on the specific monitor
                    window.Show();
                    window.WindowState = WindowState.Maximized;
                    return;
                }
                
            }




        }


        //EVERYTHING BELOW IS THREAD STUFF LIKE SUSPENDING THREADS. I HAD IT A WHILE AGO TO SUSPEND GAMES TO PREVENT CONTROLLER INPUT FROM WORKING


        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }


        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CloseHandle(IntPtr handle);




    }
}
