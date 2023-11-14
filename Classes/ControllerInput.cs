using Everything_Handhelds_Tool.Classes.Actions;
using Everything_Handhelds_Tool.Classes.Actions.ActionClass;
using Everything_Handhelds_Tool.Models;
using Everything_Handhelds_Tool.Pages;
using Nefarius.Utilities.DeviceManagement.Extensions;
using Nefarius.Utilities.DeviceManagement.PnP;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Exceptions;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
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
using System.Windows.Documents;
using System.Windows.Threading;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace Everything_Handhelds_Tool.Classes
{
    public class ControllerInput
    {
        //add profiles to perform either mouse or controller mapping

        public ButtonPressEvent buttonPressEvent = new ButtonPressEvent();
        public ControllerConnectionChangedEvent controllerConnectionChangedEvent = new ControllerConnectionChangedEvent();
        public Controller? controller;

        //FOR LEGION GO SPECIFIC
        public Controller? controllerR;
        public Controller? controllerL;

        private ViGEmClient _ViGEmClient;
        //   wasControllerConnected is a previous state condition used to determine when the controller was connected to not connected
        // it helps fire the controllerConnectionChangedEvent
        private bool wasControllerConnected = false;

        public Dictionary<ushort, Actions.Action> controllerHotKeyDictionary = new Dictionary<ushort, Actions.Action>();


        //Variable to stop events in the case of programming a hot key
        private bool suspendEventsForHotKeyProgramming { get; set; } = false;
        private bool suspendEventsForNewHotKeyList { get; set; } = false;
        private bool suspendEventsForOSK { get; set; } = false;

        public bool publicSuspendEventsForOSK
        {
            get
            {
                return suspendEventsForOSK;
            }
            set
            {
                suspendEventsForOSK = value;
            }
        }
        public bool publicSuspendEventsForNewHotKeyList
        {
            get
            {
                return suspendEventsForNewHotKeyList;
            }
            set
            {
                suspendEventsForNewHotKeyList = value;
            }
        }
        public bool publicSuspendEventsForHotKeyProgrammingt
        {
            get
            {
                return suspendEventsForHotKeyProgramming;
            }
            set
            {
                suspendEventsForHotKeyProgramming = value;
            }
        }
        public ControllerInput()
        {

            getBT();

            Thread controllerThread = new Thread(MainControllerThreadLoopLGo);
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


                    if (!suspendEventsForHotKeyProgramming && !suspendEventsForNewHotKeyList && !suspendEventsForOSK)
                    {
                        //var watch = System.Diagnostics.Stopwatch.StartNew();
                        //main controller thread is here. Start with getting controller
                        GetConnectedController();

                        currentGamepadState = controller.GetState().Gamepad;
                        //reset continousInputCurrent
                        continousInputCurrent = "";


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
                                Actions.Action action;
                                if (controllerHotKeyDictionary.TryGetValue(currentButtonCombo, out action))
                                {
                                    action.OnActivate();
                                    goto continueloop;
                                }
                            }

                        }


                        //this is the normal business routine after hotkey checking

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


                    continueloop:
                        //set previous states to current for reference
                        previousGamepadState = currentGamepadState;
                        continousInputPrevious = continousInputCurrent;
                    }
                    else
                    {
                        await Task.Delay(100);
                        if (suspendEventsForOSK)
                        {
                            if (!Application.Current.Windows.OfType<OSK>().Any())
                            {
                                suspendEventsForOSK = false;
                            }
                        }
                        if (suspendEventsForNewHotKeyList)
                        {
                            UpdateHotKeyDictionary();
                            suspendEventsForNewHotKeyList = false;
                        }
                        if (suspendEventsForHotKeyProgramming)
                        {
                            //check to see if main window frame is still on edit action page
                            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                            if (mainWindow != null)
                            {
                                if (mainWindow.frame.Content != null)
                                {
                                    if (mainWindow.frame.Content is not EditActionPage)
                                    {
                                        suspendEventsForHotKeyProgramming = false;
                                    }
                                }
                            }
                        }
                    }


                    //sleep for 10 ms to match approx. 100 Hz refresh of controller
                    await Task.Delay(10);
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

        private void UpdateHotKeyDictionary()
        {
            ActionList actions = (ActionList)XML_Management.Instance.LoadXML("ActionList");
            
            controllerHotKeyDictionary = actions.ReturnControllerActionHotKeyList();
         

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
                Log_Writer.Instance.writeLog("Error in main controller thread - dpad input handler; " + ex.Message, "CI03");
            }

            return "";
        }


        private async void GetConnectedController()
        {
            //this loops through all 4 controller slots and attempts to connect to one. If not, sleeps and starts searching again.
            try
            {
                //if controller is not null and already connected then return
                if (controller != null)
                {
                    if (controller.IsConnected) { return; }

                }

                List<UserIndex> userIndexControllers = new List<UserIndex>() { UserIndex.One, UserIndex.Two, UserIndex.Three, UserIndex.Four };

            connectController:

                foreach (UserIndex ui in userIndexControllers)
                {
                    Controller newController = new Controller(ui);
                    if (newController != null)
                    {
                        if (newController.IsConnected) 
                        { 
                            controller = newController;
                            wasControllerConnected = true;
                            controllerConnectionChangedEvent.raiseControllerConnectionChanged(true);
                            return; 
                        }
                    }
                }

                if (wasControllerConnected)
                {
                    wasControllerConnected = false;
                    controllerConnectionChangedEvent.raiseControllerConnectionChanged(false);
                }

                //if nothing connected and isn't null then we wait a few seconds and try again
                Task.Delay(4000).Wait();
                goto connectController;

            }
            catch (Exception ex)
            {
                Log_Writer.Instance.writeLog("Connecting to controller; " + ex.Message, "CI01");
            }

        }

        #region legion go

        private async void MainControllerThreadLoopLGo()
        {
            try
            {
                Log_Writer.Instance.writeLog("Starting MainControllerThreadLoop LEGION GO COMBINE PROGRAM");


                GetConnectedControllerLR();
                _ViGEmClient = new ViGEmClient();

                IXbox360Controller xbox360Controller = _ViGEmClient.CreateXbox360Controller();
                xbox360Controller.Connect();


                Gamepad currentGamepadStateL = controllerL.GetState().Gamepad;
                Gamepad currentGamepadStateR = controllerR.GetState().Gamepad;
                      

                while (this != null)
                {
                    GetConnectedController();
                    currentGamepadStateL = controllerL.GetState().Gamepad;
                    currentGamepadStateR = controllerR.GetState().Gamepad;



                    xbox360Controller.SetButtonState(Xbox360Button.A, currentGamepadStateR.Buttons.HasFlag(GamepadButtonFlags.X));


                    //sleep for 10 ms to match approx. 100 Hz refresh of controller
                    xbox360Controller.SubmitReport();
                    await Task.Delay(10);
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
        private async void GetConnectedControllerLR()
        {
            //LEGION GO SPECIFIC
            //this loops through all 4 controller slots and attempts to connect to one. If not, sleeps and starts searching again.
            try
            {
                //if controller is not null and already connected then return
                if (controllerR != null && controllerL != null)
                {
                    if (controllerL.IsConnected && controllerR.IsConnected) { return; }

                }

                List<UserIndex> userIndexControllers = new List<UserIndex>() { UserIndex.One, UserIndex.Two, UserIndex.Three, UserIndex.Four };

            connectControllers:

                foreach (UserIndex ui in userIndexControllers)
                {
                    Controller newController = new Controller(ui);
                    if (newController != null)
                    {
                        if (newController.IsConnected)
                        {
                            controllerL = newController;
                            wasControllerConnected = true;
                            controllerConnectionChangedEvent.raiseControllerConnectionChanged(true);
                            return;
                        }
                    }
                }

                foreach (UserIndex ui in userIndexControllers)
                {
                    Controller newController = new Controller(ui);
                    if (newController != null && newController != controllerL)
                    {
                        if (newController.IsConnected)
                        {
                            controllerR = newController;
                            wasControllerConnected = true;
                            controllerConnectionChangedEvent.raiseControllerConnectionChanged(true);
                            return;
                        }
                    }
                }


                if (wasControllerConnected)
                {
                    wasControllerConnected = false;
                    controllerConnectionChangedEvent.raiseControllerConnectionChanged(false);
                }

                //if nothing connected and isn't null then we wait a few seconds and try again
                Task.Delay(4000).Wait();
                goto connectController;

            }
            catch (Exception ex)
            {
                Log_Writer.Instance.writeLog("Connecting to controller; " + ex.Message, "CI01");
            }

        }

        private async void getBT()
        {
            DeviceInformationCollection PairedBluetoothDevices = await DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelector());
            //Connected bluetooth devices

            foreach (DeviceInformation device in PairedBluetoothDevices)
            {
                Debug.WriteLine(device.Name);
                Debug.WriteLine(device.IsEnabled);
                Debug.WriteLine(device.Pairing.IsPaired);
                Debug.WriteLine(device.Id);




            }
        }
        #endregion
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
