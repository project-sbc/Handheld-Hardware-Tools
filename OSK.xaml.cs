using Everything_Handhelds_Tool.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        Button currentHighlightButton;
        public OSK()
        {
            InitializeComponent();

            inputOSK.buttonPressEvent.controllerJoystickEventOSK += ButtonPressEvent_controllerJoystickEventOSK;
            inputOSK.buttonPressEvent.controllerInputEventOSK += ButtonPressEvent_controllerInputEventOSK;

            AddButtonsToDictionary();

            LoadLeftKeyboardDisplayText();
        }
        private void LoadLeftKeyboardDisplayText()
        {
            foreach (KeyValuePair<int,string> kvp in leftKeyboardLowerAlpha)
            {
                Button button = keyboardIndexToButton[kvp.Key];
                button.Content = kvp.Value;
            }
        }
        private void AddButtonsToDictionary()
        {
            keyboardIndexToButton.Add(0, btn0);
            keyboardIndexToButton.Add(1, btn1);
            keyboardIndexToButton.Add(2, btn2);
            keyboardIndexToButton.Add(3, btn3);
            keyboardIndexToButton.Add(4, btn4);
            keyboardIndexToButton.Add(5, btn5);
            keyboardIndexToButton.Add(6, btn6);
            keyboardIndexToButton.Add(7, btn7);
            keyboardIndexToButton.Add(8, btn8);
            keyboardIndexToButton.Add(9, btn9);
            keyboardIndexToButton.Add(10, btn10);
            keyboardIndexToButton.Add(11, btn11);
            keyboardIndexToButton.Add(12, btn12);
            keyboardIndexToButton.Add(13, btn13);
            keyboardIndexToButton.Add(14, btn14);
        }

        private void ButtonPressEvent_controllerInputEventOSK(object? sender, controllerInputEventArgsOSK e)
        {
            
        }

        private Dictionary<int, string> leftKeyboardLowerAlpha = new Dictionary<int, string>()
        {
            {0, "g" }, {1, "t"}, {2, "r"}, {3, "e"},{4, "w"},{5, "q"}, {6, "a"}, {7, "z"}, {8, "x"}, {9, "c"}, {10, "v"}, {11, "b"}, {12, "d"}, {13, "s"}, {14, "f"}
        };

        private Dictionary<int, int> keyboardAngleToIndex = new Dictionary<int, int>()
        {
            {15, 0 }, {45 ,1}, {75, 2}, {105 , 3}, {135, 4}, {165, 5}, {195, 6}, {225, 7}, {255, 8}, {285, 9}, {315, 10}, {345, 11}, {360, 0}
        };

        private Dictionary<int, Button> keyboardIndexToButton = new Dictionary<int, Button>();



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
                if (lr < 22000)
                {
                    if (langle <= 90 || langle >= 270) { x = 14; }
                    else { x = 13; }
                }
                else
                {
                    foreach (KeyValuePair<int, int> keyValuePair in keyboardAngleToIndex)
                    {
                        if (langle <= keyValuePair.Key) { x = keyValuePair.Value; break; }
                    }
                }
            }

            Button newHighlightButton = keyboardIndexToButton[x];
            if (currentHighlightButton != null)
            {
                if (newHighlightButton != currentHighlightButton)
                {
                    currentHighlightButton.Background = Brushes.Transparent;
                    currentHighlightButton = newHighlightButton;
                    currentHighlightButton.Background = Brushes.Blue;
                }
            }
            else
            {
                currentHighlightButton = newHighlightButton;
                currentHighlightButton.Background = Brushes.Blue;
            }

          
            
        }

        
    }
}
