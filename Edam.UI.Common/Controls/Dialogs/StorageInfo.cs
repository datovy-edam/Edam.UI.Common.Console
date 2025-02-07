using Edam.InOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edam.UI.Common.Controls.Dialogs
{

   public class StorageInfo : IStorageInfo
   {
      public Action<IDialogObjectInfo> CallBack { get; set; }
      public List<string> FileTypeFilter { get; set; }

      public StorageAction StorageAction { get; set; }
      public FolderFileItemInfo StorageItem { get; set; }

      public string CommandText { get; set; }
      public string ItemType { get; set; }
      public string ItemName { get; set; }
      public object DataObject { get; set; }
      public object Result { get; set; }
      public bool Cancelled { get; set; }
   }

}
