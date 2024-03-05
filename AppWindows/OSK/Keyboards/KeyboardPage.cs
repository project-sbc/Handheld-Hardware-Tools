using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.MouseMode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace Everything_Handhelds_Tool.AppWindows.OSK.Keyboards
{
    public class KeyboardPage : Page
    {
      
        public InputSimulator inputSimulator = new InputSimulator();

        public Button leftButton;
        public Button rightButton;

        public Point leftPoint;
        public Point rightPoint;

        public Ellipse leftCircle;
        public Ellipse rightCircle;

        public Canvas virtualMainCanvas;
        public Grid virtualGrid;


        public MouseProfile mouseProfile;

        public KeyboardPage(bool startMouse = false)
        {
            if (startMouse)
            {
                
            }
        }

        public virtual void ToggleControllerIconViewbox(bool connected) { }

        public void HandleJoystickInput(double lx, double ly, double rx, double ry)
        {
            //IM PUTTING THIS HERE TO TRY TO GET AROUND THE CIRCLE DISAPPEARING ISSUE
            HandleLeftCircleMovement(lx, ly);
            HandleRightCircleMovement(rx, ry);

        }

      

        public bool _shiftPressed = false;
        public bool _capsPressed = false;
        public Button virtualShift = null;
        public Button virtualCaps =null;
        public bool shiftPressed
        {
            get
            {
                return _shiftPressed;
            }
            set
            {
                if (value)
                {
                    virtualShift.Background = Brushes.LightSlateGray;
                }
                else
                {
                    if (leftButton != virtualShift && rightButton != virtualShift)
                    {
                        virtualShift.Background = Brushes.Gray;
                    }
                    else
                    {
                        virtualShift.Background = Brushes.LightBlue;
                    }
                }
                _shiftPressed = value;
                ToggleShift(value);



            }
        }
        public bool capsPressed
        {
            get
            {
                return _capsPressed;
            }
            set
            {
                if (value)
                {
                    virtualCaps.Background = Brushes.LightSlateGray;
                }
                else
                {
                    if (leftButton != virtualCaps && rightButton != virtualCaps)
                    {
                        virtualCaps.Background = Brushes.Gray;
                    }
                    else
                    {
                        virtualCaps.Background = Brushes.LightBlue;
                    }
                }
                _capsPressed = value;
                ToggleCaps(value);

            }
        }
        public void ToggleShift(bool value)
        {
            if (value)
            {
                virtualShift.Background = Brushes.LightSlateGray;
            }
            else
            {
                if (leftButton == virtualShift || rightButton == virtualShift)
                {
                    virtualShift.Background = Brushes.LightBlue;
                }
                else
                {
                    virtualShift.Background = Brushes.Gray;
                }
            }

            foreach (Control control in virtualGrid.Children)
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
        public void ToggleCaps(bool value)
        {
            if (value)
            {
                virtualCaps.Background = Brushes.LightSlateGray;
            }
            else
            {
                if (leftButton == virtualCaps || rightButton == virtualCaps)
                {
                    virtualCaps.Background = Brushes.LightBlue;
                }
                else
                {
                    virtualCaps.Background = Brushes.Gray;
                }
            }

            foreach (Control control in virtualGrid.Children)
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

        public void SetUpForControllerInput()
        {
            //identify start buttons and make input circles but keep them transparent. This prevents any kind of error due to the circles not being generated when the controller connected event happens

            //set upperLimitY based on window size
            foreach (OSK mw in Application.Current.Windows.OfType<OSK>())
            {
                upperLimitY = mw.Top - circleDiameter / 2;
            }


            //upperLimitY = mw.Top - circleDiameter / 2;

            //identify the start buttons for the controller input (f and j keys)
            leftButton = (Button)virtualGrid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == 2 && Grid.GetColumn(e) == 9);
            rightButton = (Button)virtualGrid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == 2 && Grid.GetColumn(e) == 15);

            //setup start points for circles
            leftPoint = leftButton.TranslatePoint(new Point(0, 0), virtualMainCanvas);
            leftPoint.Offset(leftButton.ActualWidth / 2, leftButton.ActualHeight / 2);
            leftPoint.Offset(-circleDiameter / 2, -circleDiameter / 2);

            rightPoint = rightButton.TranslatePoint(new Point(0, 0), virtualMainCanvas);
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

            virtualMainCanvas.Children.Add(leftCircle);
            virtualMainCanvas.Children.Add(rightCircle);

            //hide all the viewboxes with controller icons
            MainWindow mainwindow = Local_Object.Instance.GetMainWindow();
            if (mainwindow.controllerInput.controller != null)
            {
                if (mainwindow.controllerInput.controller.IsConnected)
                {
                    ControllerConnected();
                }
                else
                {
                    ToggleControllerIconViewbox(false);
                }
            }

        }

        
        public void CloseWindow()
        {
           
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                OSK osk = Application.Current.Windows.OfType<Everything_Handhelds_Tool.AppWindows.OSK.OSK>().Any();
                if (osk != null)
                {
                    osk.Close();
                }


            });
        }
        public void SubscribeEvents()
        {

           

            //inputOSK.buttonPressEvent.controllerJoystickEventOSK += ButtonPressEvent_controllerJoystickEventOSK;
            MainWindow mw = Local_Object.Instance.GetMainWindow();
            mw.controllerInput.joystickEvent.controllerJoystickEvent += JoystickEvent_controllerJoystickEvent;
            mw.controllerInput.buttonPressEvent.controllerInputEvent += ButtonPressEvent_controllerInputEvent;
            mw.controllerInput.controllerConnectionChangedEvent.controllerConnectionChangedEvent += ControllerConnectionChangedEvent_controllerConnectionChangedEvent;
            
            //setup timer for hiding circles
            hideCirclesTimer.Tick += HideCirclesTimer_Tick;
        }

        private void ControllerConnectionChangedEvent_controllerConnectionChangedEvent(object? sender, controllerConnectionChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (e.Connected)
                {
                    ControllerConnected();
                }
                else
                {
                    UnhighlightButton(leftButton);
                    UnhighlightButton(rightButton);
                    ToggleControllerIconViewbox(false);
                }
            });
           
        }

        private void ButtonPressEvent_controllerInputEvent(object? sender, controllerInputEventArgs e)
        {
            Dispatcher.Invoke(() =>
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
            });
        }

        private void JoystickEvent_controllerJoystickEvent(object? sender, controllerJoystickEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                HandleJoystickInput(e.lx, e.ly, e.rx, e.ry);
            });
        }

        public void UnhighlightButton(Button button)
        {
            if (button.Background == Brushes.LightBlue)
            {
                button.Background = Brushes.Gray;
            }

        }
        public void HighlightButton(Button button)
        {
            if (button.Background == Brushes.Gray)
            {
                button.Background = Brushes.LightBlue;
            }

        }

        #region handle circle movement


        //add variables for things like circle diameter, and upper limits of keyboard area to prevent circles from going out of bounds
        public const double circleDiameter = 28;
        public const double circleRadius = 14;
        public double lowerLimitX = circleDiameter / 2;
        public double lowerLimitY = circleDiameter / 2;
        public double upperLimitX = System.Windows.SystemParameters.PrimaryScreenWidth - circleDiameter / 2;
        public double upperLimitY; //calculated later after we get the on load height





        public void HandleLeftCircleMovement(double lx, double ly)
        {
            Button newButton = HandleCircleMovement(lx, ly, leftPoint, leftButton, leftCircle, true);
            if (newButton != leftButton)
            {
                UnhighlightButton(leftButton);
                HighlightButton(newButton);
                leftButton = newButton;
            }
        }
        public void HandleRightCircleMovement(double rx, double ry)
        {
            Button newButton = HandleCircleMovement(rx, ry, rightPoint, rightButton, rightCircle, false);

            if (newButton != rightButton)
            {
                UnhighlightButton(rightButton);
                HighlightButton(newButton);
                rightButton = newButton;
            }
        }


        public Button HandleCircleMovement(double x, double y, Point point, Button button, Ellipse ellipse, bool left)
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

        public Button MoveCircleAndHitTest(Ellipse ellipse, Point point, Button button)
        {
            //due to issue with hit testing, i moved the hit test routine inside here 
            virtualMainCanvas.Children.Remove(ellipse);


            //NEED TO PERFORM HIT TESTING HERE SINCE THE HIT TEST HITS THE ELLIPSE INSTEAD OF THE BUTTON, SO PUTTING HERE BETWEEN THE REMOVE AND ADD
            Point hitPoint = new Point(point.X + circleRadius, point.Y + circleRadius);
            Button newButton = HitTestLocationForButton(button, hitPoint);

            Canvas.SetTop(ellipse, point.Y);
            Canvas.SetLeft(ellipse, point.X);

            virtualMainCanvas.Children.Add(ellipse);
            
            return newButton;
        }


        public Button HitTestLocationForButton(Button currentButton, Point point)
        {

            HitTestResult result = VisualTreeHelper.HitTest(virtualMainCanvas, point);

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

        public T FindParent<T>(DependencyObject child) where T : DependencyObject
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
        public double JoystickToCircleMovementOffset(double Input)
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
        public Point OffSetPoint(Point point, Double dx, Double dy)
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
        public void ControllerConnected()
        {
            HighlightButton(leftButton);
            HighlightButton(rightButton);


            ShowCircles();

            leftCircle.Fill = Brushes.DarkGray;
            rightCircle.Fill = Brushes.DarkGray;

            //show controller icons on special keys
            ToggleControllerIconViewbox(true);

        }

        public void HideCirclesTimer_Tick(object? sender, EventArgs e)
        {

            hideCirclesTimer.Stop();
            leftCircle.Visibility = Visibility.Collapsed;
            rightCircle.Visibility = Visibility.Collapsed;

        }
        public void ShowCircles()
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

    }
}
