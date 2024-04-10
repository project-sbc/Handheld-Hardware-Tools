using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
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
        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.KeyDown += GlobalHook_KeyEvent;
            m_GlobalHook.KeyUp += GlobalHook_KeyEvent;
        }

        private void GlobalHook_KeyEvent(object? sender, System.Windows.Forms.KeyEventArgs e)
        {
            KeyEventArgsExt args = (KeyEventArgsExt)e;

            if (args.IsKeyDown)
            {
                runningKeyStroke = e.KeyData.ToString();


               // if (Global_Variables.Global_Variables.KBHotKeyDictionary.Count != null)
               // {

                  //  if (Global_Variables.Global_Variables.KBHotKeyDictionary.ContainsKey(runningKeyStroke) && !programmingKeystroke)
                   //{

                      //  args.SuppressKeyPress = true;
                       // ActionParameter action = Global_Variables.Global_Variables.KBHotKeyDictionary[runningKeyStroke];
                       // QuickAction_Management.runHotKeyAction(action);
                       // runningKeyStroke = "";

                    //}

                //}
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
