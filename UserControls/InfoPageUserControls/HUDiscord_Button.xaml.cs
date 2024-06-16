using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Handheld_Hardware_Tools.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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


namespace Handheld_Hardware_Tools.UserControls.InfoPageUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class HUDiscord_Button : ControllerUserControl
    {

      
        public HUDiscord_Button()
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = button;


            string picLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Images\\Logos\\HULogo.jpg");


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
