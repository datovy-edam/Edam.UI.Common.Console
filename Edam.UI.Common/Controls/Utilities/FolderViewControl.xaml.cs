// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

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
using Edam.UI.Common.Resources;
using Edam.InOut;

namespace Edam.UI.Common.Controls.Utilities;


public sealed partial class FolderViewControl : UserControl
{

    public NotificationEvent SelectionChangedEvent { get; set; }
    private FolderViewModel m_ViewModel = new FolderViewModel();
    public FolderViewModel ViewModel
    {
        get { return m_ViewModel; }
    }
    public FolderViewControl()
    {
        this.InitializeComponent();
        DataContext = m_ViewModel;
    }

    private void FileViewer_SelectionChanged(
       object sender, SelectionChangedEventArgs e)
    {
        FileDetailInfo details = null;
        if (e.AddedItems.Count > 0)
        {
            details = e.AddedItems[0] as FileDetailInfo;
        }
        if (SelectionChangedEvent != null && details != null)
        {
            NotificationArgs args = new NotificationArgs();
            args.Type = NotificationType.ItemSelected;
            args.EventData = details;
            args.MessageText = details.Name.Replace(".json", "");
            SelectionChangedEvent(this, args);
        }
    }

}
