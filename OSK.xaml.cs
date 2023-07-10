using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WindowsInput;
using WindowsInput.Native;

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
        private Dictionary<int, Button> keyboardIndexToButtonLeft = new Dictionary<int, Button>();
        private Dictionary<int, Button> keyboardIndexToButtonRight = new Dictionary<int, Button>();
        Classes.Models.Keyboard keyboard = new QWERTYKeyboard();

        InputSimulator inputSimulator = new InputSimulator();
        List<VirtualKeyCode> vkcChord = new List<VirtualKeyCode>();
        public OSK()
        {
            InitializeComponent();

            SetLocation();

            inputOSK.buttonPressEvent.controllerJoystickEventOSK += ButtonPressEvent_controllerJoystickEventOSK;
            inputOSK.buttonPressEvent.controllerInputEventOSK += ButtonPressEvent_controllerInputEventOSK;

            AddButtonsToDictionary();

            currentHighlightButtonLeft = btnL12;
            currentHighlightButtonRight = btnR12;

            LoadLeftKeyboardDisplayText();
            LoadRightKeyboardDisplayText();
        }
        private void SetLocation()
        {
            this.Width = SystemParameters.FullPrimaryScreenWidth;
            this.Height = Math.Round(SystemParameters.FullPrimaryScreenHeight * 0.35,0);
            this.Top = SystemParameters.FullPrimaryScreenHeight - Math.Round(SystemParameters.FullPrimaryScreenHeight * 0.35, 0);
        }



        private void LoadLeftKeyboardDisplayText()
        {
            foreach (KeyValuePair<int,VirtualKeyCodeDisplayCharacter> kvp in keyboard.leftKeyboard)
            {
                Button button = keyboardIndexToButtonLeft[kvp.Key];
                VirtualKeyCodeDisplayCharacter character = kvp.Value;
                button.Content = character.DisplayCharacter;
            }
        }

        private void LoadRightKeyboardDisplayText()
        {
            foreach (KeyValuePair<int, VirtualKeyCodeDisplayCharacter> kvp in keyboard.rightKeyboard)
            {
                Button button = keyboardIndexToButtonRight[kvp.Key];
                VirtualKeyCodeDisplayCharacter character = kvp.Value;
                button.Content = character.DisplayCharacter;
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
            if (e.Action == "LeftShoulder") { currentHighlightButtonLeft.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent)); }
            if (e.Action == "RightShoulder") { currentHighlightButtonRight.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent)); }
            if (e.Action == "X") { inputSimulator.Keyboard.KeyPress(VirtualKeyCode.BACK); }
            if (e.Action == "Y") { inputSimulator.Keyboard.KeyPress(VirtualKeyCode.SPACE); }
        }


        private Dictionary<int, int> keyboardAngleToIndex = new Dictionary<int, int>()
        {
           //old set up before i made the cardinals only 10 degrees wide {15, 0 }, {45 ,1}, {75, 2}, {105 , 3}, {135, 4}, {165, 5}, {195, 6}, {225, 7}, {255, 8}, {285, 9}, {315, 10}, {345, 11}, {360, 0}
            {5, 0 }, {45 ,1}, {85, 2}, {95 , 3}, {135, 4}, {175, 5}, {185, 6}, {225, 7}, {265, 8}, {275, 9}, {315, 10}, {355, 11}, {360, 0}
        };



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
                    currentHighlightButtonLeft.Background = Brushes.Gray;
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
                    currentHighlightButtonRight.Background = Brushes.Gray;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int index = Int32.Parse(button.Name.Substring(4, button.Name.Length - 4));
            string keyboardSide = button.Name.Substring(0, 4);
            VirtualKeyCode virtualKeyCode;
            int indexButton = -1;
            switch(keyboardSide)
            {
                case "btnL":
                    indexButton = keyboardIndexToButtonLeft.FirstOrDefault(x => x.Value == button).Key;
                    virtualKeyCode =(VirtualKeyCode)((VirtualKeyCodeDisplayCharacter)keyboard.leftKeyboard[indexButton]).vkc;
                    SendVirtualKeyCode(virtualKeyCode);
                    break;
                case "btnR":
                    indexButton = keyboardIndexToButtonRight.FirstOrDefault(x => x.Value == button).Key;
                    virtualKeyCode = (VirtualKeyCode)((VirtualKeyCodeDisplayCharacter)keyboard.rightKeyboard[indexButton]).vkc;
                    SendVirtualKeyCode(virtualKeyCode);
                    break;
                default:

                    break;
            }


        }

        private void SendVirtualKeyCode(VirtualKeyCode virtualKeyCode)
        {
            inputSimulator.Keyboard.KeyDown(virtualKeyCode);
            inputSimulator.Keyboard.KeyUp(virtualKeyCode);
        }

    }
  
}
