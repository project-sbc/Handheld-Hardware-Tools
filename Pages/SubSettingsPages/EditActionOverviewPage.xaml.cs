
using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Actions;
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

            Classes.Actions.Action newAction = new Classes.Actions.Action();
            newAction.arguments = new List<string>();
            newAction.arguments.Add("6");
            newAction.hotkeyType = "Controller";
            newAction.hotKey = "A+B";
            actions.Add(newAction);

            foreach(Classes.Actions.Action action in actions)
            {
                UserControl userControl = new EditAction_UserControl(action);
                stackPanel.Children.Add(userControl);
                userControls.Add((ControllerUserControl)userControl);
            }

        }



    }
}
