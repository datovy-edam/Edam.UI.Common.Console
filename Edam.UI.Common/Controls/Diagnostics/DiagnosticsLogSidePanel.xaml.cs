using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236
using Edam.UI.Common.Controls.Utilities;

namespace Edam.UI.Common.Controls.Diagnostics;

public sealed partial class DiagnosticsLogSidePanel : UserControl
{
    public ExpanderModel Expander { get; } = new ExpanderModel();
    public DiagnosticsLogSidePanel()
    {
        this.InitializeComponent();
        //Expander.PanelVisibility = Visibility.Visible;
        ClearContentButton.Visibility = Visibility.Collapsed;
    }

    private void ToggleExplander(object sender, PointerRoutedEventArgs e)
    {
        Expander.TogglePanelVisibility();
        ClearContentButton.Visibility = Expander.PanelVisibility;
    }

    private void ClearContent(object sender, PointerRoutedEventArgs e)
    {
        DiagnosticsLogViewer.ClearContent(sender, e);
    }
}
