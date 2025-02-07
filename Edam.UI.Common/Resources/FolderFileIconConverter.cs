using System;
using System.Collections.Generic;

using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI;

using Edam.InOut;

namespace Edam.UI.Common.Resources;


public class FolderFileIconConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter,
       string culture)
    {
        int val = value == null ? 1 : (int)value;
        switch (val)
        {
            case 1:
                // folder v:ED41.c-ED43.o h:F12B.c-ED25.o
                return "\xED25";
            case 2:
                return "\xF000"; // document
            default:
                return "\xED25"; // folder
        }
    }
    public object ConvertBack(object value, Type targetType,
       object parameter, string culture)
    {
        return null;
    }

}
