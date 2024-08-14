using Handheld_Hardware_Tools.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows;

namespace Handheld_Hardware_Tools.UserControls.HomePageUserControls
{
    public class EPP_Slider : CommonUserControl_Slider
    {
        public EPP_Slider()
        {
            //set up device specific settings

         
            slider.Maximum = 100;
            slider.Minimum = 0;
            slider.Interval = 5;
            slider.TickFrequency = 5;

            //set value to current EPP
            slider.Value = Powercfg_Management.Instance.ReadAndReturnEPP();

            //set common user control stuff like text and symbols

            glyph = "";
            fontIconVisibility = System.Windows.Visibility.Collapsed;
            symbolRegular = Wpf.Ui.Common.SymbolRegular.DeveloperBoardLightning20;
            symbolIconVisibility = System.Windows.Visibility.Visible;
            mainText = Application.Current.Resources["Usercontrol_EPP_Maintext"].ToString();
            subText = Application.Current.Resources["Usercontrol_EPP_Subtext"].ToString();
            valueText = Application.Current.Resources["Usercontrol_EPP_Unitvalue"].ToString();
        }

        public override void ControlChangeValueHandler()
        {
            int epp = (int)Math.Round(slider.Value, 0);
            Powercfg_Management.Instance.ChangeEPP(epp);
        }
    }
}
