using SmokeNote.Logic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SmokeNote.Client.Converters
{
    public class ArticleDisplayToItemTemplateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var type = (ArticleDisplays)value;

            string key = null;
            switch (type)
            {
                case ArticleDisplays.List:
                    key = "ArticleListItemTemplate";
                    break;
                case ArticleDisplays.Summary:
                case ArticleDisplays.Thumb:
                    key = "ArticleSummaryItemTemplate";
                    break;
            }

            return System.Windows.Application.Current.MainWindow.Resources[key];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
