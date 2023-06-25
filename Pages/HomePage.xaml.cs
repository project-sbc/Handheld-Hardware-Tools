using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using ModernWpf.Controls;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;

namespace Everything_Handhelds_Tool.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : ControllerPage
    {
        private Dictionary<ControllerUserControl,bool> hpl = new HomePageList();
        public HomePage()
        {
            InitializeComponent();

            AddUserControlsToStackPanel();
        }

        public void AddUserControlsToStackPanel()
        {
            foreach (KeyValuePair<ControllerUserControl,bool> keyValuePair in hpl)
            {
                if (keyValuePair.Value == true) 
                {
                    stackPanel.Children.Add(keyValuePair.Key);
                }
                
            }
        }

        public override void HandleControllerInput(string action)
        {
           
        }

    }
}
