using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;

// -----------------------------------------------------------------------------
using Edam.Diagnostics;
using app = Edam.Application;
using Edam.DataObjects.ReferenceData;
using System.Collections.ObjectModel;
using Edam.DataObjects.Data;
using settings = Edam.Application.Settings;

using Edam.Data;
using Edam.Application;
using Edam.UI.Common.Controls.ReferenceLists;
using menus = Edam.UI.Common.Menus;
using Edam.UI.Common.Models.ReferenceData;

namespace Edam.UI.Common.Application;


// Looking for: FilePath or AbsolutePath or FileURI?
//    find those in: ConfigurationHelper

public class ApplicationHelper
{

    #region -- 1.00 - Constants Properties and Fields

    public const string HOME_CONTROL = "HomeControl";
    private const string PROJECT_VIEW = "ProjectView";
    private const string REFERENCE_LIST_VIEW = "ReferenceListView";
    public const string EDAM_STUDIO = "Edam.Studio";

    // TODO: put license in config file
    public const string REFERENCE_DATA = "Reference Data";

    public static string HomeControl { get; set; }

    private static menus.IMenu m_ApplicationMenuControl;
    public static menus.IMenu ApplicationMenuControl
    {
        get { return m_ApplicationMenuControl; }
    }

    private static Window m_MainWindow;
    public static Window MainWindow
    {
        get { return m_MainWindow; }
    }

    private static ApplicationLog m_ApplicationLog;
    public static ApplicationLog ApplicationLog
    {
        get { return m_ApplicationLog; }
    }

    private static ObservableCollection<ReferenceDataTemplateInfo>
       m_ReferenceDataTemplates;
    public static ObservableCollection<ReferenceDataTemplateInfo>
       ReferenceDataTemplates
    {
        get { return m_ReferenceDataTemplates; }
    }

    public static Dictionary<string, object> NamedInstance { get; set; } = 
        new Dictionary<string, object>();

    #endregion

    #region -- 4.00 - Helper methods, id home control, menu options... others

    /// <summary>
    /// Get UI Element instance.
    /// </summary>
    /// <param name="name">instance name</param>
    /// <returns>instance is returned</returns>
    public static UIElement? GetUiElement(string name)
    {
        if (NamedInstance.TryGetValue(name, out var uiElement))
        {
            return uiElement as UIElement;
        }
        // try to find registered type with same name
        var item = AppAssembly.FetchInstance(name);
        if (item != null)
        {
            NamedInstance.Add(name, item);
            return item as UIElement;
        }
        return null;
    }

    /// <summary>
    /// Get Home Control stating what is the location of "Home" when user 
    /// selects the "Home" button.
    /// </summary>
    /// <returns>instance of an UIElement control is returned</returns>
    public static UIElement GetHomeControl()
    {
        //return GetControl();
        HomeControl = app.AppSettings.GetSectionString(HOME_CONTROL);
        UIElement control = null;
        switch (HomeControl)
        {
            case REFERENCE_LIST_VIEW:
                control = new ReferenceListViewControl();
                break;
            default:
            case PROJECT_VIEW:
                var item = ApplicationHelper.Find(menus.MenuOption.Projects);
                if (item == null)
                {
                    control = GetUiElement(PROJECT_VIEW); 
                    //new ProjectViewerControl();
                }
                else
                {
                    control = item.Instance as UIElement;
                }
                break;
        }
        return control;
    }

    public static UIElement GetControl(
       menus.MenuOption option = menus.MenuOption.Unknown)
    {
        UIElement control = null;
        var item = ApplicationHelper.Find(menus.MenuOption.Viewer);
        if (item == null)
        {
            control = GetUiElement(PROJECT_VIEW);
            //new ProjectViewerControl();
        }
        else
        {
            item.Instance = item.Instance ??
               Activator.CreateInstance(item.TargetType);
            control = item.Instance as UIElement;
        }
        return control;
    }

    /// <summary>
    /// Every application has a single "Menu Control" interface.  The app 
    /// should call this method to set the shared menu control for the app.
    /// </summary>
    /// <param name="item">menu control instance</param>
    public static void SetApplicationMenuControl(menus.IMenu item)
    {
        m_ApplicationMenuControl = item;
    }

    public static void PinLogin(object state)
    {
        menus.GotoEventArgs a = new menus.GotoEventArgs();
        a.MenuOption = menus.MenuOption.PinLogin;
        a.State = state;
        m_ApplicationMenuControl.Goto(m_ApplicationMenuControl, a);
    }

