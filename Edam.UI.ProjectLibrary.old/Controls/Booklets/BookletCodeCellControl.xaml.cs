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
using Edam.Data.Books;

namespace Edam.UI.Controls.Booklets;


public sealed partial class BookletCodeCellControl : UserControl,
  IBooklet, IBookCellView
{

    private CellViewModel m_ViewModel;
    public CellViewModel ViewModel
    {
        get { return m_ViewModel; }
        set
        {
            m_ViewModel = value;
            DataContext = value;
            PanelControl.ViewModel = value.ViewModel;
            m_ViewModel.BaseControl = this;
        }
    }

    public FramePanelControl FramePanel
    {
        get { return PanelControl; }
    }

    public BookletInfo Booklet { get; set; } = new BookletInfo();
    public object Instance
    {
        get { return this; }
    }

    public BookletCodeCellControl()
    {
        this.InitializeComponent();
    }

    public BookletCellInfo GetCell()
    {
        return Tag as BookletCellInfo;
    }

    public void SetCell(BookletCellInfo cell)
    {
        ViewModel.SetCell(cell);
    }

    public string GetInputText()
    {
        return CodeInputPanel.Text;
    }

    public string GetOutputText()
    {
        return CodeOutputPanel.Text;
    }

    public void SetInputText(string text)
    {
        CodeInputPanel.Text = text;
    }

    public void SetOutputText(string text)
    {
        CodeOutputPanel.Text = text;
        CodeOutputPanel.Visibility = Visibility.Visible;
    }

    private void StackPanel_GotFocus(object sender, RoutedEventArgs e)
    {
        BookletCellInfo cell = this.Tag as BookletCellInfo;
        if (cell != null)
        {
            ViewModel.SetCell(cell);
        }
    }

}
