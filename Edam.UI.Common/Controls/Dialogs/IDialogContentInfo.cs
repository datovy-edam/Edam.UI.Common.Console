using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Edam.DataObjects.Models;

namespace Edam.UI.Common.Controls.Dialogs;

public interface IDialogModelData
{
  ModelData ModelData { get; set; }
}

public interface IDialogContentInfo : IDialogObjectInfo, IDialogModelData
{
  string Title { get; set; }
  string Message { get; set; }
  Action<IDialogObjectInfo> CallBack { get; set; }
  string PrimaryText { get; set; }
  string SecondaryText { get; set; }
  string CloseText { get; set; }
}
