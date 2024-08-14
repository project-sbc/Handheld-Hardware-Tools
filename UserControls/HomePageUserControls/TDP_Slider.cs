using Handheld_Hardware_Tools.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;


namespace Handheld_Hardware_Tools.UserControls.HomePageUserControls
{
    public class TDP_Slider : CommonUserControl_Slider
    {
        public TDP_Slider()
        {
            //set up device specific settings


            //get settings to find out tdp min and maxes
            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");
            slider.Maximum = settings.maxTDP;
            slider.Minimum = settings.minTDP;
            slider.Interval = 1;
            slider.TickFrequency = 1;

            //set value to current tdp
            slider.Value = TDP_Management.Instance.ReadAndReturnSustainedTDP();

            //set common user control stuff like text and symbols

            glyph = "";
            fontIconVisibility = System.Windows.Visibility.Collapsed;
            symbolRegular = Wpf.Ui.Common.SymbolRegular.DeveloperBoardLightning20;
            symbolIconVisibility = System.Windows.Visibility.Visible;
            mainText = Application.Current.Resources["Usercontrol_TDP_Maintext"].ToString();
            subText = Application.Current.Resources["Usercontrol_TDP_Subtext"].ToString();
            valueText = Application.Current.Resources["Usercontrol_TDP_Unitvalue"].ToString();
        }


        public override void ControlChangeValueHandler()
        {
            int tdp = (int)Math.Round(slider.Value, 0);
            TDP_Management.Instance.ChangeSustainedTDP(tdp);
        }
    }
}
