using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WindowsInput;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Everything_Handhelds_Tool.Classes.MouseMode
{
    public class MouseMode
    {
        MouseProfile mouseProfile = new MouseProfile();
    
        public void SubscribeControllerEvents()
        {
            ControllerInput controllerInput = Local_Object.Instance.GetMainWindowControllerInput();
            if (controllerInput != null )
            {
                controllerInput.joystickEvent.controllerJoystickEvent += JoystickEvent_controllerJoystickEvent;
                controllerInput.buttonPressEvent.controllerInputEvent += ButtonPressEvent_controllerInputEvent;
            }
        }

        private void ButtonPressEvent_controllerInputEvent(object? sender, controllerInputEventArgs e)
        {
            //this clip of code checks if any of the windows are visible, the ! makes it so its only true when none of the windows are visislbe
            //this is what i want for mousemode, only work when the user enables it AND no windows are open, otherwise controller input takes over
            if (!Application.Current.Windows.Cast<System.Windows.Window>().Any(x => x.Visibility == Visibility.Visible))
            {
                Classes.MouseMode.Actions.MouseAction mouseAction = mouseProfile.mouseActionList[e.Action];
                if (mouseAction != null)
                {
                    mouseAction.ClickEvent();
                }
            }

           
        }

        private void JoystickEvent_controllerJoystickEvent(object? sender, controllerJoystickEventArgs e)
        {
            //handle mouse movement and scroll from joysticks

            //this clip of code checks if any of the windows are visible, the ! makes it so its only true when none of the windows are visislbe
            //this is what i want for mousemode, only work when the user enables it AND no windows are open, otherwise controller input takes over
            if (!Application.Current.Windows.Cast<System.Windows.Window>().Any(x => x.Visibility == Visibility.Visible))
            {
               
            }
        }

        public void UnsubscribeControllerEvents() 
        {
            ControllerInput controllerInput = Local_Object.Instance.GetMainWindowControllerInput();
            if (controllerInput != null)
            {
                controllerInput.joystickEvent.controllerJoystickEvent -= JoystickEvent_controllerJoystickEvent;
                controllerInput.buttonPressEvent.controllerInputEvent -= ButtonPressEvent_controllerInputEvent;
            }

        }

        private bool _updateMouseProfile { get; set; } = false;

        public bool UpdateMouseProfile
        {
            get
            {
                return _updateMouseProfile;
            }
            set
            {
                _updateMouseProfile = value;
            }
        }

    }
}
