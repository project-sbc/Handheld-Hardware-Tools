
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Wpf.Ui.Controls;

namespace Handheld_Hardware_Tools.Classes.Controller_Object_Classes
{
    public class ControllerWindow : UiWindow
    {
        //controllerNavigateWindow determines if input stays on window level or gets passed down

        public bool controllerNavigateWindow = true;

        //common frame is the main page
        //instructionframe is for the controller input page (the little ABXY etc. button prompts)
        public Frame commonFrame;
        public Frame instructionFrame;

        public void SetControllerNavigateWindow(bool navigateValue)
        {
            //set the controllerNavigateWindow from page level
            controllerNavigateWindow = navigateValue;
            if (!controllerNavigateWindow)
            {
                ControllerPage a = (ControllerPage)commonFrame.Content;
                a.HandleControllerInput("Highlight First Control");
            }


        }

        public bool DetermineControllerConnected()
        {
            QuickAccessMenu mw = (QuickAccessMenu)Application.Current.MainWindow;
            if (mw.controllerInput != null)
            {
                if (mw.controllerInput.controller != null)
                {
                    if (mw.controllerInput.controller.IsConnected)
                    {
                        return true;
                    }
                }
            }


            return false;
        }
        public virtual void SetControllerInputPage(string pageName)
        {
            //Sets the controller instruction on the bottom of the page
            //If controller is not connected the uri should be null to make the instruction disappear
            Uri uri = null;
            //if the controller icon, which updates based on controller connection, is visible, then change instruction
            if (DetermineControllerConnected()) 
            {
                switch (pageName)
                {

                    case "ToggleBackMoveUpDown":
                        uri = new Uri("ControllerInstructionPages\\ToggleBackMoveUpDown.xaml", UriKind.Relative);
                        break;

                    case "SelectSaveBack":
                        uri = new Uri("ControllerInstructionPages\\SelectSaveBack.xaml", UriKind.Relative);
                        break;
                    case "SelectAddBack":
                        uri = new Uri("ControllerInstructionPages\\SelectAddBack.xaml", UriKind.Relative);
                        break;
                    case "SelectBack":
                        uri = new Uri("ControllerInstructionPages\\SelectBack.xaml", UriKind.Relative);
                        break;
                    case "SelectHide":
                        uri = new Uri("ControllerInstructionPages\\SelectHide.xaml", UriKind.Relative);
                        break;
                    case "ChangeBack":
                        uri = new Uri("ControllerInstructionPages\\ChangeBack.xaml", UriKind.Relative);
                        break;
                    case "ChangeToggleBack":
                        uri = new Uri("ControllerInstructionPages\\ChangeToggleBack.xaml", UriKind.Relative);
                        break;
                    case "ToggleBack":
                        uri = new Uri("ControllerInstructionPages\\ToggleBack.xaml", UriKind.Relative);
                        break;
                    case "EditDeleteBackMoveUpDown":
                        uri = new Uri("ControllerInstructionPages\\EditDeleteBackMoveUpDown.xaml", UriKind.Relative);
                        break;


                    default:
                        uri = null;
                        break;
                }
            }


            instructionFrame.Source = uri;
        }


        public virtual void HandleControllerInputTopLevel(object? sender, controllerInputEventArgs e) {}

        public void SubscribeControllerEvents()
        {
            QuickAccessMenu mw;
            if (this is QuickAccessMenu)
            {
                mw= (QuickAccessMenu)this;
            }
            else
            {
                mw = Application.Current.MainWindow as QuickAccessMenu;
            }
            
            if (mw != null)
            {
                if (mw.controllerInput != null)
                {
                    //subscribe to controller input
                    mw.controllerInput.buttonPressEvent.controllerInputEvent += HandleControllerInputTopLevel;
                    //subscribe to controller connection changed event
                    mw.controllerInput.controllerConnectionChangedEvent.controllerConnectionChangedEvent += HandleControllerConnectionChanged;
                }
            }

           
        }
        public void UnsubscribeControllerEvents()
        {
            QuickAccessMenu mw;
            if (this is QuickAccessMenu)
            {
                mw = (QuickAccessMenu)this;
            }
            else
            {
                mw = Application.Current.MainWindow as QuickAccessMenu;
            }
            if (mw != null)
            {
                if (mw.controllerInput != null)
                {
                    //subscribe to controller input
                    mw.controllerInput.buttonPressEvent.controllerInputEvent -= HandleControllerInputTopLevel;
                    //subscribe to controller connection changed event
                    mw.controllerInput.controllerConnectionChangedEvent.controllerConnectionChangedEvent -= HandleControllerConnectionChanged;
                }
            }


        }
        private void HandleControllerConnectionChanged(object sender, controllerConnectionChangedEventArgs e)
        {
            if (e.Connected) { SetControllerInputPage("SelectHide"); }
            else { SetControllerInputPage(""); }
        }
    }
}
