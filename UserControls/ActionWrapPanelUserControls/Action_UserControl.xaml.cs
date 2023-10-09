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
            switch(action.actionName)
            {
                case "Change_TDP":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.DeveloperBoardLightning20;
                    textBlock.Text = "Change TDP";
                    textBlock2.Text = action.arguments[0].ToString();
                    break;
                case "Cycle_TDP":
                    symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.DeveloperBoardLightning20;
                    break;
                case "":

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
            }
   
        }
    }
}
