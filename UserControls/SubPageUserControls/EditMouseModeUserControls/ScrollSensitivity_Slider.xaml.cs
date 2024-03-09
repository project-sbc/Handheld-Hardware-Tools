using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Pages;
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

namespace Everything_Handhelds_Tool.UserControls.SubPageUserControls.EditMouseModeUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class ScrollSensitivity_Slider : ControllerUserControl
    {

        private bool dragStarted = false;
        public ScrollSensitivity_Slider(double value)
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = slider;

            //set control
            ConfigureControl(value) ;

        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("ChangeBack");
        }

        private void ConfigureControl(double value)
        {
            slider.Value = value;
        }

     

        private void ControllerUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ControlChangeValueHandler();
        }

        private void control_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            dragStarted = false;
            ControlChangeValueHandler();
        }

        private void control_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            dragStarted = true;
        }

     
        public override void ControlChangeValueHandler()
        {
            MouseModeSettingsPage mouseModeSettingsPage = (MouseModeSettingsPage)Local_Object.Instance.GetGeneralWindowPage(this);

            if (mouseModeSettingsPage != null)
            {
                mouseModeSettingsPage.mouseProfile.sensitivityScrollValue = slider.Value;
            }
        }
    }
}
