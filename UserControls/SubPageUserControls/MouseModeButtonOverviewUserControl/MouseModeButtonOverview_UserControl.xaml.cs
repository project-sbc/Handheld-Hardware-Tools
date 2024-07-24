using Handheld_Hardware_Tools.Classes;
using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using Handheld_Hardware_Tools.Classes.MouseMode.Actions;
using Handheld_Hardware_Tools.Pages;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Design.Behavior;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Handheld_Hardware_Tools.UserControls.ActionOverviewUserControls
{
    /// <summary>
    /// Interaction logic for TDP_Slider.xaml
    /// </summary>
    public partial class MouseModeButtonOverview_UserControl : ControllerUserControl
    {
        public Handheld_Hardware_Tools.Classes.MouseMode.MouseActionItem action = null;
        public bool isMovingUserControlInStackPanel = false;
        public MouseModeButtonOverview_UserControl(Handheld_Hardware_Tools.Classes.MouseMode.MouseActionItem newAction)
        {
            InitializeComponent();
            
            //set virtual border
            borderControl = border;

            //main control
            mainControl = null;

            action = newAction;

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
            SetActionNameTextBlock();

           

            //set main icon and arguments
            SetMainIconAndArguments();


        }
        private void SetActionNameTextBlock()
        {
            if (action.mouseAction != null)
            {
                actionName.Text = Application.Current.Resources["MouseAction_" + action.mouseAction.mouseActionName.ToString()].ToString();
            }
            else
            {
                actionName.Text = Application.Current.Resources["MouseAction_None"].ToString();
                actionArguments.Visibility = Visibility.Collapsed;
            }
            switch (action.button)
            {
                case "A":
                    buttonIcon.Glyph = '\uF093';
                    break;
                case "B":
                    buttonIcon.Glyph = '\uF094';
                    break;
                case "X":
                    buttonIcon.Glyph = '\uF095';
                    break;
                case "Y":
                    buttonIcon.Glyph = '\uF096';
                    break;
                case "LeftShoulder":
                    buttonIcon.Glyph = '\uF10C';
                    break;
                case "RightShoulder":
                    buttonIcon.Glyph = '\uF10D';
                    break;
                case "LeftTrigger":
                    buttonIcon.Glyph = '\uF10A';
                    break;
                case "RightTrigger":
                    buttonIcon.Glyph = '\uF10B';
                    break;
                case "LeftThumb":
                    buttonIcon.Glyph = '\uF108';
                    break;
                case "RightThumb":
                    buttonIcon.Glyph = '\uF109';
                    break;
                case "Start":
                    buttonIcon.Glyph = '\uEDE3';
                    break;
                case "Back":
                    buttonIcon.Glyph = '\uEECA';
                    break;
                case "DPadUp":
                    buttonIcon.Glyph = '\uF0AD';
                    break;
                case "DPadDown":
                    buttonIcon.Glyph = '\uF0AE';
                    break;
                case "DPadLeft":
                    buttonIcon.Glyph = '\uF0B0';
                    break;
                case "DPadRight":
                    buttonIcon.Glyph = '\uF0AF';
                    break;

            }
        }
      
       

      
        private void SetMainIconAndArguments()
        {
            if (action.mouseAction.mouseActionName.Contains("Scroll"))
            {
                actionArguments.Text = action.mouseAction.ar
            }


            switch (action.mouseAction)
            {
                case ScrollLeft:
                    

                    break;
                default:
                    //MessageBox.Show("NO HANDLER FOR THIS MOUSE ACTION, ADD IT YOU DUMMY. MouseModeActionOverview_UserControl.xaml.cs ");
                    break;
            }
            //hide arguments if there are none in the action
   
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
                        SendCommandToEditActionOverviewPage("MoveUp");
                        break;
                    case "DPadDown":
                        SendCommandToEditActionOverviewPage("MoveDown");
                        break;
                    case "X":
                        SendCommandToEditActionOverviewPage("Delete");
                        break;
                    case "A":
                        SendCommandToEditActionOverviewPage("Edit");
                        break;
                }
            }
           
           

        }

        private void SendCommandToEditActionOverviewPage(string strAction)
        {
            QuickAccessMenu mainWindow = (QuickAccessMenu)Application.Current.MainWindow;

            //EditActionOverviewPage editActionOverviewPage = Local_Object.Instance.GetMainWindowFramePage() as EditActionOverviewPage;

            //if (editActionOverviewPage != null)
            //{
               //
                
            //}

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
