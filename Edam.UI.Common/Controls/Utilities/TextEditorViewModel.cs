using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

using Microsoft.UI.Xaml;

// -----------------------------------------------------------------------------
using CommunityToolkit.Mvvm.ComponentModel;

namespace Edam.UI.Common.Controls.Utilities
{

   public class TextEditorViewModel : ObservableObject
   {

      private int m_IconTypeNo;
      public int IconTypeNo
      {
         get { return m_IconTypeNo; }
         set
         {
            if (m_IconTypeNo != value)
            {
               m_IconTypeNo = value;
               OnPropertyChanged("IconTypeNo");
            }
         }
      }

      private string m_SelectedText;
      public string SelectedText
      {
         get { return m_SelectedText; }
         set
         {
            if (m_SelectedText != value)
            {
               m_SelectedText = value;
               OnPropertyChanged("SelectedText");
            }
         }
      }

      private Visibility m_TextVisibility;
      public Visibility TextVisibility
      {
         get { return m_TextVisibility; }
         set
         {
            if (m_TextVisibility != value)
            {
               m_TextVisibility = value;
               OnPropertyChanged("TextVisibility");
            }
         }
      }

      private Visibility m_TextEditorVisibility;
      public Visibility TextEditorVisibility
      {
         get { return m_TextEditorVisibility; }
         set
         {
            if (m_TextEditorVisibility != value)
            {
               m_TextEditorVisibility = value;
               OnPropertyChanged("TextEditorVisibility");
            }
         }
      }

      //public Visibility EditorVisibility
      //{
      //   get
      //   {
      //      return TextEditorVisibility;
      //   }
      //   set
      //   {
      //      var v = value == Visibility.Collapsed ?
      //         Visibility.Visible : Visibility.Collapsed;
      //      TextVisibility = v;
      //      TextEditorVisibility = value;
      //   }
      //}

      public TextEditorViewModel()
      {
         TextVisibility = Visibility.Collapsed;
         TextEditorVisibility = Visibility.Collapsed;
      }

      public void SetEditorVisibility(bool editorVisibility)
      {
         TextEditorVisibility = editorVisibility ? 
            Visibility.Visible : Visibility.Collapsed;
         TextVisibility = editorVisibility ?
            Visibility.Collapsed : Visibility.Visible;
      }

      public void InitializeVisibility(bool editorVisibility = false)
      {
         SetEditorVisibility(editorVisibility);
      }

      public virtual void TextBlockSetText(string text)
      {
         SetEditorVisibility(false);
         SelectedText = text;
      }

      public virtual void TextBlock_DoubleTapped()
      {

      }

      #region -- 4.00 - Manage Editor - Text Toggling...

      public void EditingItemNameDone()
      {
         SetEditorVisibility(false);
      }

      public void TextBlock_Clicked(object item)
      {
         SetEditorVisibility(true);
      }

      #endregion

   }

}
