using Everything_Handhelds_Tool.AppWindows.OSK;
using Everything_Handhelds_Tool.Classes.Actions;
using Everything_Handhelds_Tool.Classes.Actions.ActionClass;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.MouseMode;
using Everything_Handhelds_Tool.Classes.MouseMode.Actions;
using Everything_Handhelds_Tool.Models;
using Everything_Handhelds_Tool.Pages;

using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;


namespace Everything_Handhelds_Tool.Classes
{
    public class ControllerInput
    {
        //add profiles to perform either mouse or controller mapping

        public ButtonPressEvent buttonPressEvent = new ButtonPressEvent();
        public ControllerConnectionChangedEvent controllerConnectionChangedEvent = new ControllerConnectionChangedEvent();
        public JoystickEvent joystickEvent = new JoystickEvent();
        public Controller? controller;

           
        public Dictionary<ushort, Actions.Action> controllerHotKeyDictionary = new Dictionary<ushort, Actions.Action>();


        //Variable to stop events in the case of programming a hot key
        private bool _suspendEventsForHotKeyProgramming { get; set; } = false;
        private bool _suspendEventsForNewHotKeyList { get; set; } = false;
        private bool _suspendEventsForOSK { get; set; } = false;
        private bool _suspendEventsMouseModeProfileUpdate { get; set; } = false;

      


        public bool suspendEventsMouseModeProfileUpdate
        {
            get
            {
                return _suspendEventsMouseModeProfileUpdate;
            }
            set
            {
                _suspendEventsMouseModeProfileUpdate = value;
            }
        }

        public bool suspendEventsForOSK
        {
            get
            {
                return _suspendEventsForOSK;
            }
            set
            {
                _suspendEventsForOSK = value;
            }
        }
        public bool suspendEventsForNewHotKeyList
        {
            get
            {
                return _suspendEventsForNewHotKeyList;
            }
            set
            {
                _suspendEventsForNewHotKeyList = value;
            }
        }
        public bool suspendEventsForHotKeyProgramming
        {
            get
            {
                return _suspendEventsForHotKeyProgramming;
            }
            set
            {
                _suspendEventsForHotKeyProgramming = value;
            }
        }
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
                Log_Writer.Instance.writeLog("Starting MainControllerThreadLoop");


                GetConnectedController();

                Gamepad currentGamepadState = controller.GetState().Gamepad;
                Gamepad previousGamepadState = controller.GetState().Gamepad;
                string continousInputCurrent = "";
                string continousInputPrevious = "";
                int continousInputCounter = 0;

                UpdateHotKeyDictionary();

                while (this != null)
                {
                    //check bool suspendEventsForNewHotKeyList to see if the hot key list needs updating
                    //if true it needs to be handled separately because live updating while it loops causes an error when it hits the hotkey detection
                    if (!suspendEventsForNewHotKeyList && !suspendEventsMouseModeProfileUpdate)
                    {

                        //main controller thread is here. Start with getting controller
                        GetConnectedController();


                        //check bool suspendEventsForHotKeyProgramming to see if events should stop firing while a hotkey is programmed
                        //this needs to be after GetConnectedController because the hotkey programming uses the controller object here
                        if (!suspendEventsForHotKeyProgramming)
                        {

                            currentGamepadState = controller.GetState().Gamepad;
                            //reset continousInputCurrent, this helps with establishing long button presses
                            continousInputCurrent = "";

                            //check for hot key press is dual function, it does the hot key detection and runs the action
                            //if hotkey detected, it returns true we want to skip most of the routine so it can loop around. this prevents controller input piggybacking on
                            //a hotkey like opening a window and immediately having the controller input do something in the window. that is unintended behavior
                            if (CheckForHotKeyPress(currentGamepadState, previousGamepadState)) { goto continueloop; }


                            //handlebuttondetction is dual function, it does the button press events including triggers, and it updates continousInputCurrent
                            //as needed. If no continous buttons pressed it just returns the same value
                            continousInputCurrent = HandleButtonDetection(currentGamepadState, previousGamepadState, continousInputCurrent);


                            //handlejoystickdetection fires the joystick event if any of the stick values are greater than deadzone value
                            HandleJoystickDetection(currentGamepadState, previousGamepadState);


                            //call routine that handles continous input controller input events and counts usage
                            continousInputCounter = HandleContinousInput(continousInputCurrent, continousInputPrevious, continousInputCounter);
                                                       
                        continueloop:
                            //set previous states to current for reference
                            previousGamepadState = currentGamepadState;
                            continousInputPrevious = continousInputCurrent;


                        }
                        else
                        {
                            //handle hotkey programming
                            //this below is a safeguard to prevent the editactionpage from not flipping the bool to allow controller input to resume
                            //it checks main window page and checks if page is null or is not EditActionPage. In both cases it flips the bool to allow controller input to resume
                            //if neither case is true, editactionpage is still visible and we will rely on that to flip the bool back

                            //MAKE SURE TO WRAP IN A DISPATCHER BECAUSE THIS THREAD DOESNT OWN THE PAGE THREAD
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                Page page = Local_Object.Instance.GetMainWindowFramePage();
                                if (page != null)
                                {
                                    if (page is not EditActionPage)
                                    {
                                        suspendEventsForHotKeyProgramming = false;
                                    }
                                }
                                else
                                {
                                    suspendEventsForHotKeyProgramming = false;
                                }
                            });
                           
                        }
                        //add additional delay as it isnt necessary to make this thread refresh every 10 ms until its safe to resume
                

                    }
                    else
                    {
                        UpdateHotKeyDictionary();
                        suspendEventsForNewHotKeyList = false;
                    }

