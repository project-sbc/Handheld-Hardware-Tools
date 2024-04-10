using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Profiles.ProfileActions.ProfileActionClass;
using RadialMenu.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Handheld_Hardware_Tools.AppWindows.QuickActionWheel.QuickActionWheelItem_Actions
{
    public class QuickActionWheelItem_Cycle_TDP : QuickActionWheelItem
    {
        public bool hasSubMenu = true;

        public override void SetUpIconsAndTextblock(SymbolIcon symbolIcon, FontAwesome.WPF.FontAwesome fontAwesomeIcon, TextBlock textBlock, out List<RadialMenuItem> subRadialMenuItems, RadialMenuCentralItem radialMenuCentralItem, Classes.Actions.Action action, out string displayName)
        {
            base.SetUpIconsAndTextblock(symbolIcon, fontAwesomeIcon, textBlock, out subRadialMenuItems, radialMenuCentralItem, action, out displayName);
        }
               

        public override List<RadialMenuItem> SetUpSubRadialMenu(Classes.Actions.Action action)
        {
            List<RadialMenuItem> subRadialMenuItems = new List<RadialMenuItem>();
            foreach (string tdp in action.parameters)
            {
                RadialMenuItem rmi = new RadialMenuItem()
                {
                    Content = new TextBlock() { FontSize = 30, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Text = tdp + " W" },

                };
               
                subRadialMenuItems.Add(rmi);
            }
            return subRadialMenuItems;
        }

        public override void SetUpUserControlIcons(SymbolIcon symbolIcon, FontAwesome.WPF.FontAwesome fontAwesomeIcon) 
        {
            fontAwesomeIcon.Visibility = Visibility.Collapsed;
            symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.DeveloperBoardLightning20;
        }
        public override void SetUpUserControlTextblock(TextBlock textBlock, Classes.Actions.Action action) 
        {
            textBlock.Text = TDP_Management.Instance.ReadAndReturnSustainedTDP().ToString() + " W";
    }
        public override void SetUpRadialCentralMenuItem(Classes.Actions.Action action, out RadialMenuCentralItem radialMenuCentralItem)
        {
            radialMenuCentralItem = new RadialMenuCentralItem() { Content = Application.Current.Resources["Action_" + action.actionName].ToString() };
           
       
        }

        

    }
}
