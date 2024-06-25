
using Handheld_Hardware_Tools.AppWindows.QuickActionWheel.QuickActionWheelItem_Actions;
using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Actions;
using Handheld_Hardware_Tools.Classes.Actions.ActionClass;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Handheld_Hardware_Tools.Classes.Models;
using Handheld_Hardware_Tools.Classes.MouseMode;
using Handheld_Hardware_Tools.UserControls.ActionOverviewUserControls;
using Handheld_Hardware_Tools.UserControls.HomePageUserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Handheld_Hardware_Tools.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class EditMouseModeButtonOverviewPage : ControllerPage
    {

        public EditMouseModeButtonOverviewPage(Classes.MouseMode.MouseActionItem? action = null, bool updateDictionary = false)
        {
            //Move initilize components to sub routine and async it to make pages feel smoother
            Application.Current.Dispatcher.BeginInvoke(new System.Action(() => InitializeActions(action, updateDictionary)));

        }

        public override void HandleControllerInput(string action)
        {
            //typically this is handled in the generic page class, but because this page has a Y action of starting a new action, I need to override
            //the generic class, call the base class, and add Y action here. REFERENCE GENERIC CLASS, because we only care to do something different if controllerNavigatePage is true
            //and action is Y
            base.HandleControllerInput(action);

      
        }


        private void InitializeActions(Classes.MouseMode.MouseActionItem? action = null, bool updateDictionary = false)
        {
            InitializeComponent();
            virtualStackPanel = stackPanel;
            AddControlsToArray(action,  updateDictionary);
            SetPageDefaultInstruction();

        }

        private void ReBaseIDsInActionList()
        {
            int index = 0;
            foreach (object child in stackPanel.Children)
            {
                if (child is ActionOverview_UserControl)
                {
                    ActionOverview_UserControl actionOverview_UserControl = (ActionOverview_UserControl)child;
                    actionOverview_UserControl.action.ID= index;
                    index++;
                }
            }

        }

        public override void SetPageDefaultInstruction()
        {
            //EDITACTIONOVERVIEW PAGE IS SPECIAL BECAUSE IT DOESNST GET THE USUAL CONTROLLER INSTRUCIONT OF SELECT BACK. TYPICALLY THIS IS HANDLED AT THE GENERIC CLASS LEVEL BUT I HAVE THE ADD NEW ACTION BUTTON
            //AND I NEED TO MAKE A SPECIAL INSRUCTION FOR THIS ONE. SO I WILL OVERRIDE THE GENERIC PAGE CLASS RETURNCONTROLTOPAGE and i have to add the instruction at init too so thats the first thing that shows up

            General_Functions.ChangeControllerInstructionPage("SelectAddBack");
        }


        private void AddControlsToArray(Classes.MouseMode.MouseActionItem? saveAction = null, bool updateDictionary = false)
        {
            MouseProfile mouseProfile = (MouseProfile)XML_Management.Instance.LoadXML("MouseProfile");
            

            if (saveAction != null)
            {
                MouseActionItem mai = mouseProfile.mouseActionList.Find(mai => mai.button == saveAction.button);
                int indexMai = mouseProfile.mouseActionList.IndexOf(mai);
                mouseProfile.mouseActionList[indexMai] = saveAction;

                mouseProfile.SaveToXML(); 


                if (updateDictionary)
                {
                    QuickAccessMenu qam = Local_Object.Instance.GetQAMWindow();
                    qam.mouseMode.UpdateMouseProfile();
                 
                }
            }


            List<MouseActionItem> actions = mouseProfile.mouseActionList;

            if (actions.Count > 0)
            {
                foreach (Classes.MouseMode.MouseActionItem action in actions)
                {
                    UserControl userControl = new MouseModeButtonOverview_UserControl(action);
                    stackPanel.Children.Add(userControl);
                    userControls.Add((ControllerUserControl)userControl);
                
                }
            }
           

          

           

        }

        private void SaveActionList()
        {
            //grabs all the actions from each control and then saves them to the XML
            ActionList actions = new ActionList();

            ReBaseIDsInActionList();

            foreach (object child in stackPanel.Children)
            {
                if (child != null)
                {
                    ActionOverview_UserControl editAction_UserControl = (ActionOverview_UserControl)child;
                    actions.Add(editAction_UserControl.action);
                }
            }

            actions.SaveToXML();


            //set controller input class to update controller hotkey dictionary
            QuickAccessMenu mw = (QuickAccessMenu)Application.Current.MainWindow;
            mw.controllerInput.suspendEventsForNewHotKeyList = true;
        }

        public void HandleUserControlInputs(UserControl userControl, string action, string arguments = "")
        {
            int ucIndex = stackPanel.Children.IndexOf(userControl);
            if (ucIndex >= 0)
            {
                switch(action)
                {
                  
                
                    case "Edit":
                        ActionOverview_UserControl actionOverview_UserControl = (ActionOverview_UserControl)userControl;
                        System.Windows.Controls.Page page = new EditActionPage(actionOverview_UserControl.action);

                        QuickAccessMenu mainWindow = (QuickAccessMenu)Application.Current.MainWindow;
                        mainWindow.frame.Content = page;

                        break;
             

                }


               
            }
        }

        public override void PressBPageHandler()
        {
            //override base function to send you back to general settings page
            System.Windows.Controls.Page page = new SettingsPage();

            QuickAccessMenu mainWindow = (QuickAccessMenu)Application.Current.MainWindow;
            mainWindow.frame.Content = page;
        }

        private void Back_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PressBPageHandler();
        }
  



        private void ControllerPage_Unloaded(object sender, RoutedEventArgs e)
        {
            SaveActionList();
        }
    }
}
