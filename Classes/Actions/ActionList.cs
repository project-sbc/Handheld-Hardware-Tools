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
    public class DefaultActionList:ActionList
    {
        public DefaultActionList() 
        {
            Cycle_TDP cycle_TDP = new Cycle_TDP() { parameters = new List<string> { "10", "15", "20" }, ID = 0, displayInActionPanel = true };
            this.Add(cycle_TDP);

            Toggle_QAM toggle_QAM = new Toggle_QAM() {ID = 1, displayInActionPanel = true };
            this.Add(toggle_QAM);

            Toggle_OSK toggle_OSK = new Toggle_OSK() { ID = 2, displayInActionPanel = true };
            this.Add(toggle_OSK);

            Toggle_WinOSK toggle_WOSK = new Toggle_WinOSK() { ID = 3, displayInActionPanel = true };
            this.Add(toggle_WOSK);

            Toggle_WindowManager toggle_WM = new Toggle_WindowManager() { ID = 4, displayInActionPanel = true };
            this.Add(toggle_WM);
        }
    }


    public class ActionList : List<Action>
    {
        public ActionList()
        {
            
        }

        public void SaveToXML()
        {
            XML_Management.Instance.SaveXML("ActionList", this);
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
