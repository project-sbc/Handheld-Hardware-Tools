using Everything_Handhelds_Tool.Classes.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Everything_Handhelds_Tool.Classes
{
    public class Local_Object
    {

        private static Local_Object _instance = null;
        private static readonly object lockObj = new object();
        private Local_Object()
        {
        }


        public static Local_Object Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new Local_Object();
                        }
                    }
                }
                return _instance;
            }
        }



        public Page GetMainWindowFramePage()
        {
           return (Page)((MainWindow)Application.Current.MainWindow).frame.Content;
        }

        public Device GetMainWindowDevice()
        {
           return ((MainWindow)Application.Current.MainWindow).device;
        }

        public MainWindow GetMainWindow()
        {
            return ((MainWindow)Application.Current.MainWindow);
        }



    }
}
