using Everything_Handhelds_Tool.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Handheld_Hardware_Tools.Classes.DisplayHelper;


namespace Handheld_Hardware_Tools.Classes
{


    public static class DisplayHelper
    {
        [DllImport("user32.dll")]
        public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, ChangeDisplaySettingsFlags dwflags, IntPtr lParam);


        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern int ChangeDisplaySettings(
        [In, Out] ref DEVMODE lpDevMode,
        [param: MarshalAs(UnmanagedType.U4)] uint dwFlags
    );


        public delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 32;
            private const int CCHFORMNAME = 32;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }



        [DllImport("User32.dll")]
        public static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

     

        [StructLayout(LayoutKind.Sequential)]
        public struct DISPLAY_DEVICE
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        [Flags]
        public enum DisplayDeviceStateFlags : int
        {
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,
            PrimaryDevice = 0x4,
            MirroringDriver = 0x8,
            VGACompatible = 0x10,
            Removable = 0x20,
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }

        public const int DISP_CHANGE_SUCCESSFUL = 0;
        public const int ENUM_CURRENT_SETTINGS = -1;

        [Flags]
        public enum ChangeDisplaySettingsFlags : uint
        {
            CDS_UPDATEREGISTRY = 0x01,
            CDS_TEST = 0x02,
            CDS_FULLSCREEN = 0x04,
            CDS_GLOBAL = 0x08,
            CDS_SET_PRIMARY = 0x10,
            CDS_VIDEOPARAMETERS = 0x20,
            CDS_RESET = 0x40000000,
            CDS_NORESET = 0x80000000
        }

    }
    public class Display_Management
    {
        private static Display_Management _instance = null;
        private static readonly object lockObj = new object();
        private Display_Management()
        {
        }
        public static Display_Management Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Display_Management();
                        }
                    }
                }
                return _instance;
            }
        }



        public Tuple<int, int> GetPrimaryMonitorResolution()
        {
            DISPLAY_DEVICE displayDevice = new DISPLAY_DEVICE();
          
            DEVMODE devMode = new DEVMODE();
            EnumDisplaySettings(displayDevice.DeviceName, -1, ref devMode);

            return new Tuple<int, int>(devMode.dmPelsWidth, devMode.dmPelsHeight);
        }

        public int GetPrimaryMonitorRefreshRate()
        {
            DISPLAY_DEVICE displayDevice = GetPrimaryDisplayDevice();
            displayDevice.cb = Marshal.SizeOf(displayDevice);

            EnumDisplayDevices(null, 0, ref displayDevice, 0);

            DEVMODE devMode = new DEVMODE();
            EnumDisplaySettings(displayDevice.DeviceName, -1, ref devMode);

            return devMode.dmDisplayFrequency;
        }
        public void ChangePrimaryMonitorResolution(int newWidth, int newHeight)
        {
            // Get primary display device
            DISPLAY_DEVICE primaryDevice = GetPrimaryDisplayDevice();

            if (primaryDevice.cb > 0)
            {
                DEVMODE devMode = new DEVMODE();
                devMode.dmSize = (short)Marshal.SizeOf(devMode);

                if (EnumDisplaySettings(primaryDevice.DeviceName, ENUM_CURRENT_SETTINGS, ref devMode))
                {
                    devMode.dmPelsWidth = newWidth;
                    devMode.dmPelsHeight = newHeight;

                    // Retrieve available refresh rates for the primary monitor
                    List<int> displayFrequencies = GetAvailableRefreshRatesForPrimaryMonitor();

                    // Check if the current refresh rate is in the list and adjust if not
                    if (!displayFrequencies.Contains(devMode.dmDisplayFrequency))
                    {
                        if (displayFrequencies.Count > 0)
                        {
                            // Set it to the highest frequency for the resolution
                            devMode.dmDisplayFrequency = displayFrequencies[displayFrequencies.Count - 1];
                        }
                    }

                    // Change display settings
                    int result = ChangeDisplaySettings(ref devMode, 0);
                    if (result != DISP_CHANGE_SUCCESSFUL)
                    {
                        // Failed to change display settings
                        Console.WriteLine("Failed to change display settings.");
                    }
                    else
                    {
                        // Display settings changed successfully
                        Console.WriteLine("Display settings changed successfully.");
                    }
                }
                else
                {
                    // Failed to retrieve current display settings
                    Console.WriteLine("Failed to retrieve current display settings.");
                }
            }
            else
            {
                // Primary display device not found
                Console.WriteLine("Primary display device not found.");
            }

           
        }

        public void ChangePrimaryMonitorRefreshRate(int newRefreshRate)
        {
            // Get primary display device
            DISPLAY_DEVICE primaryDevice = GetPrimaryDisplayDevice();

            if (primaryDevice.cb > 0)
            {
                DEVMODE devMode = new DEVMODE();
                devMode.dmSize = (short)Marshal.SizeOf(devMode);

                if (EnumDisplaySettings(primaryDevice.DeviceName, ENUM_CURRENT_SETTINGS, ref devMode))
                {
                    devMode.dmDisplayFrequency = newRefreshRate;

                    // Change display settings
                    int result = ChangeDisplaySettings(ref devMode, 0);
                    if (result != DISP_CHANGE_SUCCESSFUL)
                    {
                        // Failed to change display settings
                        Console.WriteLine("Failed to change display settings.");
                    }
                    else
                    {
                        // Display settings changed successfully
                        Console.WriteLine("Display settings changed successfully.");
                    }
                }
                else
                {
                    // Failed to retrieve current display settings
                    Console.WriteLine("Failed to retrieve current display settings.");
                }
            }
            else
            {
                // Primary display device not found
                Console.WriteLine("Primary display device not found.");
            }
        }
        private DISPLAY_DEVICE GetPrimaryDisplayDevice()
        {//used to get primary display
            DISPLAY_DEVICE displayDevice = new DISPLAY_DEVICE();
            displayDevice.cb = Marshal.SizeOf(displayDevice);
            uint deviceIndex = 0;

            while (EnumDisplayDevices(null, deviceIndex, ref displayDevice, 0))
            {
                if (displayDevice.StateFlags.HasFlag(DisplayDeviceStateFlags.PrimaryDevice)) // Check if the device is the primary display
                {
                    return displayDevice;
                }
                deviceIndex++;
            }

            return new DISPLAY_DEVICE();
        }
        public List<Tuple<int, int>> GetAvailableResolutionsForPrimaryMonitor()
        {
           
            List<Tuple<int, int>> resolutions = new List<Tuple<int, int>>();

            DISPLAY_DEVICE displayDevice = new DISPLAY_DEVICE();
            displayDevice.cb = Marshal.SizeOf(displayDevice);
            uint deviceIndex = 0;

            while (EnumDisplayDevices(null, deviceIndex, ref displayDevice, 0))
            {
                if (displayDevice.StateFlags.HasFlag(DisplayDeviceStateFlags.PrimaryDevice)) // Check if device is the primary display
                {
                    DEVMODE devMode = new DEVMODE();
                    int modeIndex = 0;
                    while (EnumDisplaySettings(displayDevice.DeviceName, modeIndex, ref devMode))
                    {
                        Tuple<int, int> resolution = new Tuple<int, int>(devMode.dmPelsWidth, devMode.dmPelsHeight);
                        if (!resolutions.Contains(resolution))
                        {
                            resolutions.Add(resolution);
                        }
                        modeIndex++;
                    }
                    break; // Once primary display's resolutions are found, exit the loop
                }
                deviceIndex++;
            }

            return resolutions;

        }

        public List<int> GetAvailableRefreshRatesForPrimaryMonitor(int xRes = 0, int yRes = 0)
        {
            //this gets all the refresh rates for the current (or specified) resolution

            List<int> refreshRates = new List<int>();

            DISPLAY_DEVICE displayDevice = new DISPLAY_DEVICE();
            displayDevice.cb = Marshal.SizeOf(displayDevice);
            uint deviceIndex = 0;

            while (EnumDisplayDevices(null, deviceIndex, ref displayDevice, 0))
            {
                if (displayDevice.StateFlags.HasFlag(DisplayDeviceStateFlags.PrimaryDevice)) // Check if device is the primary display
                {
                    DEVMODE devMode = new DEVMODE();
                    int iModeNum = 0;

                    //we need to filter to only the current resolution (or specificed argument in function)
                    if (xRes == 0 || yRes == 0)
                    {
                        Tuple<int, int> resolution = GetPrimaryMonitorResolution();
                        xRes = resolution.Item1;
                        yRes = resolution.Item2;
                    }



                    while (EnumDisplaySettings(displayDevice.DeviceName, iModeNum, ref devMode))
                    {
                        if (devMode.dmPelsWidth == xRes && devMode.dmPelsHeight == yRes)
                        {
                            int refresh = devMode.dmDisplayFrequency;
                            if (!refreshRates.Contains(refresh))
                            {
                                refreshRates.Add(refresh);
                            }

                        }

                        iModeNum++;
                    }
                    return refreshRates;


                }
                deviceIndex++;
            }

            return refreshRates;
        }

    }

}
