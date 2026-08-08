using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace CareerCounsellingApp.Converters;

public class LanguageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool useMalayalam)
        {
            return useMalayalam ==true? "മലയാളം" : "English";
        }
        return "English";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string language)
        {
            return language == "മലയാളം";
        }
        return false;
    }
}
