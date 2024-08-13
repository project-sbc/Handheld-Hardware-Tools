using Handheld_Hardware_Tools.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace Handheld_Hardware_Tools.UserControls
{
    public class TDP_Slider : CommonUserControl_Slider
    {
        public TDP_Slider()
        {
            userControlIconSymbol.Symbol = Wpf.Ui.Common.SymbolRegular.DeveloperBoardLightning20;
            
            
            //set up device specific settings
            Settings settings = (Settings)XML_Management.Instance.LoadXML("Settings");
            slider.Maximum = settings.maxTDP;
            slider.Minimum = settings.minTDP;
            slider.Value = TDP_Management.Instance.ReadAndReturnSustainedTDP();

            mainText = "Cheese";
            subText = "grater";
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
