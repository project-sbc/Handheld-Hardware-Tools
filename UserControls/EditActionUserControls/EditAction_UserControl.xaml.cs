using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Everything_Handhelds_Tool.UserControls.EditActionUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class EditAction_UserControl : ControllerUserControl
    {
        public Everything_Handhelds_Tool.Classes.Actions.Action action = null;
        public bool isMovingUserControlInStackPanel = false;
        public EditAction_UserControl(Everything_Handhelds_Tool.Classes.Actions.Action newAction)
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = null;

            action = newAction;

            //Configure text and symbol to match the action
            ConfigureTextAndSymbol();
        
        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("SelectBack");
        }

        #region set textblocks and icons
        private void ConfigureTextAndSymbol()
        {
            //4 textblocks and a couple icons to set
            SetActionNameTextBlock();

            //display in panel textblock
            SetIsActionDisplayedInPanelTextBlock();

            //set hotkey icon and text
            SetHotKeyTextBlockAndIcon();

            //set main icon and arguments
            SetMainIconAndArguments();

        }
        private void SetActionNameTextBlock()
        {
            actionName.Text = Application.Current.Resources["Action_" + action.actionName].ToString();
        }
        private void SetIsActionDisplayedInPanelTextBlock()
        {
            if (action.displayInActionPanel)
            {
                actionPanelVisible.Text = Application.Current.Resources["ActionUserControl_DisplayInPanel"].ToString();
            }
            else
            {
                actionPanelVisible.Text = Application.Current.Resources["ActionUserControl_HiddenInPanel"].ToString();
            }
        }
        private void SetHotKeyTextBlockAndIcon()
        {
            if (action.hotkeyType == "")
            {
                symbolIconHotKey.Visibility = Visibility.Collapsed;
                actionHotKey.Visibility = Visibility.Collapsed;
            }
            else
            {
                //only set if keyboard, default icon is controller so no action needed if action is controller based
                if (action.hotkeyType == "Keyboard")
                {
                    symbolIconHotKey.Symbol = Wpf.Ui.Common.SymbolRegular.Keyboard20;
                    actionHotKey.Text = action.hotKey;
                }
                else
                {
                    actionHotKey.Text = ConvertControllerUshortToString(action.hotKey);
                }
            }
        }
        private void SetMainIconAndArguments()
        {
            switch (action.actionName)
            {
                case "Change_TDP":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.DeveloperBoardLightning20;
                    string args = action.arguments[0].ToString();
                    if (!args.Contains("-")) { args = "+" + args; }
                    actionArguments.Text = args + " W";
                    break;
                case "Cycle_TDP":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.DeveloperBoardLightning20;

                    break;
                case "Toggle_WifiAP":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Router24;

                    break;
                default:
                    break;
            }
        }

        #endregion

        #region convert controller ushort to string
        private string ConvertControllerUshortToString(string hotkey)
        {
            string gamepadCombo = "";
            Gamepad gamepad = new Gamepad();


            ushort uShorthotkey;

            if (ushort.TryParse(hotkey, out uShorthotkey))
            {
                gamepad.Buttons = (GamepadButtonFlags)(uShorthotkey);

                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.A)) { gamepadCombo = MakeGamepadButtonString(gamepadCombo, "A"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.B)) { gamepadCombo = MakeGamepadButtonString(gamepadCombo, "B"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.X)) { gamepadCombo = MakeGamepadButtonString(gamepadCombo, "X"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.Y)) { gamepadCombo = MakeGamepadButtonString(gamepadCombo, "Y"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftShoulder)) { gamepadCombo = MakeGamepadButtonString(gamepadCombo, "LB"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.RightShoulder)) { gamepadCombo = MakeGamepadButtonString(gamepadCombo, "RB"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftThumb)) { gamepadCombo = MakeGamepadButtonString(gamepadCombo, "LStick"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.RightThumb)) { gamepadCombo = MakeGamepadButtonString(gamepadCombo, "RStick"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.Start)) { gamepadCombo = MakeGamepadButtonString(gamepadCombo, "Start"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.Back)) { gamepadCombo = MakeGamepadButtonString(gamepadCombo, "Back"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp)) { gamepadCombo = MakeGamepadButtonString(gamepadCombo, "DPadUp"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown)) { gamepadCombo = MakeGamepadButtonString(gamepadCombo, "DPadDown"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft)) { gamepadCombo = MakeGamepadButtonString(gamepadCombo, "DPadLeft"); }
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight)) { gamepadCombo = MakeGamepadButtonString(gamepadCombo, "DPadRight"); }
            }


            return gamepadCombo;

        }
        private string MakeGamepadButtonString(string currentValue, string addValue)
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

        Dictionary<string, ushort> controllerFlagUshortLookup =
  new Dictionary<string, ushort>()
  {
           {"A", 4096},
           {"B", 8192 },
           {"X", 16384 },
           {"Y", 32768 },
           {"LB", 256 },
           {"RB", 512 },
           {"DPadUp", 1},
           {"DPadDown", 2 },
           {"DPadLeft", 4 },
           {"DPadRight", 8},
         {"Start", 16 },
           {"Back", 32 },
           {"LStick", 64 },
            {"RStick", 128 }

  };

        #endregion

        public override void SelectControl()
        {
            if (borderControl != null)
            {
                if (borderControl is Border)
                {
                    Border border = (Border)borderControl;
                    border.BorderBrush = System.Windows.Media.Brushes.White;
                }
                ChangeMainWindowControllerInstructionPage();
                card.IsExpanded = true;
            }
        }

        public override void HandleControllerInput(string action)
        {
            if (action == "B")
            {
                card.IsExpanded = false;
                ReturnControlToPage();
                return;
            }

            if (card.IsExpanded)
            {
                switch (action)
                {

                }
            }
            else
            {
                switch(action)
                {

                }
            }
           

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMoveUp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMoveDown_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
