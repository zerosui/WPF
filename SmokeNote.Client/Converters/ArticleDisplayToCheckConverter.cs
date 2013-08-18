using SmokeNote.Logic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SmokeNote.Client.Converters
{
    public class ArticleDisplayToCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ArticleDisplays type = (ArticleDisplays)value;
            byte v = (byte)type;

            if (v.ToString() == parameter.ToString())
            {
                return true;
            }
            return false; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isChecked = (bool)value;
            if (isChecked)
            {
                byte b = System.Convert.ToByte(parameter);
                return (ArticleDisplays)b;
            }
            return null;
        }
    }
}
