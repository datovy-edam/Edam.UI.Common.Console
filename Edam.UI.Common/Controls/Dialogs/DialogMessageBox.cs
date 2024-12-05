using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml.Controls;

using mapp = Edam.Application;
using app = Edam.UI.Common.Application;

namespace Edam.UI.Common.Controls.Dialogs;


public class DialogMessageBox : mapp.IMessageBox
{
  public const string COMMAND_DELETE = "Delete";
  public const string COMMAND_CANCEL = "Cancel";
  public const string COMMAND_YES = "Yes";
  public const string COMMAND_NO = "No";
  public const string COMMAND_DONE = "Done";
  public const string COMMAND_OK = "OK";

  public const string MESSAGE_SURE = "Are you sure?";

  /// <summary>
  /// General Message Box Dialog...
  /// </summary>
  /// <param name="title"></param>
  /// <param name="message"></param>
  /// <param name="callback"></param>
  /// <param name="primaryText"></param>
  /// <param name="secondaryText"></param>
  /// <param name="closeText"></param>
  public static async void ShowMessage(IDialogContentInfo info)
  {
     string cText = 
        String.IsNullOrWhiteSpace(info.CloseText) ? 
        COMMAND_CANCEL : info.CloseText;
     string pText = 
        String.IsNullOrWhiteSpace(info.PrimaryText) ? 
        COMMAND_OK : info.PrimaryText;

     ContentDialog dialogBox = new ContentDialog
     {
        Title = info.Title,
        Content = info.Message,
        PrimaryButtonText = pText,
        CloseButtonText = cText,
        DefaultButton = ContentDialogButton.Primary,
        XamlRoot = app.ApplicationHelper.MainWindow.Content.XamlRoot
     };

     string sText =
        String.IsNullOrWhiteSpace(info.SecondaryText) ? 
        null : info.SecondaryText;
     if (sText != null)
     {
        dialogBox.SecondaryButtonText = sText;
     }

     ContentDialogResult result = await dialogBox.ShowAsync();

     if (info.CallBack != null)
     {
        info.CommandText = 
           result == ContentDialogResult.Primary ? pText : 
           result == ContentDialogResult.Secondary ? cText : null;
        info.CallBack(info);
     }
  }

  /// <summary>
  /// Generic Delete Item (something) dialog box...
  /// </summary>
  /// <param name="itemName">(nullable) Item Name</param>
  /// <param name="itemType">(nullable) item Type (i.e. File)</param>
  /// <param name="callBack">(nullable) call back action text.  Pass null
  /// to just returned without processing result</param>
  /// <param name="title">(optional) title</param>
  public static void DeleteItem(DialogInfo info)
  {
     info.Title = String.IsNullOrWhiteSpace(info.Title) ? 
        MESSAGE_SURE : info.Title;
     info.ItemType = String.IsNullOrWhiteSpace(info.ItemType) ? 
        String.Empty : " " + info.ItemType;
     info.Message = COMMAND_DELETE +
        info.ItemType + (String.IsNullOrWhiteSpace(info.ItemName) ? 
        String.Empty : " (" + info.ItemName + ")?");

     info.PrimaryText = COMMAND_DELETE;
     DialogMessageBox.ShowMessage(info);
  }

  /// <summary>
  /// Process Results of a bool Action...
  /// </summary>
  /// <param name="result"></param>
  private void ProcessResult(Dialogs.IDialogObjectInfo result)
  {
     var rslt = result as Dialogs.IDialogObjectInfo;
     if (rslt == null || String.IsNullOrWhiteSpace(rslt.CommandText))
     {
        return;
     }

     var callback = result.DataObject as Action<bool>;
     if (callback != null)
     {
        callback(true);
     }
  }

  /// <summary>
  /// Show Message Box using given info.
  /// </summary>
  /// <param name="prompt"></param>
  /// <param name="message"></param>
  /// <param name="callback"></param>
  /// <param name="type"></param>
  public void ShowMessageBox(string prompt, string message,
     Action<bool> callback, mapp.MessageBoxType type)
  {
     DialogInfo dialogInfo = new DialogInfo()
     {
        Title = prompt,
        Message = message,
        CallBack = ProcessResult,
        DataObject = callback
     };
     switch (type)
     {
        case mapp.MessageBoxType.YesNo:
           dialogInfo.PrimaryText = COMMAND_YES;
           dialogInfo.CloseText = COMMAND_NO;
           break;
        case mapp.MessageBoxType.Done:
        default:
           dialogInfo.PrimaryText = COMMAND_DONE;
           break;
     }
     DialogMessageBox.ShowMessage(dialogInfo);
  }

}
