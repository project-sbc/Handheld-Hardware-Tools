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
using System.Windows.Threading;

namespace Everything_Handhelds_Tool.AppWindows.Guide
{
    /// <summary>
    /// Interaction logic for Guide.xaml
    /// </summary>
    public partial class Guide : Window
    {
        DispatcherTimer closeWindow = new DispatcherTimer() { Interval = new TimeSpan(0,0,0,0,400) };


        public Guide()
        {
            InitializeComponent();

            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            closeWindow.Tick += CloseWindow_Tick;
            closeWindow.Start();
        }

        private void CloseWindow_Tick(object? sender, EventArgs e)
        {
            //this routine will automatically close the window, its designed to stay open as long as you hold the hotkey button
            closeWindow.Stop();
            closeWindow.Tick -= CloseWindow_Tick;
            this.Close();
        }

        private void AddHotKeys()
        {
            List<Classes.Actions.Action> list = ((ActionList)XML_Management.Instance.LoadXML("ActionList")).Where(p => p.hotKey != "").OrderBy(o => o.hotkeyType).ToList();

         

            double height = (wrapPanel.ActualHeight/10);
            double width = (wrapPanel.ActualWidth);
            int numCols = 1;

            if (width > 500)
            {
                numCols = (int)Math.Round((width - 500) / 500, 0,MidpointRounding.AwayFromZero);
            }
         


            width = wrapPanel.ActualWidth/numCols;

           
            foreach (Classes.Actions.Action action in list)
            {
                wrapPanel.Children.Add(new ActionHotkeyGuide_UserControl(action, height, width));
            }
        }
        public void ResetTimer()
        {//this resets timer when hotkey is still pressed
            if (closeWindow.IsEnabled)
            {
                closeWindow.Stop();
                closeWindow.Start();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddHotKeys();
        }
    }
}
