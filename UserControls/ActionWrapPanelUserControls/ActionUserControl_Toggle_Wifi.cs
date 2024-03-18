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
    public partial class ActionUserControl_Toggle_Wifi : ActionUserControl_DefaultHandler
    {
        public ActionUserControl_Toggle_Wifi(Classes.Actions.Action newAction) : base(newAction)
        {
        }

        public override async void ConfigureControls(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled, Grid iconGrid, FontAwesome.WPF.FontAwesome faIcon)
        {
           
            textBlock2.Text = Application.Current.Resources["Action_Toggle_Wifi"].ToString();

            bool wifiOn = await Task.Run(() => Wifi_BT_Management.Instance.GetWifiIsEnabledAsync()); 
   

            if (wifiOn == false)
            {
                symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.WifiOff24; 
                textBlock.Text = Application.Current.Resources["ActionUserControl_Disabled"].ToString();
            }
            else
            {
                symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Wifi124;
                textBlock.Text = Application.Current.Resources["ActionUserControl_Enabled"].ToString();
            }

        }

        
        public override void ButtonPress(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled) 
        {
            action.OnActivate();
            if (symbolIcon.Symbol == Wpf.Ui.Common.SymbolRegular.WifiOff24)
            {
                symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Wifi124;
                textBlock.Text = Application.Current.Resources["ActionUserControl_Enabled"].ToString();
            }
            else
            {
                symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.WifiOff24;
                textBlock.Text = Application.Current.Resources["ActionUserControl_Disabled"].ToString();
            }

        
        }


    }
}
