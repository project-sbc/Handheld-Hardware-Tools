using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
        private bool showMainWindowAtClose = false;


        ControllerInputOSK inputOSK = new ControllerInputOSK();
        Button currentHighlightButtonLeft;
        int currentLeftIndex = -1;
        int currentRightIndex = -1;

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

            SuspendMainControllerInputCheckMainWindowOpen();

            AddButtonsToDictionary();

            HighlightButton(btnL12);
            HighlightButton(btnR12);

            LoadLeftKeyboardDisplayText();
            LoadRightKeyboardDisplayText();

            inputOSK.buttonPressEvent.controllerJoystickEventOSK += ButtonPressEvent_controllerJoystickEventOSK;
            inputOSK.buttonPressEvent.controllerInputEventOSK += ButtonPressEvent_controllerInputEventOSK;
            inputOSK.buttonPressEvent.controllerJoystickDPadEventOSK += ButtonPressEvent_controllerJoystickDPadEventOSK;
        }

        private void ButtonPressEvent_controllerJoystickDPadEventOSK(object? sender, controllerInputEventArgsOSK e)
        {
            //this is for when the keyboard is not in ABSOLUTE joystick mode (the mode where you need angle and radius to figure out the button)
            //we need a left and right joystick "Dpad"  input to mimic basically two dpads that will control button navigation
            
            //format is Left_[direction] and Right_[direction] so lets start
            //by distinguishing between left and right
            if (e.Action.Contains("Left_"))
            {
                UnhighlightButton(currentHighlightButtonLeft);
                currentHighlightButtonLeft = HandleButtonOffsetFromControllerInput(currentHighlightButtonLeft, e.Action.Replace("Left_", ""), leftGrid);
                HighlightButton(currentHighlightButtonLeft);
            }
            else
            {
                UnhighlightButton(currentHighlightButtonRight);
                currentHighlightButtonRight = HandleButtonOffsetFromControllerInput(currentHighlightButtonRight, e.Action.Replace("Right_", ""), rightGrid);
                HighlightButton(currentHighlightButtonRight);
            }

        }

        private Button HandleButtonOffsetFromControllerInput(Button button, string direction, Grid grid)
        {
            int gridRow =Grid.GetRow(button);
            int gridCol =Grid.GetColumn(button);

            switch (direction)
            {
                case "Up":
                    if (gridRow > 0)
                    {
                        gridRow = gridRow - 1;
                    }
                    break;
                case "Down":
                    if (gridRow < grid.RowDefinitions.Count-1)
                    {
                        gridRow = gridRow + 1;
                    }

                    break;
                case "Left":
                    if (gridCol > 0)
                    {
                        gridCol = gridCol - 1;
                    }

                    break;
                case "Right":
                    if (gridCol < grid.ColumnDefinitions.Count - 1)
                    {
                        gridCol = gridCol + 1;
                    }

                    break;
            }


            Button newHighlightButton = (Button)grid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == gridRow && Grid.GetColumn(e) == gridCol);

            if (newHighlightButton != null)
            {
                return newHighlightButton;
            }
            else
            {
                return button;
            }

        }

        private void SuspendMainControllerInputCheckMainWindowOpen()
        {
            //suspend controller input at controller input class by toggling the bool
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.controllerInput.publicSuspendEventsForOSK = true;

            //check to see if the main window should show again after toggling the keyboard
            //if window was open, we will show it again when OSK closes, otherwise we wont
            if (mainWindow.Visibility == Visibility.Visible)
            {
                showMainWindowAtClose = true;
                ToggleMainWindow();
            }
        }

        private void SetLocation()
        {
            this.Width = SystemParameters.FullPrimaryScreenWidth;
            this.Height = Math.Round(SystemParameters.FullPrimaryScreenHeight * 0.35,0);
            this.Top = SystemParameters.FullPrimaryScreenHeight - Math.Round(SystemParameters.FullPrimaryScreenHeight * 0.35, 0);
        }

        private void ToggleMainWindow()
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.ToggleWindow();

            });
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
            if (e.Action == "B") { this.Close(); }
        }


        private Dictionary<int, int> keyboardAngleToIndex = new Dictionary<int, int>()
        {
           //old set up before i made the cardinals only 10 degrees wide
           {15, 0 }, {45 ,1}, {75, 2}, {105 , 3}, {135, 4}, {165, 5}, {195, 6}, {225, 7}, {255, 8}, {285, 9}, {315, 10}, {345, 11}, {360, 0}
            // set up where cardinals are 10 degrees wide{5, 0 }, {45 ,1}, {85, 2}, {95 , 3}, {135, 4}, {175, 5}, {185, 6}, {225, 7}, {265, 8}, {275, 9}, {315, 10}, {355, 11}, {360, 0}
        };



        private int ReturnButtonZoneIndex(double x, double y, int previousIndex)
        {
            double radius = Math.Sqrt((x) * (x) + (y) * (y));

            double iAngle = Math.Tanh((y) / (x));
            double angle = 0;
            if (x < 0) { angle = (iAngle + 2); }
            if (x >= 0 && x >= 0) { angle = iAngle; }
            if (x >= 0 && y < 0) { angle = (4 + iAngle); }
            angle = (angle * 90);

         
            if (radius < 3500) { return 12; }
            if (radius < 22000)
            {
                if (angle <= 85 || angle >= 275) { return 14; }
                else
                {
                    if (angle >= 95 && angle <= 265)
                    {
                        return 13;
                    }
                    else
                    {
                        if (angle < 95) { return 3; }
                        else
                        {
                            return 9;
                        }

                    }
                }

            }


            //this section handles the outside circle angle
            //i want to incorproate a hysterisis on the angle of 5-10 degrees
            //in order to go to the next letter you need to go beyond the zone by 5-10 degrees so this section will cover
            //handling the primary index
            
            //if the previous index = current index after looking it up, thats fine
            //the tricky part is saying they DONT match and modifying the dictionary range by the 5-10 degrees

            //lets start by finding the range of the previous index
            //during initial the preivous index is -1 before it gets real value so lets if condition that out

            if (previousIndex != -1 && previousIndex < 12)
            {
                //now lets look up the range of the first index
                int upperRange = keyboardAngleToIndex.FirstOrDefault(x => x.Value == previousIndex).Key;

                //since 0 index bridges the gap between 0 and 360 lets handle that one specially
                if (previousIndex == 0)
                {
                    //hard code for the 0 index because of overlap on 0-360 area
                    if (angle < upperRange + 5 || angle > 340)
                    {
                        return previousIndex;
                    }
                }
                else
                {
                    if (angle < upperRange + 5 && angle > upperRange - 35)
                    {
                        return previousIndex;
                    }
                }
            }

            int newIndex = 0;
            foreach (KeyValuePair<int, int> keyValuePair in keyboardAngleToIndex)
            {
                if (angle <= keyValuePair.Key) { newIndex = keyValuePair.Value; break; }
            }

            //return the new index as that is the only result left to return
            return newIndex;
        }

        private void HighlightButton(Button button)
        {
            button.Opacity = 1;
            button.Background = System.Windows.Media.Brushes.Gray;
            if (button.Name.Contains("btnL"))
            {
                currentHighlightButtonLeft = button;
            }
            else
            {
                currentHighlightButtonRight = button;
            }
        }
        private void UnhighlightButton(Button button)
        {
            button.Opacity = 0.7;
            button.Background = System.Windows.Media.Brushes.Gray;
        }
        private void ButtonPressEvent_controllerJoystickEventOSK(object? sender, controllerJoystickEventArgsOSK e)
        {
            //left stick inputs first
            int leftButtonIndex = ReturnButtonZoneIndex(e.lx, e.ly, currentLeftIndex);
            int rightButtonIndex = ReturnButtonZoneIndex(e.rx, e.ry, currentRightIndex);
            currentLeftIndex = leftButtonIndex;
            currentRightIndex = rightButtonIndex;


            Button newHighlightButtonLeft = keyboardIndexToButtonLeft[leftButtonIndex];
            if (currentHighlightButtonLeft != newHighlightButtonLeft)
            {
                if (newHighlightButtonLeft != currentHighlightButtonLeft)
                {
                    UnhighlightButton(currentHighlightButtonLeft);
                    HighlightButton(newHighlightButtonLeft);
                }
            }
          
            Button newHighlightButtonRight = keyboardIndexToButtonRight[rightButtonIndex];
            if (currentHighlightButtonRight != newHighlightButtonRight)
            {
                if (newHighlightButtonRight != currentHighlightButtonRight)
                {
                    UnhighlightButton(currentHighlightButtonRight);
                    HighlightButton(newHighlightButtonRight);
                }
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

        private void Window_Closed(object sender, EventArgs e)
        {
            if (showMainWindowAtClose)
            {
                ToggleMainWindow();
            }
        }
    }
  
}
