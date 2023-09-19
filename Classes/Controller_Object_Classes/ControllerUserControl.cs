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
        public Object borderControl;
        public Object mainControl;
        public virtual void HandleControllerInput(string action) 
        { 
            if (mainControl != null)
            {
                if (mainControl is Slider)
                {
                    Slider slider = mainControl as Slider;
                    switch(action)
                    {
                        case "Right":
                            slider.Value = slider.Value + slider.Interval; 
                            break;
                        case "Left":
                            slider.Value = slider.Value - slider.Interval;
                            break;
                        case "A":
                            ControlChangeValueHandler();
                            break;
                        case "B":
                            ReturnControlToPage();
                            break;

                        default:break; 
                    }
                }
            }
        }
        public virtual void ControlChangeValueHandler() { }
        public virtual void HighlightControl() 
        { 
            if (borderControl != null) 
            {
                if (borderControl is Wpf.Ui.Controls.Card)
                {
                    Card card = (Card)borderControl;
                    card.BorderBrush = System.Windows.Media.Brushes.White;
                }
                if (borderControl is Wpf.Ui.Controls.CardExpander)
                {
                    CardExpander card = (CardExpander)borderControl;
                    card.BorderBrush = System.Windows.Media.Brushes.White;
                }
            }
        }

        public virtual void UnhighlightControl() 
        {
            if (borderControl != null)
            {
                if (borderControl is Wpf.Ui.Controls.Card)
                {
                    Card card = (Card)borderControl;
                    card.BorderBrush = System.Windows.Media.Brushes.Transparent;
                }
                if (borderControl is Wpf.Ui.Controls.CardExpander)
                {
                    CardExpander card = (CardExpander)borderControl;
                    card.BorderBrush = System.Windows.Media.Brushes.Transparent;
                }
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
