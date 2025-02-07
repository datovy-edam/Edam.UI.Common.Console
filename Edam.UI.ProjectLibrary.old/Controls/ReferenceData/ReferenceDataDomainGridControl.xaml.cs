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
using CommunityToolkit.WinUI.UI.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Edam.UI.Controls.ReferenceData
{

   public sealed partial class ReferenceDataDomainGridControl : UserControl
   {
      private ReferenceDataDomainViewModel m_ViewModel;
      public ReferenceDataDomainViewModel ViewModel
      {
         get { return m_ViewModel; }
      }

      public ReferenceDataDomainGridControl()
      {
         this.InitializeComponent();
         m_ViewModel = new ReferenceDataDomainViewModel();
         DataContext = m_ViewModel;
      }

   }

}
