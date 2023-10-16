
using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Actions;
using Everything_Handhelds_Tool.Classes.Actions.ActionClass;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.Models;
using Everything_Handhelds_Tool.UserControls.EditActionUserControls;
using Everything_Handhelds_Tool.UserControls.HomePageUserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Everything_Handhelds_Tool.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class EditActionOverviewPage : ControllerPage
    {

        public EditActionOverviewPage()
        {
            //Move initilize components to sub routine and async it to make pages feel smoother
            Dispatcher.BeginInvoke(new System.Action(() => Initialize()));

        }

        private void Initialize()
        {
            InitializeComponent();
            virtualStackPanel = stackPanel;
            AddControlsToArray();
        }
        private void AddControlsToArray()
        {
            ActionList actions = (ActionList)XML_Management.Instance.LoadXML("ActionList");

            Classes.Actions.Action newAction = new Change_TDP();
            newAction.arguments = new List<string>();
            newAction.arguments.Add("6");
            newAction.hotkeyType = "Controller";
            newAction.hotKey = "772";
            actions.Add(newAction);

            foreach(Classes.Actions.Action action in actions)
            {
                UserControl userControl = new EditAction_UserControl(action);
                stackPanel.Children.Add(userControl);
                userControls.Add((ControllerUserControl)userControl);
            }

        }

        public void HandleUserControlInputs(UserControl userControl, string action)
        {
            int ucIndex = stackPanel.Children.IndexOf(userControl);
            if (ucIndex >= 0)
            {
                switch(action)
                {
                    case "MoveUp":
                        if (ucIndex > 0)
                        {
                            if (ucIndex == highlightedUserControl)
                            {
                                highlightedUserControl = ucIndex - 1;
                            }

                            stackPanel.Children.Remove(userControl);
                            stackPanel.Children.Insert(ucIndex-1,userControl);

                            userControls.Remove((ControllerUserControl)userControl);
                            userControls.Insert(ucIndex-1, (ControllerUserControl)userControl);

                            
                        }

                        break;
                    case "MoveDown":
                        if (ucIndex < userControls.Count-1)
                        {
                            if (ucIndex == highlightedUserControl)
                            {
                                highlightedUserControl = ucIndex + 1;
                            }

                            stackPanel.Children.Remove(userControl);
                            stackPanel.Children.Insert(ucIndex + 1, userControl);

                            userControls.Remove((ControllerUserControl)userControl);
                            userControls.Insert(ucIndex + 1, (ControllerUserControl)userControl);


                        }

                        break;

                }


            }
        }


    }
}
