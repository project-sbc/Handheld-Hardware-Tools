using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Controller_Object_Classes;
using Everything_Handhelds_Tool.Classes.HomePage;
using Everything_Handhelds_Tool.Pages;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Everything_Handhelds_Tool.UserControls.ActionOverviewUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class HomePageOverview_UserControl : ControllerUserControl
    {
        public HomePageItem hpi = null;
        public bool isMovingUserControlInStackPanel = false;
        public HomePageOverview_UserControl(HomePageItem newHPI)
        {
            InitializeComponent();

            //set virtual border
            borderControl = border;

            //main control
            mainControl = null;

            hpi = newHPI;

            //Configure text and symbol to match the action
            ConfigureTextAndToggleSwitch();
        
        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("EditDeleteBackMoveUpDown");
        }

        #region set textblocks and icons
        private void ConfigureTextAndToggleSwitch()
        {
            //textblock and toggle switch to set
            SetDisplayNameTextBlock();

            //
            if (hpi.UserControlVisible)
            {
                toggleSwitch.IsChecked = true;
            }
            else
            {
                toggleSwitch.IsChecked = false;
            }
          

        }
        private void SetDisplayNameTextBlock()
        {
            hpiDisplayName.Text = Application.Current.Resources[hpi.UserControlName].ToString();

        }
      

        #endregion

 

        public override void SelectControl()
        {
            if (borderControl != null)
            {
                if (borderControl is Border)
                {
                    Border border = (Border)borderControl;
                    border.BorderBrush = System.Windows.Media.Brushes.White;
                }
                ChangeMainWindowControllerInstructionPage();
                //card.IsExpanded = true;
            }
        }

        public override void HandleControllerInput(string action)
        {
            if (action == "B")
            {
              
                ReturnControlToPage();
  
            }
            else
            {
                switch (action)
                {
                    case "DPadUp":
                        SendCommandToHomePageOverviewPage("MoveUp");
                        break;
                    case "DPadDown":
                        SendCommandToHomePageOverviewPage("MoveDown");
                        break;

                    case "A":
                     
                        toggleSwitch.IsChecked = !toggleSwitch.IsChecked;
                        break;
                }
            }
           
           

        }

        private void SendCommandToHomePageOverviewPage(string action)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            EditHomeOverviewPage editHomeOverviewPage = Local_Object.Instance.GetMainWindowFramePage() as EditHomeOverviewPage;

            if (editHomeOverviewPage != null)
            {
                editHomeOverviewPage.HandleUserControlInputs(this, action);
            }

        }

       

        private void btnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            SendCommandToHomePageOverviewPage("MoveUp");
        }

        private void btnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            SendCommandToHomePageOverviewPage("MoveDown");
        }

        private void toggleVisibilitySwitch()
        {
            if (this.IsLoaded)
            {
                if (toggleSwitch.IsChecked == true)
                {
                    hpi.UserControlVisible = true;
                }
                else
                {
                    hpi.UserControlVisible = false;
                }
                SendCommandToHomePageOverviewPage("Toggle");
            }
         
         
        }

        private void toggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            toggleVisibilitySwitch();
        }

        private void toggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            toggleVisibilitySwitch();
        }
    }
}
