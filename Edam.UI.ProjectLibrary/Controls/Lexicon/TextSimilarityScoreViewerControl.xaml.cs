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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236
using Edam.UI.Controls.ViewModels;

namespace Edam.UI.Controls.Lexicon
{

   public sealed partial class TextSimilarityScoreViewerControl : UserControl
   {
      private LexiconSimilarityTextViewModel m_ViewModel =
         new LexiconSimilarityTextViewModel();
      public LexiconSimilarityTextViewModel ViewModel
      {
         get { return m_ViewModel; }
      }
      public TextSimilarityScoreViewerControl()
      {
         this.InitializeComponent();
         DataContext = m_ViewModel;
      }
   }

}
