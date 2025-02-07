using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// -----------------------------------------------------------------------------
using Edam.InOut;

namespace Edam.UI.Common.Controls.Dialogs
{

   public interface IStorageInfo : IDialogObjectInfo
   {
      List<string> FileTypeFilter { get; set; }
      Action<IDialogObjectInfo> CallBack { get; set; }
   }

}
