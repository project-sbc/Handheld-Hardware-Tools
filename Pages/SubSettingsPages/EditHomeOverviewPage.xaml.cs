
using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Actions;
using Everything_Handhelds_Tool.Classes.Actions.ActionClass;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.HomePage;
using Everything_Handhelds_Tool.Classes.Models;
using Everything_Handhelds_Tool.UserControls.ActionOverviewUserControls;
using Everything_Handhelds_Tool.UserControls.HomePageUserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Everything_Handhelds_Tool.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class EditHomeOverviewPage : ControllerPage
    {

        public EditHomeOverviewPage(Classes.Actions.Action? action = null)
        {
            //Move initilize components to sub routine and async it to make pages feel smoother
            Dispatcher.BeginInvoke(new System.Action(() => InitializeActions()));

        }

     

        private void InitializeActions()
        {
            InitializeComponent();
            virtualStackPanel = stackPanel;
            AddControlsToArray();
            SetPageDefaultInstruction();

        }

      

      


        private void AddControlsToArray()
        {
            HomePageUserControlList homePageList = (HomePageUserControlList)XML_Management.Instance.LoadXML("HomePageUserControlList");



            if (homePageList.Count > 0)
            {
                foreach (HomePageItem hpi in homePageList)
                {
                    UserControl userControl = new HomePageOverview_UserControl(hpi);
                    stackPanel.Children.Add(userControl);
                    userControls.Add((ControllerUserControl)userControl);
                  
                }
            }
           

          

           

        }

        private void SaveActionList()
        {
            //grabs all the actions from each control and then saves them to the XML
            HomePageUserControlList homePageList = new HomePageUserControlList();

           

            foreach (object child in stackPanel.Children)
            {
                if (child != null)
                {
                    HomePageOverview_UserControl editHPI_UserControl = (HomePageOverview_UserControl)child;
                    homePageList.Add(editHPI_UserControl.hpi);
                }
            }

            homePageList.SaveToXML();

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
                    case "Toggle":
                        

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


                if (action == "MoveUp" || action == "MoveDown" || action == "Toggle")
                {
                    //make sure to save XML and rebase IDs when moving or deleting, because that will screw up the order
                    SaveActionList();
                }

            }
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
       

       

        private void ControllerPage_Unloaded(object sender, RoutedEventArgs e)
        {
            SaveActionList();
        }
    }
}
