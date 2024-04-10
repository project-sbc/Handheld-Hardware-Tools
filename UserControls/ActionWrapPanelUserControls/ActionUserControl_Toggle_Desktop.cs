using Handheld_Hardware_Tools.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Handheld_Hardware_Tools.UserControls.ActionWrapPanelUserControls
{
    public partial class ActionUserControl_Toggle_Desktop : ActionUserControl_DefaultHandler
    {
        public ActionUserControl_Toggle_Desktop(Classes.Actions.Action newAction) : base(newAction)
        {
        }

        public override async void ConfigureControls(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled, Grid iconGrid, FontAwesome.WPF.FontAwesome faIcon)
        {
           
            textBlock2.Text = Application.Current.Resources["Action_Toggle_Desktop"].ToString();

            symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.TabDesktopBottom24;
            textBlock.Text = "";

            iconGrid.HorizontalAlignment = HorizontalAlignment.Center;

        }

        
        public override void ButtonPress(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled) 
        {
            action.OnActivate();
          
        
        }


    }
}
