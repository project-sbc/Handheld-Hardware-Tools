using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Actions;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Everything_Handhelds_Tool.UserControls.EditActionUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class EditAction_ArgumentListView : ControllerUserControl
    {


        public EditAction_ArgumentListView(string actionName = "")
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = listView;


            //get page's action details

            //set control
           ConfigureControl(actionName);

        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("ChangeBack");
        }


        public Dictionary<string, string>actionDictionary = new DefaultActionItemDictionary();
        private void ConfigureControl(string actionName = "")
        {
          

        }
              

        public override void ControlChangeValueHandler()
        {
            //set originalselectedindex to the NEW selected index to prevent accidental change when returning control to page (where it will reset to original if indexes dont match)
           

        }
    }


    
}
