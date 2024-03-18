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
    public partial class ActionUserControl_Open_SteamBP : ActionUserControl_DefaultHandler
    {
        public ActionUserControl_Open_SteamBP(Classes.Actions.Action newAction) : base(newAction)
        {
        }

        public override void ConfigureControls(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled, Grid iconGrid, FontAwesome.WPF.FontAwesome faIcon)
        {
            symbolIcon.Visibility = Visibility.Collapsed;
            faIcon.Visibility = Visibility.Visible;
            faIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Steam;
            textBlock2.Text = Application.Current.Resources["Action_Toggle_Guide"].ToString();

            textBlock.Text = Application.Current.Resources["ActionUserControl_Enabled"].ToString();

          

        }
        public override void ButtonPress(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled) 
        {
            action.OnActivate();
           
        }


    }
}
