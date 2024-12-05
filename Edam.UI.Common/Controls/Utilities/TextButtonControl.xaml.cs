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
using Microsoft.UI;

namespace Edam.UI.Common.Controls.Utilities;

public sealed partial class TextButtonControl : UserControl
{

    private Brush m_DefaultBackgroundBrush = null;
    private TextButtonViewModel m_ViewModel = new TextButtonViewModel();
    public TextButtonViewModel ViewModel
    {
        get { return m_ViewModel; }
    }

    public static readonly DependencyProperty ButtonTextProperty =
        DependencyProperty.Register(
            nameof(Texto), typeof(string), typeof(TextButtonControl),
            new PropertyMetadata(String.Empty, TextoChanged));

    public string Texto
    {
        get { return (string)GetValue(ButtonTextProperty); }
        set
        {
            var oldValue = (string)GetValue(ButtonTextProperty);
            if (oldValue != value)
                SetValue(ButtonTextProperty, value);
        }
    }

    public TextButtonControl()
    {
        this.InitializeComponent();
        DataContext = m_ViewModel;

        var binding = new Microsoft.UI.Xaml.Data.Binding
        {
            Path = new PropertyPath(nameof(Texto)),
            Mode = BindingMode.TwoWay
        };
        ButtonText.SetBinding(TextBlock.TextProperty, binding);
        m_DefaultBackgroundBrush = ControlBorder.Background;
        if (m_DefaultBackgroundBrush == null)
            m_DefaultBackgroundBrush = new SolidColorBrush(Colors.White);
    }

    private static void TextoChanged(
        DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        TextButtonControl c = obj as TextButtonControl;
        if (c == null)
            return;
        c.ViewModel.Texto = e.NewValue.ToString();
    }

    private void ButtonText_PointerEntered(
       object sender, PointerRoutedEventArgs e)
    {
        ControlBorder.Background = new SolidColorBrush(Colors.AliceBlue);
    }

    private void ButtonText_PointerExited(
       object sender, PointerRoutedEventArgs e)
    {
        ControlBorder.Background = m_DefaultBackgroundBrush;
    }
}
