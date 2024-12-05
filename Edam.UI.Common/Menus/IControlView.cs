using System;

// -----------------------------------------------------------------------------

namespace Edam.UI.Common.Menus;


public interface IControlView
{
  IMenu ParentMenu { get; set; }

  // TODO: Implement this as a Delegate
  void Back(Object sender, EventArgs e);
}
