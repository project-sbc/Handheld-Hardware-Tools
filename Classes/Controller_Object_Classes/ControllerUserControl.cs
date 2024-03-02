using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Wpf.Ui.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Everything_Handhelds_Tool.Classes.Controller_Object_Classes
{
    public class ControllerUserControl : UserControl
    {
        public Object borderControl;
        public Object mainControl;
        public Object toggleSwitchControl;
        public bool useableOnDevice;
        
        public ControllerUserControl()
        {
            useableOnDevice = UseableOnThisDevice();
        }

        public virtual void ControlChangeValueHandler() { }

        public virtual void ChangeMainWindowControllerInstructionPage() { }
        public virtual void HighlightControl() 
        { 
            if (borderControl != null) 
            {
                if (borderControl is Border)
                {
                    Border border = (Border)borderControl;
                    border.BorderBrush = (System.Windows.Media.Brush)Application.Current.Resources["HighlightCardBorder"];
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
                    //border.BorderBrush = System.Windows.Media.Brushes.White;
                    border.BorderBrush = (System.Windows.Media.Brush)Application.Current.Resources["SelectedCardBorder"];
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
        public virtual void ReturnControlToPage() 
        {
            HighlightControl();

            ControllerPage page = Local_Object.Instance.GetGeneralWindowPage(this);

            // Check if we found a page
            if (page != null)
            {
                if (page is ControllerPage)
                {
                    page.controllerNavigatePage = true;
                }
            }
         
            
        }

        public void ReturnControlToWindow()
        {
           

            //OLD CODE WHEN I ONLY HAD ONE WINDOW, NOW TRYING TO COMMONIZE
            //MainWindow wnd = (MainWindow)Application.Current.MainWindow;
            //wnd.SetControllerNavigateWindow(true);
        }

        public virtual bool UseableOnThisDevice()
        {
            return true;
        }

        #region handling controller input
        public virtual void HandleControllerInput(string action)
        {
            if (mainControl != null)
            {


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
                        else
                        {
                            if (action == "B")
                            {
                                ReturnControlToPage();
                            }
                        }
                    }
                }
                else 
                {
                    MainControlInputHandlerSwitchBoard(action);
                }
            }
            else
            {
                if (action == "B")
                {
                    ReturnControlToPage();
                }
            }
        }
        private void MainControlInputHandlerSwitchBoard(string action)
        {
            if (mainControl is Slider) { HandleSliderInput(action); }
            if (mainControl is ToggleSwitch) { HandleToggleSwitchInput(action); }
            if (mainControl is System.Windows.Controls.Button) { HandleButtonInput(action); }
            if (mainControl is ComboBox) { HandleComboBoxInput(action); }
            if (mainControl is ListView) { HandleListViewInput(action); }
            if (mainControl is Hyperlink) { HandleListViewInput(action); }
    
        }

        private void HandleHyperlinkInput(string action)
        {
            if(action == "A")
            {
                Hyperlink hyperlink = (Hyperlink)mainControl;
                hyperlink.RaiseEvent(new RoutedEventArgs(Hyperlink.ClickEvent));
            }
        }
        private void HandleToggleSwitchInput(string action)
        {
            ToggleSwitch tS = mainControl as ToggleSwitch;
            switch (action)
            {
                case "A":
                    tS.IsChecked = !tS.IsChecked;
                    break;
                case "B":
                    ReturnControlToPage();
                    break;
                default: break;
            }
        }
        private void HandleButtonInput(string action)
        {
            System.Windows.Controls.Button button = mainControl as System.Windows.Controls.Button;
            switch (action)
            {
                case "A" or "Button_A":
                    button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    break;
                case "B":
                    ReturnControlToPage();
                    break;
                default: break;
            }
        }
        private void HandleComboBoxInput(string action)
        {
            ComboBox comboBox = mainControl as ComboBox;
            switch (action)
            {

                case "A":
                    if (comboBox.IsDropDownOpen)
                    {
                        ControlChangeValueHandler();
                        comboBox.IsDropDownOpen = false;
                    }
                    else
                    {
                        comboBox.IsDropDownOpen = true;
                    }
                    break;
                case "B":
                    if (comboBox.IsDropDownOpen)
                    {
                        comboBox.IsDropDownOpen = false;
                    }
                 
                    ReturnControlToPage();
                    break;
                case "DPadUp" or "DPadDown":
                    HandleComboBoxNavigation(comboBox, action);
                    break;
                default: break;
            }
        }
        private void HandleListViewInput(string action)
        {
            ListView lv = mainControl as ListView;
            switch (action)
            {
                                
                case "B":
                   

                    ReturnControlToPage();
                    break;
                case "DPadUp" or "DPadDown" or "A":
                    HandleListViewNavigation(lv, action);
                    break;
                default: break;
            }
        }
        private void HandleComboBoxNavigation(ComboBox cbo, string action)
        {
            if (cbo == null)
            {
                return;
            }
            if (cbo.Items.Count == 0)
            {
                return;
            }

            if (cbo.SelectedIndex == -1)
            {
                cbo.SelectedIndex = 0;
                return;
            }
            else
            {
                switch (action)
                {
                    case "DPadUp":
                        if (cbo.SelectedIndex > 0)
                        {
                            cbo.SelectedIndex = cbo.SelectedIndex - 1;
                        }
                        else
                        {
                            cbo.SelectedIndex = cbo.Items.Count - 1;
                        }
                        break;
                    case "DPadDown":
                        if (cbo.SelectedIndex < cbo.Items.Count -1)
                        {
                            cbo.SelectedIndex = cbo.SelectedIndex + 1;
                        }
                        else
                        {
                            cbo.SelectedIndex = 0;
                        }
                        break;
                    case "A":
                        ControlChangeValueHandler();
                        break;
                }


            }

        }
        private void HandleListViewNavigation(ListView lv, string action)
        {
            if (lv == null)
            {
                return;
            }
            if (lv.Items.Count == 0)
            {
                return;
            }

            if (lv.SelectionMode == SelectionMode.Single)
            {
                if (lv.SelectedIndex == -1)
                {
                    lv.SelectedIndex = 0;
                    return;
                }
                else
                {
                    switch (action)
                    {
                        case "DPadUp":
                            if (lv.SelectedIndex > 0)
                            {
                                lv.SelectedIndex = lv.SelectedIndex - 1;
                            }
                            else
                            {
                                lv.SelectedIndex = lv.SelectedIndex - 1;
                            }
                            lv.ScrollIntoView(lv.SelectedItem);
                            break;
                        case "DPadDown":
                            if (lv.SelectedIndex < lv.Items.Count - 1)
                            {
                                lv.SelectedIndex = lv.SelectedIndex + 1;
                            }
                            else
                            {
                                lv.SelectedIndex = 0;
                            }
                            lv.ScrollIntoView(lv.SelectedItem);
                            break;
                        case "A":
                            ControlChangeValueHandler();
                            break;
                    }


                }
            }
            else
            {
                //when multi selectable items you gotta set selected item to different color
                ItemCollection itemCollection = lv.Items;


                //index is the currently selected item, we need to find it by searching through items for the light gray background item
                //if there isn't a lightgray item, the index is -1 and we need to start at the top
                int index = -1;
                foreach(ListViewItem item in itemCollection)
                {
                    if (item.Background == System.Windows.Media.Brushes.LightGray)
                    {
                        index = lv.Items.IndexOf(item);
                        break;
                    }
                }
                //for -1, we didn't find a currently selected item in the list so we start at 0 index
                if (index == -1)
                {
                    ListViewItem listViewItem = lv.Items[0] as ListViewItem;
                    listViewItem.Background = System.Windows.Media.Brushes.LightGray;
                }
                else
                {
                    ListViewItem listViewItem = lv.Items[index] as ListViewItem;
                    ListViewItem newListViewItem = null;
                    
                    switch (action)
                    {
                        case "DPadUp":
                            listViewItem.Background = System.Windows.Media.Brushes.Transparent;
                            if (index > 0)
                            {
                                index = index - 1;
                            }
                            else
                            {
                                index = lv.Items.Count -1;
                            }
                            newListViewItem = lv.Items[index] as ListViewItem;
                            newListViewItem.Background = System.Windows.Media.Brushes.LightGray;
                            lv.ScrollIntoView(newListViewItem);
                          
                            break;
                        case "DPadDown":
                            listViewItem.Background = System.Windows.Media.Brushes.Transparent;
                            if (index < lv.Items.Count- 1)
                            {
                                index = index + 1;
                            }
                            else
                            {
                                index = 0;
                            }
                            newListViewItem = lv.Items[index] as ListViewItem;
                            newListViewItem.Background = System.Windows.Media.Brushes.LightGray;
                            lv.ScrollIntoView(newListViewItem);
                            break;
                        case "A":
                            if (lv.SelectedItems.Count > 0)
                            {
                                if (!lv.SelectedItems.Contains(listViewItem))
                                {
                                    lv.SelectedItems.Add(listViewItem);
                                }
                                else
                                {
                                    lv.SelectedItems.Remove(listViewItem);
                                }
                            }


                            ControlChangeValueHandler();
                            break;
                    }


                }


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
                    ReturnControlToPage();
                    break;

                default: break;
            }
        }
        #endregion
    }
}
