using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using SmokeNote.Logic.Enums;

namespace SmokeNote.Client.Converters
{
    public class NotebookTypeToCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            NoteBookType type = (NoteBookType)value;
            byte v = (byte)type;

            if (v.ToString() == parameter.ToString())
            {
                return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            if (isChecked)
            {
                byte b = System.Convert.ToByte(parameter);
                return (NoteBookType)b;
            }
            return null;
        }
    }
}
