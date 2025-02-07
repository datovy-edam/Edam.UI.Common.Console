using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Text;

using Edam.UI.Controls.ViewModels;

namespace Edam.UI.Controls.DataModels
{

   public class TextDocumentModel
   {
      private CodeEditorViewModel m_ViewModel;

      private string m_Text;
      public string Text
      {
         get { return m_Text; }
         set
         {
            m_Text = value;
            m_ViewModel.NotifyEditorTextAvailable(this);
         }
      }
      public string LanguageText { get; set; }

      public TextDocumentModel(CodeEditorViewModel model)
      {
         m_ViewModel = model;
      }

      public void SetText(TextSetOptions options, string text, string language)
      {
         LanguageText = language;
         m_ViewModel.SetEditorText(text, language);
      }

      public async Task<string> GetText()
      {
         var text = await m_ViewModel.GetEditorText();
         return text;
      }

   }

}
