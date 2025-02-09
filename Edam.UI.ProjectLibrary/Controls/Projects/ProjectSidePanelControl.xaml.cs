using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
using Edam.UI.Controls.ViewModels;
using Edam.UI.Common.Controls;
using Edam.UI.Common;

namespace Edam.UI.Controls.Projects;

public sealed partial class ProjectSidePanelControl : UserControl
{
    private ProjectViewerViewModel m_ViewModel;
    public ProjectViewerViewModel ViewModel
    {
        get { return m_ViewModel; }
        set { m_ViewModel = value; }
    }
    public ProjectSidePanelControl()
    {
        this.InitializeComponent();
        m_ViewModel = new ProjectViewerViewModel();
        DataContext = m_ViewModel;
    }

    private void ProjectRefresh_Click(object sender, RoutedEventArgs e)
    {
        m_ViewModel.FetchProjectFolderInfo();
    }

    private void ProjectAdd_Click(object sender, RoutedEventArgs e)
    {
        m_ViewModel.ProjectAddNew();
    }

    private void TreeViewItem_PointerPressed(
       object sender, PointerRoutedEventArgs e)
    {
    }

    private void TreeItemCopy_Click(object sender, RoutedEventArgs e)
    {
        m_ViewModel.ItemCopy();
    }

    private void TreeAddItems_Click(object sender, RoutedEventArgs e)
    {
        m_ViewModel.SetItem(TreeView.SelectedItem);
        m_ViewModel.ProjectAddItems();
    }

    private void ProjectRename_Click(object sender, RoutedEventArgs e)
    {

    }

    #region -- 4.00 - Select Item Support Methods...

    /// <summary>
    /// The Arguments are selected... notify others.
    /// </summary>
    /// <param name="selectedItem">selected item</param>
    private void ItemSelected(object selectedItem = null)
    {
        object sitem = selectedItem == null ?
           TreeView.SelectedItem : selectedItem;
        var results = m_ViewModel.TreeItemSelected(sitem);

        if (results.Success)
        {
            string mess = String.IsNullOrWhiteSpace(results.ReturnText) ?
               String.Empty : results.ReturnText;

            if (m_ViewModel.NotifyEventCompletion != null)
            {
                NotificationArgs args = new NotificationArgs();
                args.Type = NotificationType.ProjectItemSelected;
                args.ResultsLog = results;
                args.EventData = results.Instance;
                m_ViewModel.NotifyEventCompletion(this, args);
            }
            //AssetEditorControl.SetEditorText(
            //   results.Data, results.ReturnText);
        }
    }

    private void TextBlock_DoubleTapped(
       object sender, DoubleTappedRoutedEventArgs e)
    {
        ItemSelected();
    }

    private void TreeItemEdit_Click(object sender, RoutedEventArgs e)
    {
        ItemSelected();
    }

    #endregion
    #region -- 4.00 - Manage Item Text Viewing and Editing

    private void ItemText_PointerPressed(
       object sender, PointerRoutedEventArgs e)
    {
        var txtBox = e.OriginalSource as TextBlock;
        if (txtBox != null)
        {
            m_ViewModel.TreeItemSetupEditor(txtBox.DataContext);
            ItemSelected(txtBox.DataContext);
        }
    }

    private void ItemTextEdit_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            m_ViewModel.TreeItemSetupEditor(TreeView.SelectedItem, false);
        }
        else if (e.Key == Windows.System.VirtualKey.Escape)
        {
            m_ViewModel.TreeItemSetupEditor(TreeView.SelectedItem, false, true);
        }
    }

    #endregion
    #region -- 4.00 - Delete Tree Item (File or Folder)

    private void TreeItemDelete_Click(object sender, RoutedEventArgs e)
    {
        m_ViewModel.SetItem(TreeView.SelectedItem);
        m_ViewModel.DeleteTreeItem(TreeView);
    }

    private void TreeViewItem_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Delete ||
           e.Key == Windows.System.VirtualKey.Back)
        {
            m_ViewModel.SetItem(TreeView.SelectedItem);
            m_ViewModel.DeleteTreeItem(TreeView);
            e.Handled = true;
        }
    }

    #endregion

}
