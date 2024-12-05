using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

// -----------------------------------------------------------------------------
using CommunityToolkit.Mvvm.ComponentModel;
using Edam.Diagnostics;
using Edam.Application;
namespace Edam.UI.Common.Controls.Web
{

   public class WebBrowserViewModel : ObservableObject
   {
      private ResultLog m_ResultsLog = new ResultLog();
      private Uri m_UrlSource;

      public WebView2 WebViewer = null;

      public Uri UrlSource
      {
         get { return m_UrlSource; }
         set
         {
            if (m_UrlSource != value)
            {
               m_UrlSource = value;
               OnPropertyChanged("UrlSource");
            }
         }
      }
      
      public WebBrowserViewModel()
      {
         string url = AppSettings.GetString("DefaultWebSource");
         if (!String.IsNullOrWhiteSpace(url))
         {
            Navigate(url);
         }
      }

      public void Navigate(string url)
      {
         try
         {
            m_ResultsLog.Clear();
            UrlSource = new Uri(url);
         }
         catch(Exception ex)
         {
            m_ResultsLog.Failed(ex);
         }
      }

   }

}
