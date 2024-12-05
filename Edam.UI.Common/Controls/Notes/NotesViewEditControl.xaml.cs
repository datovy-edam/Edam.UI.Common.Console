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

// -----------------------------------------------------------------------------
using Edam.DataObjects.DataCodes;

namespace Edam.UI.Common.Controls.Notes
{
   public sealed partial class NotesViewEditControl : UserControl
   {
      private readonly NoteViewModel m_ViewModel = new NoteViewModel();
      public NoteViewModel ViewModel
      {
         get { return m_ViewModel; }
      }

      public NotesViewEditControl()
      {
         this.InitializeComponent();
         DataContext = m_ViewModel;
      }

      private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (e.AddedItems == null || e.AddedItems.Count == 0)
            return;
         m_ViewModel.NoteTypeIndexChanged(e.AddedItems[0] as DataCodeInfo);
      }
   }
}
