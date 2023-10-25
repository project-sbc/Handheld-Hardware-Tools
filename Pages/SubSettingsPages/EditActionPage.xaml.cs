
using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Actions;
using Everything_Handhelds_Tool.Classes.Actions.ActionClass;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.Models;
using Everything_Handhelds_Tool.UserControls.ActionOverviewUserControls;
using Everything_Handhelds_Tool.UserControls.EditActionUserControls;
using Everything_Handhelds_Tool.UserControls.HomePageUserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Everything_Handhelds_Tool.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class EditActionPage : ControllerPage
    {
        public Classes.Actions.Action action;
        public EditAction_Combobox actionCombobox;
        public EditAction_ArgumentListView actionArgumentListView;
        public EditActionPage(Classes.Actions.Action importAction = null)
        {
            //Move initilize components to sub routine and async it to make pages feel smoother
            Dispatcher.BeginInvoke(new System.Action(() => InitializeActions(importAction)));

         
        }

      

        private void InitializeActions(Classes.Actions.Action? importAction = null)
        {
            //needs to set action first, because controls generated during InitializeComponent rely on this value
            if (importAction != null)
            {
                action = importAction;
            }

            InitializeComponent();
            virtualStackPanel = stackPanel;
            AddControlsToArray();


        }


        private void AddControlsToArray()
        {
            //There is a general function that searches the stack panel and adds to the list of ControllerUserControls. It makes sure
            //visibility isn't collapsed too

            //add controls passing values as needed
            if (action != null) 
            { 
                stackPanel.Children.Add(new EditAction_Combobox(action.actionName)); 
                stackPanel.Children.Add(new EditAction_ArgumentListView(action)); 
            }
            else 
            { 
                stackPanel.Children.Add(new EditAction_Combobox());
                stackPanel.Children.Add(new EditAction_ArgumentListView());
            }


            userControls = General_Functions.SearchStackPanelReturnArray(stackPanel);
        }


        public void UpdateActionFromSubControl()
        {

        }

        private void SaveActionList()
        {
            //grabs all the actions from each control and then saves them to the XML
            ActionList actions = new ActionList();

            foreach (object child in stackPanel.Children)
            {
                if (child != null)
                {
                    ActionOverview_UserControl editAction_UserControl = (ActionOverview_UserControl)child;
                    actions.Add(editAction_UserControl.action);
                }
            }

            actions.SaveToXML();

        }

      
    }
}
