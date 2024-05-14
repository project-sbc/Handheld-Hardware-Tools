using Gma.System.MouseKeyHook;
using Handheld_Hardware_Tools.Classes.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            m_GlobalHook.KeyDown += GlobalHook_KeyEvent;
            m_GlobalHook.KeyUp += GlobalHook_KeyEvent;
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
