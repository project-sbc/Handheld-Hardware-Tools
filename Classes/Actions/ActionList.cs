using Everything_Handhelds_Tool.Classes.Actions.ActionClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Everything_Handhelds_Tool.Classes.Actions
{
    public class ActionList : List<Action>
    {
        public ActionList()
        {
            this.Add(new Change_TDP(new List<string> { "6"}) { ID = 1, hotkeyType="Controller", hotKey= "772" });
        }


        public Dictionary<ushort, Action> ReturnControllerActionHotKeyList()
        {
            Dictionary<ushort, Action> actionHotKeys = new Dictionary<ushort, Action>();

            List<Action> list = this.Where(p  => p.hotkeyType == "Controller" && p.hotKey != "").ToList();

            foreach (Action action in list)
            {
                ushort hotkey;
                if (ushort.TryParse(action.hotKey, out hotkey))
                {
                    actionHotKeys.Add(hotkey, action);
                }
                    
            }

            return actionHotKeys;
        }

    }

   

}
