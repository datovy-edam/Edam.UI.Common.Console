using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;

// -----------------------------------------------------------------------------
using prjs = Edam.Data.AssetProject;
using Edam.UI.Common;
using Edam.Diagnostics;
using Edam.InOut;
using Edam.UI.Controls.DataModels;
using Edam.DataObjects.Models;
using Edam.UI.Common.Controls.Dialogs;
using Edam.Data.AssetProject;
using Edam.Data.AssetSchema;
using uiApp = Edam.Application.Settings;

namespace Edam.UI.Controls.Projects;


public class ProjectViewerViewModel : ObservableObject
{

    #region -- 1.00 - Properties

    public const string TAB_FILE = "FileTab";
    public const string TAB_ASSET = "AssetTab";
    public const string TAB_USE_CASE = "UseCaseTab";
    public const string TAB_DOMAINS = "DomainsTab";

    public const string SUPPORTED_EXTENSIONS =
       "xml xsd json jsonld txt sql gsql csv";
    private static string[] m_SupportedExtensions =
       SUPPORTED_EXTENSIONS.Split(" ");

    private const string PROJECT = "Project";
    private const string PROJECT_NAME = "ProjectName";
    private const string DESCRIPTION = "Description";
    private const string PROJECT_NEW = "New Project";
    private const string PROJECT_DIALOG_NEW = "Project.NewProject.Dialog";

    public bool AllowItemEditing { get; set; }

    private ObservableCollection<UriItemInfo> m_UriItems;
    public ObservableCollection<UriItemInfo> UriItems
    {
        get { return m_UriItems; }
        set
        {
            if (m_UriItems != value)
            {
                m_UriItems = value;
                OnPropertyChanged(nameof(m_UriItems));
            }
        }
    }

    private UriItemInfo m_SelectedUriItem;
    public UriItemInfo SelectedUriItem
    {
        get { return m_SelectedUriItem; }
        set
        {
            if (value != m_SelectedUriItem)
            {
                m_SelectedUriItem = value;
                OnPropertyChanged(nameof(m_SelectedUriItem));
                FetchProjectFolderInfo();
            }
        }
    }

    private string m_SelectedItemName;
    public string SelectedItemName
    {
        get { return m_SelectedItemName; }
        set
        {
            if (m_SelectedItemName != value)
            {
                m_SelectedItemName = value;
                OnPropertyChanged("SelectedItemName");
            }
        }
    }

    private ProjectItem m_PreviousSelectedItem;
    public ProjectItem PreviousSelectedItem
    {
        get { return m_PreviousSelectedItem; }
        set
        {
            if (m_PreviousSelectedItem != value)
            {
                m_PreviousSelectedItem = value;
                OnPropertyChanged("PreviousSelectedItem");
            }
        }
    }

    private string m_NotifiedMessageText;
    public string NotifiedMessageText
    {
        get { return m_NotifiedMessageText; }
        set
        {
            if (value != m_NotifiedMessageText)
            {
                m_NotifiedMessageText = value;
                OnPropertyChanged("NotifiedMessageText");
            }
        }
    }

    /// <summary>
    /// The TreeItem refers to the root of the visual tree...
    /// </summary>
    private ProjectItem m_TreeView;
    public ProjectItem TreeView
    {
        get { return m_TreeView; }
        set
        {
            if (m_TreeView != value)
            {
                m_TreeView = value;
                ProjectContext.TreeView = value;
                OnPropertyChanged(nameof(TreeView));
            }
        }
    }

