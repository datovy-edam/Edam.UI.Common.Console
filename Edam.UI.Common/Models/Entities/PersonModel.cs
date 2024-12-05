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
using Edam.DataObjects.References;
using Edam.DataObjects.DataCodes;
using CommunityToolkit.Mvvm.ComponentModel;
using Edam.DataObjects.Entities;
using Edam.UI.Common.Menus;

namespace Edam.UI.Common.Models.Entities
{

    public class PersonModel : ObservableObject, Menus.IMenuView
    {

        #region -- 1.00 - Properties and definitions...

        //private NoteViewModel m_Notes;
        //public NoteViewModel Notes
        //{
        //   get { return m_Notes; }
        //   set
        //   {
        //      if (m_Notes != value)
        //      {
        //         m_Notes = value;
        //         OnPropertyChanged(nameof(Notes));
        //      }
        //   }
        //}

        private ReferenceInfo m_Reference =
           new ReferenceInfo();
        private PersonInfo m_Person = new PersonInfo();

        public IMenuItemParent ParentMenu { get; set; }

        public String ReferenceId
        {
            get { return m_Reference.ReferenceId; }
            set
            {
                if (m_Reference.ReferenceId != value)
                {
                    m_Reference.ReferenceId = value;
                    OnPropertyChanged(nameof(ReferenceId));
                }
            }
        }
        public DateTime? ReferenceDate
        {
            get { return m_Reference.ReferenceDate; }
            set
            {
                if (m_Reference.ReferenceDate != value)
                {
                    m_Reference.ReferenceDate = value;
                    OnPropertyChanged(nameof(ReferenceDate));
                }
            }
        }
        public String ReferenceDescription
        {
            get { return m_Reference.ReferenceDescription; }
            set
            {
                if (m_Reference.ReferenceDescription != value)
                {
                    m_Reference.ReferenceDescription = value;
                    OnPropertyChanged(nameof(ReferenceDescription));
                }
            }
        }
        public ReferenceType ReferenceType
        {
            get { return m_Reference.ReferenceType; }
            set
            {
                if (m_Reference.ReferenceType != value)
                {
                    m_Reference.ReferenceType = value;
                    OnPropertyChanged(nameof(ReferenceType));
                }
            }
        }

