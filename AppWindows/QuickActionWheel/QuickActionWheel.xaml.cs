using Everything_Handhelds_Tool.AppWindows.QuickActionWheel.QuickActionWheelItem_Actions;
using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Actions;
using Everything_Handhelds_Tool.Classes.Actions.ActionClass;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.Devices;
using RadialMenu.Controls;
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
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Everything_Handhelds_Tool.AppWindows.QuickActionWheel
{
    /// <summary>
    /// Interaction logic for WindowManager.xaml
    /// </summary>
    public partial class QuickActionWheel : Window
    {
        RadialMenuItem highlightedItem = null;
        RadialMenuItem selectedItem = null;
        RadialMenuCentralItem highlightCentralItem = null;
        System.Windows.Media.Brush defaultBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF3C3C3C");

        List<QuickActionWheelItem> quickActionWheelItems = new List<QuickActionWheelItem>();
        List<RadialMenuItem> mainLevelItems = new List<RadialMenuItem>();
        List<RadialMenuItem> subLevelItems = null;

        bool navigateMainLevel = true;

       

        public QuickActionWheel()
        {
            InitializeComponent();

            GetMainLevelItems();
        }
               
        private void GetMainLevelItems()
        {
            ActionList actions = (ActionList)XML_Management.Instance.LoadXML("ActionList");

            foreach (Classes.Actions.Action action in actions)
            {
                RadialMenuItem rmi = new RadialMenuItem();
                RadialMenu_UserControl rmuc = new RadialMenu_UserControl(action);
                rmi.Content = rmuc;


                //this hides the arrow if there isn't a sub menu
                if (rmuc.subRadialMenuItems != null) 
                {
                    if (rmuc.subRadialMenuItems.Count == 0)
                    {
                        rmi.ArrowBackground = System.Windows.Media.Brushes.Transparent;
                                      
                    }
                }
                else
                {
                    rmi.ArrowBackground = System.Windows.Media.Brushes.Transparent;
              
                }

                mainLevelItems.Add(rmi);
            }

            radialMenu.Items = mainLevelItems;

        }

        

        private void SubscribeEvents()
        {
            ControllerInput controllerInput = Local_Object.Instance.GetMainWindowControllerInput();
            if (controllerInput != null )
            {
                controllerInput.joystickEvent.controllerJoystickEvent += JoystickEvent_controllerJoystickEvent;
                controllerInput.buttonPressEvent.controllerInputEvent += ButtonPressEvent_controllerInputEvent; ;
            }
        }

        private void UnsubscribeEvents()
        {
            ControllerInput controllerInput = Local_Object.Instance.GetMainWindowControllerInput();
            if (controllerInput != null)
            {
                controllerInput.joystickEvent.controllerJoystickEvent -= JoystickEvent_controllerJoystickEvent;
                controllerInput.buttonPressEvent.controllerInputEvent -= ButtonPressEvent_controllerInputEvent; ;
            }
        }

        private async void HandleCloseWindow()
        {
            UnsubscribeEvents();
            radialMenu.IsOpen = false;
            await Task.Delay(200);
            this.Close();
        }

        private void ButtonPressEvent_controllerInputEvent(object? sender, controllerInputEventArgs e)
        {
            switch (e.Action)
            {
                case "A":
                    if (highlightedItem != null)
                    {
                        HandleRadialMenuItemClick(highlightedItem);
                    }
                    else if (highlightCentralItem != null)
                    {
                        HandleCentralItemClick(highlightCentralItem);
                    }
                    break;
                case "B":
                    if (navigateMainLevel)
                    {
                        HandleCloseWindow();
                    }
                    else
                    {
                        HandleNavigateBackToMainLevel();
                    }
                    break;
                default: break;
            }
        }


        private void HandleCentralItemClick(RadialMenuCentralItem rmci)
        {
            if (navigateMainLevel)
            {
                HandleCloseWindow();
            }
            else
            {
                HandleNavigateBackToMainLevel();
            }
        }


        private async void NavigateToSubMenu(RadialMenuItem rmi)
        {
            selectedItem = rmi;
            navigateMainLevel = false;
            RadialMenu_UserControl rmuc = rmi.Content as RadialMenu_UserControl;

            radialMenu.IsOpen = false;
            await Task.Delay(400);
            radialMenu.Items = rmuc.subRadialMenuItems;
            radialMenu.IsOpen = true;

        }

        private async void HandleNavigateBackToMainLevel()
        {
            actionNameTextblock.Text = "";
            navigateMainLevel = true;
            selectedItem = null;

            radialMenu.IsOpen = false;
            await Task.Delay(400);
            radialMenu.Items = mainLevelItems;
            radialMenu.IsOpen = true;


        }

        private void JoystickEvent_controllerJoystickEvent(object? sender, controllerJoystickEventArgs e)
        {
            HandleJoystickInput(e.lx, -e.ly);
        }

        private void HandleJoystickInput(double x, double y)
        {
            //from 0 to 360
            double angle = 0;
            //from 0 to 100
            double radius = 0;

            ConvertJoystickToPolarCoordinates(x/33000,y/33000,out angle, out radius);

            Debug.WriteLine(angle);
            if (radius > 0.6)
            {
                HighlightRadialMenuItem(angle);
            }
            else
            {
                HighlightCentralMenuItem(centralItem);
            }

        }

        private void HighlightRadialMenuItem(double angle)
        {
            foreach (RadialMenuItem rmi in radialMenu.Items)
            {
                double endAngle = rmi.StartAngle + rmi.AngleDelta;

                if (angle >= rmi.StartAngle && angle <= endAngle && rmi != highlightedItem)
                {

                    UnhighlightMenuItem(highlightedItem);
                    HighlightMenuItem(rmi);
                    break;
                }
            }
        }

        private void UnhighlightMenuItem(RadialMenuItem rmi)
        {
            if (rmi != null)
            {
                rmi.Background = defaultBrush;
                highlightedItem = null;
            }
            
        }
        private void UnhighlightCentralMenuItem()
        {
            highlightCentralItem.Background = defaultBrush;
            highlightCentralItem = null;

        }
        private void HighlightCentralMenuItem(RadialMenuCentralItem rmci)
        {
            UnhighlightMenuItem(highlightedItem);
            rmci.Background = System.Windows.Media.Brushes.DimGray;
            highlightCentralItem = rmci;

            //this is to stop display action name from disappearing in a sub menu
            if (navigateMainLevel)
            {
                actionNameTextblock.Text = "";
            }
           
        }
        private void HighlightMenuItem(RadialMenuItem rmi)
        {
            if (rmi != null)
            {
                highlightedItem = rmi;
                rmi.Background = System.Windows.Media.Brushes.DimGray;
                if (highlightCentralItem != null)
                {
                    UnhighlightCentralMenuItem();
                }

                //this code below sets the outline textblock. i added navigateMainLevel so that the action name
                //persists when in the sub menu
                if (rmi.Content is RadialMenu_UserControl && navigateMainLevel)
                {
                    RadialMenu_UserControl rmuc = rmi.Content as RadialMenu_UserControl;
                    actionNameTextblock.Text = rmuc.displayActionName;
                }
            }
            
        }

        private void ConvertJoystickToPolarCoordinates(double x, double y, out double angle, out double radius)
        {
            // Calculate radius
            radius = Math.Sqrt(x * x + y * y);

            // Calculate angle with 0 degree mark on the positive y-axis
            angle = Math.Atan2(x, -y);

            // Convert angle to degrees
            angle = angle * 180 / Math.PI;

            // Ensure angle is within [0, 360) range
            angle = (angle < 0) ? angle + 360 : angle;
        }

      
        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            HandleCloseWindow();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
   

            SubscribeEvents();

            ShowRadialMenuAtStartup();

        }

        private async void ShowRadialMenuAtStartup()
        {//add small delay otherwise animation for opening isnt consistent
            await Task.Delay(200);
            radialMenu.IsOpen = true;
            
        }

        private void HandleRadialMenuItemClick(RadialMenuItem rmi)
        {
            if (navigateMainLevel)
            {
                if (rmi != null)
                {
                    if (rmi.ArrowBackground == System.Windows.Media.Brushes.Transparent)
                    {
                        if (rmi.Content is RadialMenu_UserControl)
                        {
                            RadialMenu_UserControl rmuc = rmi.Content as RadialMenu_UserControl;
                            if (rmuc != null)
                            {
                                rmuc.ClickEvent();
                                HandleCloseWindow();
                            }
                        }
                    }
                    else
                    {//we set it up so that items with a sub menu dont have transparent arrow head background
                        NavigateToSubMenu(rmi);
                    }
                }
            }
            else
            {
               
                if (rmi != null)
                {
                    if (rmi.Content is TextBlock)
                    {
                        TextBlock textBlock = (TextBlock)rmi.Content;
                        if (textBlock != null)
                        {
                            string argument = textBlock.Text.ToString();
                            HandleSubRadialMenuItemClick(selectedItem, argument);
                        }
                    }

                    
                }


            }



            
        }

        private void HandleSubRadialMenuItemClick(RadialMenuItem rmi, string argument)
        {
            if (rmi != null)
            {
                if (rmi.Content is RadialMenu_UserControl)
                {
                    RadialMenu_UserControl rmuc = rmi.Content as RadialMenu_UserControl;
                    if (rmuc != null)
                    {
                        rmuc.action.OnActivate(argument);
                        HandleCloseWindow();
                    }
                }
            }
        }


        private void RadialMenuItem_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void centralItem_Click(object sender, RoutedEventArgs e)
        {
            HandleCentralItemClick(sender as RadialMenuCentralItem);
        }
    }
  
}
