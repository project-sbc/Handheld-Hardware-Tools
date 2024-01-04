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
    public partial class ActionUserControl_Toggle_QAM : ActionUserControl_DefaultHandler
    {
        public ActionUserControl_Toggle_QAM(Classes.Actions.Action newAction) : base(newAction)
        {
        }

        public override async void ConfigureControls(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled)
        {
           
            textBlock2.Text = Application.Current.Resources["Action_Toggle_QAM"].ToString();

            symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.WindowHeaderVertical20;
            textBlock.Text = Application.Current.Resources["ActionUserControl_Toggle"].ToString();

           

        }

        
        public override void ButtonPress(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled) 
        {
            action.OnActivate();
          
        
        }


    }
}
