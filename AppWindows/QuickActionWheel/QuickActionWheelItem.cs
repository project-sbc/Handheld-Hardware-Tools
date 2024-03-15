using Everything_Handhelds_Tool.AppWindows.QuickActionWheel.QuickActionWheelItem_Actions;
using Everything_Handhelds_Tool.Classes.Actions.ActionClass;
using RadialMenu.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everything_Handhelds_Tool.AppWindows.QuickActionWheel
{
    public class QuickActionWheelItem
    {
        public RadialMenuItem mainRadialMenuItem {  get; set; }
        public List<RadialMenuItem> subRadialMenuItems { get; set; }
        public RadialMenuCentralItem subRadialMenuCentralItem { get; set; }

        public Classes.Actions.Action action { get; set; }
        public QuickActionWheelItem(Classes.Actions.Action Action)
        {
            action = Action;
        }
        
    }

   public static class QuickActionWheelItemConverter
    {
        public static QuickActionWheelItem ActionToQuickActionWheelItemConverter(Classes.Actions.Action action)
        {
            switch (action)
            {
                case Cycle_TDP:
                    return new QuickActionWheelItem_Cycle_TDP(action);
                    break;
            }
            return null;
        }
    }

}
