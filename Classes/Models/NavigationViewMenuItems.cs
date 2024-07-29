using Handheld_Hardware_Tools.Classes.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace Handheld_Hardware_Tools.Models
{
    public class NavigationViewMenuItems : List<Tuple<Wpf.Ui.Common.SymbolRegular, string>>
    {
        public NavigationViewMenuItems()
        {
            //making changes here, update the general function for looking up page based on icons -->   TranslateIconToPageURI
            this.Add(new Tuple<Wpf.Ui.Common.SymbolRegular, string>(Wpf.Ui.Common.SymbolRegular.Home16, Application.Current.Resources["MainWindow_NavigationViewMenuItem_Home"].ToString()));
            this.Add(new Tuple<Wpf.Ui.Common.SymbolRegular, string>(Wpf.Ui.Common.SymbolRegular.Play12, Application.Current.Resources["MainWindow_NavigationViewMenuItem_Actions"].ToString()));
            this.Add(new Tuple<Wpf.Ui.Common.SymbolRegular, string>(Wpf.Ui.Common.SymbolRegular.Info16, Application.Current.Resources["MainWindow_NavigationViewMenuItem_Information"].ToString()));
            this.Add(new Tuple<Wpf.Ui.Common.SymbolRegular, string>(Wpf.Ui.Common.SymbolRegular.Settings20, Application.Current.Resources["MainWindow_NavigationViewMenuItem_Settings"].ToString()));
            this.Add(new Tuple<Wpf.Ui.Common.SymbolRegular, string>(Wpf.Ui.Common.SymbolRegular.Power20, Application.Current.Resources["MainWindow_NavigationViewMenuItem_Power"].ToString()));





        }
    }
}
