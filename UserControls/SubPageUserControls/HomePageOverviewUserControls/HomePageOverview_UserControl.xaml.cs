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
            ConfigureTextAndSymbol();
        
        }
        public override void ChangeMainWindowControllerInstructionPage()
        {
            General_Functions.ChangeControllerInstructionPage("EditDeleteBackMoveUpDown");
        }

        #region set textblocks and icons
        private void ConfigureTextAndSymbol()
        {
            //4 textblocks and a couple icons to set
            SetDisplayNameTextBlock();

          

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
                card.IsExpanded = false;
                ReturnControlToPage();
  
            }
            else
            {
                switch (action)
                {
                    case "DPadUp":
                        SendCommandToEditActionOverviewPage("MoveUp");
                        break;
                    case "DPadDown":
                        SendCommandToEditActionOverviewPage("MoveDown");
                        break;
                    case "Delete":
                        SendCommandToEditActionOverviewPage("Delete");
                        break;
                    case "A":
                        SendCommandToEditActionOverviewPage("Edit");
                        break;
                }
            }
           
           

        }

        private void SendCommandToEditActionOverviewPage(string action)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            EditActionOverviewPage editActionOverviewPage = Local_Object.Instance.GetMainWindowFramePage() as EditActionOverviewPage;

            if (editActionOverviewPage != null)
            {
                editActionOverviewPage.HandleUserControlInputs(this, action);
            }

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            SendCommandToEditActionOverviewPage("Edit");
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            SendCommandToEditActionOverviewPage("Delete");
        }

        private void btnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            SendCommandToEditActionOverviewPage("MoveUp");
        }

        private void btnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            SendCommandToEditActionOverviewPage("MoveDown");
        }
    }
}
