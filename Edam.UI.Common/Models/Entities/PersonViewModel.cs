using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;

using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml;

//using Xamarin.Forms;
//using device = Xamarin.Forms.Device;

// -----------------------------------------------------------------------------
using Edam.Application;
using CommunityToolkit.Mvvm.ComponentModel;

using references = Edam.DataObjects.References;
using resource = Edam.Application.ApplicationHelper;
using services = Edam.DataObjects.Services;

using Edam.DataObjects;
using Edam.DataObjects.Dashboards;
using Edam.DataObjects.Requests;
using Edam.DataObjects.Entities;
using Edam.DataObjects.Services;
using Edam.DataObjects.Objects;

using Edam.UI.Common.Menus;
using Edam.UI.Common.Controls.Notes;
using Edam.UI.Common.Models.Notes;
using Edam.UI.Common.Controls.ReferenceLists;

namespace Edam.UI.Common.Models.Entities;


public class PersonViewModel : ObservableObject, IMenuView
{

    #region -- 1.00 - Properties for Items - Persons Collection

    protected ObservableCollection<PersonModel> m_Items =
       new ObservableCollection<PersonModel>();
    public ObservableCollection<PersonModel> Items
    {
        get { return m_Items; }
    }

    private ReferenceListGroupViewModel m_ReferenceListGroupViewModel;
    public ReferenceListGroupViewModel ReferenceListGroup
    {
        get { return m_ReferenceListGroupViewModel; }
        set
        {
            if (m_ReferenceListGroupViewModel != value)
            {
                m_ReferenceListGroupViewModel = value;
                OnPropertyChanged("ReferenceListGroup");
            }
        }
    }

    private NoteViewModel m_Notes;
    public NoteViewModel Notes
    {
        get { return m_Notes; }
        set
        {
            if (m_Notes != value)
            {
                m_Notes = value;
                OnPropertyChanged(DataElementName.Notes);
            }
        }
    }

    protected Boolean m_HasRecord;
    public Boolean HasRecord
    {
        get
        {
            if (SelectedItem == null)
                return false;
            return SelectedItem.HasRecord;
        }
        set
        {
            if (m_HasRecord != value)
            {
                m_HasRecord = value;
                OnPropertyChanged(DataElementName.HasRecord);
            }
        }
    }

    #endregion
    #region -- 1.00 - Properties and definitions...

    private PersonModel m_SelectedItem;
    public PersonModel SelectedItem
    {
        get { return m_SelectedItem; }
        set
        {
            if (m_SelectedItem != value)
            {
                Set(inViewer: true);
                m_SelectedItem = value;
                OnPropertyChanged(DataElementName.SelectedItem);
                if (value != null && Notes != null)
                {
                    Notes.SetReferenceEntity(value);
                }
                Set(inViewer: true);
                HasRecord = !m_HasRecord;

                if (ReferenceListGroup != null)
                    ReferenceListGroup.GetItems(null, value?.EntityId);
            }
        }
    }

    public IMenuItemParent ParentMenu { get; set; }

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

    private String m_TitleText = null;
    public String TitleText
    {
        get => m_TitleText == null ? String.Empty : m_TitleText.ToUpper();
        set
        {
            if (m_TitleText != value)
            {
                m_TitleText = value;
                OnPropertyChanged(DataElementName.TitleText);
            }
        }
    }

    private Visibility m_ShowEditorVisible;
    public Visibility ShowEditorVisible
    {
        get { return m_ShowEditorVisible; }
        set
        {
            if (m_ShowEditorVisible != value)
            {
                m_ShowEditorVisible = value;
                OnPropertyChanged("ShowEditorVisible");
            }
        }
    }

    private bool m_ShowEditor;
    public bool ShowEditor
    {
        get { return m_ShowEditor; }
        set
        {
            if (m_ShowEditor != value)
            {
                m_ShowEditor = value;
                OnPropertyChanged("ShowEditor");
            }
        }
    }


    private Visibility m_Editing = Visibility.Collapsed;
    public Visibility Editing
    {
        get { return m_Editing; }
        set
        {
            if (m_Editing != value)
            {
                m_Editing = value;
                OnPropertyChanged("Editing");
            }
        }
    }

    private Visibility m_InEditor = Visibility.Collapsed;
    public Visibility InEditor
    {
        get { return m_InEditor; }
        set
        {
            if (m_InEditor != value)
            {
                m_InEditor = value;
                OnPropertyChanged(DataElementName.InEditor);
            }
        }
    }