    /// <summary>
    /// A project has been selected.
    /// </summary>
    private ProjectItem m_SelectedItem;
    public ProjectItem SelectedItem
    {
        get { return m_SelectedItem; }
        set
        {
            if (m_SelectedItem != value)
            {
                SelectedItemName = value == null ?
                   String.Empty : value.Item.NameFull;
                m_SelectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }
    }

    /// <summary>
    /// The LastSelectedItem refers to any item within the tree that has been
    /// selected.
    /// </summary>
    private ProjectItem m_LastSelectedItem;
    public ProjectItem LastSelectedItem
    {
        get { return m_LastSelectedItem; }
        set
        {
            if (m_LastSelectedItem != value)
            {
                m_LastSelectedItem = value;
                OnPropertyChanged("LastSelectedItem");
            }
        }
    }

    public NotificationEvent NotifyEventCompletion { get; set; }

    private Visibility m_ShowProjectSidePanel;
    public Visibility ShowProjectSidePanel
    {
        get { return m_ShowProjectSidePanel; }
        set
        {
            if (m_ShowProjectSidePanel != value)
            {
                m_ShowProjectSidePanel = value;
                OnPropertyChanged("ShowProjectSidePanel");
            }
        }
    }

    private Visibility m_AssetViewerVisibility;
    public Visibility AssetViewerVisibility
    {
        get { return m_AssetViewerVisibility; }
        set
        {
            if (m_AssetViewerVisibility != value)
            {
                m_AssetViewerVisibility = value;
                OnPropertyChanged("AssetViewerVisibility");
            }
        }
    }

    private Visibility m_AssetMapViewerVisibility;
    public Visibility AssetMapViewerVisibility
    {
        get { return m_AssetMapViewerVisibility; }
        set
        {
            if (m_AssetMapViewerVisibility != value)
            {
                m_AssetMapViewerVisibility = value;
                OnPropertyChanged("AssetMapViewerVisibility");
            }
        }
    }

    private Visibility m_ShowAssetSidePanel;
    public Visibility ShowAssetSidePanel
    {
        get { return m_ShowAssetSidePanel; }
        set
        {
            if (m_ShowAssetSidePanel != value)
            {
                m_ShowAssetSidePanel = value;
                OnPropertyChanged("ShowAssetSidePanel");
            }
        }
    }

    private string m_SidePanelTab;
    public string SidePanelTab
    {
        get { return m_SidePanelTab; }
        set
        {
            if (value != m_SidePanelTab)
            {
                m_SidePanelTab = value;
                ShowProjectSidePanel = value == TAB_FILE ?
                   Visibility.Visible : Visibility.Collapsed;
                ShowAssetSidePanel = value == TAB_FILE ?
                   Visibility.Collapsed : Visibility.Visible;
                OnPropertyChanged("SidePanelTab");
            }
        }
    }

    #endregion
    #region -- 4.00 - Constructor...

    public ProjectViewerViewModel()
    {
        PrepareUriItems();
        AllowItemEditing = false;
        SidePanelTab = TAB_FILE;
        SetAssetViewerVisibility(Visibility.Visible);

        if (UriItems.Count > 0)
        {
            SelectedUriItem = UriItems.First();
        }
        //FetchProjectFolderInfo();
    }

    #endregion
    #region -- 4.00 - Support methods...

    private void PrepareUriItems()
    {
        ObservableCollection<UriItemInfo> l =
           new ObservableCollection<UriItemInfo>();
        var items = uiApp.AppSettings.GetUriList(UriType.ConsolePath);
        foreach (var i in items)
        {
            l.Add(i);
        }
        UriItems = l;
    }

    public void SetAssetViewerVisibility(Visibility? visibility)
    {
        AssetViewerVisibility = visibility.HasValue ?
           visibility.Value : Visibility.Visible;
        AssetMapViewerVisibility =
           AssetViewerVisibility == Visibility.Visible ?
              Visibility.Collapsed : Visibility.Visible;
    }

    public void ShowFileTab()
    {
        SidePanelTab = TAB_FILE;
    }
    public void ShowAssetTab()
    {
        SidePanelTab = TAB_ASSET;
    }

    /// <summary>
    /// Identify what kind of Assets are being worked on...
    /// </summary>
    /// <param name="name">TAB Viewer selected TAB Name</param>
    public void SetSidePanel(string name)
    {
        switch (name)
        {
            case TAB_DOMAINS:
                ShowFileTab();
                ProjectContext.AssetType = AssetType.Asset;
                break;
            case TAB_FILE:
                ShowFileTab();
                ProjectContext.AssetType = AssetType.Asset;
                break;
            case TAB_ASSET:
                ShowAssetTab();
                ProjectContext.AssetType = AssetType.Asset;
                break;
            case TAB_USE_CASE:
                ProjectContext.AssetType = AssetType.UseCase;
                break;
            default:
                ShowAssetTab();
                ProjectContext.AssetType = AssetType.Asset;
                break;
        }
    }

    public static bool IsSupportedExtensions(string extension)
    {
        if (String.IsNullOrWhiteSpace(extension))
        {
            return false;
        }
        string ext = extension[0] == '.' ? extension.Substring(1) : extension;
        return m_SupportedExtensions.Contains(ext.ToLower());
    }

    public void FetchProjectFolderInfo()
    {
        var item = SelectedUriItem;
        FolderFileItemInfo ffinfo = prjs.Project.GetProjectItems(
           SelectedUriItem == null ? null : SelectedUriItem.UriText);
        TreeView = ProjectDataModel.ToObservable(ffinfo);
        Project.SetProjectsPath(item.UriText);
    }

    public void ManageResults(IResultsLog results)
    {

    }

    public void ClearResetAll()
    {
        SelectedItemName = String.Empty;
        NotifiedMessageText = String.Empty;
    }

    public bool NewItemSelected(ProjectItem item)
    {
        ClearResetAll();
        if (item == null)
        {
            return false;
        }
        SelectedItem = item;
        SelectedItemName = item.Item.NameFull;
        return true;
    }

    #endregion
    #region -- 4.00 - Select Tree View Item

    public void SetItem(object item)
    {
        LastSelectedItem = item as ProjectItem;
    }

    /// <summary>
    /// Compare provided item with last selected...
    /// </summary>
    /// <param name="item"></param>
    /// <returns>true is returned if those are the same</returns>
    public bool SelectedCompare(object item)
    {
        ProjectItem itm = item as ProjectItem;
        if (itm == null)
        {
            if (LastSelectedItem == null)
            {
                return true;
            }
            return false;
        }
        if (LastSelectedItem == null)
        {
            return false;
        }
        return itm.Item.Full == LastSelectedItem.Item.Full;
    }

    /// <summary>
    /// Tree Item haas been selected, setup editor...
    /// </summary>
    /// <param name="tree"></param>
    public ResultsLog<ProjectItem> TreeItemSelected(object item)
    {
        ResultsLog<ProjectItem> results = new ResultsLog<ProjectItem>();
        if (item == null)
        {
            results.Failed("item expected none found.");
            return results;
        }

        string returnMessage = String.Empty;
        ProjectItem itm = item as ProjectItem;

        if (itm != null)
        {
            if (itm.TextEditorVisibility == Visibility.Visible)
            {
                results.Succeeded();
                return results;
            }

            if (itm.Item.Type != ItemType.File)
            {
                SelectedItem = null;
                SelectedItemName = String.Empty;
                results.Succeeded();
                return results;
            }
        }

        LastSelectedItem = itm;
        if (itm != null &&
           ProjectViewerViewModel.IsSupportedExtensions(itm.Item.Extension))
        {
            results.Instance = itm;
            SelectedItem = itm;
            SelectedItemName = itm.Item.NameFull;
            itm.SetEditorVisibility(false);
            returnMessage = System.IO.File.ReadAllText(itm.Item.Full);
            results.ReturnText = returnMessage;
            results.Succeeded();
        }
        else
        {
            SelectedItem = null;
            SelectedItemName = String.Empty;
            results.ReturnText = String.Empty;
            results.Succeeded();
            return results;
            //returnMessage = "FILE TYPE OR EXTENSION (" +
            //   itm.Item.Extension + ") NOT SUPPORTED";
        }

        return results;
    }

    public void TreeItemClose(ProjectItem item)
    {
        if (item.TextEditorVisibility == Visibility.Visible)
        {
            item.SetEditorVisibility(false);
        }
        foreach (var i in item.Children)
        {
            TreeItemClose(i);
        }
    }

    public void TreeItemSetupEditor(
       object item, bool visible = true, bool cancel = false)
    {
        ProjectItem itm = item as ProjectItem;
        LastSelectedItem = itm;

        if (!AllowItemEditing)
        {
            return;
        }

        if (itm != null)
        {
            if ((PreviousSelectedItem != null &&
               PreviousSelectedItem.Item.Full != itm.Item.Full) ||
               PreviousSelectedItem == null)
            {
                if (PreviousSelectedItem != null)
                {
                    PreviousSelectedItem.SetEditorVisibility(false);
                }
                PreviousSelectedItem = itm;
                return;
            }

            TreeItemClose(TreeView);
            PreviousSelectedItem = visible ? itm : null;
            itm.SetEditorVisibility(visible);
            if (cancel)
            {
                itm.SelectedText = itm.Item.NameFull;
                return;
            }
        }
    }

    #endregion
    #region -- 4.00 - Delete Tree View Item

    /// <summary>
    /// Process Result...
    /// </summary>
    /// <param name="result"></param>
    private void ProcessResult(IDialogObjectInfo result)
    {
        var rslt = result as IDialogObjectInfo;
        if (rslt == null || String.IsNullOrWhiteSpace(rslt.CommandText))
        {
            return;
        }

        if (rslt.CommandText == DialogMessageBox.COMMAND_DELETE)
        {
            var item = rslt.DataObject as ProjectItem;
            if (item != null)
            {
                Diagnostics.ResultLog results = new ResultLog();

                try
                {
                    System.IO.File.Delete(item.Item.Full);
                    ProjectHelper.DeleteItem(TreeView, item.Item.Full);
                }
                catch (Exception ex)
                {
                    results.Failed(ex);
                }

                ManageResults(results);
            }
        }
    }

    /// <summary>
    /// Delete tree item...
    /// </summary>
    /// <param name="tree"></param>
    public void DeleteTreeItem(TreeView tree)
    {
        ProjectItem itm = tree.SelectedItem as ProjectItem;
        if (itm == null || itm.Item == null ||
           String.IsNullOrWhiteSpace(itm.Item.NameFull))
        {
            return;
        }

        DialogInfo d = new DialogInfo
        {
            ItemName = itm.Item.NameFull,
            ItemType = DialogInfo.ITEM_TYPE_FILE,
            CallBack = ProcessResult,
            DataObject = itm
        };

        DialogMessageBox.DeleteItem(d);
    }

    #endregion
    #region -- 4.00 - Tree Item Save/Copy Methods

    /// <summary>
    /// Make a copy of the currently selected item...
    /// </summary>
    public void ItemCopy()
    {

    }

    /// <summary>
    /// Save edited text...
    /// </summary>
    /// <param name="fileName">name of the file to be updated</param>
    /// <param name="text">text to save</param>
    public void ItemSave(string fileName, string text)
    {
        Diagnostics.ResultLog results = new ResultLog();

        if (SelectedItem == null || SelectedItem.Item.NameFull != fileName)
        {
            return;
        }

        try
        {
            System.IO.File.WriteAllText(SelectedItem.Item.Full, text);
            results.Succeeded();
        }
        catch (Exception ex)
        {
            results.Failed(ex);
        }

        ManageResults(results);
    }

    /// <summary>
    /// Add multiple items selected from the file picker.
    /// </summary>
    public void ItemSave(string text)
    {
        StorageInfo storageInfo = new StorageInfo();
        storageInfo.CallBack = ProcessAddItemsResult;
        StoragePickerDialog.SavePickerAsync(storageInfo);
    }

    #endregion
    #region -- 4.00 - Project Add/New Dialog Support Methods

    /// <summary>
    /// Manage the New-Project dialog results and as needed, create the 
    /// project and show it on the Items-Tree control...
    /// </summary>
    /// <param name="result">dialog results passed in the callBack</param>
    private void ProcessNewProjectResult(
       IDialogObjectInfo result)
    {
        var rslt = result as IDialogObjectInfo;
        if (rslt == null || String.IsNullOrWhiteSpace(rslt.CommandText))
        {
            return;
        }

        ElementNodeInfo node = result.DataObject as ElementNodeInfo;
        if (rslt.CommandText == DialogBox.SUBMIT && node != null &&
           node.Description == PROJECT_NEW)
        {
            var pname = node.GetItem(PROJECT_NAME);
            var pdesc = node.GetItem(DESCRIPTION);
            if (pname == null || pdesc == null)
            {
                return;
            }

            var results = Project.CreateProject(pname.ValueText, false);
            string path = results.ResultValueObject as string;
            if (!String.IsNullOrWhiteSpace(path))
            {
                FolderFileItemInfo ffinfo = Project.GetProjectItems(path);
                var tItem = ProjectDataModel.ToObservable(ffinfo);
                TreeView.Children.Add(tItem);
            }
        }
    }

    /// <summary>
    /// Add a new Project...
    /// </summary>
    public void ProjectAddNew()
    {
        DialogBox.ShowDialog(
           PROJECT_DIALOG_NEW, PROJECT, PROJECT_NEW, ProcessNewProjectResult,
           DialogBox.SUBMIT);
    }

    /// <summary>
    /// Process Add Items Results
    /// </summary>
    /// <param name="result"></param>
    private void ProcessAddItemsResult(IDialogObjectInfo result)
    {
        FolderFileItemInfo storage = result.Result as FolderFileItemInfo;
        if (storage != null && storage.Children.Count > 0)
        {
            var targetFolder = LastSelectedItem.ItemFolderPath;
            if (targetFolder == null)
            {
                return;
            }

            // copy items to this folder
        }
    }

    /// <summary>
    /// Add multiple items selected from the file picker.
    /// </summary>
    public void ProjectAddItems()
    {
        StorageInfo storageInfo = new StorageInfo();
        storageInfo.CallBack = ProcessAddItemsResult;
        StoragePickerDialog.MultiFilesPickerAsync(storageInfo);
    }

    #endregion

}
