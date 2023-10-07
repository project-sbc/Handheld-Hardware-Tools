
using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.Models;
using Everything_Handhelds_Tool.UserControls.HomePageUserControls;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms.Integration;

namespace Everything_Handhelds_Tool.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class PowerPage : ControllerPage
    {
        
        public PowerPage()
        {
            InitializeComponent();

            virtualStackPanel = stackPanel;

            AddControlsToArray();
        }

        private void AddControlsToArray()
        {
            //There is a general function that searches the stack panel and adds to the list of ControllerUserControls. It makes sure
            //visibility isn't collapsed too
            userControls= General_Functions.SearchStackPanelReturnArray(stackPanel);
        }



    }
}
