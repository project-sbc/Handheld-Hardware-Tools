using Handheld_Hardware_Tools.Classes.Actions.ActionClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Handheld_Hardware_Tools.Classes.Actions
{
    public class DefaultActionList:ActionList
    {
        public DefaultActionList() 
        {
            Cycle_TDP cycle_TDP = new Cycle_TDP() { parameters = new List<string> { "10", "15", "20" }, ID = 0, displayInActionPanel = true, displayInRadialMenu = true };
            this.Add(cycle_TDP);

            Toggle_QAM toggle_QAM = new Toggle_QAM() {ID = 1, displayInActionPanel = true, hotkeyType="Controller", hotKey="772" };
            this.Add(toggle_QAM);

            Toggle_OSK toggle_OSK = new Toggle_OSK() { ID = 2, displayInActionPanel = true, hotkeyType = "Controller", hotKey = "770" };
            this.Add(toggle_OSK);

            Toggle_WinOSK toggle_WOSK = new Toggle_WinOSK() { ID = 3, displayInActionPanel = true };
            this.Add(toggle_WOSK);

            Toggle_WindowManager toggle_WM = new Toggle_WindowManager() { ID = 4, displayInActionPanel = true, displayInRadialMenu = true };
            this.Add(toggle_WM);

            Toggle_MouseMode toggle_MM = new Toggle_MouseMode() { ID = 5, displayInActionPanel = true,  displayInRadialMenu = true };
            this.Add(toggle_MM);

            Toggle_VolumeMute toggle_VM = new Toggle_VolumeMute() { ID = 6, displayInActionPanel = true };
            this.Add(toggle_VM);

            Toggle_MicrophoneMute toggle_MicM = new Toggle_MicrophoneMute() { ID = 7, displayInActionPanel = true };
            this.Add(toggle_MicM);

            Toggle_Wifi toggle_wifi = new Toggle_Wifi() { ID = 8, displayInActionPanel = true, displayInRadialMenu = true };
            this.Add(toggle_wifi);

            Toggle_BT toggle_BT = new Toggle_BT() { ID = 9, displayInActionPanel = true, displayInRadialMenu = true };
            this.Add(toggle_BT);

            Toggle_QuickActionWheel toggle_QAW = new Toggle_QuickActionWheel() { ID=10, displayInActionPanel = true , hotkeyType = "Controller", hotKey = "769" };
            this.Add(toggle_QAW);

            Toggle_Desktop toggle_Desktop = new Toggle_Desktop() { ID=11, displayInRadialMenu= true, displayInActionPanel = true };
            this.Add(toggle_Desktop);   

            Cycle_Volume cycle_Volume = new Cycle_Volume() { ID=12, displayInRadialMenu = true, displayInActionPanel = true, parameters = new List<string> { "0", "10", "20", "30", "50", "70", "80", "90", "100" } };
            this.Add(cycle_Volume);

            Toggle_Guide toggle_Guide = new Toggle_Guide() { ID = 13, hotkeyType = "Controller", hotKey = "771" };
            this.Add(toggle_Guide);

            Toggle_VolumeMute toggle_VolumeMute = new Toggle_VolumeMute() { ID=14, displayInActionPanel= true, displayInRadialMenu=true };
            this.Add(toggle_VolumeMute);

            Toggle_FreeSync toggle_FreeSync = new Toggle_FreeSync() { ID = 15, displayInActionPanel = true }; 
            this.Add(toggle_FreeSync);
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
        public Dictionary<string, Action> ReturnKeyboardActionHotKeyList()
        {
            Dictionary<string, Action> actionHotKeys = new Dictionary<string, Action>();

            List<Action> list = this.Where(p => p.hotkeyType == "Keyboard" && p.hotKey != "").ToList();

            foreach (Action action in list)
            {
                actionHotKeys.Add(action.hotKey, action);

            }

            return actionHotKeys;
        }
    }

   

}
