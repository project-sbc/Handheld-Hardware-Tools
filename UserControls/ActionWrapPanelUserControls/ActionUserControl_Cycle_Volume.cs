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
    public partial class ActionUserControl_Cycle_Volume : ActionUserControl_DefaultHandler
    {
        public ActionUserControl_Cycle_Volume(Classes.Actions.Action newAction) : base(newAction)
        {
        }

        public override void ConfigureControls(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled, Grid iconGrid)
        {
            symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Speaker216;
            textBlock.Text = Volume_Management.Instance.ReadAndReturnVolume().ToString() + " %";
            textBlock2.Text = Application.Current.Resources["Action_Cycle_Volume"].ToString();
        }
        public override void ButtonPress(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled) 
        {
            action.OnActivate();
            textBlock.Text = Volume_Management.Instance.ReadAndReturnVolume().ToString() + " %";
        }


    }
}
