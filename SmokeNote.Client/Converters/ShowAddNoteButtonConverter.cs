using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SmokeNote.Client.Converters
{
    public class ShowAddNoteButtonConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var notes = System.Convert.ToInt32(values[0]);
                bool isRecycle = System.Convert.ToBoolean(values[1]);
                string keywords = values[2] as string;
                if (notes == 0 && !isRecycle && string.IsNullOrWhiteSpace(keywords))
                {
                    return System.Windows.Visibility.Visible;
                }
                return System.Windows.Visibility.Collapsed;
            }
            catch
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
