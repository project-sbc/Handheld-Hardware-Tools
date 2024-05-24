using Gma.System.MouseKeyHook;
using Handheld_Hardware_Tools.Classes.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Handheld_Hardware_Tools.Classes
{
    public class MouseKeyHook
    {
        public IKeyboardMouseEvents m_GlobalHook;
        public string runningKeyStroke = "";

        public bool programmingKeystroke = false;
        public keyboardEvents keyboardEvents = new keyboardEvents();

        public Dictionary<string, Actions.Action> keyboardAction = new Dictionary<string, Actions.Action>();

        public MouseKeyHook()
        {
            UpdateDictionary();
        }

        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();

            //key events for hot keys
            m_GlobalHook.KeyDown += GlobalHook_KeyEvent;
            m_GlobalHook.KeyUp += GlobalHook_KeyEvent;

            //mouse drag events for whether the QAM should open by touch drag
            m_GlobalHook.MouseDragStarted += M_GlobalHook_MouseDragStarted;
            m_GlobalHook.MouseDragFinished += M_GlobalHook_MouseDragFinished;
  
        }

        Point startDragPoint = new Point(0,0);
        Screen screen = null;
        private void M_GlobalHook_MouseDragFinished(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            //this checks if drag event should open the QAM
            if (screen != null)
            {
                Screen endScreen = Screen.FromPoint(e.Location);

                if (endScreen.DeviceName == screen.DeviceName)
                {

                    //this checks if the Y posistion of the end drag is within 10% of the start drag.
                    double yTolerance = endScreen.Bounds.Height * 0.1;
                    if (e.Y < startDragPoint.Y + yTolerance && e.Y > startDragPoint.Y - yTolerance)
                    {
                        QuickAccessMenu qam = Local_Object.Instance.GetQAMWindow();

                        if (qam.Visibility != System.Windows.Visibility.Visible)
                        {
                            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");
                            if (settings.qamOnRightSide && startDragPoint.X > screen.Bounds.Width * 0.9)
                            {
                                qam.Show();
                            }
                            if (!settings.qamOnRightSide && startDragPoint.X < screen.Bounds.Width * 0.1)
                            {
                                qam.Show();
                            }
                        }

                    }



                }
            }

            //reset these after drag event regardless
            startDragPoint = new Point(0, 0);
            screen = null;       


        }

       

        private void M_GlobalHook_MouseDragStarted(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            //start event for detecting swipe input for opening menu
            Screen tempScreen = Screen.FromPoint(e.Location);
            
            if (e.X > tempScreen.Bounds.Width * 0.9 || e.X < tempScreen.Bounds.Width * 0.1)
            {
                
                if (e.Y > tempScreen.Bounds.Height * 0.33 && e.Y < tempScreen.Bounds.Height * 0.66)
                {
                    startDragPoint = e.Location;
                    screen = tempScreen;
                }
            }


        }

        public void UpdateDictionary()
        {
            ActionList actions = (ActionList)XML_Management.Instance.LoadXML("ActionList");

            keyboardAction = actions.ReturnKeyboardActionHotKeyList();
        }
        private void GlobalHook_KeyEvent(object? sender, System.Windows.Forms.KeyEventArgs e)
        {
            KeyEventArgsExt args = (KeyEventArgsExt)e;

            if (args.IsKeyDown)
            {
                runningKeyStroke = e.KeyData.ToString();


                if (keyboardAction.Count != null)
                {
                    Debug.WriteLine(runningKeyStroke);
                    if (keyboardAction.ContainsKey(runningKeyStroke) && !programmingKeystroke)
                   {

                        args.SuppressKeyPress = true;
                        Actions.Action action = keyboardAction[runningKeyStroke];
                        action.OnActivate();
                        runningKeyStroke = "";

                    }

                }
            }
            if (args.IsKeyUp)
            {
                keyboardEvents.raiseKeyboardStringPress(runningKeyStroke);

                runningKeyStroke = "";

            }

        }



        public void Unsubscribe()
        {
            if (m_GlobalHook != null)
            {
                m_GlobalHook.KeyDown -= GlobalHook_KeyEvent;
                m_GlobalHook.KeyUp -= GlobalHook_KeyEvent;

                //It is recommened to dispose it
                m_GlobalHook.Dispose();
            }

        }




    }
    public class keyboardEvents
    {

        public event EventHandler keyboardStringPress;
        public void raiseKeyboardStringPress(string keyCombo)
        {

            keyboardStringPress?.Invoke(keyCombo, EventArgs.Empty);
        }


    }
}
