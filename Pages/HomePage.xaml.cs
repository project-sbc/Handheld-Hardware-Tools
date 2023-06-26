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
            CompleteHomePageList chpl = (CompleteHomePageList)XML_Management.Load_XML("UserConfiguration\\HomePage\\HomePage.xml", "CompleteHomePageList");

            foreach (HomePageItem hpi in chpl)
            {
                hpi.UserControlVisible = false;
                
            }
            
            chpl.SaveToXML();
        }

        public override void HandleControllerInput(string action)
        {
           
        }

    }
}
