using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI;

// -----------------------------------------------------------------------------
using Edam.Data.AssetSchema;

namespace Edam.UI.Controls.Assets;


public class CustomRowStyleSelector : StyleSelector
{
    public const string STYLE_SELECTOR = "styleselector";

    public UserControl Parent { get; set; }
    protected override Style SelectStyleCore(object item, DependencyObject container)
    {
        var row = item;// (item as DataRowBase).RowData;
        var data = row as AssetDataElement;

        Style style;
        switch (data.ElementType)
        {
            case Data.Asset.ElementType.type:
                style = Parent.Resources["typeStyle"] as Style;
                break;
            case Data.Asset.ElementType.enumerator:
                style = Parent.Resources["enumStyle"] as Style;
                break;
            default:
                style = Parent.Resources["normalStyle"] as Style;
                break;
        }
        return style;
    }
}
