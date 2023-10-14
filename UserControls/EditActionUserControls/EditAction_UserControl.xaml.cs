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

namespace Everything_Handhelds_Tool.UserControls.EditActionUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class EditAction_UserControl : ControllerUserControl
    {
        public Everything_Handhelds_Tool.Classes.Actions.Action action = null;
      
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

        private void ConfigureTextAndSymbol()
        {
            actionName.Text = Application.Current.Resources["Action_" + action.actionName].ToString();

            if (action.hotkeyType == "")
            {
                symbolIconHotKey.Visibility = Visibility.Collapsed;
                actionHotKey.Visibility = Visibility.Collapsed;
            }

            if (action.displayInActionPanel)
            {
                actionPanelVisible.Text = Application.Current.Resources["ActionUserControl_DisplayInPanel"].ToString();
            }
            else
            {
                actionPanelVisible.Text = Application.Current.Resources["ActionUserControl_HiddenInPanel"].ToString();
            }

            switch(action.actionName)
            {
                case "Change_TDP":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.DeveloperBoardLightning20;
                   
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            
   
        }
    }
}
