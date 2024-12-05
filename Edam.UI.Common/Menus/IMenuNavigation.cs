using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edam.UI.Common.Menus;


public interface IMenuNavigation
{
  void Goto(Object sender, GotoEventArgs e);
  IMenuItem Find(MenuOption option);
}
