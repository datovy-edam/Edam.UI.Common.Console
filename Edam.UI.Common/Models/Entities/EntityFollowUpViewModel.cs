using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml;

//using Xamarin.Forms;

// -----------------------------------------------------------------------------
using SQLite;
using Edam.Data;
using Edam.UI;

using Edam.Application.Resources;
using Edam.Diagnostics;
using Edam.DataObjects.Requests;
using Edam.DataObjects.Entities;
using Edam.DataObjects.References;
using Edam.DataObjects.Dashboards;
using Edam.Application;
using Edam.DataObjects;
using Edam.UI.Common.Menus;
using Edam.UI.Common.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using Edam.UI.Common.Controls.Notes;

namespace Edam.UI.Common.Models.Entities;


public class EntityFollowUpViewModel : ObservableObject, Menus.IMenuView
{

    #region -- 1.00 - Properties and definitions...

    private ReferencePeriodInfo m_Period =
       new ReferencePeriodInfo();

    public PersonViewModel Persons { get; set; }

    public NoteViewModel Notes
    {
        get { return Persons.Notes; }
        set
        {
            Persons.Notes = value;
        }
    }

    private String m_CurrentPeriodText = String.Empty;
    public String CurrentPeriodText
    {
        get
        {
            if (String.IsNullOrWhiteSpace(m_CurrentPeriodText))
                m_Period.PeriodDate.Now();
            m_CurrentPeriodText = m_Period.PeriodDate.PeriodText;
            return m_CurrentPeriodText;
        }
        set
        {
            if (String.IsNullOrEmpty(value))
                m_Period.PeriodDate.Now();
            value = m_Period.PeriodDate.PeriodText;
            if (m_CurrentPeriodText != value)
            {
                m_CurrentPeriodText = value;
                OnPropertyChanged(DataElementName.CurrentPeriodText);
            }
        }
    }

    private String m_StatusMessageText = null;
    public String StatusMessageText
    {
        get { return m_StatusMessageText; }
        set
        {
            if (m_StatusMessageText != value)
            {
                m_StatusMessageText = value;
                OnPropertyChanged(DataElementName.StatusMessageText);
            }
        }
    }

    private Visibility m_InViewer = Visibility.Visible;
    public Visibility InViewer
    {
        get { return m_InViewer; }
        set
        {
            if (m_InViewer != value)
            {
                m_InViewer = value;
                OnPropertyChanged(DataElementName.InViewer);
            }
        }
    }

    public IMenuItemParent ParentMenu { get; set; }

    #endregion
    #region -- 1.00 - Commands

    public ICommand NextPeriodCommand { protected set; get; }
    public ICommand PreviousPeriodCommand { protected set; get; }
    public ICommand RefreshPeriodCommand { protected set; get; }

    public ICommand GoBackCommand { protected set; get; }
    public ICommand OnPersonDoubleClickCommand { protected set; get; }

    #endregion
    #region -- 1.50 - Initialize Resources

    public EntityFollowUpViewModel()
    {
        InitializeCommands();
        Persons = new PersonViewModel();
        InViewer = Visibility.Visible;
    }

    #endregion
    #region -- 2.00 - MVVM Commands

    private void InitializeCommands()
    {
        NextPeriodCommand = new Command(DoNextPeriod);
        PreviousPeriodCommand = new Command(DoPreviousPeriod);
        RefreshPeriodCommand = new Command(DoRefreshPeriod);
    }

    #endregion
    #region -- 4.00 - Command Methods

    public void DoNextPeriod()
    {
        m_Period.PeriodDate.NextWeek();
        CurrentPeriodText = m_Period.PeriodDate.PeriodText;
    }

    public void DoPreviousPeriod()
    {
        m_Period.PeriodDate.PreviousWeek();
        CurrentPeriodText = m_Period.PeriodDate.PeriodText;
    }

    public void DoRefreshPeriod()
    {
        m_Period.PeriodDate.Toggle();
        CurrentPeriodText = m_Period.PeriodDate.PeriodText;
    }

    #endregion
    #region -- 4.00 - Menu Support

    public void SetState(Object state)
    {

    }

    #endregion

}
