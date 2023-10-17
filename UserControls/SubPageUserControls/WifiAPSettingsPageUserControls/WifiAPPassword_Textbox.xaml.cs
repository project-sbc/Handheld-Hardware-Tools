using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
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

namespace Everything_Handhelds_Tool.UserControls.SubPageUserControls.WifiAPSettingsPageUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class WifiAPPassword_Textbox : ControllerUserControl
    {


        public WifiAPPassword_Textbox()
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = textbox;

            //set control
           ConfigureControl();

        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("ChangeBack");
        }

        private void ConfigureControl()
        {
            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");


            textbox.Text = settings.passwordWifiAP;
        }

        
     
        public override void ControlChangeValueHandler()
        {
            string pass = textbox.Text;
            if (pass.Length > 3)
            {
                Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");
                settings.passwordWifiAP = pass;
                settings.SaveToXML();
            }
        }
    }
}
