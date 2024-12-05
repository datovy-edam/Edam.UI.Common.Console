using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236
// -----------------------------------------------------------------------------
using Edam.UI.Common.Menus;

namespace Edam.UI.Common.Controls
{
   public sealed partial class AboutControl : UserControl, IMenuView
   {
      public IMenuItemParent ParentMenu { get; set; }
      public string Title { get; set; }

      public AboutControl()
      {
         this.InitializeComponent();
      }

      public void SetState(object state)
      {
         if (state is IMenuItem)
         {
            var i = state as IMenuItem;
            Title = i.Title;
         }
      }
   }
}
