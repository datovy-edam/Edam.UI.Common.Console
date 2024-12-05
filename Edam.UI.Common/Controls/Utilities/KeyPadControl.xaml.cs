using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.UI.Core;
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
using Edam.DataObjects.Devices;
using Edam.UI.Common.Menus;

namespace Edam.UI.Common.Controls.Utilities;

public sealed partial class KeyPadControl : UserControl, IMenuView
{
    private string m_Text = String.Empty;
    private bool m_ShowDigits = false;

    private KeyPadViewModel m_ViewModel = new KeyPadViewModel();
    public KeyPadViewModel ViewModel
    {
        get { return m_ViewModel; }
        set
        {
            m_ViewModel = value;
            DataContext = value;
        }
    }

    public IMenuItemParent ParentMenu { get; set; }

    public KeyPadControl()
    {
        this.InitializeComponent();
        DataContext = m_ViewModel;
        m_ViewModel.OKButton = OKButton;
    }

    private void ClearTextBox()
    {
        m_Text = String.Empty;
        this.TextBox.Text = String.Empty;
        m_ViewModel.ClearAll();
    }

    private void ManageInputText(string inputText)
    {
        if (inputText == "OK")
        {
            m_ViewModel.OnOKPressed(m_Text);
            return;
        }
        if (inputText == "Cancel")
        {
            ClearTextBox();
            return;
        }
        if (inputText == "<")
        {
            if (m_Text.Length == 0)
            {
                m_Text = String.Empty;
            }
            else
            {
                this.TextBox.Text = this.TextBox.Text.Substring(
                    0, m_Text.Length - 1);
                m_Text = m_Text.Substring(0, m_Text.Length - 1);
            }
        }
        else
        {
            this.TextBox.Text += m_ShowDigits ? inputText : "*";
            m_Text += inputText;
        }
        m_ViewModel.PinNumberText = m_Text;
    }

    private void TextButtonControl_PointerPressed(
       object sender, PointerRoutedEventArgs e)
    {
        TextBlock s = e.OriginalSource as TextBlock;
        if (s == null)
            return;

        ManageInputText(s.Text);
    }

    public void SetState(object state)
    {
        m_ViewModel.ParentState = state;
    }

    private void StackPanel_KeyDown(object sender, KeyRoutedEventArgs e)
    {

    }

    //public new void OnKeyDown(KeyRoutedEventArgs e)
    //{
    //   ManageInputText(e.Key.ToString());
    //}
}
