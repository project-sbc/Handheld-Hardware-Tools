using Everything_Handhelds_Tool.Models;
using ModernWpf;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;

namespace Everything_Handhelds_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ControllerInput controllerInput = new ControllerInput();
        public bool controllerNavigateWindow = true;
        public MainWindow()
        {
            InitializeComponent();

            //Write log to tell app is open
            Log_Writer.writeLog("App started");

            //set nav menu items from model
            SetNavigationMenuItemSource();

            //subscribe to controller input
            controllerInput.buttonPressEvent.controllerInputEvent += HandleControllerInput;

            //
            
        }

        private void HandleControllerInput(object? sender, controllerInputEventArgs e)
        {
            Debug.WriteLine(e.Action);
            ControllerPage a = (ControllerPage)frame.Content;
            a.HandleControllerInput(e.Action);
          
        }

        public void SetNavigationMenuItemSource()
        {
            navigationView.MenuItemsSource = new NavigationViewMenuItems();
        }

        private void navigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
     
            if (navigationView.SelectedItem != null)
            {
                NavigationViewItem nvi = navigationView.SelectedItem as NavigationViewItem;
                frame.Source = new Uri($"\\Pages\\{nvi.Tag.ToString()}.xml");
            }
        }
    }
}
