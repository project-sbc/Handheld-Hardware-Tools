using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.Models;
using Everything_Handhelds_Tool.Models.DefaultModels;
using Everything_Handhelds_Tool.UserControls.HomePageUserControls;
using ModernWpf.Controls;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;

namespace Everything_Handhelds_Tool.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : ControllerPage
    {
       
        public HomePage()
        {
            InitializeComponent();

            AddUserControlsToStackPanel();
        }

        public void AddUserControlsToStackPanel()
        {
            //Get the XML list from the UserConfiguration folder for what controls go on the home page
            CompleteHomePageList chpl = (CompleteHomePageList)XML_Management.Load_XML("UserConfiguration\\HomePage\\HomePage.xml", "CompleteHomePageList");

            //loop through and add the usercontrols
            foreach (HomePageItem hpi in chpl)
            {
                if (hpi.UserControlVisible)
                {
                    stackPanel.Children.Add(ReturnUserControlByName(hpi.UserControlName));
                }   
            }
            
            chpl.SaveToXML();
        }
        private UserControl ReturnUserControlByName(string name)
        {
            switch(name)
            {
                case "TDP_Slider":
                    return new TDP_Slider();

                default:
                    return null;
            }
        }
        public override void HandleControllerInput(string action)
        {
           
        }

    }
}
