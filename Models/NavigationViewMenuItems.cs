using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using ModernWpf.Controls;

namespace Everything_Handhelds_Tool.Models
{
    public class NavigationViewMenuItems : List<NavigationViewItem>
    {
        public NavigationViewMenuItems()
        {
            this.Add(new NavigationViewItem { Content = Application.Current.Resources["MainWindow_NavigationViewMenuItem_Home"] , Icon = new FontIcon {FontFamily= new FontFamily("Segoe MDL2 Assets"), Glyph = "\xE80F" }, Tag="HomePage" });
            this.Add(new NavigationViewItem { Content = Application.Current.Resources["MainWindow_NavigationViewMenuItem_Home"], Icon = new FontIcon { FontFamily = new FontFamily("Segoe MDL2 Assets"), Glyph = "\xE8F1" } });
            this.Add(new NavigationViewItem { Content = Application.Current.Resources["MainWindow_NavigationViewMenuItem_Home"], Icon = new FontIcon { FontFamily = new FontFamily("Segoe MDL2 Assets"), Glyph = "\xE768" } });
            this.Add(new NavigationViewItem { Content = Application.Current.Resources["MainWindow_NavigationViewMenuItem_Home"], Icon = new FontIcon { FontFamily = new FontFamily("Segoe MDL2 Assets"), Glyph = "\xE967" } });
            this.Add(new NavigationViewItem { Content = Application.Current.Resources["MainWindow_NavigationViewMenuItem_Home"], Icon = new FontIcon { FontFamily = new FontFamily("Segoe MDL2 Assets"), Glyph = "\xE946" } });
            this.Add(new NavigationViewItem { Content = Application.Current.Resources["MainWindow_NavigationViewMenuItem_Home"], Icon = new FontIcon { FontFamily = new FontFamily("Segoe MDL2 Assets"), Glyph = "\xE962" } });

        }
    }
}