                    //task delay for 10 ms to match approx. 100 Hz refresh of controller
                    await Task.Delay(10);

                }
                Log_Writer.Instance.writeLog("Ending MainControllerThreadLoop");
            }
            catch (Exception ex)
            {
                Log_Writer.Instance.writeLog("Error in main controller thread; " + ex.Message, "CI02");
            }
        }

        #region support routines for main thread

       
        private void UpdateHotKeyDictionary()
        {
            //IMPORTANT: updates the hot key list when an action is saved
            ActionList actions = (ActionList)XML_Management.Instance.LoadXML("ActionList");

            controllerHotKeyDictionary = actions.ReturnControllerActionHotKeyList();

        }
        private string HandleButtonDetection(Gamepad currentGamepadState, Gamepad previousGamepadState, string continousInputCurrent)
        {
            //IMPORANT MAIN THREAD ROUTINE
            //this is the normal business routine after hotkey checking
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

            //handle triggers
            if (currentGamepadState.LeftTrigger > 80 && previousGamepadState.LeftTrigger <= 80)
            {
                buttonPressEvent.raiseControllerInput("LeftTrigger");
            }

            if (currentGamepadState.RightTrigger > 80 && previousGamepadState.RightTrigger <= 80)
            {
                buttonPressEvent.raiseControllerInput("RightTrigger");
            }
            return continousInputCurrent;
        }
        private void HandleJoystickDetection(Gamepad currentGamepadState, Gamepad previousGamepadState)
        {
            //IMPORANT MAIN THREAD ROUTINE

            int ly = currentGamepadState.LeftThumbY;
            int lx = currentGamepadState.LeftThumbX;
            int ry = currentGamepadState.RightThumbY;
            int rx = currentGamepadState.RightThumbX;
            //APPLY DEADZONE BECAUSE SMALL DRIFT KILLS THIS
            int deadZone = 2500;
            if (Math.Abs(lx) > deadZone || Math.Abs(ly) > deadZone || Math.Abs(rx) > deadZone || Math.Abs(ry) > deadZone)
            {
                joystickEvent.raiseStickInput(currentGamepadState.LeftThumbX, currentGamepadState.LeftThumbY, currentGamepadState.RightThumbX, currentGamepadState.RightThumbY);
            }
        }

        private bool CheckForHotKeyPress(Gamepad currentGamepadState, Gamepad previousGamepadState)
        {
            //IMPORANT MAIN THREAD ROUTINE

            //bool value determines main thread loop
            //if hot key detected and activates, return true so we can go back to the beginning of the main thread. i dont want to piggyback
            //controller input ontop of a hot key action
            //if false, main thread continues

            //check for ushort hotkey combo press from action list
            if (controllerHotKeyDictionary.Count > 0)
            {
                //this converts controller button presses to unique ushort value
                ushort currentButtonCombo = (ushort)currentGamepadState.Buttons;

                //if the value exists WE ONLY DO THE ACTION, NOTHING ELSE and repeat the while loop

                //If the value doesn't exist, we continue on like normal. This is important because we dont want to register other button presses 
                //that might mess up a hot key aciton like opening the menu, you could accidentally send a action
                //Good example is LB+RB+Dpad, the last dpad press could translate into the open QAM action
                if (controllerHotKeyDictionary.ContainsKey(currentButtonCombo))
                {
                    //Add the current vs previous gamepad button state to prevent mutliple actions from being called back to back
                    Actions.Action action;
                    if (controllerHotKeyDictionary.TryGetValue(currentButtonCombo, out action))
                    {
                        if (currentGamepadState.Buttons != previousGamepadState.Buttons)
                        {
                            action.OnActivate();
                            return true;
                        }
                        else
                        {//this is a hold button action for keeping the guide up for reference, its action handler will take care of this
                            if (action is Toggle_Guide)
                            {
                                action.OnActivate();
                            }
                        }
                    }

                   



                }

            }
            return false;
        }

        private int HandleContinousInput(string continuousInputCurrent, string continuousInputPrevious, int counter)
        {
            //IMPORTANT MAIN THREAD ROUTINE

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

        //controllerConnected supports the event for controller connnected/disconnect, previous method was clunky and relied on too many if statements
        private bool _controllerConnected { get; set; } = false;

        public bool controllerConnected
        {
            get { return _controllerConnected; }

            set
            {
                if (value != _controllerConnected)
                {//only call event if new value != current value
                    _controllerConnected = value;
                    controllerConnectionChangedEvent.raiseControllerConnectionChanged(value);
                 
                }
            }
        }
        private async void GetConnectedController()
        {
            //IMPORTANT MAIN THREAD ROUTINE
            //this loops through all 4 controller slots and attempts to connect to one. If not, sleeps and starts searching again.
            try
            {
                //this code runs every few seconds and checks to see if a controller is pressing the start/back keys to swap to that controller as controlling controller 
                CheckSwapControllerNumber();

                //if controller is not null and already connected then return
                if (controller != null)
                {
                    if (controller.IsConnected) { return; }

                }

               

            connectController:
                //add safe measures to detect if app closes, this could potentially be a thread hang up spot if a controller isnt ocnnected when app crashes/closes. use this ==null to detect
                if (this == null) { return; }


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

                controllerConnected = false;

                //if nothing connected and isn't null then we wait a few seconds and try again
                Task.Delay(3000).Wait();
                goto connectController;

            }
            catch (Exception ex)
            {
                Log_Writer.Instance.writeLog("Connecting to controller; " + ex.Message, "CI01");
            }

        }

        private DateTime dateTimeControllerCheck = DateTime.Now;

        private void CheckSwapControllerNumber()
        {
            //IMPORTANT MAIN THREAD ROUTINE
            //this is my special method to swap controller numbers in case multiple are connected.
            //I dont want this to run every cycle but i want it to run once every second or so
            //this checks to see if a specific controller is pressing start/back at same time and will swap to that controller

            //lets start by determing if it should run, do a date time delta 
            TimeSpan timeDifference = DateTime.Now - dateTimeControllerCheck;
            //if time is less than 2 seconds return, dont finish the routine
            if (timeDifference.Seconds < 2) { return; }

            //reset time checked, if this doesnt happen then it will do this check every cycle
            dateTimeControllerCheck = DateTime.Now;

            foreach (UserIndex ui in userIndexControllers)
            {
                Controller newController = new Controller(ui);
                if (newController != null)
                {
                    if (newController.IsConnected)
                    {
                        Gamepad gamepad = newController.GetState().Gamepad;
                        if (gamepad.Buttons.HasFlag(GamepadButtonFlags.Start) && gamepad.Buttons.HasFlag(GamepadButtonFlags.Back))
                        {
                            controller = newController;
                            controllerConnected = true;

                            return;
                        }

                    }
                }
            }

        }
        #endregion


  

        #region support lists for controller thread
        private List<UserIndex> userIndexControllers = new List<UserIndex>() { UserIndex.One, UserIndex.Two, UserIndex.Three, UserIndex.Four };


        //list of button flags to loop through in main thread
        private List<GamepadButtonFlags> gamepadButtonFlags = new List<GamepadButtonFlags>
        { GamepadButtonFlags.A, GamepadButtonFlags.B, GamepadButtonFlags.X, GamepadButtonFlags.Y, GamepadButtonFlags.DPadUp, GamepadButtonFlags.DPadDown, GamepadButtonFlags.DPadLeft, GamepadButtonFlags.DPadRight, GamepadButtonFlags.LeftShoulder, GamepadButtonFlags.RightShoulder, GamepadButtonFlags.Start, GamepadButtonFlags.Back, GamepadButtonFlags.LeftThumb, GamepadButtonFlags.RightThumb };
        #endregion

        //KEEP HANDLEDPADINPUT AROUND IN CASE I WANT TO ADD JOYSTICK SUPPORT FOR MAIN APP MOVEMENT, otherwise not in use
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
                Log_Writer.Instance.writeLog("Error in main controller thread - dpad input handler; " + ex.Message, "CI03");
            }

            return "";
        }
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

    public class JoystickEvent
    {

        public event EventHandler<controllerJoystickEventArgs> controllerJoystickEvent;

        public void raiseStickInput(double lx, double ly, double rx, double ry)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(() =>
            {
                controllerJoystickEvent?.Invoke(this, new controllerJoystickEventArgs(lx, ly, rx, ry));
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

    public class controllerJoystickEventArgs : EventArgs
    {
        public double lx { get; set; }
        public double ly { get; set; }
        public double rx { get; set; }
        public double ry { get; set; }
        public controllerJoystickEventArgs(double lx, double ly, double rx, double ry)
        {
            this.lx = lx;
            this.ly = ly;
            this.rx = rx;
            this.ry = ry;
        }
    }

    public class ControllerConnectionChangedEvent
    {

        public event EventHandler<controllerConnectionChangedEventArgs> controllerConnectionChangedEvent;

        public void raiseControllerConnectionChanged(bool connected)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(() =>
            {
                controllerConnectionChangedEvent?.Invoke(this, new controllerConnectionChangedEventArgs(connected));
            });
        }
    }
    public class controllerConnectionChangedEventArgs : EventArgs
    {
        public bool Connected { get; set; }
        public controllerConnectionChangedEventArgs(bool connected)
        {
            this.Connected = connected;
        }
    }
}
