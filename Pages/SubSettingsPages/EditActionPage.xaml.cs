﻿
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

        //bool to determine if existing or new action
        private bool newAction = true;
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
                newAction = false;
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
                actionCombobox = new EditAction_Combobox(action.actionName);
                stackPanel.Children.Add(actionCombobox);
                actionArgumentListView = new EditAction_ArgumentListView(action);
                stackPanel.Children.Add(actionArgumentListView);

                
                stackPanel.Children.Add(new EditAction_InActionPanelToggleSwitch(action.displayInActionPanel));
            }
            else 
            {
                actionCombobox = new EditAction_Combobox();
                stackPanel.Children.Add(actionCombobox);
                actionArgumentListView = new EditAction_ArgumentListView();
                stackPanel.Children.Add(actionArgumentListView);
                stackPanel.Children.Add(new EditAction_InActionPanelToggleSwitch());
            }


            userControls = General_Functions.SearchStackPanelReturnArray(stackPanel);
        }


        public void UpdateActionFromSubControl()
        {

        }

        private void SaveActionToEditOverviewPage()
        {
            //gets the overview page, sends to the frame on the mainwindow, passes the action to the page so it will add it and then save
            EditActionOverviewPage editActionOverviewPage = new EditActionOverviewPage(action);
            
            //get mainwindow frame
            MainWindow mainWindow = Local_Object.Instance.GetMainWindow();
            mainWindow.frame.Content = editActionOverviewPage;
        }

        private void ValidateInputs()
        {

        }
        private void save_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //on save we need to make sure everything is good by calling validate inputs
            //THEN save the action by sending to the overview page with action argument, and the editoverview page will handle the saving
            ValidateInputs();  
            SaveActionToEditOverviewPage();
        }

        private void Back_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
