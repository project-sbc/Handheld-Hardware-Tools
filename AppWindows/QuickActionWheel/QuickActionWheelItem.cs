using Everything_Handhelds_Tool.AppWindows.QuickActionWheel.QuickActionWheelItem_Actions;
using Everything_Handhelds_Tool.Classes.Actions.ActionClass;
using FontAwesome.WPF;
using RadialMenu.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Everything_Handhelds_Tool.AppWindows.QuickActionWheel
{
    public class QuickActionWheelItem
    {
        public bool hasSubMenu;

        public virtual void SetUpIconsAndTextblock(SymbolIcon symbolIcon, FontAwesome.WPF.FontAwesome fontAwesomeIcon, TextBlock textBlock, out List<RadialMenuItem> subRadialMenuItems, RadialMenuCentralItem radialMenuCentralItem,  Classes.Actions.Action action, out string displayName)
        {
            subRadialMenuItems = SetUpSubRadialMenu(action);

            SetUpUserControlIcons(symbolIcon, fontAwesomeIcon);
            SetUpUserControlTextblock(textBlock, action);

            displayName = Application.Current.Resources["Action_" + action.actionName].ToString();

        } 

        //this can be overridden at the individual class level, default returns null
        public virtual List<RadialMenuItem> SetUpSubRadialMenu(Classes.Actions.Action action) { return null; }

        //
        public virtual void SetUpUserControlIcons(SymbolIcon symbolIcon, FontAwesome.WPF.FontAwesome fontAwesomeIcon) { }
        public virtual void SetUpUserControlTextblock(TextBlock textBlock, Classes.Actions.Action action) { }
        public virtual void OnClickEvent() { }
        public virtual void SetUpRadialCentralMenuItem(Classes.Actions.Action action, out RadialMenuCentralItem radialMenuCentralItem) { radialMenuCentralItem = null; }
    }

   public static class QuickActionWheelItemConverter
    {
        public static QuickActionWheelItem ActionToQuickActionWheelItemConverter(Classes.Actions.Action action)
        {
            switch (action)
            {
                case Cycle_TDP:
                    return new QuickActionWheelItem_Cycle_TDP();
                    break;
            }
            return null;
        }
    }

}
