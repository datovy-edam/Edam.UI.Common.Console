using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using Microsoft.UI.Xaml;
using Windows.Storage.Pickers;
using Windows.Storage;

// -----------------------------------------------------------------------------
using inout = Edam.InOut;

namespace Edam.UI.Common.Controls.Dialogs;


public static class WinUiHelper
{
    public static bool InitializePicker(FileOpenPicker picker)
    {
        if (Window.Current == null)
        {
            WinRT.Interop.InitializeWithWindow.Initialize(
               picker, GetActiveWindow());
            return true;
        }
        return false;
    }

    public static bool InitializePicker(FolderPicker picker)
    {
        if (Window.Current == null)
        {
            WinRT.Interop.InitializeWithWindow.Initialize(
               picker, GetActiveWindow());
            return true;
        }
        return false;
    }

    public static bool InitializePicker(FileSavePicker picker)
    {
        if (Window.Current == null)
        {
            WinRT.Interop.InitializeWithWindow.Initialize(
               picker, GetActiveWindow());
            return true;
        }
        return false;
    }

    [ComImport, Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1"),
       InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInitializeWithWindow
    {
        void Initialize([In] IntPtr hwnd);
    }

    [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto,
       PreserveSig = true, SetLastError = false)]
    public static extern IntPtr GetActiveWindow();
}

public class StoragePickerHelper
{

    public inout.FolderFileItemInfo LastSelectedItem { get; set; }

    /// <summary>
    /// Process Add Items Results
    /// </summary>
    /// <param name="result"></param>
    private void ProcessAddItemsResult(Dialogs.IDialogObjectInfo result)
    {
        inout.FolderFileItemInfo storage = result.Result as inout.FolderFileItemInfo;
        if (storage != null)
        {
            LastSelectedItem = storage;

            // do something with the list
            if (storage.Children.Count > 0)
            {

            }

            if (result.StorageAction == StorageAction.Save)
            {
                String txt = result.DataObject as String;
                if (txt != null)
                {
                    System.IO.File.WriteAllText(result.StorageItem.Full, txt);
                }
            }
        }
    }

    /// <summary>
    /// Add multiple items selected from the file picker.
    /// </summary>
    public void ItemSave(string text, string suggestedName, string fileType)
    {
        StorageInfo storageInfo = new StorageInfo();
        storageInfo.DataObject = text;
        storageInfo.ItemType = fileType;
        storageInfo.ItemName = suggestedName;
        storageInfo.StorageAction = StorageAction.Save;
        storageInfo.CallBack = ProcessAddItemsResult;
        StoragePickerDialog.SavePickerAsync(storageInfo);
    }

    public void ItemOpen(
       Action<IDialogObjectInfo> callBack, string commandText)
    {
        StorageInfo storageInfo = new StorageInfo();
        storageInfo.StorageAction = StorageAction.Open;
        storageInfo.CallBack = callBack;
        storageInfo.CommandText = commandText;
        StoragePickerDialog.SilgleFilePickerAsync(storageInfo);
    }

}

/// <summary>
/// Provide access to Supported Storage Pickers...
/// </summary>
public class StoragePickerDialog
{

    private static void SetupFileTypeFilters(
       IList<string> pickerItems, List<string> items)
    {
        if (items == null || items.Count == 0)
        {
            pickerItems.Add(".txt");
            pickerItems.Add(".json");
            pickerItems.Add(".xml");
        }
        else
        {
            pickerItems.Clear();
            foreach (var i in items)
            {
                pickerItems.Add(i);
            }
        }
    }

    private static void SetupFileTypeFilters(
       IDictionary<string, IList<string>> pickerItems, List<string> items)
    {
        if (items == null || items.Count == 0)
        {
            pickerItems.Add(
               "Plain Text", new List<string>() { ".txt", ".json", ".xml" });
        }
        else
        {
            pickerItems.Clear();
            foreach (var i in items)
            {
                pickerItems.Add("File Types", items);
            }
        }
    }

    /// <summary>
    /// Open File Picker and return list of files
    /// </summary>
    public static async void MultiFilesPickerAsync(IStorageInfo info)
    {
        FileOpenPicker picker = new();
        picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
        SetupFileTypeFilters(picker.FileTypeFilter, info.FileTypeFilter);

        if (WinUiHelper.InitializePicker(picker))
        {
            var files = await picker.PickMultipleFilesAsync();

            if (files != null)
            {
                info.StorageItem = new inout.FolderFileItemInfo();
                foreach (var file in files.ToList())
                {
                    info.StorageItem.AddFile(file.Path, null);
                }

                if (info.CallBack != null)
                {
                    info.Result = info.StorageItem;
                    info.CallBack(info);
                }
                //var text = await FileIO.ReadTextAsync(file);
                //MessageDialog dlg = new(text);
                //await dlg.ShowAsync();
            }
        }
    }

    /// <summary>
    /// Open File Picker and return one file.
    /// </summary>
    /// <param name="info">storage info..</param>
    public static async void SilgleFilePickerAsync(IStorageInfo info)
    {
        FileOpenPicker picker = new();
        picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        SetupFileTypeFilters(picker.FileTypeFilter, info.FileTypeFilter);
        picker.ViewMode = PickerViewMode.List;

        if (WinUiHelper.InitializePicker(picker))
        {
            var file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                info.StorageItem = new inout.FolderFileItemInfo(file.Path, null);
                if (info.CallBack != null)
                {
                    info.Result = info.StorageItem;
                    info.CallBack(info);
                }
                //var text = await FileIO.ReadTextAsync(file);
                //MessageDialog dlg = new(text);
                //await dlg.ShowAsync();
            }
        }
    }

    /// <summary>
    /// Open File Picker and return one file.
    /// </summary>
    public static async void SavePickerAsync(IStorageInfo info)
    {
        FileSavePicker picker = new();
        picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        SetupFileTypeFilters(picker.FileTypeChoices, info.FileTypeFilter);

        picker.SuggestedFileName =
           String.IsNullOrWhiteSpace(info.ItemName) ?
              "" : info.ItemName;

        if (WinUiHelper.InitializePicker(picker))
        {
            var file = await picker.PickSaveFileAsync();

            if (file != null)
            {
                info.StorageItem = new inout.FolderFileItemInfo(file.Path, null);
                if (info.CallBack != null)
                {
                    info.Result = info.StorageItem;
                    info.CallBack(info);
                }
                //var text = await FileIO.ReadTextAsync(file);
                //MessageDialog dlg = new(text);
                //await dlg.ShowAsync();
            }
        }
    }

    /// <summary>
    /// Open File Picker and return one file.
    /// </summary>
    public static async void FolderPickerAsync(IStorageInfo info)
    {
        FolderPicker picker = new();
        picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
        SetupFileTypeFilters(picker.FileTypeFilter, info.FileTypeFilter);

        if (WinUiHelper.InitializePicker(picker))
        {
            var folder = await picker.PickSingleFolderAsync();

            if (folder != null)
            {
                if (info.CallBack != null)
                {
                    info.CallBack(info);
                }
                //var text = await FileIO.ReadTextAsync(file);
                //MessageDialog dlg = new(text);
                //await dlg.ShowAsync();
            }
        }
    }

}
