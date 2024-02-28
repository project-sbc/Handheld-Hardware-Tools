using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Everything_Handhelds_Tool.AppWindows.OSK
{
    public class ControllerInputOSK
    {

        public ButtonPressEventOSK buttonPressEvent = new ButtonPressEventOSK();
        public Controller? controller;


        public Thread controllerThread;


        //controllerConnected keeps track of controller state to send to the page to handle hiding controller icons, etc.
        private bool _controllerConnected = false;

        private bool controllerConnected
        {
            get
            {
                return _controllerConnected;

            }

            set
            {//if new value doesnt equal what it was (_controllerConnected) then something changed
                if (_controllerConnected != value)
                {
                    //if new value is false   (!value) then it must be it disconnected
                    if (!value)
                    {
                        buttonPressEvent.raiseControllerConnectedDisconnected("Disconnected");
                    }
                    else
                    {//else if its true then its connected
                        buttonPressEvent.raiseControllerConnectedDisconnected("Connected");
                    }
                    //dont forget to change the _controllerConnected value to keep track of the state
                    _controllerConnected = value;
                }
            }
        }

        private bool _abortThread { get; set; } = false;

        public bool abortThread
        {
            get { return _abortThread; }
            set { _abortThread = value; }
        }
        public ControllerInputOSK()
        {
            controllerThread = new Thread(MainControllerThreadLoop);
            controllerThread.IsBackground = true;
            controllerThread.Name = "ControllerThreadOSK";
            controllerThread.Priority = ThreadPriority.Highest;
            controllerThread.Start();
        }

        public void AbortThread()
        {
            abortThread = true;
        }


        private async void MainControllerThreadLoop()
        {
            try
            {

                GetConnectedController();
                //add a abort thread check because if controller not connected, an abort thread signal will end up here and throw an error on controllerGamepadState
                if (abortThread) { return; }

                Gamepad currentGamepadState = controller.GetState().Gamepad;
                Gamepad previousGamepadState = controller.GetState().Gamepad;
                string continousInputCurrent = "";
                string continousInputPrevious = "";
                int continousInputCounter = 0;


                while (_abortThread == false && System.Windows.Application.Current != null)
                {
                    //var watch = System.Diagnostics.Stopwatch.StartNew();
                    //main controller thread is here. Start with getting controller
                    GetConnectedController();

                    //add a abort thread check because if controller not connected, an abort thread signal will end up here and throw an error on controllerGamepadState
                    if (abortThread) { return; }

                    currentGamepadState = controller.GetState().Gamepad;
                    //reset continousInputCurrent
                    continousInputCurrent = "";
                    foreach (GamepadButtonFlags gbf in gamepadButtonFlags)
                    {
                        if (currentGamepadState.Buttons.HasFlag(gbf) && !previousGamepadState.Buttons.HasFlag(gbf))
                        {
                            //raise event for button press
                            buttonPressEvent.raiseControllerInput(gbf.ToString());

                        }
                        if (currentGamepadState.Buttons.HasFlag(gbf) && previousGamepadState.Buttons.HasFlag(gbf))
                        {
                            continousInputCurrent = gbf.ToString();

                        }
                    }

                    if (currentGamepadState.LeftTrigger > 80 && previousGamepadState.LeftTrigger <= 80)
                    {
                        buttonPressEvent.raiseControllerInput("LeftTrigger");
                    }

                    if (currentGamepadState.RightTrigger > 80 && previousGamepadState.RightTrigger <= 80)
                    {
                        buttonPressEvent.raiseControllerInput("RightTrigger");
                    }

                    //add all joystick inputs to determine if we should trigger the movement event, use absolute value and add to see if > 0 otherwise DONT SEND EVENT


                    //APPLY DEADZONE BECAUSE SMALL DRIFT KILLS THIS
                 

                    int ly = currentGamepadState.LeftThumbY;
                    int lx = currentGamepadState.LeftThumbX;
                    int ry = currentGamepadState.RightThumbY;
                    int rx = currentGamepadState.RightThumbX;
                    int deadZone = 2500;
                    if (Math.Abs(lx) > deadZone || Math.Abs(ly) > deadZone || Math.Abs(rx) > deadZone || Math.Abs(ry) > deadZone)
                    {
                        buttonPressEvent.raiseStickInput(currentGamepadState.LeftThumbX, currentGamepadState.LeftThumbY, currentGamepadState.RightThumbX, currentGamepadState.RightThumbY);
                    }


                    //call routine that handles continous input controller input events and counts usage
                    continousInputCounter = HandleContinousInput(continousInputCurrent, continousInputPrevious, continousInputCounter);

                    //set previous states to current for reference
                    previousGamepadState = currentGamepadState;
                    continousInputPrevious = continousInputCurrent;

                    //sleep for 8 ms to match approx. 100 Hz refresh of controller
                    await Task.Delay(8);
                    //watch.Stop();
                    //Debug.WriteLine($"Total Execution Time: {watch.ElapsedMilliseconds} ms");
                }

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        private int HandleContinousInput(string continuousInputCurrent, string continuousInputPrevious, int counter)
        {
            //Routine checks to see if the same input (up down left right) has been going on for more than 39 times (390 ms)
            //if greater than 39 times then send that controller input over and over every 5 cycles (% 5 is modulus) which is 50 ms.
            //Dont do every cycle or its too quick
            if (continuousInputCurrent == continuousInputPrevious && continuousInputCurrent != "")
            {
                if (counter > 39)
                {
                    //use % 5 modulus for that it sends every 5th time instead of every time (which is too quick)
                    if (counter % 5 == 0)
                    {
                        buttonPressEvent.raiseControllerInput(continuousInputCurrent);
                    }
                }
                //if continous then add 1 to counter
                return counter + 1;
            }
            //otherwise return 1 by default
            return 1;
        }


        private void GetConnectedController()
        {
            //this loops through all 4 controller slots and attempts to connect to one. If not, sleeps and starts searching again.
            try
            {
                List<UserIndex> userIndexControllers = new List<UserIndex>() { UserIndex.One, UserIndex.Two, UserIndex.Three, UserIndex.Four };

            connectController:
                if (!abortThread)
                {
                    foreach (UserIndex ui in userIndexControllers)
                    {
                        Controller newController = new Controller(ui);
                        if (newController != null)
                        {
                            if (newController.IsConnected)
                            {
                                controller = newController;
                                controllerConnected = true;
                                return;
                            }
                        }
                    }

                    //if nothing connected and isn't null then we wait a few seconds and try again
                    controllerConnected = false;
                    Thread.Sleep(3000);
                    goto connectController;
                }


            }
            catch (Exception ex)
            {

            }

        }


        //list of button flags to loop through in main thread
        private List<GamepadButtonFlags> gamepadButtonFlags = new List<GamepadButtonFlags>
        { GamepadButtonFlags.A, GamepadButtonFlags.B, GamepadButtonFlags.X, GamepadButtonFlags.Y, GamepadButtonFlags.DPadUp, GamepadButtonFlags.DPadDown, GamepadButtonFlags.DPadLeft, GamepadButtonFlags.DPadRight, GamepadButtonFlags.LeftShoulder, GamepadButtonFlags.RightShoulder, GamepadButtonFlags.Start, GamepadButtonFlags.Back, GamepadButtonFlags.LeftThumb, GamepadButtonFlags.RightThumb };

    }

    public class ButtonPressEventOSK
    {

        public event EventHandler<controllerInputEventArgsOSK> controllerInputEventOSK;
        public event EventHandler<controllerJoystickEventArgsOSK> controllerJoystickEventOSK;
        public event EventHandler<controllerInputEventArgsOSK> controllerConnectedDisconnectedEventOSK;

        public void raiseControllerInput(string action)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(() =>
            {
                controllerInputEventOSK?.Invoke(this, new controllerInputEventArgsOSK(action));
            });


        }

        public void raiseStickInput(double lx, double ly, double rx, double ry)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(() =>
            {
                controllerJoystickEventOSK?.Invoke(this, new controllerJoystickEventArgsOSK(lx, ly, rx, ry));
            });


        }

        public void raiseControllerConnectedDisconnected(string action)
        {

            System.Windows.Application.Current.Dispatcher.BeginInvoke(() =>
            {
                controllerConnectedDisconnectedEventOSK?.Invoke(this, new controllerInputEventArgsOSK(action));
            });

        }

    }
    public class controllerInputEventArgsOSK : EventArgs
    {
        public string Action { get; set; }
        public controllerInputEventArgsOSK(string action)
        {
            Action = action;
        }
    }
    public class controllerJoystickEventArgsOSK : EventArgs
    {
        public double lx { get; set; }
        public double ly { get; set; }
        public double rx { get; set; }
        public double ry { get; set; }
        public controllerJoystickEventArgsOSK(double lx, double ly, double rx, double ry)
        {
            this.lx = lx;
            this.ly = ly;
            this.rx = rx;
            this.ry = ry;
        }
    }
}
