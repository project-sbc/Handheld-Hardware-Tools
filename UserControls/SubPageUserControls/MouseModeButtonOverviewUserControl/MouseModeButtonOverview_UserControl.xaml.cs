﻿using Handheld_Hardware_Tools.Classes;
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
            actionName.Text = Application.Current.Resources["MouseAction_" + action.mouseAction.ToString()].ToString();
        }
      
       

        private void SetActionTextBlockAndIcon()
        {
           
        }
        private void SetMainIconAndArguments()
        {
            switch (action.mouseAction)
            {
                case ScrollLeft:
                   

                    break;
                default:
                    MessageBox.Show("NO HANDLER FOR THIS MOUSE ACTION, ADD IT YOU DUMMY. MouseModeActionOverview_UserControl.xaml.cs ");
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