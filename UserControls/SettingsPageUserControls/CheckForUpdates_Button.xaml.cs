using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Actions;
using Handheld_Hardware_Tools.Classes.Actions.ActionClass;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Handheld_Hardware_Tools.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Handheld_Hardware_Tools.UserControls.SettingsPageUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class CheckForUpdates_Button : ControllerUserControl
    {

      
        public CheckForUpdates_Button()
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = button;


            UpdateTextfields();


        }
        private void UpdateTextfields()
        {
            currentVersion.Text = Application.Current.Resources["Usercontrol_CurrentVersion"].ToString() + ": " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();


            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");
            if (settings.lastUpdateCheck != "")
            {
                lastChecked.Text = Application.Current.Resources["Usercontrol_LastChecked"].ToString() + ": " + settings.lastUpdateCheck + "; " + Application.Current.Resources["Usercontrol_Version"].ToString() + ": " + settings.lastUpdateCheckVersion;
            }
            else
            {
                lastChecked.Text = Application.Current.Resources["Usercontrol_LastChecked"].ToString() + ": N/A";
            }

        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("SelectBack");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Update_Management.Instance.CheckForUpdates(false);
            UpdateTextfields();
        }
    }
}