    public static void ResetApplication()
    {
        // review data sources...
        settings.AppSettings.VerifySetConnectionString();

        // reset app now....
        menus.GotoEventArgs a = new menus.GotoEventArgs();
        a.MenuOption = menus.MenuOption.ResetApplication;
        m_ApplicationMenuControl.Goto(m_ApplicationMenuControl, a);
    }

    public static void SetMenuOption(menus.MenuOption option)
    {
        menus.GotoEventArgs a = new menus.GotoEventArgs();
        a.MenuOption = app.Session.IsUserLogged ? 
            option : menus.MenuOption.Login;
        m_ApplicationMenuControl.Goto(m_ApplicationMenuControl, a);
    }

    public static menus.IMenuItem Find(menus.MenuOption option)
    {
        return m_ApplicationMenuControl.Find(option);
    }

    #endregion
    #region -- 4.00 - Login and Logout

    public static void LoginApplication()
    {
        menus.GotoEventArgs a = new menus.GotoEventArgs();
        a.MenuOption = menus.MenuOption.Login;
        m_ApplicationMenuControl.Goto(m_ApplicationMenuControl, a);
        Edam.Application.Session.LogoutUser();
    }

    public static void LogoutApplication()
    {
        menus.GotoEventArgs a = new menus.GotoEventArgs();
        a.MenuOption = menus.MenuOption.Logout;
        m_ApplicationMenuControl.Goto(m_ApplicationMenuControl, a);
        Edam.Application.Session.LogoutUser();
    }

    #endregion
    #region -- 4.00 - Manage file templates...

    /// <summary>
    /// Load File Templates...
    /// </summary>
    /// <returns>Returns the loaded File Templates</returns>
    public static ResultsLog<List<ReferenceDataTemplateInfo>>
       LoadFileTemplates()
    {
        ResultsLog<List<ReferenceDataTemplateInfo>> results =
           new ResultsLog<List<ReferenceDataTemplateInfo>>();
        string folderPath =
           app.AppSettings.GetString("ReferenceDataTemplatesFolder");
        if (String.IsNullOrWhiteSpace(folderPath))
        {
            // TODO: put label in resources...
            results.Failed("Didn't found Templates Folder as expected.");
            return results;
        }
        try
        {
            // add default templates
            m_ReferenceDataTemplates =
               new ObservableCollection<ReferenceDataTemplateInfo>();
            ReferenceDataTemplateInfo t = new ReferenceDataTemplateInfo();

            t.Metadata = new ReferenceDataTemplateMetadata
            {
                TemplateName = "Reference Data",
                TemplateURI =
               "http://www.datovy.com/referencedata/template/" +
               "referencedata/v0r0",
                TemplateVersionId = "v0r0"
            };

            m_ReferenceDataTemplates.Add(t);

            // add file templates
            var reader = new ReferenceDataTemplateFileReader();
            results.Instance = reader.FromFolder(folderPath);

            foreach (var i in results.Instance)
            {
                m_ReferenceDataTemplates.Add(i);
            }

            results.Succeeded();
        }
        catch (Exception ex)
        {
            results.Failed(ex);
        }
        return results;
    }

    #endregion
    #region -- 4.00 - Manage Application Folders

    /// <summary>
    /// Get the Application Installed Location.
    /// </summary>
    /// <returns>Installed location is returned</returns>
    public static string GetApplicationInstalledLocation()
    {
        StorageFolder localFolder =
           Windows.ApplicationModel.Package.Current.InstalledLocation;
        return localFolder.Path;
    }

    public static string GetLocalFolder()
    {
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        return localFolder.Path;
    }

    #endregion
    #region -- 4.00 - Connection String resolution

    /// <summary>
    /// Get Reference Data Connection String By (Default) Key.
    /// </summary>
    /// <remarks>
    /// Find the default reference data here:
    /// 
    /// "ReferenceData": {
    ///    "ConnectionStringKey": "RefDatalocal"
    /// },
    /// 
    /// Upon application initialization and after it has been installed and
    /// executed for the first time, check if this connection string is 
    /// available and ask for it if can't be found.
    /// </remarks>
    /// <returns>the connection string is returned</returns>
    public static string GetReferenceDataConnectionStringByKey()
    {
        string kstring = ReferenceDataHelper.GetConnectionStringKey();
        DataSourceInfo dataSource = DataSources.GetDataSource(kstring);
        return dataSource.ConnectionString;
    }

    #endregion

}
