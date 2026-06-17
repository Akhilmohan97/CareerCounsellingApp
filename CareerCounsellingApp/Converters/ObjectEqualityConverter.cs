using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace CareerCounsellingApp.Converters;

public class ObjectEqualityConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.Equals(parameter) ?? false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isChecked && isChecked)
        {
            return parameter;
        }
        return Avalonia.Data.BindingOperations.DoNothing;
    }
}
