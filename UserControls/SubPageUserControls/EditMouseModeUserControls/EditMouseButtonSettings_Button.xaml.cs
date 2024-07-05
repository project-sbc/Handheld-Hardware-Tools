using Handheld_Hardware_Tools.AppWindows.MainWindow;
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

namespace Handheld_Hardware_Tools.UserControls.SubPageUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class EditMouseButtonSettings_Button : ControllerUserControl
    {
       
      
        public EditMouseButtonSettings_Button()
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = button;

        
        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("SelectBack");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //navigate to the specific settings page


            QuickAccessMenu mainWindow = (QuickAccessMenu)Application.Current.MainWindow;
            MouseModeSettingsPage mouseModeSettingsPage = (MouseModeSettingsPage)mainWindow.frame.Content;

            Page page = new EditMouseModeButtonOverviewPage();

            mainWindow.frame.Content = page;

        }
    }
}
