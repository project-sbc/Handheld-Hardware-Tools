using Everything_Handhelds_Tool.Classes.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Everything_Handhelds_Tool.Models
{
    public class NavigationViewMenuItems : List<NavigationViewMenuItem>
    {
        public NavigationViewMenuItems()
        {
            NavigationViewMenuItem nvmi1 = new NavigationViewMenuItem();
            nvmi1.symbolIcon = Wpf.Ui.Common.SymbolRegular.Home12;
            nvmi1.uri = new System.Uri("Pages/HomePage.xaml", System.UriKind.Relative);
            this.Add(nvmi1);
            
        }
    }
}
