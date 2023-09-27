using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Wpf.Ui.Controls;
using System.Windows.Controls.Primitives;

namespace Everything_Handhelds_Tool.Classes.Controller_Object_Classes
{
    public class ControllerUserControl : UserControl
    {
        public Object borderControl;
        public Object mainControl;
        public Object toggleSwitchControl;

      
        public virtual void ControlChangeValueHandler() { }

        public virtual void ChangeMainWindowControllerInstructionPage() { }
        public virtual void HighlightControl() 
        { 
            if (borderControl != null) 
            {
                if (borderControl is Border)
                {
                    Border border = (Border)borderControl;
                    border.BorderBrush = System.Windows.Media.Brushes.Gray;
                }
            }
        }
        public virtual void SelectControl()
        {
            if (borderControl != null)
            {
                if (borderControl is Border)
                {
                    Border border = (Border)borderControl;
                    border.BorderBrush = System.Windows.Media.Brushes.White;
                }
                ChangeMainWindowControllerInstructionPage();
            }
        }
        public virtual void UnhighlightControl() 
        {
            if (borderControl != null)
            {
                if (borderControl is Border)
                {
                    Border border = (Border)borderControl;
                    border.BorderBrush = System.Windows.Media.Brushes.Transparent;
                }
            }
        }
        public void ReturnControlToPage() 
        {
            HighlightControl();
            MainWindow wnd = (MainWindow)Application.Current.MainWindow;
            ControllerPage controllerPage = wnd.frame.Content as ControllerPage;
            controllerPage.ReturnControlToPage();
            
        }


        #region handling controller input
        public virtual void HandleControllerInput(string action)
        {
            if (mainControl != null)
            {
                if (action == "B")
                {
                    ReturnControlToPage();
                }

                if (toggleSwitchControl != null)
                {
                    ToggleSwitch tS = toggleSwitchControl as ToggleSwitch;
                    if (action == "X")
                    {
                        tS.IsChecked = !tS.IsChecked;
                        ChangeMainWindowControllerInstructionPage();
                    }
                    else
                    {
                        if (tS.IsChecked == true)
                        {
                            MainControlInputHandlerSwitchBoard(action);
                        }
                    }
                }
                else 
                {
                    MainControlInputHandlerSwitchBoard(action);
                }
            }
        }
        private void MainControlInputHandlerSwitchBoard(string action)
        {
            if (mainControl is Slider) { HandleSliderInput(action); }
            if (mainControl is ToggleSwitch) { HandleToggleSwitchInput(action); }
            if (mainControl is System.Windows.Controls.Button) { HandleButtonInput(action); }
    
        }
        private void HandleToggleSwitchInput(string action)
        {
            ToggleSwitch tS = mainControl as ToggleSwitch;
            switch (action)
            {
                case "X":
                    tS.IsChecked = !tS.IsChecked;
                    break;
            
                default: break;
            }
        }
        private void HandleButtonInput(string action)
        {
            System.Windows.Controls.Button button = mainControl as System.Windows.Controls.Button;
            switch (action)
            {
                case "A":
                    button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    break;

                default: break;
            }
        }
        private void HandleSliderInput(string action)
        {
            Slider slider = mainControl as Slider;
            switch (action)
            {
                case "DPadRight":
                    slider.Value = slider.Value + slider.Interval;
                    break;
                case "DPadLeft":
                    slider.Value = slider.Value - slider.Interval;
                    break;
                case "A":
                    ControlChangeValueHandler();
                    break;
                case "B":
                    ControlChangeValueHandler();
                    break;

                default: break;
            }
        }
        #endregion
    }
}
