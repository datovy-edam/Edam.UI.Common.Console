using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using System;

// -----------------------------------------------------------------------------
using Edam.UI.Common.Menus;

namespace Edam.UI.Common.Resources
{

   [ContentProperty(Name = "ItemTemplate")]
   public class MenuItemTemplateSelector : DataTemplateSelector
   {
      public DataTemplate ItemTemplate { get; set; }

      //public string PaneTitle { get; set; }

      protected override DataTemplate SelectTemplateCore(object item)
      {
         return item is Separator ? SeparatorTemplate : 
            item is Header ? HeaderTemplate : ItemTemplate;
      }

      protected override DataTemplate SelectTemplateCore(
         object item, DependencyObject container)
      {
         return item is Separator ? SeparatorTemplate : 
            item is Header ? HeaderTemplate : ItemTemplate;
      }

      internal DataTemplate HeaderTemplate = (DataTemplate)XamlReader.Load(
          @"<DataTemplate 
              xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
               <NavigationViewItemHeader Content='{Binding Title}' />
            </DataTemplate>");

      internal DataTemplate SeparatorTemplate = (DataTemplate)XamlReader.Load(
          @"<DataTemplate 
              xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
              <NavigationViewItemSeparator />
            </DataTemplate>");
   }

}
