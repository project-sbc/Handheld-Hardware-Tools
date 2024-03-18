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
    public partial class ActionUserControl_Toggle_BT : ActionUserControl_DefaultHandler
    {
        public ActionUserControl_Toggle_BT(Classes.Actions.Action newAction) : base(newAction)
        {
        }

        public override async void ConfigureControls(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled, Grid iconGrid, FontAwesome.WPF.FontAwesome faIcon)
        {
           
            textBlock2.Text = Application.Current.Resources["Action_Toggle_BT"].ToString();

            bool btOn = await Task.Run(() => Wifi_BT_Management.Instance.GetBTIsEnabledAsync()); 
   

            if (btOn == false)
            {
                symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.BluetoothDisabled24; 
                textBlock.Text = Application.Current.Resources["ActionUserControl_Disabled"].ToString();
            }
            else
            {
                symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Bluetooth24;
                textBlock.Text = Application.Current.Resources["ActionUserControl_Enabled"].ToString();
            }

        }

        
        public override void ButtonPress(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled) 
        {
            action.OnActivate();
            if (symbolIcon.Symbol == Wpf.Ui.Common.SymbolRegular.BluetoothDisabled24)
            {
                symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Bluetooth24;
                textBlock.Text = Application.Current.Resources["ActionUserControl_Enabled"].ToString();
            }
            else
            {
                symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.BluetoothDisabled24;
                textBlock.Text = Application.Current.Resources["ActionUserControl_Disabled"].ToString();
            }

        
        }


    }
}
