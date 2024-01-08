
using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Actions;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.Models;
using Everything_Handhelds_Tool.UserControls.HomePageUserControls;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using Everything_Handhelds_Tool.UserControls.ActionWrapPanelUserControls;

namespace Everything_Handhelds_Tool.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class ActionPage : ControllerWrapPanelPage
    {
        
        public ActionPage()
        {
            //Move initilize components to sub routine and async it to make pages feel smoother
            Dispatcher.BeginInvoke(new System.Action(() => Initialize()));

        }

        private void Initialize()
        {
            InitializeComponent();
            virtualWrapPanel = wrapPanel;

           
        }
        private void AddControlsToArray()
        {
            //There is a general function that searches the wrap panel and adds to the list of ControllerUserControls. It makes sure
            //visibility isn't collapsed too

            ActionList actions = (ActionList)XML_Management.Instance.LoadXML("ActionList");



            foreach (Classes.Actions.Action action in actions)
            {
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
