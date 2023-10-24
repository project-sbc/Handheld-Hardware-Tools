using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
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

namespace Everything_Handhelds_Tool.UserControls.ActionWrapPanelUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class Action_UserControl : ControllerUserControl
    {
        public Everything_Handhelds_Tool.Classes.Actions.Action action = null;
      
        public Action_UserControl(Everything_Handhelds_Tool.Classes.Actions.Action newAction)
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = button;

            action = newAction;

            //Configure text and symbol to match the action
            ConfigureTextAndSymbol();
        
        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("SelectBack");
        }

        private void ConfigureTextAndSymbol()
        {
         
            switch (action.actionName)
            {
                case "Change_TDP":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.DeveloperBoardLightning20;
                    textBlock.Text = TDP_Management.Instance.ReadAndReturnSustainedTDP().ToString() + " W";
                    //override the first text assignment above, most of the
                    if (action.parameters[0].ToString().Contains("-"))
                    {
                        textBlock.Text = Application.Current.Resources["Action_" + action.actionName].ToString() + action.parameters[0].ToString() + " W";
                    }
                    else
                    {
                        textBlock.Text = Application.Current.Resources["Action_Change_TDP"].ToString() + "+" + action.parameters[0].ToString() + " W";
                    }

                    break;
                case "Cycle_TDP":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.DeveloperBoardLightning20;
                    textBlock2.Text = TDP_Management.Instance.ReadAndReturnSustainedTDP().ToString() + " W";
                    textBlock.Text = Application.Current.Resources["Action_Cycle_TDP"].ToString();
                    break;
                case "Toggle_WifiAP":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Router24;
                    textBlock2.Text = "Toggle Wifi AP";
                    if (Wifi_Management.Instance.IsWifiRunning()) 
                    { 
                        symbolIconDisabled.Visibility = Visibility.Hidden; 
                        textBlock.Text = Application.Current.Resources["ActionUserControl_Enabled"].ToString();
                    }
                    else
                    {
                        symbolIconDisabled.Visibility = Visibility.Visible;
                        textBlock.Text = Application.Current.Resources["ActionUserControl_Disabled"].ToString();
                    }
                    break;
                default:
                    break;
            }

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (action != null)
            {
                action.OnActivate();

                switch (action.actionName)
                {
                    case "Change_TDP":
                        textBlock2.Text = TDP_Management.Instance.ReadAndReturnSustainedTDP().ToString() + " W";
                        break;
                    case "Cycle_TDP":
                        textBlock2.Text = TDP_Management.Instance.ReadAndReturnSustainedTDP().ToString() + " W";
                        break;
                    case "Toggle_WifiAP":
                        if (symbolIconDisabled.Visibility == Visibility.Visible)
                        {
                            symbolIconDisabled.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            symbolIconDisabled.Visibility = Visibility.Visible;
                        }
                        
                        break;
                    default:
                        break;
                }

            }
   
        }
    }
}
