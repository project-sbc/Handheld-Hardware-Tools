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
    public partial class ActionUserControl_Toggle_WifiAP : ActionUserControl_DefaultHandler
    {
        public ActionUserControl_Toggle_WifiAP(Classes.Actions.Action newAction) : base(newAction)
        {
        }

        public override void ConfigureControls(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled, Grid iconGrid)
        {
            symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Router24;
            textBlock2.Text = Application.Current.Resources["Action_Toggle_WifiAP"].ToString();



            MainWindow mW = Local_Object.Instance.GetMainWindow();

            if (mW.wifiAP == null)
            {
                symbolIconDisabled.Visibility = Visibility.Visible;
                textBlock.Text = Application.Current.Resources["ActionUserControl_Disabled"].ToString();
            }
            else
            {
                if (mW.wifiAP.Status() == Windows.Devices.WiFiDirect.WiFiDirectAdvertisementPublisherStatus.Started)
                {
                    symbolIconDisabled.Visibility = Visibility.Hidden;
                    textBlock.Text = Application.Current.Resources["ActionUserControl_Enabled"].ToString();
                }

            }

        }
        public override void ButtonPress(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled) 
        {
            action.OnActivate();
            if (symbolIconDisabled.Visibility == Visibility.Visible)
            {
                symbolIconDisabled.Visibility = Visibility.Hidden;
                textBlock.Text = Application.Current.Resources["ActionUserControl_Enabled"].ToString();
            }
            else
            {
                symbolIconDisabled.Visibility = Visibility.Visible;
                textBlock.Text = Application.Current.Resources["ActionUserControl_Disabled"].ToString();
            }
        }


    }
}
