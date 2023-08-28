using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Everything_Handhelds_Tool.Classes
{
    public static class General_Functions
    {
        //Collection of classes used throughout the program
        public static double GetWindowHeight(Window window)
        {
            //Function to get height of window accounting for dpi scaling, set default height to 720
            double height = 720;

            PresentationSource ps = PresentationSource.FromVisual(window);
            if (ps != null)
            {
                double scaling = ps.CompositionTarget.TransformToDevice.M11;
                WpfScreen wpfScreen = WpfScreen.GetScreenFrom(window);
                if (wpfScreen != null)
                {
                    height = wpfScreen.WorkingArea.Height/scaling;
                }
            }

            return height;

        }

        public static string GetCPUManufacturer()
        {
            string cpuManufacturer = "";


            return cpuManufacturer;
        }

    }
}
