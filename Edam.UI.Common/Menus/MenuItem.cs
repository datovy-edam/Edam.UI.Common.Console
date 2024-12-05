using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

// -----------------------------------------------------------------------------

namespace Edam.UI.Common.Menus
{
   public class MenuItemBase { }

   public class MenuItem : MenuItemBase, 
      IMenuItem, IMenuProcess, IMenuNavigation
   {

      public MenuOption MenuOption
      {
         get { return (MenuOption)Id; }
         set { Id = (int)value; }
      }

      public int Id { get; set; }
      public string Title { get; set; }
      public string Tooltip { get; set; }
      public Symbol Glyph { get; set; }
      public Type TargetType { get; set; }
      public IViewModel ViewModel { get; set; }
      public Action Exec { get; set; }
      public IMenuItemParent Parent { get; set; }
      public Object Instance { get; set; }
      public bool Visible { get; set; }
      public bool Navigation { get; set; }

      public MenuItem(Type pageType = null, IMenuItemParent parent = null)
      {
         Glyph = Symbol.More;
         TargetType = pageType;
         ViewModel = null;
         Parent = parent;
         Visible = true;
         Navigation = true;
      }

      public void Goto(Object sender, GotoEventArgs e)
      {
         if (e == null)
            e = new GotoEventArgs();
         if (Parent != null)
            Parent.Goto(sender, e);
      }

      public IMenuItem Find(MenuOption option)
      {
         return this;
      }

   }

   public class Separator : MenuItemBase { }

   public class Header : MenuItemBase
   {
      public string Title { get; set; }
   }

}
