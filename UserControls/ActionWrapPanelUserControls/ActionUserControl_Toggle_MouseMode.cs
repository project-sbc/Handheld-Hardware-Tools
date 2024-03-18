using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.MouseMode;
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
    public partial class ActionUserControl_Toggle_MouseMode : ActionUserControl_DefaultHandler
    {
        public ActionUserControl_Toggle_MouseMode(Classes.Actions.Action newAction) : base(newAction)
        {
        }

        public override async void ConfigureControls(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled, Grid iconGrid, FontAwesome.WPF.FontAwesome faIcon)
        {
           
            textBlock2.Text = Application.Current.Resources["Action_Toggle_MouseMode"].ToString();

            symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.KeyboardMouse16;


            MouseMode mm = Local_Object.Instance.GetMainWindowMouseMode();
            if (mm != null)
            {
                textBlock.Text = Application.Current.Resources["ActionUserControl_Enabled"].ToString();

            }
            else
            {
                textBlock.Text = Application.Current.Resources["ActionUserControl_Disabled"].ToString();

            }



        }

        
        public override void ButtonPress(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled) 
        {
            action.OnActivate();

            //update label depending on status
            MouseMode mm = Local_Object.Instance.GetMainWindowMouseMode();
            if (mm != null)
            {
                textBlock.Text = Application.Current.Resources["ActionUserControl_Enabled"].ToString();

            }
            else
            {
                textBlock.Text = Application.Current.Resources["ActionUserControl_Disabled"].ToString();

            }
        }


    }
}