        public String OrganizationId
        {
            get { return m_Person.OrganizationId; }
            set
            {
                if (m_Person.OrganizationId != value)
                {
                    m_Person.OrganizationId = value;
                    OnPropertyChanged(nameof(OrganizationId));
                }
            }
        }
        public String EntityId
        {
            get { return m_Person.EntityId; }
            set
            {
                if (m_Person.EntityId != value)
                {
                    m_Person.EntityId = value;
                    OnPropertyChanged(nameof(EntityId));
                }
            }
        }
        public String AlternateId
        {
            get { return m_Person.AlternateId; }
            set
            {
                if (m_Person.AlternateId != value)
                {
                    m_Person.AlternateId = value;
                    OnPropertyChanged(nameof(AlternateId));
                }
            }
        }
        public String Description
        {
            get { return m_Person.Description; }
            set
            {
                if (m_Person.Description != value)
                {
                    m_Person.Description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }
        public String Email
        {
            get { return m_Person.Email; }
            set
            {
                if (m_Person.Email != value)
                {
                    m_Person.Email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }
        public String PhoneNumber
        {
            get { return m_Person.Phone.PhoneNumber; }
            set
            {
                if (m_Person.Phone.PhoneNumber != value)
                {
                    m_Person.Phone.PhoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }
        public String GivenName
        {
            get { return m_Person.Name.GivenName; }
            set
            {
                if (m_Person.Name.GivenName != value)
                {
                    m_Person.Name.GivenName = value;
                    OnPropertyChanged(nameof(GivenName));
                }
            }
        }
        public String MiddleName
        {
            get { return m_Person.Name.MiddleName; }
            set
            {
                if (m_Person.Name.MiddleName != value)
                {
                    m_Person.Name.MiddleName = value;
                    OnPropertyChanged(nameof(MiddleName));
                }
            }
        }
        public String FatherLastName
        {
            get { return m_Person.Name.FatherSurname; }
            set
            {
                if (m_Person.Name.FatherSurname != value)
                {
                    m_Person.Name.FatherSurname = value;
                    OnPropertyChanged(nameof(FatherLastName));
                }
            }
        }
        public String MotherLastName
        {
            get { return m_Person.Name.MotherSurname; }
            set
            {
                if (m_Person.Name.MotherSurname != value)
                {
                    m_Person.Name.MotherSurname = value;
                    OnPropertyChanged(nameof(MotherLastName));
                }
            }
        }
        public String StateProvince
        {
            get { return m_Person.StateProvince; }
            set
            {
                if (m_Person.StateProvince != value)
                {
                    m_Person.StateProvince = value;
                    OnPropertyChanged(nameof(StateProvince));
                }
            }
        }
        public String PostalCode
        {
            get { return m_Person.PostalCode; }
            set
            {
                if (m_Person.PostalCode != value)
                {
                    m_Person.PostalCode = value;
                    OnPropertyChanged(nameof(PostalCode));
                }
            }
        }

        private Visibility m_BirthDateIsRequired;
        public Visibility BirthDateIsRequired
        {
            get { return m_BirthDateIsRequired; }
            set
            {
                if (m_BirthDateIsRequired != value)
                {
                    m_BirthDateIsRequired = value;
                    OnPropertyChanged("BirthDateIsRequired");
                }
            }
        }
        public DateTime BirthDate
        {
            get
            {
                return m_Person.BirthDate.HasValue ? m_Person.BirthDate.Value :
                new DateTime();
            }
            set
            {
                if (m_Person.BirthDate != value)
                {
                    BirthDateIsRequired = (Edam.NullDateTime.Value == value) ?
                       Visibility.Visible : Visibility.Collapsed;
                    m_Person.BirthDate = value;
                    OnPropertyChanged(nameof(BirthDate));
                }
            }
        }

        public DateTimeOffset BirthDateOffset
        {
            get
            {
                return m_Person.BirthDate.HasValue ? m_Person.BirthDate.Value :
                   new DateTime();
            }
            set
            {
                if (m_Person.BirthDate != value.Date)
                {
                    BirthDateIsRequired = (Edam.NullDateTime.Value == value) ?
                       Visibility.Visible : Visibility.Collapsed;
                    m_Person.BirthDate = value.Date;
                    OnPropertyChanged("BirthDateOffset");
                }
            }
        }

        public Int32 AddressCount
        {
            get
            {
                return m_Person.Locations == null ? 0 : m_Person.Locations.Count;
            }
        }

        public Int32 NotesCount
        {
            get { return m_Person.NoteCount; }
            set
            {
                if (m_Person.NoteCount != value)
                {
                    m_Person.NoteCount = value;
                    OnPropertyChanged(nameof(NotesCount));
                }
            }
        }

        public string NoteDaysSinceLastText
        {
            get
            {
                return m_Person.NoteDaysSinceLast < 0 ||
                   m_Person.NoteDaysSinceLast > 1000 ? String.Empty :
                   m_Person.NoteDaysSinceLast.ToString();
            }
            set
            {
                OnPropertyChanged("NoteDaysSinceLastText");
            }
        }

        public Int32 NoteDaysSinceLast
        {
            get { return m_Person.NoteDaysSinceLast; }
            set
            {
                OnPropertyChanged("NoteDaysSinceLast");
            }
        }

        public String NameText
        {
            get
            {
                return m_Person.Name.GetFullName();
            }
            set
            {
                OnPropertyChanged(nameof(NameText));
            }
        }

        private Boolean m_HasRecord;
        public Boolean HasRecord
        {
            get
            {
                m_HasRecord = RecordHasId(false);
                return m_HasRecord;
            }
            set
            {
                if (m_HasRecord != value)
                {
                    m_HasRecord = RecordHasId(false);
                    OnPropertyChanged("HasRecord");
                }
            }
        }

        public PersonInfo Record
        {
            get { return m_Person; }
            set
            {
                m_Person = value;
                RecordHasId();
            }
        }

        public Boolean IsValid
        {
            get { return m_Person == null ? false : m_Person.Validate(); }
            set
            {
                OnPropertyChanged(nameof(IsValid));
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
                    OnPropertyChanged(nameof(StatusMessageText));
                }
            }
        }

        private String m_TitleText = null;
        public String TitleText
        {
            get { return m_TitleText; }
            set
            {
                if (m_TitleText != value)
                {
                    m_TitleText = value;
                    OnPropertyChanged(nameof(TitleText));
                }
            }
        }

        private DataCodeInfo m_RoleCode;
        public DataCodeInfo RoleCode
        {
            get { return m_RoleCode; }
            set
            {
                if (m_RoleCode != value)
                {
                    m_RoleCode = value;
                    OnPropertyChanged(nameof(RoleCode));
                }
            }
        }

        private Visibility m_InEditor;
        public Visibility InEditor
        {
            get { return m_InEditor; }
            set
            {
                if (m_InEditor != value)
                {
                    m_InEditor = value;
                    OnPropertyChanged(nameof(InEditor));
                }
            }
        }

        private Visibility m_InSearch;
        public Visibility InSearch
        {
            get { return m_InSearch; }
            set
            {
                if (m_InSearch != value)
                {
                    m_InSearch = value;
                    OnPropertyChanged(nameof(InSearch));
                }
            }
        }

        private Visibility m_InViewer;
        public Visibility InViewer
        {
            get { return m_InViewer; }
            set
            {
                if (m_InViewer != value)
                {
                    m_InViewer = value;
                    OnPropertyChanged(nameof(InViewer));
                }
            }
        }

        private Visibility m_IsAdding;
        public Visibility IsAdding
        {
            get { return m_IsAdding; }
            set
            {
                if (m_IsAdding != value)
                {
                    m_IsAdding = value;
                    OnPropertyChanged(nameof(IsAdding));
                }
            }
        }

        #endregion
        #region -- 1.00 - Commands

        public ICommand DoneEditCommand { protected set; get; }
        public ICommand GoBackCommand { protected set; get; }

        #endregion
        #region -- 1.50 - Initialize Resources

        public PersonModel(PersonInfo person = null)
        {
            InitializeCommands();
            m_Person = person ?? new PersonInfo();
            //Notes = new NoteViewModel();
        }

        #endregion
        #region -- 2.00 - MVVM Commands

        private void InitializeCommands()
        {
            GoBackCommand = new Command(DoGoBack);
            DoneEditCommand = new Command(OnDone);
        }

        #endregion
        #region -- 4.00 - Support Methods

        public void ClearFields()
        {
            m_Person.ClearFields();
        }

        public Boolean RecordHasId(Boolean updateProperty = true)
        {
            var h = m_Person != null && !String.IsNullOrEmpty(m_Person.EntityId);
            if (updateProperty)
                HasRecord = h;
            return h;
        }

        public static Int32? Find(
              ObservableCollection<PersonModel> observable,
              PersonInfo item)
        {
            Int32? foundIndex = null;
            for (var x = 0; x < observable.Count; x++)
            {
                if ((item.EntityId == observable[x].EntityId) ||
                    (item.Phone.PhoneNumber == observable[x].PhoneNumber))
                {
                    foundIndex = x;
                    break;
                }
            }

            return foundIndex;
        }

        #endregion
        #region -- 4.00 - Services (GET, POST and DELETE) Methods

        #endregion
        #region -- 4.00 - Command Methods

        public void OnDone()
        {
            StatusMessageText = String.Empty;
            InEditor = Visibility.Collapsed;
            InSearch = Visibility.Collapsed;
            InViewer = Visibility.Visible;
            IsAdding = Visibility.Collapsed;
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

}
