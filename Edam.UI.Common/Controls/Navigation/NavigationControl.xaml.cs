using System;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236
// -----------------------------------------------------------------------------
using Edam.UI.Common.Menus;
using app = Edam.UI.Common.Application;

namespace Edam.UI.Common.Controls.Navigation;


public sealed partial class NavigationControl : UserControl,
  Menus.IMenu, IMenuItemParent
{
    private MenuController m_MenuController;
    private readonly NavigationViewModel m_ViewModel =
       new NavigationViewModel();
    public NavigationViewModel ViewModel
    {
        get { return m_ViewModel; }
    }

    public Boolean MenuIsVisible
    {
        get
        {
            return ViewModel.DisplayMode ==
               NavigationViewPaneDisplayMode.Left;
        }
    }

    public NavigationControl()
    {
        this.InitializeComponent();
        this.DataContext = m_ViewModel;
        VisibleMenuItems.Source = m_ViewModel.VisibleItems;
        app.ApplicationHelper.SetApplicationMenuControl(this);

        m_MenuController = new MenuController(PanelContent, ViewModel.Items);

        //ProjectViewerControl projectViewer = new ProjectViewerControl();
        //m_MenuController.SetupInstance(MenuOption.Projects, projectViewer);

        app.ApplicationHelper.SetMenuOption(MenuOption.Login);
    }

    /// <summary>
    /// IMenu::ShowMenu helper to allow child Views/Controls to request to see
    /// the Menu...
    /// </summary>
    public void ShowMenu()
    {
        ViewModel.DisplayMode = NavigationViewPaneDisplayMode.Left;
    }

    #region -- 4.00 - Manage Presenting Page within the Frame (PanelContent)

    /// <summary>
    /// Present Page as specified in the given (menu) item...
    /// </summary>
    /// <param name="item">menu item</param>
    private void PresentPage(IMenuItem item, Object state = null,
       Boolean menuItemsVisible = false)
    {
        var mitem = m_MenuController.PresentPage(item, state);

        if (mitem == null)
            return;

        //if (mitem.TargetType == null)
        //   mitem.TargetType = typeof(AboutControl);

        //mitem.Instance = mitem.Instance ?? 
        //   Activator.CreateInstance(item.TargetType);

        //if (mitem.Instance is IControlView view)
        //   view.ParentMenu = this;
        //if (mitem.Instance is IMenuView menuView)
        //{
        //   menuView.ParentMenu = this;
        //   IMenuView v = mitem.Instance as IMenuView;
        //   if (state != null)
        //      v.SetState(state);
        //}

        if (mitem.Navigation)
        {
            NoNavigationContent.Visibility = Visibility.Collapsed;
            PageNavigation.Visibility = Visibility.Visible;
            //PanelContent.Content = mitem.Instance as Control;
        }
        else
        {
            PageNavigation.Visibility = Visibility.Collapsed;
            NoNavigationContent.Visibility = Visibility.Visible;
            NoNavigationContent.Content = mitem.Instance as Control;
        }

        //ViewModel.DisplayMode = menuItemsVisible ?
        //   NavigationViewPaneDisplayMode.Left :
        //   NavigationViewPaneDisplayMode.LeftCompact;
    }

    /// <summary>
    /// Go to a given menu option.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Goto(Object sender, GotoEventArgs e)
    {
        ViewModel.DisplayMode = (e == null) ?
           NavigationViewPaneDisplayMode.Left :
           (e.ShowMainMenu ? NavigationViewPaneDisplayMode.Left :
               NavigationViewPaneDisplayMode.LeftCompact);

        IMenuItem item = m_MenuController.FindMenu(sender, e);
        //if (e == null || e.MenuOption == MenuOption.Unknown)
        //   return;

        //IMenuItem item = NavigationViewModel.Find(e.MenuOption);
        if (item != null)
        {
            m_ViewModel.SelectedItem = null;
            PresentPage(item, e, MenuIsVisible);
        }
    }

    public IMenuItem Find(MenuOption option)
    {
        return m_MenuController.Find(option);
    }

    private void PageNavigation_SelectionChanged(NavigationView sender,
       NavigationViewSelectionChangedEventArgs args)
    {
        m_ViewModel.SelectedItem = args.SelectedItem as MenuItem;

        var item = args.SelectedItem as MenuItem;
        if (item == null)
            return;

        IMenuProcess p = item as IMenuProcess;
        if (p == null || p.Exec == null)
            PresentPage(item, item);
        else
            p.Exec();
    }

    #endregion

    private void Control_SizeChanged(
       object sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
    {
        if (e.PreviousSize.Height == 0)
        {
            PageNavigation.Height = e.NewSize.Height;
        }
        else
        {
            PageNavigation.Height += e.NewSize.Height - e.PreviousSize.Height;
        }
        NoNavigationContent.Height = PageNavigation.Height;
    }

    private void SidePanelToggle_Click(object sender, RoutedEventArgs e)
    {
        m_ViewModel.Expander.TogglePanelVisibility();
    }

}