    private Visibility m_InSearch = Visibility.Visible;
    public Visibility InSearch
    {
        get { return m_InSearch; }
        set
        {
            if (m_InSearch != value)
            {
                m_InSearch = value;
                OnPropertyChanged(DataElementName.InSearch);
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

    private Visibility m_IsAdding = Visibility.Visible;
    public Visibility IsAdding
    {
        get { return m_IsAdding; }
        set
        {
            if (m_IsAdding != value)
            {
                m_IsAdding = value;
                OnPropertyChanged(DataElementName.IsAdding);
                //OnPropertyChanged(DataElementName.NotAdding);
            }
        }
    }

    #endregion
    #region -- 1.00 - Commands

    public ICommand AddCommand { protected set; get; }
    public ICommand RetireCommand { protected set; get; }
    public ICommand EditCommand { protected set; get; }
    public ICommand SaveCommand { protected set; get; }

    public ICommand SearchCommand { protected set; get; }
    public ICommand FindCommand { protected set; get; }

    public ICommand DoneEditCommand { protected set; get; }
    public ICommand GoBackCommand { protected set; get; }

    #endregion
    #region -- 1.50 - Initialize Resources

    public PersonViewModel()
    {
        InitializeCommands();
        //ReferenceListGroup = new ReferenceListGroupViewModel();
        //Notes = new NoteViewModel();
    }

    #endregion
    #region -- 2.00 - MVVM Commands

    private void InitializeCommands()
    {
        GoBackCommand = new Command(DoGoBack);
        DoneEditCommand = new Command(OnDone);
        RetireCommand = new Command(OnRetire);
        SaveCommand = new Command(OnSave);
        EditCommand = new Command(OnEdit);

        SearchCommand = new Command(OnSearch);
        FindCommand = new Command(OnFind);

        AddCommand = new Command(DoAdd);
    }

    #endregion
    #region -- 4.00 - Support Methods

    private Visibility Visible(bool value)
    {
        return value ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inViewer"></param>
    /// <param name="inSearch"></param>
    /// <param name="inEditor"></param>
    /// <param name="isAdding"></param>
    public void SetIndicators(bool inViewer = false, bool inSearch = false,
       bool inEditor = false, bool isAdding = false, bool editing = false)
    {
        InViewer = Visible(inViewer);
        InSearch = Visible(inSearch);
        InEditor = Visible(inEditor);
        IsAdding = Visible(isAdding);
        Editing = Visible(editing);
    }

    private void Set(bool inViewer = false, bool inSearch = false,
       bool inEditor = false, bool isAdding = false, bool editing = false)
    {
        SetIndicators(inViewer, inSearch, inEditor, isAdding, editing);
        ShowEditorVisible = inEditor ? Visibility.Visible : Visibility.Collapsed;
        ShowEditor = inEditor;
    }

    public void ResetControls()
    {
        Items.Clear();
        if (Notes != null)
        {
            Notes.SelectedItem = null;
            Notes.Items = new ObservableCollection<NoteModel>();
        }
        if (ReferenceListGroup != null)
        {
            ReferenceListGroup.Items =
               new ObservableCollection<ReferenceListGroupModel>();
        }
    }

    public void ClearFields()
    {
        if (SelectedItem != null)
            SelectedItem.ClearFields();
    }

    public static Int32? Find(ObservableCollection<PersonModel> observable,
       PersonInfo item)
    {
        Int32? foundIndex = null;
        if (observable == null)
            return foundIndex;
        for (var x = 0; x < observable.Count; x++)
        {
            if ((item.EntityId == observable[x].EntityId) ||
                (item.Email == observable[x].Email) ||
                (item.Phone.PhoneNumber == observable[x].PhoneNumber))
            {
                foundIndex = x;
                break;
            }
        }

        return foundIndex;
    }

    public void DeleteFromItems(String entityId)
    {
        foreach (var i in Items)
        {
            if (i.EntityId == entityId)
            {
                Items.Remove(i);
                break;
            }
        }
    }

    public void AddPerson(PersonInfo person)
    {
        var p = new PersonModel(person);
        Items.Add(p);
        OnPropertyChanged(DataElementName.SelectedItem);
    }

    public static ObservableCollection<PersonModel>
       ToObservableCollection(
          ObservableCollection<PersonModel> observable,
          List<PersonInfo> items, Boolean clear = true,
          Boolean removeDuplicates = false)
    {
        if (observable == null)
            observable =
               new ObservableCollection<PersonModel>();
        else if (clear)
            observable.Clear();

        if (removeDuplicates)
        {
            Int32? foundIndex = null;
            foreach (var i in items)
            {
                foundIndex = Find(observable, i);
                if (foundIndex.HasValue)
                    items.RemoveAt(foundIndex.Value);
            }
        }
        if (items.Count == 0)
            return observable;

        PersonModel reference;
        foreach (PersonInfo i in items)
        {
            reference = new PersonModel(i);
            observable.Add(reference);
        }
        return observable;
    }

    #endregion
    #region -- 4.00 - Services (GET, POST and DELETE) Methods

    public async void Find(
       String email, String phone, String givenName, String surName,
       Action<RequestResponseInfo<List<PersonInfo>>> onFindDone = null)
    {
        PersonInfo person = new PersonInfo();
        person.Email = Edam.Convert.ToNotNullString(email);
        person.Phone.PhoneNumber = Edam.Convert.ToNotNullString(phone);
        person.Phone.PhoneNumber = Edam.Convert.ToNumbers(
           person.Phone.PhoneNumber);
        person.Name.GivenName = Edam.Convert.ToNotNullString(givenName);
        person.Name.FatherSurname = Edam.Convert.ToNotNullString(surName);

        if (!person.ValidateFind() && (
           !String.IsNullOrWhiteSpace(givenName) &&
           !String.IsNullOrWhiteSpace(surName)))
            return;

        StatusMessageText = String.Empty;
        var results = await services.PersonService.GetPersonRecord(
           null, null, person, references.ReferenceListGroup.Find);

        Int32 c = 0;
        if (results.Success)
        {
            if (results.ResponseData != null)
            {
                //if (Items == null)
                //   Items = new ObservableCollection<PersonModel>();
                List<PersonInfo> l = new List<PersonInfo>();
                foreach (var i in results.ResponseData)
                {
                    l.Clear();
                    var indx = Find(Items, i);
                    if (!indx.HasValue)
                    {
                        l.Add(i);
                        ToObservableCollection(Items, l, false, false);
                        c++;
                    }
                }
                StatusMessageText = c + " " +
                   resource.GetLocalString("RecordsFound");
                OnDone();
            }
            else
                StatusMessageText =
                   resource.GetLocalString("RecordsNotReturned");
        }
        else
            StatusMessageText =
               resource.GetLocalString("RecordsFindFailed");

        onFindDone?.Invoke(results);
    }

    public Boolean SetItem(PersonInfo person, RequestAlertInfo listGroup)
    {
        if (person == null)
            return false;
        SelectedItem = new PersonModel(person);
        if (SelectedItem == null)
            return false;
        TitleText =
           listGroup == null ? String.Empty : listGroup.RequestDescription;
        if (listGroup != null)
        {
            ReferenceListGroup.SelectedListGroup =
               (references.ReferenceListGroup)listGroup.RequestTypeNo;
        }
        return true;
    }

    public async void GetItems(PersonInfo person = null,
       Boolean addPerson = false, RequestAlertInfo listGroup = null)
    {
        if (!SetItem(person, listGroup))
            return;

        Boolean doNotClear = !addPerson;
        Boolean removeDuplicates = true;
        if (Items != null)
            Items.Clear();

        var person_ = (SelectedItem == null) ? person : SelectedItem.Record;

        var response = await services.PersonService.GetPersonRecord(
           null, null, person_, ReferenceListGroup.SelectedListGroup);

        if (response != null && response.Success)
        {
            if (response.ResponseData != null)
            {
                ToObservableCollection(
                   m_Items, response.ResponseData, doNotClear,
                   removeDuplicates);
                StatusMessageText = Items.Count.ToString() + " " +
                   resource.GetLocalString("RecordsFound");
            }
            else
                StatusMessageText =
                   resource.GetLocalString("RecordsNotReturned");
        }
        else
            StatusMessageText =
               resource.GetLocalString("RecordsFindFailed");
    }

    public async void PostPerson(String requestId, PersonInfo person)
    {
        String sid = Session.SessionId;
        person.Name.SetCamelName();
        bool allOk = false;
        var results =
           await PersonService.PostRecord(sid, String.Empty, person);
        if (results != null && results.Success)
        {
            if (results.ResponseData != null)
            {
                var isNew = String.IsNullOrWhiteSpace(person.EntityId);
                person.EntityId = results.ResponseData;
                if (isNew)
                {
                    person.Name.FullName = String.Empty;
                    AddPerson(person);
                }
                IsAdding = Visibility.Collapsed;
                StatusMessageText =
                   resource.GetLocalString("StatusSuccessSaving");
                allOk = true;
            }
        }
        if (!allOk)
            StatusMessageText =
               resource.GetLocalString("StatusFailSaving");
    }

    public async void DeletePerson(String referenceId, PersonInfo person)
    {
        if (person == null)
            return;

        String sid = Session.SessionId;
        var results = await PersonService.DeleteRecord(
           sid, person.OrganizationId,
           person.EntityId, ObjectStatus.Deleted);

        bool allOk = false;
        if (results != null && results.Success)
        {
            if (results.ResponseData != null)
            {
                DeleteFromItems(person.EntityId);
                OnDone();
            }
        }

        if (!allOk)
            StatusMessageText =
               resource.GetLocalString("StatusFailDeleting");
    }

    #endregion
    #region -- 4.00 - Command Methods

    public void DoAdd()
    {
        SelectedItem = new PersonModel();

        StatusMessageText = String.Empty;
        Set(inEditor: true, isAdding: true, editing: true);

        SelectedItem.OrganizationId =
           Edam.Application.Session.OrganizationId;
    }

    public void OnRetire()
    {
        StatusMessageText = String.Empty;
        Session.ShowMessageBox(resource.GetLocalString("AskIfSure"),
           resource.GetLocalString("AskDeleteItem"), (approved) => {
               if (approved)
                   DeletePerson(String.Empty, SelectedItem.Record);
           }, MessageBoxType.YesNo);
    }

    public void OnEdit()
    {
        Set(inEditor: true, editing: true);
    }

    /// <summary>
    /// Finally, try to save record...
    /// </summary>
    private void OnSaveRecord()
    {
        // save record...
        if (SelectedItem.IsValid)
        {
            SelectedItem.PhoneNumber =
               Edam.Convert.ToNumbers(SelectedItem.PhoneNumber);
            PostPerson(String.Empty, SelectedItem.Record);
            return;
        }
        Edam.Application.Session.ShowMessageBox(
           "Up's", "Empty or incomplete Record will not be saved!");
    }

    /// <summary>
    /// Try to find a record and if not found then save it...
    /// </summary>
    public void OnSave()
    {
        StatusMessageText = String.Empty;
        if (SelectedItem == null)
            return;

        // enough to find the person?
        if ((!String.IsNullOrWhiteSpace(SelectedItem.Email) ||
             !String.IsNullOrWhiteSpace(SelectedItem.PhoneNumber)) &&
            String.IsNullOrWhiteSpace(SelectedItem.EntityId))
        {
            Find(SelectedItem.Email, SelectedItem.PhoneNumber, String.Empty,
               String.Empty, (r) =>
               {
                   if (r != null && r.Success)
                   {
                       if (r.ResponseData != null)
                       {
                           if (r.ResponseData.Count == 0)
                               OnSaveRecord();
                       }
                   }
               });
            return;
        }

        // save record...
        OnSaveRecord();
    }

    public void OnSearch()
    {
        SelectedItem = new PersonModel();

        StatusMessageText = String.Empty;
        Set(inSearch: true);

        SelectedItem.OrganizationId =
           Edam.Application.Session.OrganizationId;
    }

    public void OnFind(
       Action<RequestResponseInfo<List<PersonInfo>>> onFindDone)
    {
        // enough to find the person?
        if (SelectedItem.Record.ValidateFind() || (
           !String.IsNullOrWhiteSpace(SelectedItem.GivenName) ||
           !String.IsNullOrWhiteSpace(SelectedItem.FatherLastName)))
        {
            Find(SelectedItem.Email, SelectedItem.PhoneNumber,
               SelectedItem.GivenName, SelectedItem.FatherLastName,
               onFindDone);
            return;
        }
        Session.ShowMessageBox(
           "Incomplete Request",
           "Something to search/find must be provided... " +
           "Enter search criteria and try again.");
    }

    public void OnFind()
    {
        OnFind(null);
    }

    public void OnDone()
    {
        StatusMessageText = String.Empty;
        Set(inViewer: true);
        HasRecord = !HasRecord;
    }

    #endregion
    #region -- 4.00 - Support Methods

    public void DoGoBack()
    {
        if (ParentMenu != null)
            ParentMenu.Goto(this, null);
    }

    public void SetState(Object state)
    {

    }

    #endregion

}
