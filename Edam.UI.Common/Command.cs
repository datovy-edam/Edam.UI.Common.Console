using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;

/*
 * Don't forget to set XamlControlsResources as your Application resources in App.xaml:

    <Application>
        <Application.Resources>
            <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />
        </Application.Resources>
    </Application>

If you have other resources, then we recommend you add those to the XamlControlsResources' MergedDictionaries.
This works with the platform's resource system to allow overrides of the XamlControlsResources resources.

<Application
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:controls="using:Microsoft.UI.Xaml.Controls">
    <Application.Resources>
        <controls:XamlControlsResources>
            <controls:XamlControlsResources.MergedDictionaries>
                <!-- Other app resources here -->
            </controls:XamlControlsResources.MergedDictionaries>
        </controls:XamlControlsResources>
    </Application.Resources>
</Application>

See http://aka.ms/winui for more information.
 */

namespace Edam.UI
{

   public class Command : StandardUICommand, ICommand
   {
      private Action m_Action;

      public Command(Action action) : base()
      { 
         m_Action = action;
         this.ExecuteRequested += new Windows.Foundation.TypedEventHandler<
            XamlUICommand, ExecuteRequestedEventArgs>(this.CommandHandler);
      }

      //public event EventHandler CanExecuteCharnged;

      public void CommandHandler(
         XamlUICommand sender, ExecuteRequestedEventArgs args)
      {
         if (m_Action == null)
            return;
         m_Action();
      }
   }

}
