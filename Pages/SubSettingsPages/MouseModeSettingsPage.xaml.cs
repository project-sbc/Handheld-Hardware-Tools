
using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.Models;
using Everything_Handhelds_Tool.Classes.MouseMode;
using Everything_Handhelds_Tool.UserControls.ActionOverviewUserControls;
using Everything_Handhelds_Tool.UserControls.HomePageUserControls;
using Everything_Handhelds_Tool.UserControls.SubPageUserControls.EditMouseModeUserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Everything_Handhelds_Tool.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class MouseModeSettingsPage : ControllerPage
    {
        
        public MouseProfile mouseProfile = (MouseProfile)XML_Management.Instance.LoadXML("MouseProfile");
        public MouseModeSettingsPage()
        {
            //Move initilize components to sub routine and async it to make pages feel smoother
            Dispatcher.BeginInvoke(new Action(() => Initialize()));

        }

        private void Initialize()
        {
            InitializeComponent();
            virtualStackPanel = stackPanel;
            AddControlsToArray();
        }
        private void AddControlsToArray()
        {
            //There is a general function that searches the stack panel and adds to the list of ControllerUserControls. It makes sure
            //visibility isn't collapsed too

            stackPanel.Children.Add(new MouseMovementStick_ToggleSwitch(mouseProfile.rightScroll));
            stackPanel.Children.Add(new MouseSensitivity_Slider(mouseProfile.sensitivityMouseValue));
            stackPanel.Children.Add(new ScrollSensitivity_Slider(mouseProfile.sensitivityScrollValue));
            stackPanel.Children.Add(new InvertScroll_ToggleSwitch(mouseProfile.reverseVerticalScroll));

            userControls = General_Functions.SearchStackPanelReturnArray(stackPanel);
        }

        public override void PressBPageHandler()
        {
            //override base function to send you back to general settings page
            System.Windows.Controls.Page page = new SettingsPage();

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.frame.Content = page;
        }

        private void Back_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PressBPageHandler();
        }
    }
}
