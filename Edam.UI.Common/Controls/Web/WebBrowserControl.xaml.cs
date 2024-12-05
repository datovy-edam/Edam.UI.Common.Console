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

namespace Edam.UI.Common.Controls.Web
{

   public sealed partial class WebBrowserControl : UserControl
   {

      private WebBrowserViewModel m_ViewModel = new WebBrowserViewModel();
      public WebBrowserViewModel ViewModel
      {
         get { return m_ViewModel; }
      }
      
      public WebBrowserControl()
      {
         this.InitializeComponent();
         DataContext = m_ViewModel;
         m_ViewModel.WebViewer = WebViewer;
      }

   }

}
