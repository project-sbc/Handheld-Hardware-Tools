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
    public partial class ActionUserControl_Toggle_FreeSync : ActionUserControl_DefaultHandler
    {
        public ActionUserControl_Toggle_FreeSync(Classes.Actions.Action newAction) : base(newAction)
        {
        }

        public override void ConfigureControls(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled, Grid iconGrid, FontAwesome.WPF.FontAwesome faIcon)
        {
            symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.DesktopSync24;
            textBlock2.Text = Application.Current.Resources["Action_Toggle_FreeSync"].ToString();



            if (ADLX_Management.HasFreeSyncSupport1())
            {
                if (ADLX_Management.IsFreeSyncEnabled1())
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
            if (ADLX_Management.HasFreeSyncSupport2())
            {
                if (ADLX_Management.IsFreeSyncEnabled2())
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
