using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using Microsoft.UI.Xaml.Input;
//using Xamarin.Forms;
//using device = Xamarin.Forms.Device;

using Microsoft.UI.Xaml;

// -----------------------------------------------------------------------------
using Edam.DataObjects.Notes;
using Edam.Application;
using Edam.DataObjects.References;
using resource = Edam.Application.ApplicationHelper;
using CommunityToolkit.Mvvm.ComponentModel;
using Edam.DataObjects.DataCodes;
using Edam.DataObjects;
using Edam.UI.Common.Models.Entities;
using Edam.DataObjects.Services;
using Edam.DataObjects.Objects;
using Edam.UI.Common.Models.Notes;
using Edam.UI.Common.Menus;

namespace Edam.UI.Common.Controls.Notes
{

    public class NoteViewModel : ObservableObject
    {

        #region -- 1.00 - Properties and definitions...

        protected ObservableCollection<NoteModel> m_Items =
           new ObservableCollection<NoteModel>();

        public ObservableCollection<NoteModel> Items
        {
            get { return m_Items; }
            set
            {
                if (m_Items != value)
                {
                    m_Items = value;
                    OnPropertyChanged(DataElementName.Items);

                    if (value == null || value.Count == 0)
                    {
                        TotalNotesText = String.Empty;
                    }
                }
            }
        }

        protected NoteTypeViewModel m_TypeModel = new NoteTypeViewModel();
        public NoteTypeViewModel TypeModel
        {
            get { return m_TypeModel; }
            set
            {
                if (m_TypeModel != value)
                {
                    m_TypeModel = value;
                    OnPropertyChanged("TypeModel");
                }
            }
        }

        private String m_DefaultReferenceId;
        public String DefaultReferenceId
        {
            get
            {
                return m_DefaultReferenceId;
            }
            set
            {
                if (m_DefaultReferenceId != value)
                {
                    m_DefaultReferenceId = value;
                    OnPropertyChanged("DefaultReferenceId");
                    HasReferenceId =
                       String.IsNullOrWhiteSpace(m_DefaultReferenceId) ?
                          Visibility.Collapsed : Visibility.Visible;
                }
            }
        }

        private Visibility m_HasReferenceId;
        public Visibility HasReferenceId
        {
            get
            {
                m_HasReferenceId = String.IsNullOrWhiteSpace(m_DefaultReferenceId) ?
                   Visibility.Collapsed : Visibility.Visible;
                return m_HasReferenceId;
            }
            set
            {
                m_HasReferenceId = value;
                OnPropertyChanged("HasReferenceId");
            }
        }

        private PersonModel m_ReferenceEntity = null;
        public PersonModel ReferenceEntity
        {
            get { return m_ReferenceEntity; }
            set
            {
                if (m_ReferenceEntity != value)
                {
                    m_ReferenceEntity = value;
                    OnPropertyChanged("ReferenceEntity");
                }
            }
        }

