using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
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

namespace Everything_Handhelds_Tool.UserControls.HomePageUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class EPP_Slider : ControllerUserControl
    {

        private bool dragStarted = false;
        public EPP_Slider()
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = slider;

            //set control
           ConfigureControl();

        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("ChangeBack");
        }

        private void ConfigureControl()
        {
         
            slider.Value = Powercfg_Management.Instance.ReadAndReturnEPP();
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

            int epp = (int)Math.Round(slider.Value, 0);
            Powercfg_Management.Instance.ChangeEPP(epp);
        }
    }
}
