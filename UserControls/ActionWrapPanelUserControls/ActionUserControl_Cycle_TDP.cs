using Everything_Handhelds_Tool.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Everything_Handhelds_Tool.UserControls.ActionWrapPanelUserControls
{
    public partial class ActionUserControl_Cycle_TDP : ActionUserControl_DefaultHandler
    {
        public ActionUserControl_Cycle_TDP(Classes.Actions.Action newAction) : base(newAction)
        {
        }

        public override void ConfigureControls(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled)
        {
            symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.DeveloperBoardLightning20;
            textBlock2.Text = TDP_Management.Instance.ReadAndReturnSustainedTDP().ToString() + " W";
            textBlock.Text = Application.Current.Resources["Action_Cycle_TDP"].ToString();
        }
        public override void ButtonPress(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled) 
        {
            action.OnActivate();
            textBlock2.Text = TDP_Management.Instance.ReadAndReturnSustainedTDP().ToString() + " W";
        }


    }
}
