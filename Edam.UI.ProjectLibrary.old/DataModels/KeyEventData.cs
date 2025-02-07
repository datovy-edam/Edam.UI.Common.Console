using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Input;

namespace Edam.UI.Controls.DataModels
{

   public class KeyEventData
   {
      public static double DefaultMaxDeltaMilliseconds = 800;
      public KeyRoutedEventArgs LastKeyPressed { get; set; }
      public DateTime KeyPressedDateTime { get; set; }

      public bool IsControlKeyPressed
      {
         get
         {
            return SameKey(LastKeyPressed) && 
               LastKeyPressed.Key == Windows.System.VirtualKey.Control;
         }
      }
      public KeyEventData(KeyRoutedEventArgs args)
      {
         Update(args);
      }
      public KeyEventData()
      {
         Update(null);
      }

      public void Update(KeyRoutedEventArgs args)
      {
         LastKeyPressed = args;
         KeyPressedDateTime = DateTime.UtcNow;
      }

      public bool SameKey(KeyRoutedEventArgs args)
      {
         if (args == null)
         {
            return false;
         }
         var delta = DateTime.UtcNow - KeyPressedDateTime;
         var same = LastKeyPressed.Key == args.Key;
         if (!same)
         {
            LastKeyPressed = null;
         }
         return same;
      }

   }

}
