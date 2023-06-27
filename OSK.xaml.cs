using Everything_Handhelds_Tool.Classes;
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
using System.Windows.Shapes;

namespace Everything_Handhelds_Tool
{
    /// <summary>
    /// Interaction logic for OSK.xaml
    /// </summary>
    public partial class OSK : Window
    {
        ControllerInputOSK inputOSK = new ControllerInputOSK();
        public OSK()
        {
            InitializeComponent();

            inputOSK.buttonPressEvent.controllerJoystickEventOSK += ButtonPressEvent_controllerJoystickEventOSK;
            inputOSK.buttonPressEvent.controllerInputEventOSK += ButtonPressEvent_controllerInputEventOSK;
        }

        private void ButtonPressEvent_controllerInputEventOSK(object? sender, controllerInputEventArgsOSK e)
        {
            
        }

        private void ButtonPressEvent_controllerJoystickEventOSK(object? sender, controllerJoystickEventArgsOSK e)
        {
            double lr = Math.Sqrt((e.lx)* (e.lx) + (e.ly)*(e.ly));
            double rr = Math.Sqrt((e.rx)* (e.rx) + (e.ry)*(e.ry));

            double la = Math.Tanh(e.ly / e.lx);

            Debug.WriteLine($"radius: {lr.ToString()}; angle (rad): {la.ToString()}");

        }

        
    }
}
