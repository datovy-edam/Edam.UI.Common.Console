using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Edam.UI.Common.Controls.Utilities
{

   public sealed partial class TextEditorControl : UserControl
   {

      public static readonly DependencyProperty TextoProperty =
         DependencyProperty.Register(
            "Texto",
            typeof(String),
            typeof(TextEditorControl), null
      );

      public string Texto
      {
         get { return (string)GetValue(TextoProperty); }
         set { SetValue(TextoProperty, (string)value); }
      }

      public TextEditorViewModel m_ViewModel { get; set; }
      public TextEditorControl()
      {
         m_ViewModel = new TextEditorViewModel();
         this.InitializeComponent();
         this.DataContext = m_ViewModel;
         m_ViewModel.InitializeVisibility(false);
      }

      private void TextButton_Click(object sender, RoutedEventArgs e)
      {
         m_ViewModel.TextBlock_Clicked(null);
      }

      private void TextBlock_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
      {

      }

      private void TextBlock_PointerPressed(object sender, PointerRoutedEventArgs e)
      {
         m_ViewModel.TextBlock_Clicked(null);
      }

      private void TextEdit_KeyDown(object sender, KeyRoutedEventArgs e)
      {

      }

   }

}
