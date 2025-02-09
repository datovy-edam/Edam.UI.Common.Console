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

// -----------------------------------------------------------------------------
using Edam.Data.AssetConsole;
using Edam.Data.AssetSchema;
using Edam.Diagnostics;
using Edam.Data.AssetProject;
using Edam.InOut;
using Edam.UI.Controls.ViewModels;
using Edam.UI.Controls.DataModels;
using Edam.Application;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Edam.UI.Controls.Assets;


public sealed partial class AssetDataTreeControl : UserControl
{
    private AssetDataTreeViewModel m_ViewModel = new AssetDataTreeViewModel();
    public AssetDataTreeViewModel ViewModel
    {
        get { return m_ViewModel; }
    }

    public AssetDataTreeControl()
    {
        this.InitializeComponent();
        DataContext = m_ViewModel;
    }

    public void SetDataTree(AssetDataTree tree)
    {
        DataTreeModel dataTreeModel = DataTreeModel.PrepareTree(tree);
        ViewModel.TreeView = dataTreeModel;
    }

    public ResultLog SetDataTree(AssetConsoleArgumentsInfo args)
    {
        ResultLog results = new ResultLog();
        try
        {
            var tree = AssetDataTree.GetDataTree(args, 6);
            DataTreeModel dataTreeModel = DataTreeModel.PrepareTree(tree);
            ViewModel.TreeView = dataTreeModel;
            results.Succeeded();
        }
        catch (Exception ex)
        {
            results.Failed(ex);
        }
        return results;
    }

    private void TreeView_DoubleTapped(
       object sender, DoubleTappedRoutedEventArgs args)
    {
        ViewModel.SetCurrentItem(null, DataTreeEventType.DoubleTapped);
    }

    private void TreeView_KeyDown(object sender, KeyRoutedEventArgs args)
    {
        if (ViewModel.KeyEventData == null)
        {
            ViewModel.KeyEventData = new KeyEventData(args);
        }
        else
        {
            ViewModel.KeyEventData.Update(args);
        }
        args.Handled = true;
    }

    private void TreeView_KeyUp(object sender, KeyRoutedEventArgs args)
    {
        ViewModel.KeyEventData = null;
        args.Handled = true;
    }

}
