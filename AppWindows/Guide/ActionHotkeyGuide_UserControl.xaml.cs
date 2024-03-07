using Everything_Handhelds_Tool.Classes;
using Everything_Handhelds_Tool.Classes.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
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
using Windows.System.UserProfile;
using Wpf.Ui.Controls;

namespace Everything_Handhelds_Tool.AppWindows.Guide
{
    /// <summary>
    /// Interaction logic for ActionHotkey.xaml
    /// </summary>
    public partial class ActionHotkeyGuide_UserControl : UserControl
    {
        public ActionHotkeyGuide_UserControl(Classes.Actions.Action action, double height, double width)
        {
            this.Height = height;
            this.Width = width;

            InitializeComponent();
            SetupHotKeyTypeIcon(action.hotkeyType);
            SetupButtonCombo(action);
            SetupActionName(action);
        }

        private void SetupHotKeyTypeIcon(string type)
        {
            if (type == "Controller")
            {
                hotkeyTypeSymbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.XboxController20;
            }
            else
            {
                hotkeyTypeSymbolIcon.Symbol = Wpf.Ui.Common.SymbolRegular.Keyboard20;
            }
            
        }

        private void SetupActionName(Classes.Actions.Action action)
        {
            textblockActionName.Text = action.actionName;
        }

        private void SetupButtonCombo(Classes.Actions.Action action)
        {
            if (action.hotkeyType == "Keyboard")
            {
                Label kbHotkey = new Label();
                kbHotkey.Content = action.hotKey;
                stackPanelActionHotKey.Children.Add(kbHotkey);
            }
            else
            {//else is controller
                string hotkeyCombo = General_Functions.convertControllerUshortToString(action.hotKey);

                List<string> parsedHotKey = hotkeyCombo.Split("+").ToList();

                foreach(string hotkey in parsedHotKey)
                {
                    FontIcon fontIcon = new FontIcon();
                    fontIcon.Glyph = hotkeyToFontLabel[hotkey]; 
                    stackPanelActionHotKey.Children.Add(fontIcon);

                    if (parsedHotKey.IndexOf(hotkey) < parsedHotKey.Count - 1)
                    {//this only fires if we haven't gotten to the last loop, this adds a + sign
                        stackPanelActionHotKey.Children.Add(new FontIcon() { Glyph = '\uE710' });
                    }

                }
                

            }
        }



        Dictionary<string, char> hotkeyToFontLabel = new Dictionary<string, char>()
        {
              {"A", '\uF093' },
              {"B", '\uF094' },
              {"X", '\uF096' },
              {"Y", '\uF095' },
              {"DPadUp" , '\uF0AD' },
              {"DPadDown" , '\uF0AE' },
              {"DPadRight" , '\uF0AF' },
              {"DPadLeft" , '\uF0B0' },
              {"LB" , '\uF10C' },
              {"RB" , '\uF10D' },
              {"RStick" , '\uF109' },
              {"LStick" , '\uF108' },
              {"Start" , '\uEDE3' },
              {"Back" , '\uEECA' }
        };

    }
}
