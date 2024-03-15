using RadialMenu.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace Everything_Handhelds_Tool.AppWindows.QuickActionWheel.QuickActionWheelItem_Actions
{
    public class QuickActionWheelItem_Cycle_TDP : QuickActionWheelItem
    {
        public QuickActionWheelItem_Cycle_TDP(Classes.Actions.Action Action) : base(Action)
        {
            CreateMainRadialMenuItem();
            CreateSubMenuItems();
          
        }
        public void CreateMainRadialMenuItem()
        {
            //create radialmenuitem at main level
            mainRadialMenuItem = new RadialMenuItem();

            SymbolIcon symbolIcon = new SymbolIcon();
            symbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.DeveloperBoardLightning20;
            symbolIcon.FontSize = 50;

            mainRadialMenuItem.Content = symbolIcon;

            //Make transparent if needed, for wheel items that dont have a sub menu
            //mainRadialMenuItem.ArrowBackground = System.Windows.Media.Brushes.Transparent;
        }
        
        public void CreateSubMenuItems()
        {
            //create sub menu items for each TDP
            subRadialMenuItems = new List<RadialMenuItem>();

            foreach (string tdp in action.parameters)
            {
                RadialMenuItem rmi = new RadialMenuItem();
                rmi.Content = tdp;
                subRadialMenuItems.Add(rmi);
            }
        }

    }
}
