using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using SharpDX.XInput;
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

                case SymbolRegular.Power20:
                    uri = new Uri("Pages\\PowerPage.xaml", UriKind.Relative);
                    break;
                case SymbolRegular.Flash20:
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


        //this function converts a ushort number (as string) to the long text controller input (i.e. DPadLeft+LB+A)
        public static string convertControllerUshortToString(string hotkey)
        {
            string gamepadCombo = "";
            Gamepad gamepad = new Gamepad();


            ushort uShorthotkey;

            if (ushort.TryParse(hotkey, out uShorthotkey))
            {
                gamepad.Buttons = (GamepadButtonFlags)(uShorthotkey);


                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftShoulder)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "LB"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.RightShoulder)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "RB"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftThumb)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "LStick"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.RightThumb)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "RStick"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.Start)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "Start"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.Back)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "Back"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.A)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "A"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.B)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "B"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.X)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "X"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.Y)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "Y"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "DPadUp"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "DPadDown"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "DPadLeft"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "DPadRight"); }

            }


            return gamepadCombo;

        }
        private static string makeGamepadButtonString(string currentValue, string addValue)
        {
            //routine to make string for 
            if (currentValue == "")
            {
                return addValue;
            }
            else
            {
                return currentValue + "+" + addValue;
            }

        }

    }
}
