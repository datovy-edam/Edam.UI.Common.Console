using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edam.UI.Controls.DataModels
{

   public class ListItemInfo
   {

      public const string CHECKBOX_ICON = "\uF168";
      public const string PERSONAL_FOLDER_ICON = "\uEC25";

      public string IconName { get; set; }
      public string Name { get; set; }

      public ListItemInfo()
      {
         IconName = CHECKBOX_ICON;
      }

   }

}
