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
// -----------------------------------------------------------------------------
using Edam.UI.Controls.ViewModels;
using Edam.UI.Controls.DataModels;
using Edam.Data.AssetSchema;
using Edam.Data.AssetConsole;
using Edam.UI.Common;
using Edam.Serialization;
using Edam.InOut;
using Edam.UI.Controls.Projects;

namespace Edam.UI.Controls.Assets;


public sealed partial class AssetViewerControl : UserControl
{

    private AssetViewerViewModel m_ViewModel;
    public AssetViewerViewModel ViewModel
    {
        get { return m_ViewModel; }
    }

    ProjectFileEditorControl FileEditorControl { get; set; }
    AssetDataElementGridControl ElementGridControl { get; set; }
    AssetUseCaseGridControl UseCaseGridControl { get; set; }

    public AssetViewerControl()
    {
        this.InitializeComponent();

        FileEditorControl = new ProjectFileEditorControl();
        ElementGridControl = new AssetDataElementGridControl();
        UseCaseGridControl = new AssetUseCaseGridControl();

        FrameFileEditor.Content = FileEditorControl;
        FrameElementGrid.Content = ElementGridControl;
        FrameUseCaseGrid.Content = UseCaseGridControl;

        m_ViewModel = ElementGridControl.ViewModel;
        DataContext = ElementGridControl.ViewModel;

        FileEditorControl.AssetViewerCommandEvent =
           m_ViewModel.ManageNotification;

        ViewModel.NotifyEventCompletion +=
           FileEditorControl.ManageNotification;
        ViewModel.NotifyEventCompletion += ManageNotification;

        TabViewer.SelectedItem = FileTab;
    }

    public void ManageNotification(object sender, NotificationArgs args)
    {
        if (args.Type == NotificationType.AssetDataSetAvailable)
        {
            AssetData asset = args.EventData as AssetData;
            AssetTab.IsEnabled = args.ResultsLog.Success &&
               asset != null && asset.Items.Count > 0;
            if (AssetTab.IsEnabled && asset.UseCases != null &&
               asset.UseCases.Count > 0)
            {
                UseCaseGridControl.ViewModel.SetupUseCase(asset);
                UseCaseTab.Visibility = Visibility.Visible;
            }
            else
            {
                UseCaseGridControl.ViewModel.SetupUseCase(null);
                UseCaseTab.Visibility = Visibility.Collapsed;
            }
        }
    }

    public void SetEditorText(ProjectItem item, string text)
    {
        TabViewer.SelectedItem = FileTab;
        DataDomainModel.RegisterDomain(item, text);
        FileEditorControl.SetEditorText(item, text);
    }

    public void SaveText()
    {
        FileEditorControl.SaveText();
    }

    private void TabViewer_SelectionChanged(
       object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count == 0)
        {
            return;
        }
        var tabItem = e.AddedItems[0] as TabViewItem;
        ViewModel.NotifyTabChanged(tabItem.Name, tabItem);
    }

}