        private string m_TotalNotesText;
        public string TotalNotesText
        {
            get { return m_TotalNotesText; }
            set
            {
                if (m_TotalNotesText != value)
                {
                    m_TotalNotesText = value;
                    OnPropertyChanged("TotalNotesText");
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

        public Visibility NotAdding
        {
            get
            {
                return (IsAdding == Visibility.Visible) ?
               Visibility.Collapsed : Visibility.Visible;
            }
        }

        private NoteModel m_SelectedItem = null;
        public NoteModel SelectedItem
        {
            get { return m_SelectedItem; }
            set
            {
                if (m_SelectedItem != value)
                {
                    m_SelectedItem = value;
                    OnPropertyChanged(DataElementName.SelectedItem);
                    StatusMessageText = String.Empty;
                    InSearch = value == null ?
                       Visibility.Visible : Visibility.Collapsed;
                    InEditor = value != null ?
                       Visibility.Visible : Visibility.Collapsed;
                    IsAdding = Visibility.Collapsed;
                    if (value != null)
                        m_TypeModel.SetNoteTypeByNo(value.TypeNo);
                }
            }
        }

        public Boolean IsValid
        {
            get
            {
                if (SelectedItem == null)
                    return false;
                return SelectedItem.IsValid;
            }
        }

        protected string m_StatusMessage;
        public String StatusMessageText
        {
            get { return m_StatusMessage; }
            set
            {
                if (m_StatusMessage != value)
                {
                    m_StatusMessage = value;
                    OnPropertyChanged(DataElementName.StatusMessageText);
                }
            }
        }

        public IMenuItemParent ParentMenu { get; set; }

        #endregion
        #region -- 1.00 - Commands

        public ICommand AddCommand { protected set; get; }
        public ICommand DeleteCommand { protected set; get; }
        public ICommand SaveCommand { protected set; get; }
        public ICommand GoBackCommand { protected set; get; }
        public ICommand DoneEditCommand { protected set; get; }

        #endregion
        #region -- 1.50 - Initialize Resources

        public NoteViewModel()
        {
            InitializeCommands();

            InViewer = Visibility.Collapsed;
            InEditor = Visibility.Collapsed;
            InSearch = Visibility.Visible;
            IsAdding = Visibility.Collapsed;
        }

        #endregion
        #region -- 2.00 - MVVM Methods

        #endregion
        #region -- 2.00 - MVVM Commands

        private void InitializeCommands()
        {
            AddCommand = new Command(DoAdd);
            DeleteCommand = new Command(DoDelete);
            GoBackCommand = new Command(DoGoBack);
            SaveCommand = new Command(DoSave);
            DoneEditCommand = new Command(OnDone);
        }

        #endregion
        #region -- 4.00 - Support Methods

        public void NoteTypeIndexChanged(DataCodeInfo item)
        {
            DataCodeInfo c = item as DataCodeInfo;
            if (c == null)
                return;
            if (SelectedItem != null)
            {
                SelectedItem.TypeCode = c;
                SelectedItem.TypeDescription = c.Description;
            }

            //short? value = item.CodeNo;
            //if (SelectedItem != null && value.HasValue)
            //{
            //   var i = TypeModel.NoteTypes[value.Value];
            //   SelectedItem.TypeCode = i;
            //   SelectedItem.TypeDescription = i.Description;
            //}
        }

        public void UpdateReferenceEntity()
        {
            if (ReferenceEntity == null)
                return;
            if (ReferenceEntity is PersonModel p)
            {
                p.NotesCount = Items.Count;
                p.Record.NoteLastReferenceDate = DateTime.Now;
            }
        }

        public void SetReferenceEntity(PersonModel person)
        {
            ReferenceEntity = person;
            DefaultReferenceId =
               person == null ? String.Empty : person.EntityId;

            StatusMessageText = String.Empty;
            if (person != null &&
               !String.IsNullOrWhiteSpace(person.EntityId))
            {
                GetItems(person.EntityId);
            }

            IsAdding = Visibility.Collapsed;
            InEditor = Visibility.Collapsed;
            InSearch = Visibility.Visible;
        }

        public void ToObservableItem(
           ObservableCollection<NoteModel> observable,
           NoteInfo item)
        {
            NoteModel reference = new NoteModel(item);
            m_TypeModel.SetNoteType(reference, SelectedItem);
            observable.Add(reference);
        }

        public void ToObservableCollection(
           ObservableCollection<NoteModel> observable,
           List<NoteInfo> items)
        {
            observable.Clear();
            foreach (NoteInfo i in items)
            {
                ToObservableItem(observable, i);
            }
        }

        #endregion
        #region -- 4.00 - Services (GET, POST and DELETE) Methods

        public async void GetItems(String referenceId,
           ReferenceType type = ReferenceType.Person)
        {
            var r = await NoteService.GetNotesRecord(type, referenceId,
               String.Empty, String.Empty, null);

            if (r.Success)
            {
                if (r.ResponseData != null)
                {
                    OnDone();
                    Items = new ObservableCollection<NoteModel>();
                    ToObservableCollection(m_Items, r.ResponseData);
                    TotalNotesText = Items.Count.ToString() + " Records Found.";
                }
            }
        }

        public async void PostNote(String referenceId, NoteInfo note,
           ReferenceType type = ReferenceType.Person)
        {
            String sid = Session.SessionId;
            var response = await NoteService.PostNote(sid, type, note);

            bool allOk = false;
            if (response != null && response.Success)
            {
                if (response.ResponseData != null)
                {
                    var isNew = String.IsNullOrWhiteSpace(note.NoteId);
                    note.NoteId = response.ResponseData;
                    //if (isNew)
                    //   ToObservableItem(Items, note);
                    IsAdding = Visibility.Collapsed;
                    UpdateReferenceEntity();
                    StatusMessageText =
                       resource.GetLocalString("StatusSuccessSaving");
                    allOk = true;
                }
            }
            if (!allOk)
                StatusMessageText =
                   resource.GetLocalString("StatusFailSaving");

            if (String.IsNullOrWhiteSpace(SelectedItem.NoteId))
            {
                Items.Remove(SelectedItem);
            }
            SelectedItem = null;
        }

        public void DeleteFromItems(String noteId)
        {
            foreach (var i in Items)
            {
                if (i.NoteId == noteId)
                {
                    Items.Remove(i);
                    break;
                }
            }
        }

        public async void DeleteNote(String referenceId, NoteInfo note,
           ReferenceType type = ReferenceType.Person)
        {
            String sid = Session.SessionId;
            var r = await NoteService.DeleteNote(sid, type, note.OrganizationId,
               note.ReferenceId, note.NoteId, ObjectStatus.Deleted,
               NoteOption.DeleteRecord);
            bool allOk = false;
            if (r.Success)
            {
                if (r.ResponseData != null)
                {
                    DeleteFromItems(note.NoteId);
                    UpdateReferenceEntity();
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
            StatusMessageText = String.Empty;
            InSearch = Visibility.Collapsed;
            InEditor = Visibility.Visible;
            IsAdding = Visibility.Visible;

            var n = new NoteModel(null)
            {
                TypeNo = (Int16)NoteType.FollowUpNote,
                OrganizationId =
                  Edam.Application.Session.OrganizationId,
                ReferenceId = DefaultReferenceId
            };

            Items.Add(n);
            SelectedItem = n;
            m_TypeModel.SetNoteType(SelectedItem, null);
            TotalNotesText = m_Items.Count.ToString();
        }

        public void DoDelete()
        {
            StatusMessageText = String.Empty;
            Session.ShowMessageBox(resource.GetLocalString("AskIfSure"),
               resource.GetLocalString("AskDeleteItem"), (approved) => {
                   if (approved)
                       DeleteNote(SelectedItem.ReferenceId, SelectedItem.Note);
               }, MessageBoxType.YesNo);
        }

        public void DoGoBack()
        {
            if (ParentMenu != null)
                ParentMenu.Goto(this, null);
        }

        public void OnDone()
        {
            StatusMessageText = String.Empty;
            InEditor = Visibility.Collapsed;
            InSearch = Visibility.Visible;
            IsAdding = Visibility.Collapsed;
            SelectedItem = null;
        }

        public void DoSave()
        {
            StatusMessageText = String.Empty;
            if (SelectedItem == null)
                return;
            if (SelectedItem.IsValid)
            {
                PostNote(SelectedItem.ReferenceId, SelectedItem.Note);
                return;
            }
            Edam.Application.Session.ShowMessageBox(
               "Up's", "Empty or incomplete Note will not be saved!");
        }

        public void Clear()
        {
            StatusMessageText = String.Empty;
            if (Items == null)
                return;
            Items.Clear();
        }

        #endregion
        #region -- 4.00 - Menu Support

        //public void GotoFollowUp(NoteInfo alert)
        //{
        //   if (ParentMenu != null)
        //   {
        //      GotoEventArgs e = new GotoEventArgs();
        //      e.MenuOption = MenuOption.FollowUp;
        //      e.ShowMainMenu = false;
        //      e.State = alert;
        //      ParentMenu.Goto(this, e);
        //   }
        //}

        public void SetState(object state)
        {

        }

        #endregion

    }

}
