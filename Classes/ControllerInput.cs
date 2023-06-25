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

namespace Everything_Handhelds_Tool.Classes
{
    public class ControllerInput
    {
        //add profiles to perform either mouse or controller mapping
        public MouseMode mouseMode = null;
        public ControllerMapper controllerMapper = null;
        public ButtonPressEvent buttonPressEvent = new ButtonPressEvent();
        private Controller? controller;

        //Variable to stop events in the case of programming a hot key
        public bool suspendEventsForProgramming { get; set; } = false;

        public ControllerInput()
        {
            Thread controllerThread = new Thread(MainControllerThreadLoop);
            controllerThread.IsBackground = true;
            controllerThread.Name = "ControllerThread";
            controllerThread.Priority = ThreadPriority.Highest;
            controllerThread.Start();
        }

        private async void MainControllerThreadLoop()
        {
            try
            {
                Log_Writer.writeLog("Starting MainControllerThreadLoop");
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
                        if (gbf.ToString().Contains("DPad"))
                        {
                            //call routine to send controller input events and track for continous input for any dpad input
                            string result = HandleDPadInput(gbf, currentGamepadState, previousGamepadState);
                            if (result != "") { continousInputCurrent = result; }
                        }
                        else
                        {
                            if (currentGamepadState.Buttons.HasFlag(gbf) && !previousGamepadState.Buttons.HasFlag(gbf))
                            {
                                //raise event for button press
                                buttonPressEvent.raiseControllerInput(gbf.ToString());
                            }
                        }
                       
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
                Log_Writer.writeLog("Ending MainControllerThreadLoop");
            }
            catch (Exception ex)
            {
                Log_Writer.writeLog("Error in main controller thread; " + ex.Message, "CI02");
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
        private string HandleDPadInput(GamepadButtonFlags gbf, Gamepad currentGamepadState, Gamepad previousGamepadState)
        {
            //Routine looks at both dpad or joystick for continous movement by comparing to previous state and returns the dpad direction (even for joystick)
            //to keep track of continous movement. If movement isn't continous between controller cycles return ""
            
            //define short variables to be either for the X or Y left thumb values
            try
            {
                decimal previousInputValue = 0;
                bool joystickInputDirectionCorrect = false;

                //set short variables based on dpad, use absolute values so we don't have to make different cases for up or down/left or right which requires different >=  or <= operators. Makes it simple
                if (gbf == GamepadButtonFlags.DPadUp && currentGamepadState.LeftThumbY > 12000 || gbf == GamepadButtonFlags.DPadDown && currentGamepadState.LeftThumbY < -12000)
                {
                    joystickInputDirectionCorrect = true;
                    previousInputValue = Math.Abs((decimal)previousGamepadState.LeftThumbY);
                }
                if (gbf == GamepadButtonFlags.DPadRight && currentGamepadState.LeftThumbX > 12000 || gbf == GamepadButtonFlags.DPadLeft && currentGamepadState.LeftThumbX < -12000)
                {
                    joystickInputDirectionCorrect = true;
                    previousInputValue = Math.Abs((decimal)previousGamepadState.LeftThumbX);
                }

                //this says if dpad is pressed or joystick is moved go to next if statement
                if (currentGamepadState.Buttons.HasFlag(gbf) || joystickInputDirectionCorrect)
                {
                    //this check if the previous gamepad state is pressed or joystick wasn't moved
                    if (!previousGamepadState.Buttons.HasFlag(gbf) && previousInputValue <= 12000)
                    {
                        //send controller input event if current state is on and previous state was off
                        buttonPressEvent.raiseControllerInput(gbf.ToString());
                    }
                    else
                    {
                        //otherwise capture it as a continous movement
                        return gbf.ToString();
                    }
                }
                //default return value is "", only when input is continous will it return something else
                
            }
            catch (Exception ex)
            {
                Log_Writer.writeLog("Error in main controller thread - dpad input handler; " + ex.Message, "CI03");
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
                Log_Writer.writeLog("Connecting to controller; " + ex.Message, "CI01");
            }

        }


        //list of button flags to loop through in main thread
        private List<GamepadButtonFlags> gamepadButtonFlags = new List<GamepadButtonFlags>
        { GamepadButtonFlags.A, GamepadButtonFlags.B, GamepadButtonFlags.X, GamepadButtonFlags.Y, GamepadButtonFlags.DPadUp, GamepadButtonFlags.DPadDown, GamepadButtonFlags.DPadLeft, GamepadButtonFlags.DPadRight, GamepadButtonFlags.LeftShoulder, GamepadButtonFlags.RightShoulder, GamepadButtonFlags.Start, GamepadButtonFlags.Back, GamepadButtonFlags.LeftThumb, GamepadButtonFlags.RightThumb };

    }

    public class ButtonPressEvent
    {

        public event EventHandler<controllerInputEventArgs> controllerInputEvent;

        public void raiseControllerInput(string action)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(() =>
            {
                controllerInputEvent?.Invoke(this, new controllerInputEventArgs(action));
            });
        }
    }
    public class controllerInputEventArgs : EventArgs
    {
        public string Action { get; set; }
        public controllerInputEventArgs(string action)
        {
            this.Action = action;
        }
    }

}
