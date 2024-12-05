using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using app = Edam.UI.Common.Application;
using Edam.UI.Common.Controls.Helpers;
using Edam.DataObjects.Dynamic;
using Edam.DataObjects.Models;

namespace Edam.UI.Common.Controls.Dialogs;


public class DialogBox
{
  public const string SUBMIT = "Submit";

  /// <summary>
  /// General Message Box Dialog...
  /// </summary>
  /// <param name="title"></param>
  /// <param name="message"></param>
  /// <param name="callback"></param>
  /// <param name="primaryText"></param>
  /// <param name="secondaryText"></param>
  /// <param name="closeText"></param>
  public static async void ShowDialog(IDialogContentInfo info)
  {
     // prepare the ContentDialog
     string cText =
        String.IsNullOrWhiteSpace(info.CloseText) ?
        DialogMessageBox.COMMAND_CANCEL : info.CloseText;
     string pText =
        String.IsNullOrWhiteSpace(info.PrimaryText) ?
        DialogMessageBox.COMMAND_OK : info.PrimaryText;

     ContentDialog dialogBox = new ContentDialog
     {
        Title = info.Title,
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

     dialogBox.Content = ControlHelper.GetEditorContent(info.ModelData);
     ControlHelper.SetControlsDataValue(
        info.ModelData.ModelObject, info.ModelData.Columns);

     // show dialog...
     ContentDialogResult result = await dialogBox.ShowAsync();

     // fetch the values in the controls and set those into model data items
     dynamic data = ControlHelper.GetControlsData(
        info.ModelData.ModelObject, info.ModelData.Columns, 
        out int changedCount);

     // dispose of resources
     ControlHelper.Dispose(info.ModelData);

     // invoke the CallBack if any was provided...
     if (info.CallBack != null)
     {
        info.Result = data;
        info.CommandText =
           result == ContentDialogResult.Primary ? pText : null;
        info.CallBack(info);
     }
  }

  /// <summary>
  /// Using information in a Reference Data Template prepare a dialog box
  /// and show it.
  /// </summary>
  /// <param name="dataTemplateFilePath">data template file path</param>
  /// <param name="dataEntityName">data entity name</param>
  /// <param name="dataEntityDescription">data entity description</param>
  /// <param name="callBack">(optional) call back function</param>
  /// <param name="PrimaryButtonText">primary button text</param>
  public static void ShowDialog(string dataTemplateFilePath, 
     string dataEntityName, string dataEntityDescription,
     Action<IDialogObjectInfo> callBack, string PrimaryButtonText)
  {
     ElementNodeInfo node = ApplicationElementInfo.GetElementNode(
        dataTemplateFilePath, dataEntityName, dataEntityDescription);

     Dialogs.DialogInfo d = new Dialogs.DialogInfo
     {
        Title = dataEntityDescription,
        ItemName = node.Name,
        ItemType = Dialogs.DialogInfo.ITEM_TYPE_FILE,
        CallBack = callBack,
        DataObject = node,
        PrimaryText = PrimaryButtonText,
        ModelData = node.GetModelData()
     };

     DialogBox.ShowDialog(d);
  }

  /// <summary>
  /// Using information in a Reference Data Template prepare a dialog box
  /// and show it.
  /// </summary>
  /// <param name="node">data template node</param>
  /// <param name="dataEntityName">data entity name</param>
  /// <param name="dataEntityDescription">data entity description</param>
  /// <param name="callBack">(optional) call back function</param>
  /// <param name="PrimaryButtonText">primary button text</param>
  public static void ShowDialog(
     ElementNodeInfo node, string dataEntityDescription,
     Action<IDialogObjectInfo> callBack, string PrimaryButtonText)
  {
     Dialogs.DialogInfo d = new Dialogs.DialogInfo
     {
        Title = dataEntityDescription,
        ItemName = node.Name,
        ItemType = Dialogs.DialogInfo.ITEM_TYPE_FILE,
        CallBack = callBack,
        DataObject = node,
        PrimaryText = PrimaryButtonText,
        ModelData = node.GetModelData()
     };

     DialogBox.ShowDialog(d);
  }

}
