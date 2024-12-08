using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

// -----------------------------------------------------------------------------
using System.Collections.ObjectModel;
using Edam.UI.Common.Menus;
using Edam.Diagnostics;

namespace Edam.UI.Common.Controls.Navigation;


public class MenuController : MenuItem, Menus.IMenu, IMenuItemParent
{

    private Frame m_PanelContent = null;
    public Frame PanelContent
    {
        get { return m_PanelContent; }
        set { m_PanelContent = value; }
    }

    private ObservableCollection<MenuItem> m_Items;

    public MenuController(
       Frame panelContent, ObservableCollection<MenuItem> items)
    {
        m_PanelContent = panelContent;
        m_Items = items;
    }

    /// <summary>
    /// IMenu::ShowMenu helper to allow child Views/Controls to request to see
    /// the Menu...
    /// </summary>
    public void ShowMenu()
    {
    }

    #region -- 4.00 - Manage Presenting Page within the Frame (PanelContent)

    /// <summary>
    /// Present Page as specified in the given (menu) item...
    /// </summary>
    /// <param name="item">menu item</param>
    public IMenuItem PresentPage(IMenuItem item, Object state = null)
    {
        if (item == null)
            return item;

        if (item.TargetType == null)
            item.TargetType = typeof(AboutControl);

        item.Instance = item.Instance ??
           Activator.CreateInstance(item.TargetType);

        if (item.Instance is IControlView view)
            view.ParentMenu = this;
        if (item.Instance is IMenuView menuView)
        {
            menuView.ParentMenu = this;
            IMenuView v = item.Instance as IMenuView;
            if (state != null)
                v.SetState(state);
        }

        if (item.Navigation)
        {
            m_PanelContent.Content = item.Instance as Control;
        }

        return item;
    }

    /// <summary>
    /// Add Menu Item if it has not been already registered.  Note that the 
    /// MenuOption property is unique among MenuItems therefore make sure that 
    /// it is unique before trying to register a new menu.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public IMenuItem Add(MenuItem item)
    {
        var foundMenu = Find(item.MenuOption);
        if (foundMenu != null)
        {
            ResultLog.Trace(item.MenuOption.ToString() +
                " is already registered.", 
                nameof(MenuController), SeverityLevel.Info);

            return foundMenu;
        }
        m_Items.Add(item);
        return item;
    }

    /// <summary>
    /// Find Menu Option.
    /// </summary>
    /// <param name="option">option to find</param>
    /// <returns></returns>
    public IMenuItem Find(MenuOption option)
    {
        IMenuItem selected = null;
        foreach (var i in m_Items)
        {
            if (i.MenuOption == option)
            {
                selected = i;
                break;
            }
        }
        return selected;
    }

    /// <summary>
    /// Find menu option.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public IMenuItem FindMenu(object sender, GotoEventArgs e)
    {
        if (e == null || e.MenuOption == MenuOption.Unknown)
            return null;

        return Find(e.MenuOption);
    }

    /// <summary>
    /// Go to a given menu option.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Goto(object sender, GotoEventArgs e)
    {
        if (e == null || e.MenuOption == MenuOption.Unknown)
            return;

        IMenuItem item = Find(e.MenuOption);
        if (item != null)
        {
            PresentPage(item, e);
        }
    }

    public void PageNavigation_SelectionChanged(object sender,
       NavigationViewSelectionChangedEventArgs args)
    {
        var item = args.SelectedItem as MenuItem;
        if (item == null)
            return;

        IMenuProcess p = item as IMenuProcess;
        if (p == null)
            PresentPage(item, item);
        else if (p.Exec == null)
            PresentPage(item, item);
        else
            p.Exec();
    }

    public void SelectionChanged(MenuItem item)
    {
        if (item == null)
            return;

        IMenuProcess p = item as IMenuProcess;
        if (p == null || p.Exec == null)
            PresentPage(item, item);
        else
            p.Exec();
    }

    #endregion
    #region -- 4.00 - Initialize Main Menu Set

    public void SetupInstance(MenuOption option, object instance)
    {
        var item = Find(option);
        if (item != null)
        {
            item.Instance = instance;
        }
    }

    /// <summary>
    /// Get Main Menu.
    /// </summary>
    /// <returns>return main menu</returns>
    public static ObservableCollection<MenuItem> GetMainMenu()
    {
        return new ObservableCollection<MenuItem>();
        //return new ObservableCollection<MenuItem>(new[]
        //{
        //   new MenuItem { Id = (Int32)MenuOption.Home,
        //      Title = "Home", Glyph = Symbol.Home, Tooltip = "Home",
        //      TargetType = typeof(DashboardControl) },
        //   new MenuItem { Id = (Int32)MenuOption.Library,
        //      Title = "Reference Data", Glyph = Symbol.ShowBcc,
        //      Tooltip = "Reference Data",
        //      TargetType = typeof(ReferenceDataEditorControl) },
        //   new MenuItem { Id = (Int32)MenuOption.FollowUp,
        //      Title = "Lists", Glyph = Symbol.List, Tooltip = "Lists",
        //      Visible = false, Navigation = true,
        //      TargetType = typeof(EntityFollowUpViewControl) },
        //   new MenuItem { Id = (Int32)MenuOption.Projects,
        //      Title = "Projects", Glyph = Symbol.AllApps, Tooltip = "Projects",
        //      TargetType = typeof(ProjectViewerControl) },
        //   new MenuItem { Id = (Int32)MenuOption.Login,
        //      Title = "Login", Glyph = Symbol.Home, Tooltip = "Login",
        //      Visible = false, TargetType = typeof(AccountLoginControl),
        //      Navigation = false },
        //   new MenuItem { Id = (Int32)MenuOption.ResetApplication,
        //      Title = "Reset", Glyph = Symbol.Home, Tooltip = "Reset",
        //      Visible = false, TargetType = typeof(DashboardControl),
        //      Navigation = true },
        //   new MenuItem { Id = (Int32)MenuOption.PinLogin,
        //      Title = "Pin Login", Glyph = Symbol.Pin, Tooltip = "Pin Login",
        //      Visible = false, TargetType = typeof(AccountPinLoginControl),
        //      Navigation = false }
        //});
    }


    #endregion

}
