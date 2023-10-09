
using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.HomePage;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.Models;
using Everything_Handhelds_Tool.UserControls.HomePageUserControls;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System;

namespace Everything_Handhelds_Tool.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : ControllerPage
    {
        
        public HomePage()
        {
            //Move initilize components to sub routine and async it to make pages feel smoother
            Dispatcher.BeginInvoke(new Action(() => Initialize()));

        }

        private void Initialize()
        {
            InitializeComponent();
            virtualStackPanel = stackPanel;
            AddUserControlsToStackPanel();
        }

        public void AddUserControlsToStackPanel()
        {
            //Get the XML list from the UserConfiguration folder for what controls go on the home page
            HomePageUserControlList chpl = (HomePageUserControlList)XML_Management.Instance.LoadXML("UserConfiguration\\HomePage\\HomePageUserControlList.xml", "HomePageUserControlList");

            //loop through and add the usercontrols
            foreach (HomePageItem hpi in chpl)
            {
                if (hpi.UserControlVisible)
                {
                    ControllerUserControl controllerUserControl = ReturnUserControlByName(hpi.UserControlName);
                    if (controllerUserControl != null)
                    {
                        stackPanel.Children.Add(controllerUserControl);
                        userControls.Add(controllerUserControl);
                    }

                }
            }


            chpl.SaveToXML();

        }
        private ControllerUserControl ReturnUserControlByName(string name)
        {
            switch(name)
            {
                case "TDP_Slider":
                    return new TDP_Slider();
                case "TDP_Boost_Slider":
                    return new TDP_Boost_Slider();
                case "Brightness_Slider":
                    return new Brightness_Slider();
                case "Volume_Slider":
                    return new Volume_Slider();
                default:
                    return null;
            }
        }
       
       
    }
}
