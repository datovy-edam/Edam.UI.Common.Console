using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

// -----------------------------------------------------------------------------
using Edam.DataObjects.Users;
using Edam.DataObjects.Dashboards;
using Edam.UI.Common.Menus;
using Edam.DataObjects;
using Edam.UI;
using Edam.DataObjects.Requests;
using Edam.DataObjects.Services;
using app = Edam.UI.Common.Application;

namespace Edam.UI.Common.Controls.ReferenceLists;


public class ReferenceListViewModel : ObservableObject, IMenuView
{

  #region -- 1.0 - Properties and definitions...

  private ObservableCollection<RequestAlertInfo> m_Items;
  private RequestAlertInfo m_Alert = new RequestAlertInfo();

  public Int32 MaxWidth { get; set; }

  public ObservableCollection<RequestAlertInfo> Items
  {
     get { return m_Items; }
     set
     {
        if (m_Items != value)
        {
           m_Items = value;
           OnPropertyChanged(DataElementName.Items);
        }
     }
  }

  private RequestAlertInfo m_SelectedItem = null;
  public RequestAlertInfo SelectedItem
  {
     get { return m_SelectedItem; }
     set
     {
        if (m_SelectedItem != value)
        {
           m_SelectedItem = value;
           OnPropertyChanged("SelectedItem");
           if (value != null)
              GotoFollowUp(value);
        }
     }
  }

  public String YearMonthText
  {
     get { return m_Alert.YearMonthText; }
     set
     {
        if (m_Alert.YearMonthText != value)
        {
           m_Alert.YearMonthText = value;
           OnPropertyChanged("YearMonthText");
        }
     }
  }

  public Int32 DateYear
  {
     get { return m_Alert.DateYear; }
     set
     {
        if (m_Alert.DateYear != value)
        {
           m_Alert.DateYear = value;
           OnPropertyChanged("DateYear");
        }
     }
  }

  public Int32 DateMonth
  {
     get { return m_Alert.DateMonth; }
     set
     {
        if (m_Alert.DateMonth != value)
        {
           m_Alert.DateMonth = value;
           OnPropertyChanged("DateMonth");
        }
     }
  }

  public Int32 Elaps
  {
     get { return m_Alert.Elaps; }
     set
     {
        if (m_Alert.Elaps != value)
        {
           m_Alert.Elaps = value;
           OnPropertyChanged("Elaps");
        }
     }
  }

  public String OrganizationId
  {
     get { return m_Alert.OrganizationId; }
     set
     {
        if (m_Alert.OrganizationId != value)
        {
           m_Alert.OrganizationId = value;
           OnPropertyChanged("OrganizationId");
        }
     }
  }

  public Int16 RequestTypeNo
  {
     get { return m_Alert.RequestTypeNo; }
     set
     {
        if (m_Alert.RequestTypeNo != value)
        {
           m_Alert.RequestTypeNo = value;
           OnPropertyChanged("RequestTypeNo");
        }
     }
  }

  public String RequestDescription
  {
     get { return m_Alert.RequestDescription; }
     set
     {
        if (m_Alert.RequestDescription != value)
        {
           m_Alert.RequestDescription = value;
           OnPropertyChanged("RequestDescription");
        }
     }
  }

  public Int16 StatusNo
  {
     get { return m_Alert.StatusNo; }
     set
     {
        if (m_Alert.StatusNo != value)
        {
           m_Alert.StatusNo = value;
           OnPropertyChanged("StatusNo");
        }
     }
  }

  public String StatusDescription
  {
     get { return m_Alert.StatusDescription; }
     set
     {
        if (m_Alert.StatusDescription != value)
        {
           m_Alert.StatusDescription = value;
           OnPropertyChanged("StatusDescription");
        }
     }
  }

  public DateTime? MinDate
  {
     get { return m_Alert.MinDate; }
     set
     {
        if (m_Alert.MinDate != value)
        {
           m_Alert.MinDate = value;
           OnPropertyChanged("MinDate");
        }
     }
  }

  public DateTime? MaxDate
  {
     get { return m_Alert.MaxDate; }
     set
     {
        if (m_Alert.MaxDate != value)
        {
           m_Alert.MaxDate = value;
           OnPropertyChanged("MaxDate");
        }
     }
  }

  public Int32 RequestCount
  {
     get { return m_Alert.RequestCount; }
     set
     {
        if (m_Alert.RequestCount != value)
        {
           m_Alert.RequestCount = value;
           OnPropertyChanged("RequestCount");
        }
     }
  }

  public IMenuItemParent ParentMenu { get; set; }

  #endregion
  #region -- 1.0 - Commands

  public ICommand GoBackCommand { protected set; get; }

  #endregion
  #region -- 1.5 - Initialize Resources

  public ReferenceListViewModel()
  {
     InitializeCommands();
     MaxWidth = 400;
     GetItems();
  }

  #endregion
  #region -- 2.0 - MVVM Methods

  #endregion
  #region -- 2.0 - MVVM Commands

  private void InitializeCommands()
  {
     GoBackCommand = new Command(DoGoBack);
  }

  #endregion
  #region -- 4.0 - Support Methods

  public void ClearFields()
  {
     m_Alert.ClearFields();
  }

  public RequestResponseInfo<UserAlertInfo> GetAlerts()
  {
     var a = IdentityService.GetUserAlertInfo();
     if (a.Status == TaskStatus.RanToCompletion)
     {
        if (a.Result.Success)
        {
           if (a.Result.ResponseData != null)
           {
              //m_Items = a.Result.ResponseData.RequestAlertsList;
              m_Alert = (m_Items != null && m_Items.Count > 0) ?
                 m_Items[0] : new RequestAlertInfo();
           }
        }
     }
     return a.Result;
  }

  public async void GetItems()
  {
     var info = await IdentityService.GetUserAlertInfo();
     if (info != null)
     {
        if (Items == null)
            Items = new ObservableCollection<RequestAlertInfo>();
        Items.Clear();
        foreach (var i in info.ResponseData.RequestAlertsList)
        {
            Items.Add(i);
        }
     }
  }

  public void GotoFollowUp(RequestAlertInfo alert)
  {
     //if (ParentMenu != null)
     //{
        GotoEventArgs e = new GotoEventArgs();
        e.MenuOption = MenuOption.FollowUp;
        e.ShowMainMenu = false;
        e.State = alert;
        app.ApplicationHelper.ApplicationMenuControl.Goto(this, e);
     //}
  }

  public void DoGoBack()
  {
     //if (ParentMenu != null)
     //   ParentMenu.Goto(this, null);
  }

  public void SetState(Object state)
  {
  }

  #endregion

}
