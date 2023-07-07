using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyLearningStrategyMobleXForms.Converters
{
    public class StringNullOrEmptyBoolConverter : IValueConverter
    {

        #region IValueConverter implementation

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value is string)
                {

                    if (string.IsNullOrEmpty((string)value))
                        return false;
                    else
                        return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return true;
        }

        #endregion
    }
}