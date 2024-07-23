using Handheld_Hardware_Tools.Classes.Actions.ActionClass;
using Handheld_Hardware_Tools.Classes.MouseMode.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using WindowsInput;

namespace Handheld_Hardware_Tools.Classes.MouseMode
{
    public class MouseProfile
    {
        public List<MouseActionItem> mouseActionList { get; set; }
        public bool rightScroll { get; set; } = true;
        public bool reverseVerticalScroll { get; set; } = false;


        public double sensitivityMouseValue { get; set; } = 30;
        public double sensitivityScrollValue { get; set; } = 3;
        //read as percentage of total joystick range (i.e. 4 = 4% which is 0.04*32600 for maximum joystick output)
        public double deadZone { get; set; } = 4;

        //these are in percentage, so make sure to divide by 100

        public double[] yValuesMouse { get; set; } = { 3, 5, 10, 15, 20, 25, 30, 35, 45, 55 };
        public double[] yValuesScroll { get; set; } = { 3, 5, 10, 15, 20, 25, 30, 35, 45, 55 };
       

        public void SaveToXML()
        {
            XML_Management.Instance.SaveXML("MouseProfile", this);
        }

        public Dictionary<string, MouseAction> GetMouseActionDictionary()
        {
            Dictionary<string, MouseAction> dictionary = new Dictionary<string, MouseAction>();
            foreach( MouseActionItem mouseActionItem in mouseActionList)
            {
                if (!dictionary.ContainsKey(mouseActionItem.button))
                {
                    dictionary.Add(mouseActionItem.button, mouseActionItem.mouseAction);
                }
            }
            return dictionary;
        }

    }

    [XmlInclude(typeof(MouseLeftClick))]
    [XmlInclude(typeof(MouseRightClick))]
    [XmlInclude(typeof(KeyboardClick))]
    [XmlInclude(typeof(AltTab))]
    [XmlInclude(typeof(OpenOSK))]
    public class MouseActionItem
    {
        public string button { get; set; }
        public MouseAction mouseAction { get; set; }
    }



    public static class DefaultMouseActionItems
    {
        public static List<MouseActionItem> DefaultMouseActionItemList = new List<MouseActionItem>
        {
            new MouseActionItem() { button = "A", mouseAction = new MouseLeftClick() },
            new MouseActionItem() { button = "B", mouseAction = new MouseRightClick() },
            new MouseActionItem() { button = "Y", mouseAction = new OpenOSK() },
            new MouseActionItem() { button = "X", mouseAction = new AltTab() },

            new MouseActionItem() { button = "LeftThumb", mouseAction = null },
            new MouseActionItem() { button = "RightThumb", mouseAction = null },
            new MouseActionItem() { button = "LeftShoulder", mouseAction = null },
            new MouseActionItem() { button = "RightShoulder", mouseAction = null },
            new MouseActionItem() { button = "Start", mouseAction = null },
            new MouseActionItem() { button = "Back", mouseAction = null },
            new MouseActionItem() { button = "LeftTrigger", mouseAction = null },
            new MouseActionItem() { button = "RightTrigger", mouseAction = null },


            new MouseActionItem() { button = "DPadUp", mouseAction = new KeyboardClick() { virtualKeyCode = WindowsInput.Native.VirtualKeyCode.UP } },
            new MouseActionItem() { button = "DPadDown", mouseAction = new KeyboardClick() { virtualKeyCode = WindowsInput.Native.VirtualKeyCode.DOWN } },
            new MouseActionItem() { button = "DPadLeft", mouseAction = new KeyboardClick() { virtualKeyCode = WindowsInput.Native.VirtualKeyCode.LEFT } },
            new MouseActionItem() { button = "DPadRight", mouseAction = new KeyboardClick() { virtualKeyCode = WindowsInput.Native.VirtualKeyCode.RIGHT } },

        };

    }
}
