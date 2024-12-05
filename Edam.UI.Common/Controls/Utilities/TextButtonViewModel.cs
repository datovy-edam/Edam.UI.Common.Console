using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// -----------------------------------------------------------------------------
using CommunityToolkit.Mvvm.ComponentModel;

namespace Edam.UI.Common.Controls.Utilities
{

   public class TextButtonViewModel : ObservableObject
   { 
      private string m_Texto;

      public string Texto
      {
         get { return m_Texto; }
         set
         {
            if (m_Texto != value)
            {
               m_Texto = value;
               OnPropertyChanged("Texto");
            }
         }
      }

   }

}
