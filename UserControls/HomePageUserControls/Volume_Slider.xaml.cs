﻿using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
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

namespace Handheld_Hardware_Tools.UserControls.HomePageUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class Volume_Slider : ControllerUserControl
    {
        private bool dragStarted = false;
      
        public Volume_Slider()
        {
            InitializeComponent();
            //set virtual border
            borderControl = border;
            mainControl = control;
            toggleSwitchControl = toggleSwitch;
            //set control
            if (useableOnDevice)
            {
                ConfigureControl();
            }
         

        }
        private void ConfigureControl()
        {
            int volume = Volume_Management.Instance.ReadAndReturnVolume();

            if (volume >= 0)
            {
                control.Value = volume;
                toggleSwitch.IsChecked = !Volume_Management.Instance.ReadAndReturnVolumeMute();
                
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
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

        private void control_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            dragStarted = false;
            ControlChangeValueHandler();
        }
        private void ControllerUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ControlChangeValueHandler();
        }
        private void control_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            dragStarted = true;
        }
             
        public override void ControlChangeValueHandler()
        {
            //set volume
            int volume = (int)Math.Round(control.Value, 0);
            Volume_Management.Instance.SetMasterVolume(volume);

            //set mute
            if (toggleSwitch.IsChecked == true)
            {
                
            }
            else
            {
                
            }

        }

        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("ChangeToggleBack");
        }

        private void toggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            Volume_Management.Instance.SetMasterVolumeMute(false);
            statusIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Speaker216;
        }

        private void toggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            Volume_Management.Instance.SetMasterVolumeMute(true);
            statusIcon.Symbol = Wpf.Ui.Common.SymbolRegular.SpeakerMute16;
        }
    }
}
