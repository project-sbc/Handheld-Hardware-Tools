using Handheld_Hardware_Tools.Classes.Controller_Object_Classes;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Xps.Serialization;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace Handheld_Hardware_Tools.UserControls
{
    /// <summary>
    /// Interaction logic for CommonUserControl.xaml
    /// </summary>
    public partial class CommonUserControl_Slider : ControllerUserControl
    {
        public bool dragStarted = false;

        public string mainText { get; set; } = "";
        //sub text is the long text only displayed in larger windows
        public string subText { get; set; } = "";
        //value text is for the displayed value (i.e. W or %)
        public string valueText { get; set; } = "";



        public SymbolRegular symbolRegular { get; set; } = SymbolRegular.Empty;
        public string glyph { get; set; } = "";

        public Visibility symbolIconVisibility { get; set; }
        public Visibility fontIconVisibility { get; set; }

        public CommonUserControl_Slider()
        {
            InitializeComponent();
            DataContext = this;
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e) {}

        public virtual void slider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e) {}
    }
}
