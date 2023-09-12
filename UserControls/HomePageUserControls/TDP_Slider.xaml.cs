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
    public partial class TDP_Slider : ControllerUserControl
    {

        private bool tdpValueLoaded = false;
        private bool dragStarted = false;
        public TDP_Slider()
        {
            InitializeComponent();

            borderControl = border;

            control.Value = TDP_Management.Instance.ReadTDP();

        }


        public override void HandleControllerInput(string action) 
        { 
                    
        }

        private void ControllerUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void control_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            dragStarted = false;
            ChangeTDP();
        }

        private void control_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            dragStarted = true;
        }

        private void ReadTDP()
        {
            int tdp = TDP_Management.Instance.ReadTDP();
        }
        private void ChangeTDP()
        {
            int tdp = (int)Math.Round(control.Value, 0);
            TDP_Management.Instance.changeTDP(tdp);
        }
    }
}
