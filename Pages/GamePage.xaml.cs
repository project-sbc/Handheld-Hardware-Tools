
using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Actions;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Handheld_Hardware_Tools.Classes.Models;
using Handheld_Hardware_Tools.UserControls.HomePageUserControls;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using Handheld_Hardware_Tools.UserControls.ActionWrapPanelUserControls;
using Handheld_Hardware_Tools.AppWindows.MainWindow;

namespace Handheld_Hardware_Tools.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class GamePage : ControllerWrapPanelPage
    {
        
        public GamePage()
        {
            //Move initilize components to sub routine and async it to make pages feel smoother
            Application.Current.Dispatcher.BeginInvoke(new System.Action(() => Initialize()));



        }

        private void Initialize()
        {
            InitializeComponent();
            virtualWrapPanel = wrapPanel;

            //hide top row when in main window, but keep when in QAM
            Window parentWindow = Local_Object.Instance.GetGeneralWindow(this);
            if (parentWindow is MainWindow)
            {
                grid.RowDefinitions[0].Height = new GridLength(0) ;
            }
        }
        private void AddControlsToArray()
        {
            //There is a general function that searches the wrap panel and adds to the list of ControllerUserControls. It makes sure
            //visibility isn't collapsed too

            ActionList actions = (ActionList)XML_Management.Instance.LoadXML("ActionList");



            foreach (Classes.Actions.Action action in actions)
            {
                //check if dispaly in action panel is enabled and the device supports the action 
                if (action.displayInActionPanel && action.UsableOnDevice())
                {
                    wrapPanel.Children.Add(new Action_UserControl(action));
                }
              
            }

            
           

            userControls = General_Functions.SearchWrapPanelReturnArray(wrapPanel);
        }

        private void ControllerWrapPanelPage_Loaded(object sender, RoutedEventArgs e)
        {
            //ACTION PANELS NEED TO BE ADDED AFTER LOADED BECAUSE IF DURING INIT THE UNLOADED SAVE EVENT ON THE ACTION EDIT OVERVIEW PAGE WONT SAVE BEFORE THIS IS LOADED
            //CAUSING ACTIONS TO EXIST THAT WERE JUST DELETED
           
            Dispatcher.BeginInvoke(new System.Action(() => AddControlsToArray()));
           
        }
    }
}
