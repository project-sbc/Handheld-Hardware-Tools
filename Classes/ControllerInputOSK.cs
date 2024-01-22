using Everything_Handhelds_Tool.Models;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using static System.Collections.Specialized.BitVector32;

namespace Everything_Handhelds_Tool.Classes
{
    public class ControllerInputOSK
    {
        //add profiles to perform either mouse or controller mapping

        public ButtonPressEventOSK buttonPressEvent = new ButtonPressEventOSK();
        private Controller? controller;

        //Variable to stop events in the case of programming a hot key
        public bool suspendEventsForProgramming { get; set; } = false;


        private bool _absoluteJoystickMode { get; set; } = true;

        public bool absoluteJoystickMode
        {
            get { return _absoluteJoystickMode; }
            set {  _absoluteJoystickMode = value; }
        }
        public ControllerInputOSK()
        {
            Thread controllerThread = new Thread(MainControllerThreadLoop);
            controllerThread.IsBackground = true;
            controllerThread.Name = "ControllerThreadOSK";
            controllerThread.Priority = ThreadPriority.Highest;
            controllerThread.Start();
        }

        private async void MainControllerThreadLoop()
        {
            try
            {
                Log_Writer.Instance.writeLog("Starting MainControllerThreadLoop");
                GetConnectedController();

                Gamepad currentGamepadState = controller.GetState().Gamepad;
                Gamepad previousGamepadState = controller.GetState().Gamepad;
                string continousInputCurrent = "";
                string continousInputPrevious = "";
                int continousInputCounter = 0;

                while (this != null)
                {
                    //var watch = System.Diagnostics.Stopwatch.StartNew();
                    //main controller thread is here. Start with getting controller
                    GetConnectedController();

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

                    }

                    if (absoluteJoystickMode)
                    {
                        buttonPressEvent.raiseStickInput(currentGamepadState.LeftThumbX, currentGamepadState.LeftThumbY, currentGamepadState.RightThumbX, currentGamepadState.RightThumbY);
                    }
                    else
                    {
                        HandleJoystickToDPadInputLeftRight(currentGamepadState, previousGamepadState);
                    }

                    //call routine that handles continous input controller input events and counts usage
                    continousInputCounter = HandleContinousInput(continousInputCurrent, continousInputPrevious, continousInputCounter);

                    //set previous states to current for reference
                    previousGamepadState = currentGamepadState;
                    continousInputPrevious = continousInputCurrent;

                    //sleep for 10 ms to match approx. 100 Hz refresh of controller
                    await Task.Delay(2);
                    //watch.Stop();
                    //Debug.WriteLine($"Total Execution Time: {watch.ElapsedMilliseconds} ms");
                }
                Log_Writer.Instance.writeLog("Ending MainControllerThreadLoop");
            }
            catch (Exception ex)
            {
                Log_Writer.Instance.writeLog("Error in main controller thread; " + ex.Message, "CI02");
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
        private string HandleJoystickToDPadInputLeftRight(Gamepad currentGamepadState, Gamepad previousGamepadState)
        {
            //Routine looks at both dpad or joystick for continous movement by comparing to previous state and returns the dpad direction (even for joystick)
            //to keep track of continous movement. If movement isn't continous between controller cycles return ""
            
            //define short variables to be either for the X or Y left thumb values
            try
            {
                decimal previousInputValue = 0;
                bool joystickInputDirectionCorrect = false;

                //set short variables based on dpad, use absolute values so we don't have to make different cases for up or down/left or right which requires different >=  or <= operators. Makes it simple
                if (currentGamepadState.LeftThumbY > 12000  && previousGamepadState.LeftThumbY <= 12000)
                {
                    buttonPressEvent.raiseStickDPadInput("Left_Up");
                }
                if (currentGamepadState.LeftThumbY < -12000 && previousGamepadState.LeftThumbY >= -12000)
                {
                    buttonPressEvent.raiseStickDPadInput("Left_Down");
                }
                if (currentGamepadState.RightThumbY > 12000 && previousGamepadState.RightThumbY <= 12000)
                {
                    buttonPressEvent.raiseStickDPadInput("Right_Up");
                }
                if (currentGamepadState.RightThumbY < -12000 && previousGamepadState.RightThumbY >= -12000)
                {
                    buttonPressEvent.raiseStickDPadInput("Right_Down");
                }
                if (currentGamepadState.LeftThumbX < -12000 && previousGamepadState.LeftThumbX >= -12000)
                {
                    buttonPressEvent.raiseStickDPadInput("Left_Left");
                }
                if (currentGamepadState.LeftThumbX > 12000 && previousGamepadState.LeftThumbX <= 12000)
                {
                    buttonPressEvent.raiseStickDPadInput("Left_Right");
                }
                if (currentGamepadState.RightThumbX < -12000 && previousGamepadState.RightThumbX >= -12000)
                {
                    buttonPressEvent.raiseStickDPadInput("Right_Left");
                }
                if (currentGamepadState.RightThumbX > 12000 && previousGamepadState.RightThumbX <= 12000)
                {
                    buttonPressEvent.raiseStickDPadInput("Right_Right");
                }



               
                //default return value is "", only when input is continous will it return something else
                
            }
            catch (Exception ex)
            {
                Log_Writer.Instance.writeLog("Error in main controller thread - dpad input handler; " + ex.Message, "CI03");
            }

            return "";
        }


        private void GetConnectedController()
        {
            //this loops through all 4 controller slots and attempts to connect to one. If not, sleeps and starts searching again.
            try
            {
                List<UserIndex> userIndexControllers = new List<UserIndex>() { UserIndex.One, UserIndex.Two, UserIndex.Three, UserIndex.Four };

            connectController:

                foreach (UserIndex ui in userIndexControllers)
                {
                    Controller newController = new Controller(ui);
                    if (newController != null)
                    {
                        if (newController.IsConnected) { controller = newController; return; }
                    }
                }

                //if nothing connected and isn't null then we wait a few seconds and try again
                Thread.Sleep(3000);
                goto connectController;

            }
            catch (Exception ex)
            {
                Log_Writer.Instance.writeLog("Connecting to controller; " + ex.Message, "CI01");
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
        public event EventHandler<controllerInputEventArgsOSK> controllerJoystickDPadEventOSK;

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

        public void raiseStickDPadInput(string action)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(() =>
            {
                controllerJoystickDPadEventOSK?.Invoke(this, new controllerInputEventArgsOSK(action));
            });
        }

    }
    public class controllerInputEventArgsOSK : EventArgs
    {
        public string Action { get; set; }
        public controllerInputEventArgsOSK(string action)
        {
            this.Action = action;
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
