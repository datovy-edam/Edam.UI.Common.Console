using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// -----------------------------------------------------------------------------
using Edam.Data.AssetConsole;

namespace Edam.UI.Controls.DataModels
{

   /// <summary>
   /// Data Map Instance...
   /// </summary>
   public class DataMapInstance
   {

      private AssetConsoleArgumentsInfo m_Arguments;
      public AssetConsoleArgumentsInfo Arguments
      {
         get { return m_Arguments; }
      }

      public object Instance { get; set; }
      public DataTreeModel TreeModel { get; set; }
      public string JsonInstanceSample { get; set; }

      public DataTreeEvent ManageNotification { get; set; } = null;

      /// <summary>
      /// Given information about the arguments, set the data instance 
      /// context.
      /// </summary>
      public void SetContext(AssetConsoleArgumentsInfo arguments)
      {
         m_Arguments = arguments;
         if (arguments.Lexicon == null || 
            String.IsNullOrWhiteSpace(arguments.Lexicon.LexiconId))
         {
            return;
         }
      }

   }

}
