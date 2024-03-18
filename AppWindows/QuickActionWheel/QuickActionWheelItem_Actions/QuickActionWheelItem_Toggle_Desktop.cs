using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Profiles.ProfileActions.ProfileActionClass;
using RadialMenu.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Everything_Handhelds_Tool.AppWindows.QuickActionWheel.QuickActionWheelItem_Actions
{
    public class QuickActionWheelItem_Toggle_Desktop : QuickActionWheelItem
    {
        public bool hasSubMenu = false;

        public override void SetUpIconsAndTextblock(SymbolIcon symbolIcon, FontAwesome.WPF.FontAwesome fontAwesomeIcon, TextBlock textBlock, out List<RadialMenuItem> subRadialMenuItems, RadialMenuCentralItem radialMenuCentralItem, Classes.Actions.Action action, out string displayName)
        {
            base.SetUpIconsAndTextblock(symbolIcon, fontAwesomeIcon, textBlock, out subRadialMenuItems, radialMenuCentralItem, action, out displayName);
        }
                      

        public override void SetUpUserControlIcons(SymbolIcon symbolIcon, FontAwesome.WPF.FontAwesome fontAwesomeIcon) 
        {
            fontAwesomeIcon.Visibility = Visibility.Collapsed;
            symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.TabDesktopBottom24;
        }
        public override void SetUpUserControlTextblock(TextBlock textBlock, Classes.Actions.Action action) 
        {
            textBlock.Text = Application.Current.Resources["Action_" + action.actionName].ToString();
    }
        public override void SetUpRadialCentralMenuItem(Classes.Actions.Action action, out RadialMenuCentralItem radialMenuCentralItem)
        {
            radialMenuCentralItem = new RadialMenuCentralItem() { Content = Application.Current.Resources["Action_" + action.actionName].ToString() };
           
       
        }

        

    }
}
