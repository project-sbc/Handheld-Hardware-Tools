﻿using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Handheld_Hardware_Tools.Pages;
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

namespace Handheld_Hardware_Tools.UserControls.ActionOverviewUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class ActionOverview_UserControl : ControllerUserControl
    {
        public Handheld_Hardware_Tools.Classes.Actions.Action action = null;
        public bool isMovingUserControlInStackPanel = false;
        public ActionOverview_UserControl(Handheld_Hardware_Tools.Classes.Actions.Action newAction)
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

            //display in radial menu textblock
            SetIsActionDisplayedInRadialMenuTextBlock();

            //set hotkey icon and text
            SetHotKeyTextBlockAndIcon();

            //set main icon and arguments
            SetMainIconAndArguments();


            SetNotificationsTextBlock();
        }
        private void SetActionNameTextBlock()
        {
            actionName.Text = Application.Current.Resources["Action_" + action.actionName].ToString();
        }
        private void SetIsActionDisplayedInRadialMenuTextBlock()
        {
            if (action.displayInRadialMenu)
            {
                actionRadialMenuVisible.Text = Application.Current.Resources["ActionUserControl_DisplayInRadialMenu"].ToString();
            }
            else
            {
                actionRadialMenuVisible.Text = Application.Current.Resources["ActionUserControl_HiddenInRadialMenu"].ToString();
            }
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
        private void SetNotificationsTextBlock()
        {
            if (action.displayNotification)
            {
                actionNotification.Text = Application.Current.Resources["ActionUserControl_DisplayNotifications"].ToString();
            }
            else
            {
                actionNotification.Text = Application.Current.Resources["ActionUserControl_HideNotifications"].ToString();
            }

        }

        private void SetHotKeyTextBlockAndIcon()
        {
            if (action.hotkeyType == "")
            {
                
                symbolIconHotKeyCrossout.Visibility = Visibility.Visible;
                actionHotKey.Text = Application.Current.Resources["Usercontrol_NoHotkey"].ToString();
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
                case "Cycle_EPP":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.DeveloperBoardLightning20;
                    if (action.parameters != null)
                    {
                        if (action.parameters.Count > 0)
                        {
                            string totalEPPString = "";
                            foreach (string epp in action.parameters)
                            {
                                totalEPPString = totalEPPString + epp + " %; ";
                            }
                            actionArguments.Text = totalEPPString;
                        }

                    }


                    break;
               
                case "Toggle_WifiAP":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Router24;

                    break;
                case "Toggle_Guide":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.BookStar20;

                    break;
                case "Toggle_Desktop":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.TabDesktopBottom24;

                    break;
                case "Toggle_IntegerScaling":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Blur20;

                    break;
                case "Toggle_FreeSync":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.DesktopSync24;

                    break;
                case "Toggle_MicrophoneMute":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Mic20;

                    break;
                case "Toggle_VolumeMute":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.SpeakerMute24;

                    break;
                case "Toggle_Controller":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.XboxController24;

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
                case "Toggle_QuickActionWheel":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.ArrowSyncCircle20;

                    break;
                case "Open_SteamBP":
                    symbolIcon.Visibility = Visibility.Collapsed;
                    faIcon.Visibility = Visibility.Visible;
                    faIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Steam;

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
            //hide arguments if there are none in the action
            if (action.parameters != null)
            {
                if (action.parameters.Count == 0)
                {
                    viewboxArguments.Visibility = Visibility.Collapsed;
                }
            }
            else {  viewboxArguments.Visibility = Visibility.Collapsed;}

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

        private void SendCommandToEditActionOverviewPage(string strAction)
        {
            QuickAccessMenu mainWindow = (QuickAccessMenu)Application.Current.MainWindow;

            EditActionOverviewPage editActionOverviewPage = Local_Object.Instance.GetMainWindowFramePage() as EditActionOverviewPage;

            if (editActionOverviewPage != null)
            {
                //this passes an argument to make sure the dictionary for controller/kb hotkeys gets updated
                if (strAction == "Delete" && action.hotkeyType != "")
                {
                    editActionOverviewPage.HandleUserControlInputs(this, strAction, action.hotkeyType);
                }
                else
                {
                    editActionOverviewPage.HandleUserControlInputs(this, strAction);
                }
               
                
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
