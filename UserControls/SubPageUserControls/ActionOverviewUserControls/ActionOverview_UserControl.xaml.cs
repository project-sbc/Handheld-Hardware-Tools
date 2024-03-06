using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Pages;
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

namespace Everything_Handhelds_Tool.UserControls.ActionOverviewUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class ActionOverview_UserControl : ControllerUserControl
    {
        public Everything_Handhelds_Tool.Classes.Actions.Action action = null;
        public bool isMovingUserControlInStackPanel = false;
        public ActionOverview_UserControl(Everything_Handhelds_Tool.Classes.Actions.Action newAction)
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
            General_Functions.ChangeControllerInstructionPage("EditDeleteBackMoveUpDown");
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
                    actionHotKey.Text = General_Functions.convertControllerUshortToString(action.hotKey);
                    symbolIconHotKey.Symbol = Wpf.Ui.Common.SymbolRegular.XboxController24;
                }
            }
        }
        private void SetMainIconAndArguments()
        {
            switch (action.actionName)
            {
                case "Change_TDP":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.DeveloperBoardLightning20;
                    if (action.parameters != null)
                    {
                        if (action.parameters.Count > 0)
                        {
                            string args = action.parameters[0].ToString();
                            if (!args.Contains("-")) { args = "+" + args; }
                            actionArguments.Text = args + " W";
                        }

                    }

                    break;
                case "Cycle_TDP":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.DeveloperBoardLightning20;
                    if (action.parameters != null)
                    {
                        if (action.parameters.Count > 0)
                        {
                            string totalTDPString = "";
                            foreach(string tdp in action.parameters)
                            {
                                totalTDPString = totalTDPString + tdp + " W; ";
                            }
                            actionArguments.Text = totalTDPString;
                        }

                    }


                    break;
                case "Toggle_WifiAP":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Router24;

                    break;
                case "Toggle_Desktop":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.TabDesktopBottom24;

                    break;
                case "Toggle_IntegerScaling":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Blur20;

                    break;
                case "Toggle_MicrophoneMute":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Mic20;

                    break;
                case "Toggle_VolumeMute":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.SpeakerMute24;

                    break;
                case "Toggle_Wifi":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Wifi124;

                    break;
                case "Toggle_QAM":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.WindowHeaderVertical20;

                    break;
                case "Toggle_BT":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Bluetooth24;

                    break;
                case "Toggle_WinOSK":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Keyboard24;

                    break;
                case "Toggle_OSK":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Keyboard24;

                    break;
                case "Toggle_MouseMode":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.KeyboardMouse16;

                    break;
                case "Toggle_WindowManager":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Window24;

                    break;
                case "Cycle_Volume":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Speaker216;
                    if (action.parameters != null)
                    {
                        if (action.parameters.Count > 0)
                        {
                            string totalVolString = "";
                            foreach (string vol in action.parameters)
                            {
                                totalVolString = totalVolString + vol + " %; ";
                            }
                            actionArguments.Text = totalVolString;
                        }

                    }
                    break;
                default:
                    MessageBox.Show("NO HANDLER FOR THIS ACTION, ADD IT YOU DUMMY. ActionOverview_UserControl.xaml.cs " + action.actionName);
                    break;
            }
        }

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
                //card.IsExpanded = true;
            }
        }

        public override void HandleControllerInput(string action)
        {
            if (action == "B")
            {
                card.IsExpanded = false;
                ReturnControlToPage();
  
            }
            else
            {
                switch (action)
                {
                    case "DPadUp":
                        SendCommandToEditActionOverviewPage("MoveUp");
                        break;
                    case "DPadDown":
                        SendCommandToEditActionOverviewPage("MoveDown");
                        break;
                    case "X":
                        SendCommandToEditActionOverviewPage("Delete");
                        break;
                    case "A":
                        SendCommandToEditActionOverviewPage("Edit");
                        break;
                }
            }
           
           

        }

        private void SendCommandToEditActionOverviewPage(string action)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            EditActionOverviewPage editActionOverviewPage = Local_Object.Instance.GetMainWindowFramePage() as EditActionOverviewPage;

            if (editActionOverviewPage != null)
            {
                editActionOverviewPage.HandleUserControlInputs(this, action);
            }

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            SendCommandToEditActionOverviewPage("Edit");
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            SendCommandToEditActionOverviewPage("Delete");
        }

        private void btnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            SendCommandToEditActionOverviewPage("MoveUp");
        }

        private void btnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            SendCommandToEditActionOverviewPage("MoveDown");
        }
    }
}
