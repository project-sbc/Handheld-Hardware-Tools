using Handheld_Hardware_Tools.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

namespace Handheld_Hardware_Tools.UserControls
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

            //set value to current tdp
            slider.Value = TDP_Management.Instance.ReadAndReturnSustainedTDP();

            //set common user control stuff like text and symbols

            glyph = "";
            fontIconVisibility = System.Windows.Visibility.Collapsed;
            symbolRegular = Wpf.Ui.Common.SymbolRegular.DeveloperBoardLightning20;
            symbolIconVisibility = System.Windows.Visibility.Visible;
            mainText = "TDP";
            subText = "Adjust the sustained thermal design power limit.";
            valueText = "W";
        }


        public override void slider_DragStarted(object sender, DragStartedEventArgs e)
        {
            dragStarted = true;
        }
        public override void slider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            dragStarted = false;
            ControlChangeValueHandler();
        }
    }
}
