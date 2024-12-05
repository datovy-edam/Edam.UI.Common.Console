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
using Edam.UI.Common.Menus;
using Edam.UI.Common.Controls.Utilities;

namespace Edam.UI.Common.Controls.Accounts;

public sealed partial class AccountPinLoginControl : UserControl, IMenuView
{
    private IMenuItemParent m_MenuItem = null;
    private KeyPadViewModel m_ViewModel = new KeyPadViewModel();

    public KeyPadViewModel ViewModel
    {
        get => m_ViewModel;
        set
        {
            m_ViewModel = value;
            DataContext = value;
        }
    }

    public IMenuItemParent ParentMenu
    {
        get { return m_MenuItem; }
        set => m_MenuItem = value;
    }

    public AccountPinLoginControl()
    {
        this.InitializeComponent();
        DataContext = m_ViewModel;
    }

    public void SetState(object state)
    {
        if (state == null || (state as GotoEventArgs) == null)
            throw new Exception("Fail to find Pin Parent");

        KeyPad.ViewModel.ParentState =
           (state as GotoEventArgs).State as LoginViewModel;
        m_ViewModel.ParentState = KeyPad.ViewModel.ParentState;
    }

}
