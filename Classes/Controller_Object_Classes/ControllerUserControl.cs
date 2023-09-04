using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Wpf.Ui.Controls;

namespace Everything_Handhelds_Tool.Classes.Controller_Object_Classes
{
    public class ControllerUserControl : UserControl
    {
        public Card uiCard;
        public virtual void HandleControllerInput(string action) { }
        public virtual void HighlightControl() 
        { 
            if (uiCard != null)
            {
                uiCard.BorderBrush = System.Windows.Media.Brushes.White;
            }
        }
        public virtual void UnhighlightControl() 
        {
            if (uiCard != null)
            {
                uiCard.BorderBrush = System.Windows.Media.Brushes.Transparent;
            }
        }
        public void ReturnControlToPage() 
        {
            MainWindow wnd = (MainWindow)Application.Current.MainWindow;
            ControllerPage controllerPage = wnd.frame.Content as ControllerPage;
            controllerPage.ReturnControlToPage();

        }

    }
}
