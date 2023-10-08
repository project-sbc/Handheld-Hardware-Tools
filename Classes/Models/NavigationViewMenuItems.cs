using Everything_Handhelds_Tool.Classes.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Everything_Handhelds_Tool.Models
{
    public class NavigationViewMenuItems : List<Wpf.Ui.Common.SymbolRegular>
    {
        public NavigationViewMenuItems()
        {
            this.Add(Wpf.Ui.Common.SymbolRegular.Home16);
            this.Add(Wpf.Ui.Common.SymbolRegular.FlashPlay20);

            this.Add(Wpf.Ui.Common.SymbolRegular.Power20);
 
        }
    }
}
