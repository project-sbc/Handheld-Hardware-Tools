using Everything_Handhelds_Tool.Classes.Actions;
using Everything_Handhelds_Tool.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Everything_Handhelds_Tool.AppWindows.Guide
{
    /// <summary>
    /// Interaction logic for Guide.xaml
    /// </summary>
    public partial class Guide : Window
    {
        public Guide()
        {
            InitializeComponent();

          

        }

        private void AddHotKeys()
        {

            double height = (leftWrapPanel.ActualHeight/8);
            double width =(leftWrapPanel.ActualWidth/2);

            List<Classes.Actions.Action> list = ((ActionList)XML_Management.Instance.LoadXML("ActionList")).Where(p => p.hotKey != "").OrderBy(o => o.hotkeyType).ToList();

            foreach (Classes.Actions.Action action in list)
            {
                leftWrapPanel.Children.Add(new ActionHotkeyGuide_UserControl(action, height, width));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddHotKeys();
        }
    }
}
