using Everything_Handhelds_Tool.Classes;
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
                ProcessName.Text = process.ProcessName;
            }

           //handle image
           CreateImageOfWindow();
        }

        private void CreateImageOfWindow()
        {
            System.Windows.Media.Imaging.BitmapImage bitmap = BitmapToImageSource(PrintWindow(windowHandle));
            image.Source = bitmap;

        }

        public override void HandleControllerInput(string action)
        {
            //we will completely override base handler here
            if (action == "B")
            {
                ReturnControlToPage();
                return;
            }

            //WE NEED THE PAGE TO DO THIS NEXT PART, SO WE WORK UP THE VISUAL TREE
            //use this code to work up the visual tree until you hit a controllerpage
            DependencyObject parent = System.Windows.Media.VisualTreeHelper.GetParent(this);

            // Traverse up the visual tree until we find a page
            while (parent != null && !(parent is ControllerPage))
            {
                parent = System.Windows.Media.VisualTreeHelper.GetParent(parent);
            }

            // Check if we found a page
            if (parent is ControllerPage)
            {
                WindowManagerPage page = (WindowManagerPage)parent;
                switch (action)
                {
                    case "A":
                        page.ToggleWindowState();
                        break;
                    case "RightShoulder":
                        page.MoveWindowToNextMonitorAsync();
                        break;
                    case "LeftShoulder":
                        page.MoveWindowToPreviousMonitorAsync();
                        break;
                    default:
                        MessageBox.Show("FINIUSH THIS HERE");
                        break;
                }
            }
            
        }

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
            public Point Location
            {
                get { return new Point(Left, Top); }
                set
                {
                    _Left = value.X;
                    _Top = value.Y;
                }
            }
            public Size Size
            {
                get { return new Size(Width, Height); }
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

        public override void ChangeMainWindowControllerInstructionPage()
        {
            //General_Functions.ChangeControllerInstructionPage("ChangeBack");
        }



        public override void ControlChangeValueHandler()
        {
          
        }

        private void ControllerUserControl_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
