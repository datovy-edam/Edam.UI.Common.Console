using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edam.UI.Common.Menus;

public interface IMenuView
{
  IMenuItemParent ParentMenu { get; set; }
  void SetState(Object state);
}
