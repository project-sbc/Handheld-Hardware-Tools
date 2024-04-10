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
    public partial class ActionUserControl_Toggle_VolumeMute : ActionUserControl_DefaultHandler
    {
        public ActionUserControl_Toggle_VolumeMute(Classes.Actions.Action newAction) : base(newAction)
        {
        }

        public override void ConfigureControls(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled, Grid iconGrid, FontAwesome.WPF.FontAwesome faIcon)
        {
           
            textBlock2.Text = Application.Current.Resources["Action_Toggle_VolumeMute"].ToString();



            QuickAccessMenu mW = Local_Object.Instance.GetQAMWindow();

            if (Volume_Management.Instance.ReadAndReturnVolumeMute() == true)
            {
                symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.SpeakerMute24; 
                textBlock.Text = Application.Current.Resources["ActionUserControl_Disabled"].ToString();
            }
            else
            {
                symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Speaker216;
                textBlock.Text = Application.Current.Resources["ActionUserControl_Enabled"].ToString();
            }

        }
        public override void ButtonPress(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled) 
        {
            action.OnActivate();
            if (symbolIcon.Symbol == Wpf.Ui.Common.SymbolRegular.SpeakerMute24)
            {
                symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Speaker216;
                textBlock.Text = Application.Current.Resources["ActionUserControl_Enabled"].ToString();
            }
            else
            {
                symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.SpeakerMute24;
                textBlock.Text = Application.Current.Resources["ActionUserControl_Disabled"].ToString();
            }
        }


    }
}
