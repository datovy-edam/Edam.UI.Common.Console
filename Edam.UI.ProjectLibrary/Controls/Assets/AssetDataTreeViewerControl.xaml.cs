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
using Edam.UI.Common;
using Edam.UI.Controls.DataModels;
using Edam.Data.AssetConsole;
using Edam.Data.AssetSchema;

namespace Edam.UI.Controls.Assets;


public sealed partial class AssetDataTreeViewerControl : UserControl
{

    private AssetDataTreeViewerViewModel m_ViewModel =
       new AssetDataTreeViewerViewModel();
    public AssetDataTreeViewerControl()
    {
        this.InitializeComponent();
        DataContext = m_ViewModel;
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

        context.Target.Instance = TreeView;
        if (context.Target.Arguments != null)
        {
            TreeView.SetDataTree(context.Target.Arguments);
            TreeView.ViewModel.SetMapContext(context, MapItemType.Target);
            m_ViewModel.SetContext(context);
        }
    }

    private void AssetRefresh_Click(object sender, RoutedEventArgs e)
    {

    }

}
