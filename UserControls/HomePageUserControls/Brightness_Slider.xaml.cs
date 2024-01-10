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

namespace Everything_Handhelds_Tool.UserControls.HomePageUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class Brightness_Slider : ControllerUserControl
    {

        private bool dragStarted = false;
    
        public Brightness_Slider()
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = slider;

           
            if (useableOnDevice)
            {
                ConfigureControl();
            }

        }

        public override bool UseableOnThisDevice()
        {
            try
            {
                Brightness_Management.Instance.ReturnBrightness();
                return true;
            }
            catch (Exception ex)
            {
                Log_Writer.Instance.writeLog(ex.Message, "Brightness_Slider usability");
                MessageBox.Show("Error writing to log file. " + ex.Message);
                return false;
            }
      
        }

        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("ChangeBack");
        }

        private void ConfigureControl()
        {
            if (Brightness_Management.Instance.IsDisplayBrightnessControllable())
            {
                slider.Value = Brightness_Management.Instance.ReturnBrightness();
            }
            else { this.Visibility = Visibility.Collapsed; }
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
            int brightness = (int)Math.Round(slider.Value, 0);
            Brightness_Management.Instance.SetBrightness(brightness);
        }
    }
}
