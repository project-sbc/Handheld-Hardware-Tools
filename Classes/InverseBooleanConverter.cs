using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Everything_Handhelds_Tool.Classes.Converters
{
    //this class is used for elements on the mainwindow in the XAML
    //for example, a card expander tied to a toggle switch, if i want
    //the toggle switch to be the opposite of the card expander's isexpanded property
    //i can  use this to invert it without using C# in the control's page

    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : MarkupExtension, IValueConverter
    {
        #region IValueConverter Members
        private static InverseBooleanConverter _converter = null;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null) _converter = new InverseBooleanConverter();
            return _converter;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is bool)
            {
                return !((bool)value);
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }

        #endregion
    }


}
