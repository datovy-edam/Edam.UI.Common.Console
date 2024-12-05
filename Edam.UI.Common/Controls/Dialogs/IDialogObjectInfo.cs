using Edam.InOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edam.UI.Common.Controls.Dialogs;


public enum StorageAction
{
  Unknown = 0,
  Save = 1,
  Open = 2
}

public interface IDialogObjectInfo
{
  public StorageAction StorageAction { get; set; }
  public FolderFileItemInfo StorageItem { get; set; }

  string CommandText { get; set; }
  string ItemType { get; set; }
  string ItemName { get; set; }
  object DataObject { get; set; }
  object Result { get; set; }
}
