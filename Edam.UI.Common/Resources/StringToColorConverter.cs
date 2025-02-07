using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Data;

namespace Edam.UI.Common.Resources;

public class StringToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter,
       string culture)
    {
        string valueAsString = value == null ? String.Empty : value.ToString();
        switch (valueAsString)
        {
            case ("A"):
            case (""):
                { return Colour.Default; }
            case ("D"):
                { return Colour.ColorFromHex("#b3d2e6"); }
            case ("I"):
                { return Colour.ColorFromHex("#c9c9c9"); }
            case ("AuditBlue"):
                { return Colour.ColorFromHex("#648DE1"); }
            default:
                {
                    return Colour.ColorFromHex(value.ToString()); ;
                }
        }
    }
    public object ConvertBack(object value, Type targetType,
       object parameter, string culture)
    {
        return null;
    }

}
