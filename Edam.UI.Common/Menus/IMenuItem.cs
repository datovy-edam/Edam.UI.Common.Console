using System;
using System.Collections.Generic;

namespace Edam.UI.Common.Menus;


public interface IMenuItem
{

  int Id { get; set; }
  string Title { get; set; }
  Type TargetType { get; set; }
  Object Instance { get; set; }
  IMenuItemParent Parent { get; set; }
  bool Navigation { get; set; }

}
