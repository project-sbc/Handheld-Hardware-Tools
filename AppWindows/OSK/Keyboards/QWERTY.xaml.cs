
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
using System.Windows.Threading;
using WindowsInput.Native;
using WindowsInput;
using System.Windows.Controls.Primitives;
using Everything_Handhelds_Tool.Classes;

namespace Everything_Handhelds_Tool.AppWindows.OSK.Keyboards
{
    /// <summary>
    /// Interaction logic for QWERTY.xaml
    /// </summary>

    public partial class QWERTY : KeyboardPage
    {
       
        public QWERTY()
        {

            //Move initilize components to sub routine and async it to make pages feel smoother
            Dispatcher.BeginInvoke(new System.Action(() => Initialize()));

        }

        private void Initialize()
        {
            InitializeComponent();

            virtualMainCanvas = mainCanvas;
            virtualGrid = mainGrid;
            virtualShift = shiftButton;
            virtualCaps = capsButton;

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //controller stuff needs to be done on loaded because buttons dont have a position until they have been loaded, cant properly place circles
            //circles need to be placed before subscribing to events
            SetUpForControllerInput();

            SubscribeEvents();

        }

        public override void ToggleControllerIconViewbox(bool connected)
        {
            if (connected)
            {
                controllerIconViewbox1.Visibility = Visibility.Visible;
                controllerIconViewbox2.Visibility = Visibility.Visible;
                controllerIconViewbox3.Visibility = Visibility.Visible;
                controllerIconViewbox4.Visibility = Visibility.Visible;
                controllerIconViewbox5.Visibility = Visibility.Visible;
                controllerIconViewbox6.Visibility = Visibility.Visible;
                controllerIconViewbox7.Visibility = Visibility.Visible;
            }
            else
            {
                controllerIconViewbox1.Visibility = Visibility.Collapsed;
                controllerIconViewbox2.Visibility = Visibility.Collapsed;
                controllerIconViewbox3.Visibility = Visibility.Collapsed;
                controllerIconViewbox4.Visibility = Visibility.Collapsed;
                controllerIconViewbox5.Visibility = Visibility.Collapsed;
                controllerIconViewbox6.Visibility = Visibility.Collapsed;
                controllerIconViewbox7.Visibility = Visibility.Collapsed;
            }
        }

              
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button != null)
            {
                VirtualKeyCode vkc = VirtualKeyCode.SPACE;
                switch (button.Tag)
                {
                    case "Dual":
                        vkc = oskLookup[button.CommandParameter.ToString()];
                        break;
                    case "Letter":
                        vkc = oskLookup[button.Content.ToString().ToUpper()];
                        break;
                    case "Special":
                        switch (button.CommandParameter.ToString())
                        {
                            case "Backspace":
                                vkc = VirtualKeyCode.BACK;
                                break;
                            case "Shift":

                                shiftPressed = !shiftPressed;
                                return;
                            case "Hide":
                                CloseWindow();
                                return;

                            case "CapsLock":
                                capsPressed = !capsPressed;
                                break;
                        }
                        break;
                }

                //send keyboard input


                if (button.Tag.ToString() == "Letter")
                {//logical XOR operator so that it is false when both are true or both are false
                    if (capsPressed ^ shiftPressed)
                    {//if both pressed they cancel and make a lower case
                        inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.SHIFT, vkc);
                    }
                    else
                    {
                        inputSimulator.Keyboard.KeyPress(vkc);
                    }
                }
                else
                {
                    if (shiftPressed)
                    {
                        inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.SHIFT, vkc);
                    }
                    else
                    {
                        inputSimulator.Keyboard.KeyPress(vkc);
                    }
                }


            }



        }

       


        private Dictionary<string, VirtualKeyCode> oskLookup = new Dictionary<string, VirtualKeyCode>()
        {

            {"A", VirtualKeyCode.VK_A},
            {"B", VirtualKeyCode.VK_B},
            {"C", VirtualKeyCode.VK_C},
            {"D", VirtualKeyCode.VK_D},
            {"E", VirtualKeyCode.VK_E},
            {"F", VirtualKeyCode.VK_F},
            {"G", VirtualKeyCode.VK_G},
            {"H", VirtualKeyCode.VK_H},
            {"I", VirtualKeyCode.VK_I},
            {"J", VirtualKeyCode.VK_J},
            {"K", VirtualKeyCode.VK_K},
            {"L", VirtualKeyCode.VK_L},
            {"M", VirtualKeyCode.VK_M},
            {"N", VirtualKeyCode.VK_N},
            {"O", VirtualKeyCode.VK_O},
            {"P", VirtualKeyCode.VK_P},
            {"Q", VirtualKeyCode.VK_Q},
            {"R", VirtualKeyCode.VK_R},
            {"S", VirtualKeyCode.VK_S},
            {"T", VirtualKeyCode.VK_T},
            {"U", VirtualKeyCode.VK_U},
            {"V", VirtualKeyCode.VK_V},
            {"W", VirtualKeyCode.VK_W},
            {"X", VirtualKeyCode.VK_X},
            {"Y", VirtualKeyCode.VK_Y},
            {"Z", VirtualKeyCode.VK_Z},
            {"1", VirtualKeyCode.VK_1},
            {"2", VirtualKeyCode.VK_2},
            {"3", VirtualKeyCode.VK_3},
            {"4", VirtualKeyCode.VK_4},
            {"5", VirtualKeyCode.VK_5},
            {"6", VirtualKeyCode.VK_6},
            {"7", VirtualKeyCode.VK_7},
            {"8", VirtualKeyCode.VK_8},
            {"9", VirtualKeyCode.VK_9},
            {"0", VirtualKeyCode.VK_0}

        };


        


        //I ABANDONED THESE METHODS KEEP FOR REFERENCE. I DELETED THESE ONCE AND REGRETTED IT
        //THESE DO DPAD LIKE NAVIGATIONS ON A GRID,
        #region findButtons
        private Button HandleButtonNavigationEvent(Button button, string direction)
        {
            Button returnButton = null;
            int gridRow = Grid.GetRow(button);
            int gridCol = Grid.GetColumn(button);

            switch (direction)
            {
                case "Up":
                    if (gridRow > 0)
                    {
                        gridRow = gridRow - 1;
                        returnButton = (Button)mainGrid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == gridRow && Grid.GetColumn(e) == gridCol);
                    }
                    break;
                case "Down":
                    if (gridRow < 4)
                    {
                        gridRow = gridRow + 1;
                        returnButton = (Button)mainGrid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == gridRow && Grid.GetColumn(e) == gridCol);
                    }
                    break;
                case "Left":
                    if (gridCol > 0)
                    {
                        while (gridCol > 0 && returnButton != button)
                        {
                            gridCol = gridCol - 1;
                            returnButton = (Button)mainGrid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == gridRow && Grid.GetColumn(e) == gridCol);
                        }
                    }
                    break;
                case "Right":
                    if (gridCol < 27)
                    {
                        while (gridCol < 27 && returnButton != button)
                        {
                            gridCol = gridCol - 1;
                            returnButton = (Button)mainGrid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == gridRow && Grid.GetColumn(e) == gridCol);
                        }
                    }
                    break;
            }
            if (returnButton == null)
            {
                return button;
            }
            else
            {
                return returnButton;
            }

        }


        private Button FindButtonByDirection(Button button, string direction)
        {
            Button returnButton = null;
            if (direction == "Left" || direction == "Right")
            {
                returnButton = FindButtonInRow(Grid.GetRow(button), Grid.GetColumn(button), direction);
            }
            else
            {
                //else is Up or Down
                returnButton = FindButtonInColumn(Grid.GetRow(button), Grid.GetColumn(button), direction);

            }
            if (returnButton == null) { returnButton = button; }
            return returnButton;

        }
        private Button FindButtonInColumn(int rowNum, int colNum, string offset)
        {
            List<Button> buttons = new List<Button>();

            List<Button> searchList = new List<Button>(buttons);
            searchList.Reverse(); //need to reverse because we will ALWAYS look to the left in the new row when moving up or down columns

            //if direction is up and current row is 0 then there is no change keep it there
            if (offset == "Up" && rowNum == 0) { return null; }
            //if direction is down and current row is 4 (the last row) then there is no change keep it there
            if (offset == "Down" && rowNum == 4) { return null; }


            //modify the row number based on offset, for example if direction is up subtract one from the row
            if (offset == "Up") { rowNum--; }
            if (offset == "Down") { rowNum++; }

            foreach (Button button in searchList)
            {
                //make sure to stay in same row only modify column
                if (Grid.GetRow(button) == rowNum)
                {
                    if (Grid.GetColumn(button) < colNum && colNum != 0)
                    {
                        return button;
                    }
                }
            }


            //default is return null which means we keep the same button
            return null;

        }


        private Button FindButtonInRow(int rowNum, int colNum, string offset)
        {
            List<Button> buttons = new List<Button>();
            List<Button> searchList = new List<Button>(buttons);

            //reverse list if we are working to the left (i.e. -1 offset)
            if (offset == "Left") { searchList.Reverse(); }


            foreach (Button button in searchList)
            {
                //make sure to stay in same row only modify column
                if (Grid.GetRow(button) == rowNum)
                {
                    if (offset == "Left")
                    {
                        if (Grid.GetColumn(button) < colNum && colNum != 0)
                        {
                            return button;
                        }
                    }
                    else
                    {
                        if (Grid.GetColumn(button) > colNum)
                        {
                            return button;
                        }
                    }


                }
            }



            return null;

        }


        #endregion // NOT IN USE I ABANDONED IT

        //--------------------------------------------------








        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
          
        }


    }
}
