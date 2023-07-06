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
        Button currentHighlightButtonLeft;
        Button currentHighlightButtonRight;
        public OSK()
        {
            InitializeComponent();

            inputOSK.buttonPressEvent.controllerJoystickEventOSK += ButtonPressEvent_controllerJoystickEventOSK;
            inputOSK.buttonPressEvent.controllerInputEventOSK += ButtonPressEvent_controllerInputEventOSK;

            AddButtonsToDictionary();

            LoadLeftKeyboardDisplayText();
            LoadRightKeyboardDisplayText();
        }
        private void LoadLeftKeyboardDisplayText()
        {
            foreach (KeyValuePair<int,string> kvp in leftKeyboardLowerAlpha)
            {
                Button button = keyboardIndexToButtonLeft[kvp.Key];
                button.Content = kvp.Value;
            }
        }

        private void LoadRightKeyboardDisplayText()
        {
            foreach (KeyValuePair<int, string> kvp in rightKeyboardLowerAlpha)
            {
                Button button = keyboardIndexToButtonRight[kvp.Key];
                button.Content = kvp.Value;
            }
        }
        private void AddButtonsToDictionary()
        {
            keyboardIndexToButtonLeft.Add(0, btnL0);
            keyboardIndexToButtonLeft.Add(1, btnL1);
            keyboardIndexToButtonLeft.Add(2, btnL2);
            keyboardIndexToButtonLeft.Add(3, btnL3);
            keyboardIndexToButtonLeft.Add(4, btnL4);
            keyboardIndexToButtonLeft.Add(5, btnL5);
            keyboardIndexToButtonLeft.Add(6, btnL6);
            keyboardIndexToButtonLeft.Add(7, btnL7);
            keyboardIndexToButtonLeft.Add(8, btnL8);
            keyboardIndexToButtonLeft.Add(9, btnL9);
            keyboardIndexToButtonLeft.Add(10, btnL10);
            keyboardIndexToButtonLeft.Add(11, btnL11);
            keyboardIndexToButtonLeft.Add(12, btnL12);
            keyboardIndexToButtonLeft.Add(13, btnL13);
            keyboardIndexToButtonLeft.Add(14, btnL14);

            keyboardIndexToButtonRight.Add(0, btnR0);
            keyboardIndexToButtonRight.Add(1, btnR1);
            keyboardIndexToButtonRight.Add(2, btnR2);
            keyboardIndexToButtonRight.Add(3, btnR3);
            keyboardIndexToButtonRight.Add(4, btnR4);
            keyboardIndexToButtonRight.Add(5, btnR5);
            keyboardIndexToButtonRight.Add(6, btnR6);
            keyboardIndexToButtonRight.Add(7, btnR7);
            keyboardIndexToButtonRight.Add(8, btnR8);
            keyboardIndexToButtonRight.Add(9, btnR9);
            keyboardIndexToButtonRight.Add(10, btnR10);
            keyboardIndexToButtonRight.Add(11, btnR11);
            keyboardIndexToButtonRight.Add(12, btnR12);
            keyboardIndexToButtonRight.Add(13, btnR13);
            keyboardIndexToButtonRight.Add(14, btnR14);
        }

        private void ButtonPressEvent_controllerInputEventOSK(object? sender, controllerInputEventArgsOSK e)
        {
            
        }

        private Dictionary<int, string> leftKeyboardLowerAlpha = new Dictionary<int, string>()
        {
            {0, "g" }, {1, "t"}, {2, "r"}, {3, "e"},{4, "w"},{5, "q"}, {6, "a"}, {7, "z"}, {8, "x"}, {9, "c"}, {10, "v"}, {11, "b"}, {12, "d"}, {13, "s"}, {14, "f"}
        };
        private Dictionary<int, string> rightKeyboardLowerAlpha = new Dictionary<int, string>()
        {
            {0, ";" }, {1, "p"}, {2, "o"}, {3, "i"},{4, "u"},{5, "y"}, {6, "h"}, {7, "n"}, {8, "m"}, {9, ","}, {10, "."}, {11, "?"}, {12, "k"}, {13, "j"}, {14, "l"}
        };
        private Dictionary<int, int> keyboardAngleToIndex = new Dictionary<int, int>()
        {
           // {15, 0 }, {45 ,1}, {75, 2}, {105 , 3}, {135, 4}, {165, 5}, {195, 6}, {225, 7}, {255, 8}, {285, 9}, {315, 10}, {345, 11}, {360, 0}
            {5, 0 }, {45 ,1}, {85, 2}, {95 , 3}, {135, 4}, {175, 5}, {185, 6}, {225, 7}, {265, 8}, {275, 9}, {315, 10}, {355, 11}, {360, 0}
        };

        private Dictionary<int, Button> keyboardIndexToButtonLeft = new Dictionary<int, Button>();
        private Dictionary<int, Button> keyboardIndexToButtonRight = new Dictionary<int, Button>();

        private int ReturnButtonZoneIndex(double x, double y)
        {
            double radius = Math.Sqrt((x) * (x) + (y) * (y));

            double iAngle = Math.Tanh((y) / (x));
            double angle = 0;
            if (x < 0) { angle = (iAngle + 2); }
            if (x >= 0 && x >= 0) { angle = iAngle; }
            if (x >= 0 && y < 0) { angle = (4 + iAngle); }
            angle = (angle * 90);

            int buttonIndex = 0;

            if (radius < 2500) { buttonIndex = 12; }
            else
            {
                if (radius < 22000)
                {
                    if (angle <= 85 || angle >= 275) { buttonIndex = 14; }
                    else
                    {
                        if (angle >= 95 && angle <= 265)
                        {
                            buttonIndex = 13;
                        }
                        else
                        {
                            if (angle < 95) { buttonIndex = 3; }
                            else
                            {
                                buttonIndex = 9;
                            }

                        }
                    }

                }
                else
                {
                    foreach (KeyValuePair<int, int> keyValuePair in keyboardAngleToIndex)
                    {
                        if (angle <= keyValuePair.Key) { buttonIndex = keyValuePair.Value; break; }
                    }
                }
            }

            return buttonIndex;
        }

        private void ButtonPressEvent_controllerJoystickEventOSK(object? sender, controllerJoystickEventArgsOSK e)
        {
            //left stick inputs first
            int leftButtonIndex = ReturnButtonZoneIndex(e.lx, e.ly);
            int rightButtonIndex = ReturnButtonZoneIndex(e.rx, e.ry);


            Button newHighlightButtonLeft = keyboardIndexToButtonLeft[leftButtonIndex];
            if (currentHighlightButtonLeft != null)
            {
                if (newHighlightButtonLeft != currentHighlightButtonLeft)
                {
                    currentHighlightButtonLeft.Background = Brushes.Transparent;
                    currentHighlightButtonLeft = newHighlightButtonLeft;
                    currentHighlightButtonLeft.Background = Brushes.Blue;
                }
            }
            else
            {
                currentHighlightButtonLeft = newHighlightButtonLeft;
                currentHighlightButtonLeft.Background = Brushes.Blue;
            }

            Button newHighlightButtonRight = keyboardIndexToButtonRight[rightButtonIndex];
            if (currentHighlightButtonRight != null)
            {
                if (newHighlightButtonRight != currentHighlightButtonRight)
                {
                    currentHighlightButtonRight.Background = Brushes.Transparent;
                    currentHighlightButtonRight = newHighlightButtonRight;
                    currentHighlightButtonRight.Background = Brushes.Blue;
                }
            }
            else
            {
                currentHighlightButtonRight = newHighlightButtonRight;
                currentHighlightButtonRight.Background = Brushes.Blue;
            }

        }

        
    }
}
