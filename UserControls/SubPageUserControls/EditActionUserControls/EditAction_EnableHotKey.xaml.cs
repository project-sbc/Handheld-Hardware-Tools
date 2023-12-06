using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Actions;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Pages;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Windows.Foundation.Collections;

namespace Everything_Handhelds_Tool.UserControls.EditActionUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class EditAction_EnableHotKey : ControllerUserControl
    {
        int originalSelectedIndex = -1;

        public EditAction_EnableHotKey(string newActionHotKey ="", string newActionHotKeyType = "")
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = button;
            toggleSwitchControl = toggleSwitch;

               
            //set control
           ConfigureControl(newActionHotKey, newActionHotKeyType);

        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("ChangeBack");
        }


        private void ConfigureControl(string newactionhotkey, string newactionHotKeyType)
        {
            hotKeyText = newactionhotkey;
            hotKeyType = newactionHotKeyType;


            if (newactionHotKeyType != "")
            {
                toggleSwitch.IsChecked = true;

                if (newactionHotKeyType == "Controller")
                {
                    iconHotKeyType.Symbol = Wpf.Ui.Common.SymbolRegular.XboxController24;
                    ushort controllervalue = 0;
                    ushort.TryParse(hotKeyText, out controllervalue);
                    hotKeyValue = controllervalue;
                    textBlockHotKey.Text = convertControllerUshortToString(hotKeyText);
                }
                if (newactionHotKeyType == "Keyboard")
                {
                    iconHotKeyType.Symbol = Wpf.Ui.Common.SymbolRegular.XboxController24;
                    textBlockHotKey.Text = newactionhotkey;
                }
            }
            else
            {
                toggleSwitch.IsChecked= false;
            }

        }




        public override void ReturnControlToPage()
        {


            //still run normal code for return to page
            base.ReturnControlToPage();
        }


        public override void ControlChangeValueHandler()
        {
           
            bool isToggled;
            if (toggleSwitch.IsChecked == false)
            {
                isToggled = false;
            }
            else
            {
                isToggled = true;
            }


            MainWindow mainWindow = Local_Object.Instance.GetMainWindow();

            if (mainWindow.frame.Content is EditActionPage)
            {
                EditActionPage editActionPage = mainWindow.frame.Content as EditActionPage;
                if (editActionPage != null)
                {
                   
                }
            }
        }


        private void toggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            ControlChangeValueHandler();
        }

        private void toggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            ControlChangeValueHandler();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            startKB_Controller_Timer();
        }



        #region handle hotkey inputs
        //var for hotkey input
        private DispatcherTimer gamepadTimer = new DispatcherTimer(DispatcherPriority.Render);
        private DispatcherTimer fiveSecondTimeOutTimer = new DispatcherTimer();
        private ushort controllerButtons = 0;
        private DateTime gamepadTimerTickCounter;
        private ushort currentGamepad = 0;
        private ushort previousGamepad = 0;


        private string hotKeyType = "";
        private string hotKeyText = "";
        private ushort hotKeyValue = 0;

        #region gamepad timer routines
        private void gamepad_Tick(object sender, EventArgs e)
        {
            //get gamepad from mainwindow
            MainWindow mainWindow = Local_Object.Instance.GetMainWindow();
            //get the state and turn it into a ushort
            currentGamepad = ((ushort)mainWindow.controllerInput.controller.GetState().Gamepad.Buttons);



            //start timer to read controller for inputs
            if (currentGamepad >= previousGamepad)
            {
                if (currentGamepad > 0)
                {
                    controllerButtons = currentGamepad;
                }

            }
            else
            {
                if (currentGamepad < previousGamepad && previousGamepad != 4096)
                {
                    //as soon as the button combo is LESS than the previous gamepad button AND isn't the A button (value of 4096) we know they have finished pressing all the buttons and we can now figure out what the combo is
                    

                    stopKB_Controller_Timer(false);
                    return;
                }

            }
            previousGamepad = currentGamepad;




        }

        private string convertControllerUshortToString(string hotkey)
        {
            string gamepadCombo = "";
            Gamepad gamepad = new Gamepad();


            ushort uShorthotkey;

            if (ushort.TryParse(hotkey, out uShorthotkey))
            {
                gamepad.Buttons = (GamepadButtonFlags)(uShorthotkey);


                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftShoulder)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "LB"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.RightShoulder)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "RB"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftThumb)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "LStick"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.RightThumb)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "RStick"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.Start)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "Start"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.Back)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "Back"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.A)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "A"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.B)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "B"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.X)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "X"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.Y)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "Y"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "DPadUp"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "DPadDown"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "DPadLeft"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight)) { gamepadCombo = makeGamepadButtonString(gamepadCombo, "DPadRight"); }

            }


            return gamepadCombo;

        }
        private string makeGamepadButtonString(string currentValue, string addValue)
        {
            //routine to make string for 
            if (currentValue == "")
            {
                return addValue;
            }
            else
            {
                return currentValue + "+" + addValue;
            }

        }

        private void startKB_Controller_Timer()
        {
            //set variables to stop controller / kb input from being registered
            
            //do controller first
            MainWindow mainWindow = Local_Object.Instance.GetMainWindow();
            mainWindow.controllerInput.publicSuspendEventsForHotKeyProgrammingt = true;


            //set gamepad states to 0
            currentGamepad = 0;
            previousGamepad = 0;


            //MouseKeyHook.programmingKeystroke = true;

            //set up separate timer to get controller input
            gamepadTimer.Tick += gamepad_Tick;
            gamepadTimer.Interval = TimeSpan.FromMilliseconds(70);
            gamepadTimer.Start();

            // setup 5 second timeout timer
            fiveSecondTimeOutTimer.Tick += fiveSecondTimeOutTimer_Tick;
            fiveSecondTimeOutTimer.Interval = new TimeSpan(0, 0, 5);

            //subscribe to event that tracks keyboard presses for a completed hotkey
            //FIX THIS HAVENT ADDED KB EVENT YET: MouseKeyHook.keyboardEvents.keyboardStringPress += handleKeyboardStringPress;

            //set the control to the ... signifying its listening
            textBlockHotKey.Text = "...";

            //start the 5 second timeout timer (after 5 seconds it stops and reverts the value back if no controller/kb input detected)
            fiveSecondTimeOutTimer.Start();

        }
        private void stopKB_Controller_Timer(bool timedOut)
        {
            //set variables to allow the normal function of keyboard/controller input
            //do controller first
            MainWindow mainWindow = Local_Object.Instance.GetMainWindow();
            mainWindow.controllerInput.publicSuspendEventsForHotKeyProgrammingt = false;

            //now keyboard
            // FIX THIS NO KEYBOARD IN YET: MouseKeyHook.programmingKeystroke = false;

            //stop the specific timer for tracking controller hotkey inputs AND unsubscribe to prevent threads from staying open after closure
            gamepadTimer.Stop();
            gamepadTimer.Tick -= gamepad_Tick;
            //MouseKeyHook.keyboardEvents.keyboardStringPress -= handleKeyboardStringPress;
            if (timedOut)
            {
                textBlockHotKey.Text = hotKeyText;
            }
        }



        private void handleKeyboardStringPress(object sender, EventArgs args)
        {
            //control.Content = (string)sender;
            //Global_Variables.hotKeys.editingHotkey.Hotkey = (string)sender;
            //Global_Variables.hotKeys.editingHotkey.Type = "Keyboard";
            stopKB_Controller_Timer(false);
        }



        #endregion
        #region keyboard timer
        private void fiveSecondTimeOutTimer_Tick(object sender, EventArgs e)
        {
            textBlockHotKey.Text = hotKeyText;
            stopKB_Controller_Timer(true);
        }


        #endregion


        #endregion

    }


}
