using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.MouseMode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using WindowsInput;
using WindowsInput.Native;
using static System.Net.Mime.MediaTypeNames;

namespace Handheld_Hardware_Tools.AppWindows.OSK.Keyboards
{
    public class KeyboardPage : Page
    {
      
        public InputSimulator inputSimulator = Local_Object.Instance.GetMainWindowInputSimulator();

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
            if (lx != 0  || ly != 0) { ShowLeftCircle(); }


            HandleRightCircleMovement(rx, ry);
            if (rx != 0 || ry != 0) { ShowRightCircle(); }
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
            foreach (OSK mw in System.Windows.Application.Current.Windows.OfType<OSK>())
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
            QuickAccessMenu mainwindow = Local_Object.Instance.GetQAMWindow();
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
            Window thisWindow = Local_Object.Instance.GetGeneralWindow(this);

            if (thisWindow != null)
            {
                thisWindow.Close();
            }
          
        }
        public void SubscribeEvents()
        {

           

            //inputOSK.buttonPressEvent.controllerJoystickEventOSK += ButtonPressEvent_controllerJoystickEventOSK;
            ControllerInput controllerInput = Local_Object.Instance.GetMainWindowControllerInput();
            if (controllerInput != null)
            {
                controllerInput.joystickEvent.controllerJoystickEvent += JoystickEvent_controllerJoystickEvent;
                controllerInput.buttonPressEvent.controllerInputEvent += ButtonPressEvent_controllerInputEvent;
                controllerInput.controllerConnectionChangedEvent.controllerConnectionChangedEvent += ControllerConnectionChangedEvent_controllerConnectionChangedEvent;
            }


            //setup timer for hiding circles
            hideLeftCircleTimer.Tick += HideLeftCircleTimer_Tick;
            hideRightCircleTimer.Tick += HideRightCircleTimer_Tick;
            hideLeftButtonTimer.Tick += HideLeftButtonTimer_Tick;
            hideRightButtonTimer.Tick += HideRightButtonTimer_Tick;

            
        }
        public void UnsubscribeEvents()
        {
            //unsubscribe otherwise the class will continue to exist
            ControllerInput controllerInput = Local_Object.Instance.GetMainWindowControllerInput();
            if (controllerInput != null)
            {
                controllerInput.joystickEvent.controllerJoystickEvent += JoystickEvent_controllerJoystickEvent;
                controllerInput.buttonPressEvent.controllerInputEvent -= ButtonPressEvent_controllerInputEvent;
                controllerInput.controllerConnectionChangedEvent.controllerConnectionChangedEvent -= ControllerConnectionChangedEvent_controllerConnectionChangedEvent;
            }


            //dont forget to unsubscribe the timer and stop it
            hideLeftCircleTimer.Stop();
            hideRightCircleTimer.Stop();
            hideLeftButtonTimer.Stop();
            hideRightButtonTimer.Stop();
            hideLeftCircleTimer.Tick -= HideLeftCircleTimer_Tick;
            hideRightCircleTimer.Tick -= HideRightCircleTimer_Tick;
            hideLeftButtonTimer.Tick -= HideLeftButtonTimer_Tick;
            hideRightButtonTimer.Tick -= HideRightButtonTimer_Tick;
        }
        private void ControllerConnectionChangedEvent_controllerConnectionChangedEvent(object? sender, controllerConnectionChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (this.Visibility == Visibility.Visible)
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
                }
               
            });
           
        }

        private void ButtonPressEvent_controllerInputEvent(object? sender, controllerInputEventArgs e)
        {

            Dispatcher.Invoke(() =>
            {
                if (this.Visibility == Visibility.Visible)
                {
                    switch (e.Action)
                    {
                        case "LeftThumb":
                            capsPressed = !capsPressed;
                            break;
                        case "RightThumb":

                            break;
                        case "LeftShoulder":
                            leftButton.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                            break;
                        case "RightShoulder":
                            rightButton.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                            break;
                        case "X":
                            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
                            SendOutlinePreviewTextUpdate("BACKSPACE");
                            break;
                        case "Y":
                            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.SPACE);
                            SendOutlinePreviewTextUpdate("SPACE");
                            break;
                        case "LeftTrigger":
                            shiftPressed = !shiftPressed;
                            break;
                        case "RightTrigger":
                            Task.Run(() => MoveWindowToNextMonitorAsync());
                            break;
                        case "B":
                            CloseWindow();
                            break;
                        case "Start":
                            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                            SendOutlinePreviewTextUpdate("ENTER");
                            break;

                    }
                }

               
            });
        }






        public async Task MoveWindowToNextMonitorAsync()
        {
            //WE NEED TO GET CURRENT STATE OF WINDOW BECAUSE MOVING IT WHILE NOT OPEN MAKES IT NOT WORK THE FIRST TIME
            OSK osk = Local_Object.Instance.GetOSKWindow();
            var helper = new WindowInteropHelper(osk);

            WindowState windowState = ScreenProgram_Management.GetWindowState(helper.Handle);


            //if (windowState != WindowState.Maximized)
            //{
            ScreenProgram_Management.SetWindowState(helper.Handle, WindowState.Normal);
            Thread.Sleep(200);
            //}



            List<System.Windows.Forms.Screen> screens = System.Windows.Forms.Screen.AllScreens.ToList();

            // Get the position and size of the window
            ScreenProgram_Management.RECT windowRect;
            ScreenProgram_Management.GetWindowRect(helper.Handle, out windowRect);


            // Get the screen  of the winodw
            System.Windows.Forms.Screen windowScreen = System.Windows.Forms.Screen.FromHandle(helper.Handle);

            int indexScreen = screens.IndexOf(windowScreen);



            int newIndex = 0;
            if (indexScreen != -1 && indexScreen < (screens.Count - 1))
            {
                newIndex = indexScreen + 1;
            }

            System.Windows.Forms.Screen targetScreen = screens[newIndex];
            // Calculate the new position of the window relative to the target monitor
            int newX = targetScreen.Bounds.Left; // Example: 100 pixels from the left edge
            int newY = targetScreen.Bounds.Top; // Example: 100 pixels from the top edge


            Debug.WriteLine(indexScreen.ToString() + " newX " + newX.ToString() + "; newY " + newY.ToString());

            // Move the window to the new position
            ScreenProgram_Management.SetWindowPos(helper.Handle, IntPtr.Zero, newX, newY, windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top, ScreenProgram_Management.SWP_SHOWWINDOW | ScreenProgram_Management.SWP_NOZORDER);

            Thread.Sleep(400);
            ScreenProgram_Management.SetWindowState(helper.Handle, WindowState.Maximized);
            Thread.Sleep(400);
            //maximize window and set to front

            //ScreenProgram_Management.SetForegroundWindow(selectedProcess.MainWindowHandle);



        }

        private void ToggleOutlineTextBlock()
        {
            var window = Local_Object.Instance.GetGeneralWindow(this);

            if (window is OSK)
            {
                OSK osk = (OSK)window;
                osk.ToggleOutlineTextBlock();
            }
        }

        private void SendOutlinePreviewTextUpdate(string text)
        {
            var window = Local_Object.Instance.GetGeneralWindow(this);

            if (window is OSK)
            {
                OSK osk = (OSK)window;
                osk.UpdateOutlinePreviewText(text);
            }
        }
        private void JoystickEvent_controllerJoystickEvent(object? sender, controllerJoystickEventArgs e)
        {
           
            Dispatcher.Invoke(() =>
            {
                if (this.Visibility == Visibility.Visible)
                {
                    HandleJoystickInput(e.lx, e.ly, e.rx, e.ry);

                
                }
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


            if ((newButton != leftButton && left) || (newButton != rightButton && !left))
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
        DispatcherTimer hideLeftCircleTimer = new DispatcherTimer()
        {
            Interval = new TimeSpan(0, 0, 1)
        };

        DispatcherTimer hideRightCircleTimer = new DispatcherTimer()
        {
            Interval = new TimeSpan(0, 0, 1)
        };

        DispatcherTimer hideLeftButtonTimer = new DispatcherTimer()
        {
            Interval = new TimeSpan(0, 0, 6)
        };

        DispatcherTimer hideRightButtonTimer = new DispatcherTimer()
        {
            Interval = new TimeSpan(0, 0, 6)
        };
        public void ControllerConnected()
        {
            //HighlightButton(leftButton);
            //HighlightButton(rightButton);


            

            leftCircle.Fill = Brushes.DarkGray;
            rightCircle.Fill = Brushes.DarkGray;

            //show controller icons on special keys
            ToggleControllerIconViewbox(true);

        }

        public void HideLeftCircleTimer_Tick(object? sender, EventArgs e)
        {

            hideLeftCircleTimer.Stop();
            leftCircle.Visibility = Visibility.Collapsed;
          

        }
        public void HideRightCircleTimer_Tick(object? sender, EventArgs e)
        {

            hideRightCircleTimer.Stop();
            rightCircle.Visibility = Visibility.Collapsed;


        }
        public void HideRightButtonTimer_Tick(object? sender, EventArgs e)
        {

            hideRightButtonTimer.Stop();
            UnhighlightButton(rightButton);


        }
        public void HideLeftButtonTimer_Tick(object? sender, EventArgs e)
        {

            hideLeftButtonTimer.Stop();
            UnhighlightButton(leftButton);


        }

        public void ShowLeftCircle()
        {
            //runs during the joystick movement event
            if (leftCircle.Visibility == Visibility.Collapsed)
            {
                leftCircle.Visibility = Visibility.Visible;
                HighlightButton(leftButton);
                hideLeftCircleTimer.Start();
                hideLeftButtonTimer.Start();
            }
            else
            {//reset timer using stop and then start
                hideLeftCircleTimer.Stop();
                hideLeftCircleTimer.Start();

                hideLeftButtonTimer.Stop();
                hideLeftButtonTimer.Start();
            }
        }
        public void ShowRightCircle()
        {
            //runs during the joystick movement event
            if (rightCircle.Visibility == Visibility.Collapsed)
            {
                rightCircle.Visibility = Visibility.Visible;
                HighlightButton(rightButton);
                hideRightCircleTimer.Start();
                hideRightButtonTimer.Start();
            }
            else
            {//reset timer using stop and then start
                hideRightCircleTimer.Stop();
                hideRightCircleTimer.Start();

                hideRightButtonTimer.Stop();
                hideRightButtonTimer.Start();
            }
        }

        #endregion

    }
}
