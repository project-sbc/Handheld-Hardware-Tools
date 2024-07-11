using Handheld_Hardware_Tools.AppWindows.QuickActionWheel.QuickActionWheelItem_Actions;
using Handheld_Hardware_Tools.Classes.Actions.ActionClass;
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

namespace Handheld_Hardware_Tools.AppWindows.QuickActionWheel
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
                case Cycle_EPP:
                    return new QuickActionWheelItem_Cycle_EPP();

                case Change_TDP:
                    return new QuickActionWheelItem_Change_TDP();
                case Toggle_OSK:
                    return new QuickActionWheelItem_Toggle_OSK();

                case Toggle_Controller:
                    return new QuickActionWheelItem_Toggle_Controller();
                case Toggle_IntegerScaling:
                    return new QuickActionWheelItem_Toggle_IntegerScaling();
                case Toggle_QAM:
                    return new QuickActionWheelItem_Toggle_QAM();

                case Toggle_WindowManager:
                    return new QuickActionWheelItem_Toggle_WindowManager();
                case Toggle_Guide:
                    return new QuickActionWheelItem_Toggle_Guide();

                case Toggle_MouseMode:
                    return new QuickActionWheelItem_Toggle_MouseMode();
                case Cycle_Volume:
                    return new QuickActionWheelItem_Cycle_Volume();

                case Toggle_Desktop:
                    return new QuickActionWheelItem_Toggle_Desktop();
                case Open_SteamBP:
                    return new QuickActionWheelItem_Open_SteamBP();
                case Toggle_Wifi:
                    return new QuickActionWheelItem_Toggle_Wifi();
                case Toggle_BT:
                    return new QuickActionWheelItem_Toggle_BT();
                case Toggle_VolumeMute:
                    return new QuickActionWheelItem_Toggle_VolumeMute();

            }
            return null;
        }
    }

}
