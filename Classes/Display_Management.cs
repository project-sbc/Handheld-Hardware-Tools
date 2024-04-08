﻿using Everything_Handhelds_Tool.Classes;
using System;
using System.Collections.Generic;
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

        public void ChangePrimaryMonitorRefreshRate(int newRefreshRate)
        {
            DISPLAY_DEVICE primaryDevice = new DISPLAY_DEVICE();
            primaryDevice.cb = Marshal.SizeOf(primaryDevice);

            EnumDisplayDevices(null, 0, ref primaryDevice, 0);

            DEVMODE devMode = new DEVMODE();
            devMode.dmSize = (short)Marshal.SizeOf(devMode);

            if (EnumDisplaySettings(primaryDevice.DeviceName, ENUM_CURRENT_SETTINGS, ref devMode))
            {
                devMode.dmDisplayFrequency = newRefreshRate;

                if (ChangeDisplaySettingsEx(primaryDevice.DeviceName, ref devMode, IntPtr.Zero, ChangeDisplaySettingsFlags.CDS_UPDATEREGISTRY, IntPtr.Zero) != DISP_CHANGE_SUCCESSFUL)
                {
                    
                }
            }
            else
            {
               
            }
        }

        public void ChangePrimaryMonitorResolution(int newWidth, int newHeight)
        {
            DISPLAY_DEVICE primaryDevice = new DISPLAY_DEVICE();
            primaryDevice.cb = Marshal.SizeOf(primaryDevice);

            EnumDisplayDevices(null, 0, ref primaryDevice, 0);

            DEVMODE devMode = new DEVMODE();
            devMode.dmSize = (short)Marshal.SizeOf(devMode);

            if (EnumDisplaySettings(primaryDevice.DeviceName, ENUM_CURRENT_SETTINGS, ref devMode))
            {
                devMode.dmPelsWidth = newWidth;
                devMode.dmPelsHeight = newHeight;
                devMode.dmDisplayFrequency = 60;

                int result = ChangeDisplaySettings(ref devMode, 0);
                if (result != DISP_CHANGE_SUCCESSFUL)
                {

                }
            }
            else
            {
            }
        }


        public List<Tuple<Tuple<int, int>, int>> GetAvailableResolutionsForPrimaryMonitor()
        {
            List<Tuple<Tuple<int, int>, int>> resolutions = new List<Tuple<Tuple<int, int>, int>>();

            DISPLAY_DEVICE primaryDevice = new DISPLAY_DEVICE();
            primaryDevice.cb = Marshal.SizeOf(primaryDevice);

            EnumDisplayDevices(null, 0, ref primaryDevice, 0);

            DEVMODE devMode = new DEVMODE();
            int iModeNum = 0;
            while (EnumDisplaySettings(primaryDevice.DeviceName, iModeNum, ref devMode))
            {
                resolutions.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(devMode.dmPelsWidth, devMode.dmPelsHeight), devMode.dmDisplayFrequency));
                iModeNum++;
            }

            return resolutions;
        }

        public List<Tuple<Tuple<int, int>, int>> GetAvailableResolutions()
        {
            List<Tuple<Tuple<int,int>, int>> resolutions = new List<Tuple<Tuple<int, int>, int>>();

            DisplayHelper.MonitorEnumDelegate enumDelegate = (IntPtr hMonitor, IntPtr hdcMonitor, ref DisplayHelper.RECT lprcMonitor, IntPtr dwData) =>
            {
                DISPLAY_DEVICE displayDevice = new DISPLAY_DEVICE();
                displayDevice.cb = Marshal.SizeOf(displayDevice);
                EnumDisplayDevices(null, (uint)(dwData.ToInt32()), ref displayDevice, 0);
                string deviceName = displayDevice.DeviceName.TrimEnd((char)0);
                DisplayHelper.DEVMODE devMode = new DisplayHelper.DEVMODE();
                int iModeNum = 0;
                while (EnumDisplaySettings(deviceName, iModeNum, ref devMode))
                {
                    resolutions.Add(new Tuple<Tuple<int, int>, int>(new Tuple<int, int>(devMode.dmPelsWidth, devMode.dmPelsHeight), devMode.dmDisplayFrequency));
                    iModeNum++;
                }
                return true;
            };

            DisplayHelper.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, enumDelegate, IntPtr.Zero);

            return resolutions;
        }

        public Tuple<int, int> GetPrimaryMonitorResolution()
        {
            DISPLAY_DEVICE displayDevice = new DISPLAY_DEVICE();
            displayDevice.cb = Marshal.SizeOf(displayDevice);

            EnumDisplayDevices(null, 0, ref displayDevice, 0);

            DEVMODE devMode = new DEVMODE();
            EnumDisplaySettings(displayDevice.DeviceName, -1, ref devMode);

            return new Tuple<int, int>(devMode.dmPelsWidth, devMode.dmPelsHeight);
        }

    }

}