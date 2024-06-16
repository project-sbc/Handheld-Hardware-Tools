using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Handheld_Hardware_Tools.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using System.Windows;
using System.Windows.Media.Imaging;



namespace Handheld_Hardware_Tools.UserControls.InfoPageUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class HHDiscord_Button : ControllerUserControl
    {

      
        public HHDiscord_Button()
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = button;

            string picLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Images\\Logos\\HHLogo.png");


            //
            image.Source = new BitmapImage(new Uri(picLocation));
        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("SelectBack");
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            
            Process.Start(new ProcessStartInfo(hyperlink.NavigateUri) { UseShellExecute = true });
        }
    }
}
