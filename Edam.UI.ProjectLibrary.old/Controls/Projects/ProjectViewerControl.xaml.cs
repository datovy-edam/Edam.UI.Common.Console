// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

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
using Edam.UI.Controls.DataModels;
using Edam.Diagnostics;
using Edam.UI.Common;
using Edam.UI.Controls.ViewModels;
using Edam.UI.Controls.Assets;
using Windows.UI.Core;
using Edam.UI.Common;

namespace Edam.UI.Controls.Projects;


public sealed partial class ProjectViewerControl : UserControl
{
    private ProjectViewerViewModel m_ViewModel;
    public ProjectViewerViewModel ViewModel
    {
        get { return m_ViewModel; }
    }

    public ProjectViewerControl()
    {
        this.InitializeComponent();

        m_ViewModel = TreeViewSidePanel.ViewModel;
        this.DataContext = TreeViewSidePanel.ViewModel;

        m_ViewModel.NotifyEventCompletion += ManageNotification;
        AssetViewerControl.ViewModel.NotifyEventCompletion +=
           ManageNotification;
        AssetViewSidePanel.ViewModel.NotifyAssetSaveOptionChanged +=
           AssetViewerControl.ViewModel.ManageNotification;
        AssetViewSidePanel.ViewModel.NotifyEvent +=
           ManageNotification;
    }

    public void ManageNotification(object sender, NotificationArgs args)
    {
        if (args.Type == NotificationType.AssetDataSetAvailable)
        {
            AssetViewSidePanel.AssetDataChanged(args.EventData);
        }
        else if (args.Type == NotificationType.ProjectItemSelected)
        {
            var results = args.ResultsLog as IResultsLog;
            AssetViewerControl.SetEditorText(results.DataObject as ProjectItem,
               results.ReturnText);

            ProjectContext.SetSelectedProject(
               results.DataObject as ProjectItem);
        }
        else if (args.Type == NotificationType.AssetViewerChanged)
        {
            ViewModel.SetAssetViewerVisibility(
               (args.MessageText == AssetViewOption.DataMapView.ToString()) ?
                  Visibility.Collapsed : Visibility.Visible);

            AssetViewSidePanel.ManageNotification(this, args);
            AssetMapViewer.ManageNotification(this, args);
        }
        else if (args.Type == NotificationType.AssetViewerTabChanged)
        {
            ViewModel.SetSidePanel(args.MessageText);
        }
    }

    #region -- 4.00 - Save Editor Text in File or Storage...

    private void SaveText()
    {
        AssetViewerControl.SaveText();
    }

    private void ProjectSave_Click(object sender, RoutedEventArgs e)
    {
        SaveText();
    }

    private void EditorControl_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.S)
        {
            Windows.UI.Core.CoreVirtualKeyStates ctrlKey =
               Microsoft.UI.Input.InputKeyboardSource.
                  GetKeyStateForCurrentThread(
                     Windows.System.VirtualKey.Control);
            if (ctrlKey == CoreVirtualKeyStates.Down)
            {
                SaveText();
            }
        }
    }

    #endregion

}
