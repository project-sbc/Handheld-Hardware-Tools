using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Actions;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Handheld_Hardware_Tools.Pages;
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

namespace Handheld_Hardware_Tools.UserControls.EditActionUserControls
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
            General_Functions.ChangeControllerInstructionPage("ChangeToggleBack");
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
                    textBlockHotKey.Text = General_Functions.convertControllerUshortToString(hotKeyText);
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


            QuickAccessMenu mainWindow = Local_Object.Instance.GetQAMWindow();

            if (mainWindow.frame.Content is EditActionPage)
            {
                EditActionPage editActionPage = mainWindow.frame.Content as EditActionPage;
                if (editActionPage != null)
                {
                    if (editActionPage.action != null)
                    {
                        if (isToggled)
                        {
                            //i had to put in fancy logic to make sure the hot key dictionaries in controller and keyboard events would be updated appropriately
                            //depending on the various scenarios
                            //lookback reminder: this part handles if someone makes a controller hotkey a keyboard one or vice versa, in that case
                            //BOTH dictionaries need to be updated, one to remove and one to add
                            if (hotKeyType == "Controller")
                            {
                                if (editActionPage.action.hotkeyType == "Keyboard")
                                {
                                    editActionPage.updateKBHotKeyDictionary = true;
                                }
                                if (editActionPage.action.hotKey != hotKeyValue.ToString() || editActionPage.action.hotkeyType != hotKeyType)
                                {
                                    editActionPage.updateControllerHotKeyDictionary = true;
                                }

                                editActionPage.action.hotKey = hotKeyValue.ToString();
                                editActionPage.action.hotkeyType = hotKeyType;
                            }
                            if (hotKeyType == "Keyboard")
                            {
                                if (editActionPage.action.hotkeyType == "Controller")
                                {
                                    editActionPage.updateControllerHotKeyDictionary = true;
                                }
                                if (editActionPage.action.hotKey != hotKeyText || editActionPage.action.hotkeyType != hotKeyType)
                                {
                                    editActionPage.updateKBHotKeyDictionary = true;
                                }

                                editActionPage.action.hotKey = hotKeyText;
                                editActionPage.action.hotkeyType = hotKeyType;
                            }
                        }
                        else
                        {
                            //these are the triggers for updating the dictionaries with shortcuts in them
                            if (editActionPage.action.hotkeyType == "Controller")
                            {
                                editActionPage.updateControllerHotKeyDictionary = true;
                            }
                            if (editActionPage.action.hotkeyType == "Keyboard")
                            {
                                editActionPage.updateKBHotKeyDictionary = true;
                            }
                            editActionPage.action.hotKey = "";
                            editActionPage.action.hotkeyType = "";
                        }

                    }

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

        private Dictionary<ushort, Classes.Actions.Action> controllerHotKeyDictionary;
        private void button_Click(object sender, RoutedEventArgs e)
        {
            //get dictionaries to make sure hotkeys are duplicated
            ActionList actions = (ActionList)XML_Management.Instance.LoadXML("ActionList");
            controllerHotKeyDictionary = actions.ReturnControllerActionHotKeyList();
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
            QuickAccessMenu mainWindow = Local_Object.Instance.GetQAMWindow();
            //get the state and turn it into a ushort
            if (mainWindow.controllerInput.controller == null)
            {
                return;
            }
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
                    //IM ADDING ONE MORE STEP HERE: does this hot key combo already exist?
                    //

                    //do this if the hot key doesnt exist by checking if ! in dictionary is true
                    if (!controllerHotKeyDictionary.ContainsKey(controllerButtons))
                    {
                        //as soon as the button combo is LESS than the previous gamepad button AND isn't the A button (value of 4096) we know they have finished pressing all the buttons and we can now figure out what the combo is

                        //these three variables will get passed to the action
                        hotKeyValue = controllerButtons;
                        hotKeyText = General_Functions.convertControllerUshortToString(controllerButtons.ToString());
                        hotKeyType = "Controller";

                        //set text block and icon
                        textBlockHotKey.Text = hotKeyText;
                        iconHotKeyType.Symbol = Wpf.Ui.Common.SymbolRegular.XboxController24;

                        stopKB_Controller_Timer(false);
                        return;
                    }
                    else
                    {
                        //do this if the hotkey combo already exsists (display error)
                        System.Windows.MessageBox.Show("Hot key already exists. NOTE TO SELF: make disappearing error message");
                    }

                   
                }

            }
            previousGamepad = currentGamepad;


        }

      
      

        private void startKB_Controller_Timer()
        {
            //set variables to stop controller / kb input from being registered
            
            //do controller first
            QuickAccessMenu mainWindow = Local_Object.Instance.GetQAMWindow();
            mainWindow.controllerInput.suspendEventsForHotKeyProgramming = true;


            //set gamepad states to 0
            currentGamepad = 0;
            previousGamepad = 0;


            mainWindow.mouseKeyHook.programmingKeystroke = true;

            //set up separate timer to get controller input
            gamepadTimer.Tick += gamepad_Tick;
            gamepadTimer.Interval = TimeSpan.FromMilliseconds(70);
            gamepadTimer.Start();

            // setup 5 second timeout timer
            fiveSecondTimeOutTimer.Tick += fiveSecondTimeOutTimer_Tick;
            fiveSecondTimeOutTimer.Interval = new TimeSpan(0, 0, 5);

            //subscribe to event that tracks keyboard presses for a completed hotkey
            //FIX THIS HAVENT ADDED KB EVENT YET:
            mainWindow.mouseKeyHook.keyboardEvents.keyboardStringPress += handleKeyboardStringPress;

            //set the control to the ... signifying its listening
            textBlockHotKey.Text = "...";

            //start the 5 second timeout timer (after 5 seconds it stops and reverts the value back if no controller/kb input detected)
            fiveSecondTimeOutTimer.Start();

        }
        private void stopKB_Controller_Timer(bool timedOut)
        {
            //stop timeout timer
            fiveSecondTimeOutTimer.Stop();

            //set variables to allow the normal function of keyboard/controller input
            //do controller first
            QuickAccessMenu mainWindow = Local_Object.Instance.GetQAMWindow();
            mainWindow.controllerInput.suspendEventsForHotKeyProgramming = false;

            //now keyboard
            // FIX THIS NO KEYBOARD IN YET:
            mainWindow.mouseKeyHook.programmingKeystroke = false;

            //stop the specific timer for tracking controller hotkey inputs AND unsubscribe to prevent threads from staying open after closure
            gamepadTimer.Stop();
            gamepadTimer.Tick -= gamepad_Tick;
            mainWindow.mouseKeyHook.keyboardEvents.keyboardStringPress -= handleKeyboardStringPress;
            if (timedOut)
            {
                textBlockHotKey.Text = hotKeyText;
            }
            else
            {
                //if it wasn't timed out then call the update function
                ControlChangeValueHandler();
            }
        }



        private void handleKeyboardStringPress(object sender, EventArgs args)
        {
            //these three variables will get passed to the action

            hotKeyText = (string)sender;
            hotKeyType = "Keyboard";

            //set text block and icon
            textBlockHotKey.Text = (string)sender;
            iconHotKeyType.Symbol = Wpf.Ui.Common.SymbolRegular.Keyboard24;

            stopKB_Controller_Timer(false);

        }



        #endregion
        #region keyboard timer
        private void fiveSecondTimeOutTimer_Tick(object sender, EventArgs e)
        {
            
            stopKB_Controller_Timer(true);
        }
    

        #endregion


        #endregion

    }


}
