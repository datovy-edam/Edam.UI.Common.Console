using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Windows.ApplicationModel.Contacts;

namespace Edam.UI.Common.Resources;

public class StringToBrushConverter
{
  public const string RootBrush = "Root";
  public const string FolderBrush = "Folder";
  public const string AuditBrush = "Audit";
  public const string CodeSetBrush = "CodeSet";
  public const string AssociationBrush = "Association";

  public object Convert(object value, Type targetType, object parameter,
     string culture)
  {
     string valueAsString = value == null ? String.Empty : value.ToString();
     return ToBrush(valueAsString);
  }
  public object ConvertBack(object value, Type targetType,
     object parameter, string culture)
  {
     return null;
  }

  public static SolidColorBrush ToBrush(string colorKeyText)
  {
     switch (colorKeyText)
     {
        case ("A"):
        case (""):
        case (RootBrush):
           {
              return new SolidColorBrush(Colors.Black);
           }
        case ("D"):
           { return Colour.BrushFromHex("#b3d2e6"); }
        case ("I"):
           { return Colour.BrushFromHex("#c9c9c9"); }
        case (AuditBrush):
           {
              return new SolidColorBrush(Colors.DimGray);
           }
        case (CodeSetBrush):
           {
              return new SolidColorBrush(Colors.Navy);
           }
        case (AssociationBrush):
           {
              return new SolidColorBrush(Colors.DarkOliveGreen);
           }
        case (FolderBrush):
           {
              return new SolidColorBrush(Colors.Brown);
           }
        default:
           {
              return Colour.BrushFromHex(colorKeyText); ;
           }
     }
  }
}
