﻿using Handheld_Hardware_Tools.Classes;
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
    public partial class ActionUserControl_Toggle_IntegerScaling : ActionUserControl_DefaultHandler
    {
        public ActionUserControl_Toggle_IntegerScaling(Classes.Actions.Action newAction) : base(newAction)
        {
        }

        public override void ConfigureControls(TextBlock textBlock, TextBlock textBlock2, SymbolIcon symbolIcon, SymbolIcon symbolIconDisabled, Grid iconGrid, FontAwesome.WPF.FontAwesome faIcon)
        {
            symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Blur20;
            textBlock2.Text = Application.Current.Resources["Action_Toggle_IntegerScaling"].ToString();



            if (ADLX_Management.HasIntegerScalingSupport1() || ADLX_Management.HasIntegerScalingSupport2())
            {
                if (ADLX_Management.IsIntegerScalingEnabled1() || ADLX_Management.IsIntegerScalingEnabled2())
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
            else
            {
                

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
