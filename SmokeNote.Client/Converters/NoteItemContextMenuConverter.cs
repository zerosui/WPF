using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SmokeNote.Client.Converters
{
    public class NoteItemContextMenuConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isDeleted = System.Convert.ToBoolean(values[0]);

            string key = null;

            if (!isDeleted)
            {
                key = "NormalNoteItemContextMenu";
            }
            else
            {
                key = "RecycleNoteItemContextMenu";
            }
            return System.Windows.Application.Current.MainWindow.Resources[key];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
