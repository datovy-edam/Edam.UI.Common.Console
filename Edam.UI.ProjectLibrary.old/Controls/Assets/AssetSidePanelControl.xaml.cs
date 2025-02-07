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
using Edam.Data.AssetSchema;
using Edam.UI.Controls.DataModels;
using Edam.UI.Common;
using Edam.Application;

namespace Edam.UI.Controls.Assets;


public sealed partial class AssetSidePanelControl : UserControl
{
  private MapSidePanelViewModel m_ViewModel;
  public MapSidePanelViewModel ViewModel
  {
     get { return m_ViewModel; }
  }
  public AssetSidePanelControl()
  {
     this.InitializeComponent();
     m_ViewModel = new MapSidePanelViewModel();
     DataContext = m_ViewModel;
     m_ViewModel.DataTreeControl = TreeView;
  }

  public void ManageNotification(object sender, NotificationArgs args)
  {
     if (args.MessageText != AssetViewOption.DataMapView.ToString())
     {
        return;
     }

     DataMapContext context = args.EventData as DataMapContext;
     if (context == null)
     {
        return;
     }

     context.Source.Instance = TreeView;
     TreeView.ViewModel.SetMapContext(context, MapItemType.Source);
  }

  private void AssetRefresh_Click(object sender, RoutedEventArgs e)
  {
    
  }

  public void AssetDataChanged(object dataItem)
  {
     AssetData assetData = dataItem as AssetData;
     if (assetData != null)
     {
        var tree = AssetDataTree.GetDataTree(ProjectContext.Arguments, 6);
        if (tree != null)
        {
           TreeView.SetDataTree(tree);
        }
        else
        {
           Session.ShowMessageBox("Tree Preparation",
              "Failed to find root element or other issue. " +
              "Check Arguments RootElementName and try again.");
        }
        ViewModel.AssetDataChanged(dataItem);
     }
  }

  /// <summary>
  /// Manage the item save-option processing (see ProjectHelper for the
  /// details on how it is process using ProcessSaveOption).
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void SaveOption_SelectionChanged(
     object sender, SelectionChangedEventArgs e)
  {
     if (e.AddedItems.Count == 0)
     {
        return;
     }
     ViewModel.SaveOptionChanged(e.AddedItems[0].ToString());

     // allow for a new selection...
     SaveOption.SelectedItem = null;
  }

  private void DataAssetView_Click(object sender, RoutedEventArgs e)
  {
     ViewModel.DataViewChanged(AssetViewOption.DataAssetView);
  }

  private void DataTreeView_Click(object sender, RoutedEventArgs e)
  {
     ViewModel.DataViewChanged(AssetViewOption.DataTreeView);
  }

  private void DataMapView_Click(object sender, RoutedEventArgs e)
  {
     ViewModel.DataViewChanged(AssetViewOption.DataMapView);
  }
}
