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

        private Dictionary<int, int> keyboard1 = new Dictionary<int, int>()
        {
            {15, 0 }, {45 ,1}, {75, 2}, {105 , 3}, {135, 4}, {165, 5}, {195, 6}, {225, 7}, {255, 8}, {285, 9}, {315, 10}, {345, 11}, {360, 0}
        };

        private void ButtonPressEvent_controllerJoystickEventOSK(object? sender, controllerJoystickEventArgsOSK e)
        {
            double lr = Math.Sqrt((e.lx)* (e.lx) + (e.ly)*(e.ly));
            double rr = Math.Sqrt((e.rx)* (e.rx) + (e.ry)*(e.ry));
            double la = Math.Tanh((e.ly) / (e.lx));
            double langle = 0;
            if (e.lx < 0) { langle = (la + 2); }
            if (e.lx >= 0 && e.ly >= 0) { langle = la ;  }
            if (e.lx >= 0 && e.ly < 0) { langle = (4 + la); }
            langle = (langle * 90);

            int x = 0;

            if (lr < 2500) { x = 12; }
            else
            {
                if (lr < 28000)
                {
                    if (langle <= 90 || langle >= 270) { x = 14; }
                    else { x = 13; }
                }
                else
                {
                    foreach (KeyValuePair<int, int> keyValuePair in keyboard1)
                    {
                        if (langle <= keyValuePair.Key) { x = keyValuePair.Value; break; }
                    }
                }
            }

            Debug.WriteLine($"x: {e.lx.ToString()} y: {e.ly.ToString()} radius: {lr.ToString()}; angle: {langle}   x:{x}");

        }

        
    }
}
