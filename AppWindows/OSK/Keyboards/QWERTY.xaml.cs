
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

    public partial class QWERTY : Page
    {
        ControllerInputOSK inputOSK = null;

        ControllerInput controllerInput;
        Button leftButton = null;
        Button rightButton = null;
        List<Button> buttons;

        Point leftPoint;
        Point rightPoint;

        Ellipse leftCircle;
        Ellipse rightCircle;

        public Visibility controllerInputIconsVisibility { get; set; } = Visibility.Collapsed;



        public QWERTY()
        {
            

            //get list of buttons to make searching for button changes easier - this was used when I made the joysticks into seudo dpads for left right up down movement, wasnt good because moving up and down woudlnt always get the right key because of the button offsets
            //buttons = mainGrid.Children.OfType<Button>().OrderBy(Grid.GetRow).ThenBy(Grid.GetColumn).ToList();

            //Move initilize components to sub routine and async it to make pages feel smoother
            Dispatcher.BeginInvoke(new System.Action(() => Initialize()));

        }

        private void Initialize()
        {
            InitializeComponent();
        
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //controller stuff needs to be done on loaded because buttons dont have a position until they have been loaded, cant properly place circles
            //circles need to be placed before subscribing to events
            SetUpForControllerInput();

            SubscribeEvents();

        }

        private void ToggleControllerIconViewbox()
        {
            if (controllerIconViewbox1.Visibility == Visibility.Collapsed)
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

        private void SetUpForControllerInput()
        {
            //identify start buttons and make input circles but keep them transparent. This prevents any kind of error due to the circles not being generated when the controller connected event happens

            //set upperLimitY based on window size
            foreach (OSK mw in Application.Current.Windows.OfType<OSK>())
            {
                upperLimitY = mw.Top - circleDiameter / 2;
            }
         

            //upperLimitY = mw.Top - circleDiameter / 2;

            //identify the start buttons for the controller input (f and j keys)
            leftButton = (Button)mainGrid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == 2 && Grid.GetColumn(e) == 9);
            rightButton = (Button)mainGrid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == 2 && Grid.GetColumn(e) == 15);

            //setup start points for circles
            leftPoint = leftButton.TranslatePoint(new Point(0, 0), mainCanvas);
            leftPoint.Offset(leftButton.ActualWidth / 2, leftButton.ActualHeight / 2);
            leftPoint.Offset(-circleDiameter / 2, -circleDiameter / 2);

            rightPoint = rightButton.TranslatePoint(new Point(0, 0), mainCanvas);
            rightPoint.Offset(rightButton.ActualWidth / 2, rightButton.ActualHeight / 2);
            rightPoint.Offset(-circleDiameter / 2, -circleDiameter / 2);

            leftCircle = new Ellipse
            {
                Visibility = Visibility.Collapsed,
                Fill = Brushes.Transparent,
                Width = circleDiameter,
                Height = circleDiameter,
                Opacity = 0.5,
                IsHitTestVisible = false
            };
            rightCircle = new Ellipse
            {
                Visibility = Visibility.Collapsed,
                Fill = Brushes.Transparent,
                Width = circleDiameter,
                Height = circleDiameter,
                Opacity = 0.5,
                IsHitTestVisible = false
            };

            Canvas.SetTop(leftCircle, leftPoint.Y);
            Canvas.SetLeft(leftCircle, leftPoint.X);

            Canvas.SetTop(rightCircle, rightPoint.Y);
            Canvas.SetLeft(rightCircle, rightPoint.X);

            mainCanvas.Children.Add(leftCircle);
            mainCanvas.Children.Add(rightCircle);

            //hide all the viewboxes with controller icons
            ToggleControllerIconViewbox();
        }

        private void SubscribeEvents()
        {
            inputOSK = new Everything_Handhelds_Tool.AppWindows.OSK.ControllerInputOSK();

            //subscribe to controller input events for buttons and joystick movement
            inputOSK.buttonPressEvent.controllerInputEventOSK += ButtonPressEvent_controllerInputEventOSK;

            inputOSK.buttonPressEvent.controllerJoystickEventOSK += ButtonPressEvent_controllerJoystickEventOSK;

            inputOSK.buttonPressEvent.controllerConnectedDisconnectedEventOSK += ButtonPressEvent_controllerConnectedDisconnectedEventOSK;

            //setup timer for hiding circles
            hideCirclesTimer.Tick += HideCirclesTimer_Tick;
        }

    

        private void ButtonPressEvent_controllerJoystickEventOSK(object? sender, controllerJoystickEventArgsOSK e)
        {
            
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                HandleLeftCircleMovement(e.lx, e.ly);
                HandleRightCircleMovement(e.rx, e.ry);
            }
   );

        }

        private void HandleJoystickInput(double lx, double ly, double rx, double ry)
        {
            //IM PUTTING THIS HERE TO TRY TO GET AROUND THE CIRCLE DISAPPEARING ISSUE
            HandleLeftCircleMovement(lx, ly);
            HandleRightCircleMovement(rx, ry);

        }
        private void ButtonPressEvent_controllerConnectedDisconnectedEventOSK(object? sender, controllerInputEventArgsOSK e)
        {
            if (e.Action == "Connected")
            {
                ControllerConnected();
            }
            if (e.Action == "Disconnected")
            {
                UnhighlightButton(leftButton);
                UnhighlightButton(rightButton);
                ToggleControllerIconViewbox();
            }
        }

        private void ButtonPressEvent_controllerInputEventOSK(object? sender, controllerInputEventArgsOSK e)
        {
            switch (e.Action)
            {
                case "LeftThumb":
                    capsPressed = !capsPressed;
                    break;

                case "LeftShoulder":
                    leftButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    break;
                case "RightShoulder":
                    rightButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    break;
                case "X":
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
                    break;
                case "Y":
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.SPACE);
                    break;
                case "LeftTrigger":
                    shiftPressed = !shiftPressed;
                    break;
                case "B":
                    CloseWindow();
                    break;

            }


        }


        private bool _shiftPressed = false;
        private bool _capsPressed = false;

        private bool shiftPressed
        {
            get
            {
                return _shiftPressed;
            }
            set
            {
                if (value)
                {
                    shiftButton.Background = Brushes.LightSlateGray;
                }
                else
                {
                    if (leftButton != shiftButton && rightButton != shiftButton)
                    {
                        shiftButton.Background = Brushes.Gray;
                    }
                    else
                    {
                        shiftButton.Background = Brushes.LightBlue;
                    }
                }
                _shiftPressed = value;
                ToggleShift(value);
             
         
              
            }
        }
        private bool capsPressed
        {
            get
            {
                return _capsPressed;
            }
            set
            {
                if (value)
                {
                    capsButton.Background = Brushes.LightSlateGray;
                }
                else
                {
                    if (leftButton != capsButton && rightButton != capsButton)
                    {
                        capsButton.Background = Brushes.Gray;
                    }
                    else
                    {
                        capsButton.Background = Brushes.LightBlue;
                    }
                }
                _capsPressed = value;
                ToggleCaps(value);
           
            }
        }


        private InputSimulator inputSimulator = new InputSimulator();
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

        private void CloseWindow()
        {
            inputOSK.AbortThread();
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                OSK osk = Application.Current.Windows.OfType<Everything_Handhelds_Tool.AppWindows.OSK.OSK>().First();
                if (osk != null)
                {
                    osk.Close();
                }


            });
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
        private void ToggleShift(bool value)
        {
            if (value)
            {
                shiftButton.Background = Brushes.LightSlateGray;
            }
            else
            {
                if (leftButton == shiftButton || rightButton == shiftButton)
                {
                    shiftButton.Background = Brushes.LightBlue;
                }
                else
                {
                    shiftButton.Background = Brushes.Gray;
                }
            }

            foreach (Control control in mainGrid.Children)
            {
                //loop through each control to find buttons
                if (control is Button)
                {
                    //cast as button
                    Button button = (Button)control;

                    switch (control.Tag)
                    {
                        case "Dual":
                            Grid grid = button.Content as Grid;
                            string tag1 = "";
                            string tag2 = "";
                            foreach (Viewbox viewbox in grid.Children)
                            {
                                TextBlock textbox = viewbox.Child as TextBlock;
                                if (viewbox.Tag.ToString() == "1")
                                {
                                    tag1 = textbox.Text;
                                }
                                if (viewbox.Tag.ToString() == "2")
                                {
                                    tag2 = textbox.Text;
                                }
                            }

                            foreach (Viewbox viewbox in grid.Children)
                            {
                                TextBlock textbox = viewbox.Child as TextBlock;
                                if (viewbox.Tag.ToString() == "1")
                                {
                                    textbox.Text = tag2;
                                }
                                if (viewbox.Tag.ToString() == "2")
                                {
                                    textbox.Text = tag1;
                                    if (shiftPressed)
                                    {
                                        viewbox.Visibility = Visibility.Collapsed;
                                    }
                                    else
                                    {
                                        viewbox.Visibility = Visibility.Visible;
                                    }
                                }
                            }
                            break;
                        case "Letter":
                            if (capsPressed && !shiftPressed || shiftPressed && !capsPressed)
                            {
                                button.Content = button.Content.ToString().ToUpper();
                            }
                            else
                            {
                                button.Content = button.Content.ToString().ToLower();
                            }
                            break;
                        default: break;
                    }



                }

            }
        }
        private void ToggleCaps(bool value)
        {
            if (value)
            {
                capsButton.Background = Brushes.LightSlateGray;
            }
            else
            {
                if (leftButton == capsButton || rightButton == capsButton)
                {
                    capsButton.Background = Brushes.LightBlue;
                }
                else
                {
                    capsButton.Background = Brushes.Gray;
                }
            }

            foreach (Control control in mainGrid.Children)
            {
                //loop through each control to find buttons
                if (control is Button)
                {
                    //cast as button
                    Button button = (Button)control;

                    switch (control.Tag)
                    {

                        case "Letter":
                            if (capsPressed && !shiftPressed || shiftPressed && !capsPressed)
                            {
                                button.Content = button.Content.ToString().ToUpper();
                            }
                            else
                            {
                                button.Content = button.Content.ToString().ToLower();
                            }
                            break;
                        default: break;
                    }



                }

            }
        }

        //--------------------------------------------------

        #region handle circle movement


        //add variables for things like circle diameter, and upper limits of keyboard area to prevent circles from going out of bounds
        private const double circleDiameter = 28;
        private const double circleRadius = 14;
        private double lowerLimitX = circleDiameter / 2;
        private double lowerLimitY = circleDiameter / 2;
        private double upperLimitX = System.Windows.SystemParameters.PrimaryScreenWidth - circleDiameter / 2;
        private double upperLimitY; //calculated later after we get the on load height





        private void HandleLeftCircleMovement(double lx, double ly)
        {
            Button newButton = HandleCircleMovement(lx, ly, leftPoint, leftButton, leftCircle, true);
            if (newButton != leftButton)
            {
                UnhighlightButton(leftButton);
                HighlightButton(newButton);
                leftButton = newButton;
            }
        }
        private void HandleRightCircleMovement(double rx, double ry)
        {
            Button newButton = HandleCircleMovement(rx, ry, rightPoint, rightButton, rightCircle, false);

            if (newButton != rightButton)
            {
                UnhighlightButton(rightButton);
                HighlightButton(newButton);
                rightButton = newButton;
            }
        }
 

        private Button HandleCircleMovement(double x, double y, Point point, Button button, Ellipse ellipse, bool left)
        {
            //calculate new point based on input
            Point newPoint = OffSetPoint(point, JoystickToCircleMovementOffset(x), JoystickToCircleMovementOffset(-y));

            if (left)
            {
                leftPoint = newPoint;
            }
            else { rightPoint = newPoint; }

            //Move circle to new point
            Button newButton = MoveCircleAndHitTest(ellipse, newPoint, button);


            if (newButton != leftButton && newButton != rightButton)
            {
                return newButton;
            }

            return button;
        }

        private Button MoveCircleAndHitTest(Ellipse ellipse, Point point, Button button)
        {
            //due to issue with hit testing, i moved the hit test routine inside here 
            mainCanvas.Children.Remove(ellipse);


            //NEED TO PERFORM HIT TESTING HERE SINCE THE HIT TEST HITS THE ELLIPSE INSTEAD OF THE BUTTON, SO PUTTING HERE BETWEEN THE REMOVE AND ADD
            Point hitPoint = new Point(point.X + circleRadius, point.Y + circleRadius);
            Button newButton = HitTestLocationForButton(button, hitPoint);

            Canvas.SetTop(ellipse, point.Y);
            Canvas.SetLeft(ellipse, point.X);

            mainCanvas.Children.Add(ellipse);

            return newButton;
        }


        private Button HitTestLocationForButton(Button currentButton, Point point)
        {

            HitTestResult result = VisualTreeHelper.HitTest(mainCanvas, point);

            if (result != null)
            {


                if (result.VisualHit is Border)
                {
                    Control control = FindParent<Button>(result.VisualHit);
                    if (control != null)
                    {
                        if (control is Button)
                        {
                            return (Button)control;
                        }

                    }
                }
            }

            return currentButton;
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }
        private double JoystickToCircleMovementOffset(double Input)
        {
            //
            ShowCircles();

            //Convert short from joystick to double, divide by largest value, and round to largest nubmer away from zero
            double absInput = Math.Abs(Input);

            switch (absInput)
            {
                case < 500:
                    return 0;
                case < 1000:
                    if (Input < 0)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                case < 22000:
                    return Math.Round(10 * Input / 32768, 0, MidpointRounding.AwayFromZero);
                case < 26000:
                    return Math.Round(12 * Input / 32768, 0, MidpointRounding.AwayFromZero);
                case < 30000:
                    return Math.Round(14 * Input / 32768, 0, MidpointRounding.AwayFromZero);
                case < 35000:
                    return Math.Round(18 * Input / 32768, 0, MidpointRounding.AwayFromZero);
            }

            if (absInput > 500 && absInput < 1000)
            {


                if (Input < 0)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }

            }
            else if (absInput >= 1000)
            {
                return Math.Round(10 * Input / 32768, 0, MidpointRounding.AwayFromZero);
            }
            return 0;
        }
        private Point OffSetPoint(Point point, Double dx, Double dy)
        {
            if ((point.X + dx >= lowerLimitX) && (point.X + dx <= upperLimitX))
            {
                point.Offset(dx, 0);
            }
            else
            {
                if (point.X + dx < lowerLimitX)
                {
                    point.X = lowerLimitX;
                }
                else
                {
                    point.X = upperLimitX;
                }
            }
            if ((point.Y + dy >= lowerLimitY) && (point.Y + dy <= upperLimitY))
            {
                point.Offset(0, dy);
            }
            else
            {
                if (point.Y + dy < lowerLimitY)
                {
                    point.Y = lowerLimitY;
                }
                else
                {
                    point.Y = upperLimitY;
                }
            }
            return point;
        }

        //set up dispatcher timer to hide the circles after 2 seconds of inactivity, this makes it look cleaner when using
        DispatcherTimer hideCirclesTimer = new DispatcherTimer()
        {
            Interval = new TimeSpan(0, 0, 1)
        };
        private void ControllerConnected()
        {
            HighlightButton(leftButton);
            HighlightButton(rightButton);


            ShowCircles();

            leftCircle.Fill = Brushes.DarkGray;
            rightCircle.Fill = Brushes.DarkGray;

            //show controller icons on special keys
            ToggleControllerIconViewbox();

        }

        private void HideCirclesTimer_Tick(object? sender, EventArgs e)
        {

            hideCirclesTimer.Stop();
            leftCircle.Visibility = Visibility.Collapsed;
            rightCircle.Visibility = Visibility.Collapsed;

        }
        private void ShowCircles()
        {
            //runs during the joystick movement event
            if (leftCircle.Visibility == Visibility.Collapsed)
            {
                leftCircle.Visibility = Visibility.Visible;
                rightCircle.Visibility = Visibility.Visible;
                hideCirclesTimer.Start();
            }
            else
            {//reset timer using stop and then start
                hideCirclesTimer.Stop();
                hideCirclesTimer.Start();
            }
        }

        #endregion

        //I ABANDONED THESE METHODS KEEP FOR REFERENCE
        #region findButtons
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








        private void UnhighlightButton(Button button)
        {
            if (button.Background == Brushes.LightBlue)
            {
                button.Background = Brushes.Gray;
            }

        }
        private void HighlightButton(Button button)
        {
            if (button.Background == Brushes.Gray)
            {
                button.Background = Brushes.LightBlue;
            }

        }

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


        private void SetPageSize()
        {
            this.Width = SystemParameters.FullPrimaryScreenWidth;
            this.Height = Math.Round(SystemParameters.FullPrimaryScreenHeight * 0.4, 0);


        }

        private void HideKeyboard()
        {

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            inputOSK = null;
        }


    }
}
