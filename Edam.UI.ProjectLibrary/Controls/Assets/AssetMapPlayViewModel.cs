using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

// -----------------------------------------------------------------------------
using Edam.Data.AssetConsole;
using Edam.Data.AssetSchema;
using Edam.Data.AssetProject;
using Edam.InOut;
using Edam.UI.Controls.ViewModels;
using Edam.UI.Controls.DataModels;
using Edam.Json.JsonQuery;

using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using Edam.Diagnostics;
using Edam.UI.Common.Controls.Dialogs;

namespace Edam.UI.Controls.Assets;


public class AssetMapPlayViewModel : ObservableObject
{

    private DataMapContext m_Context;
    public DataMapContext Context
    {
        get { return m_Context; }
    }

    public Action<IDialogObjectInfo> SetText { get; set; }

    public StoragePickerHelper storageHelper = new StoragePickerHelper();

    public void SetDataMapContext(DataMapContext context)
    {
        m_Context = context;
    }

    public IResultsLog ExecuteRequest(string jsonText, string query)
    {
        return Context.LanguageInstance.Execute(jsonText, query);
    }

    /// <summary>
    /// Add multiple items selected from the file picker.
    /// </summary>
    public void ItemSave(string text)
    {
        storageHelper.ItemSave(text, "", null);
    }

    /// <summary>
    /// Open File Call Back...
    /// </summary>
    /// <param name="info">file info details</param>
    public void OpenFileCallBack(IDialogObjectInfo info)
    {
        var txt = System.IO.File.ReadAllText(info.StorageItem.Full);
        if (txt != null)
        {
            if (SetText != null)
            {
                info.DataObject = txt;
                SetText(info);
            }
        }
    }

    public void ItemOpen(string commandText)
    {
        storageHelper.ItemOpen(OpenFileCallBack, commandText);
    }

}
