using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;

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

        public static void ChangeFramePageMainWindow(string uri)
        {

            //use this to access sub setting pages from outside the mainwindow navigation menu, like wifi AP settings page
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            Uri uriLink = new Uri(uri, UriKind.RelativeOrAbsolute);
            window.LoadPageInFrameExternal(uriLink);
        }

        public static Uri TranslateIconToPageURI(SymbolRegular icon)
        {
            //translates navigation menu symbol to corresponding URI for mainwindow navigation
            Uri uri = new Uri("Pages\\HomePage.xaml", UriKind.Relative);
            switch (icon)
            {
                case SymbolRegular.Home16:
                    //no need to explicitly say home, its the default
                    break;

                case SymbolRegular.WindowSettings20:
                    uri = new Uri("Pages\\PowerPage.xaml", UriKind.Relative);
                    break;
                case SymbolRegular.FlashPlay20:
                    uri = new Uri("Pages\\ActionPage.xaml", UriKind.Relative);
                    break;
                case SymbolRegular.Settings20:
                    uri = new Uri("Pages\\SettingsPage.xaml", UriKind.Relative);
                    break;

                default:

                    break;
            }

            return uri;
        }
      
        public static void ChangeControllerInstructionPage(string pageName)
        {
            //serves as a intermediary function to mainwindow from different usercontrols and pages to update controller instructions
            if (Application.Current.MainWindow != null)
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.SetControllerInputPage(pageName);
            }
        }

        public static void NavigateListView(ListBox listView, string action)
        {
            //this is just a general listview navigation routine to reduce code in other areas

            //if no items exist end routine
            if (listView.Items.Count == 0) { return; }
            if (listView.SelectedIndex == -1)
            {
                //if index is -1 then nothing is selected, go to index 0
                listView.SelectedIndex = 0;
                return;
            }
            if (action == "DPadUp" || action == "DPadLeft")
            {
                if (listView.SelectedIndex > 0)
                {
                    listView.SelectedIndex = listView.SelectedIndex - 1;
                }
                else
                {
                    listView.SelectedIndex = listView.Items.Count-1;
                }
            }
            if (action == "DPadDown" || action == "DPadRight")
            {
                if (listView.SelectedIndex < listView.Items.Count-1)
                {
                    listView.SelectedIndex = listView.SelectedIndex + 1;
                }
                else
                {
                    listView.SelectedIndex = 0;
                }
            }

        }


        public static List<ControllerUserControl> SearchStackPanelReturnArray(StackPanel stackPanel)
        {
            List<ControllerUserControl> controllerUserControls = new List<ControllerUserControl>();

            if (stackPanel != null)
            {
                foreach(object child in stackPanel.Children)
                {
                    ControllerUserControl controllerUserControl = (ControllerUserControl)child;
                    if (controllerUserControl.Visibility == Visibility.Visible)
                    {
                        controllerUserControls.Add(controllerUserControl);
                    }

                }
            }


            return controllerUserControls;


        }
        public static List<ControllerUserControl> SearchWrapPanelReturnArray(WrapPanel wrapPanel)
        {
            List<ControllerUserControl> controllerUserControls = new List<ControllerUserControl>();

            if (wrapPanel != null)
            {
                foreach (object child in wrapPanel.Children)
                {
                    ControllerUserControl controllerUserControl = (ControllerUserControl)child;
                    if (controllerUserControl.Visibility == Visibility.Visible)
                    {
                        controllerUserControls.Add(controllerUserControl);
                    }

                }
            }


            return controllerUserControls;


        }
    }
}
