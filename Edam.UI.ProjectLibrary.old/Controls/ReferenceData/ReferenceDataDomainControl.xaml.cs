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
using Edam.UI.Controls.ViewModels;
using Edam.UI.Controls.DataModels;

namespace Edam.UI.Controls.ReferenceData
{

   public sealed partial class ReferenceDataDomainControl : UserControl
   {
      private ReferenceDataDomainViewModel m_ViewModel;
      public ReferenceDataDomainViewModel ViewModel
      {
         get { return m_ViewModel; }
      }

      public ReferenceDataDomainControl()
      {
         this.InitializeComponent();

         m_ViewModel = DomainGridControl.ViewModel;
         DataContext = m_ViewModel;
      }

      private void New_Click(object sender, RoutedEventArgs e)
      {
         ViewModel.ShowDomainEditor();
      }

      private void SearchUri_Click(object sender, RoutedEventArgs e)
      {
         m_ViewModel.GetItems();
      }
   }

}
