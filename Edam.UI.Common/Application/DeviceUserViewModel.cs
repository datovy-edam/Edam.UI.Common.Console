using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
//using Xamarin.Forms;

// -----------------------------------------------------------------------------
using SQLite;
using Edam.Data;
using Edam.DataObjects.Entities;
using Edam.Diagnostics;
using Edam.DataObjects.References;
//using Edam.UI;
using Edam.Security;
using Edam.Security.Cryptography;

namespace Edam.UI.Common.Application
{

   public class DeviceUserViewModel : DeviceUserData,
      ILocalDatabaseIdNoObject, IDeviceUserData
   {

      #region -- 1.0 - constants / Macros...

      public const int HRESULT_TABLE_NOT_FOUND = -2146233088;
      //public static readonly string USER_EMAIL_TEXT = "UserEmail";
      //public static readonly string USER_PASSWORD_TEXT = "UserPassword";

      #endregion
      #region -- 1.0 - Properties and definitions...

      protected Action m_AutoLoginSubmit = null;
      protected Action m_ShowLoginControl = null;

      [PrimaryKey, AutoIncrement]
      public int IdNo
      {
         get { return m_UserNo ?? 0; }
         set { m_UserNo = value; }
      }

      public int? UserNo
      {
         get { return m_UserNo.HasValue ? m_UserNo.Value : 0; }
         set { m_UserNo = value; }
      }

      public DateTime? CreatedDate
      {
         get { return m_CreatedDate; }
         set { m_CreatedDate = value; }
      }
      public DateTime? LastUpdateDate
      {
         get { return m_LastUpdateDate; }
         set { m_LastUpdateDate = value; }
      }

      public string OrganizationId
      {
         get { return m_OrganizationId; }
         set
         {
            if (m_OrganizationId != value)
            {
               m_OrganizationId = value;
              ////OnPropertyChanged("OrganizationId");
            }
         }
      }
      public string OrganizationName
      {
         get { return m_OrganizationName; }
         set
         {
            if (m_OrganizationName != value)
            {
               m_OrganizationName = value;
               ////OnPropertyChanged("OrganizationName");
            }
         }
      }
      public string UserName
      {
         get { return m_UserName; }
         set
         {
            if (m_UserName != value)
            {
               m_UserName = value;
               ////OnPropertyChanged("UserName");
            }
         }
      }

      [Indexed]
      public string UserId { get; set; }

      public string UserAlternateId
      {
         get { return m_UserAlternateId; }
         set
         {
            if (m_UserAlternateId != value)
            {
               m_UserAlternateId = value;
               ////OnPropertyChanged("UserAlternateId");
            }
         }
      }
      public string UserEmail
      {
         get { return m_UserEmail; }
         set
         {
            if (m_UserEmail != value)
            {
               m_UserEmail = value;
               ////OnPropertyChanged("UserEmail");
            }
         }
      }
      public string UserPassword
      {
         get { return m_UserPassword; }
         set
         {
            if (m_UserPassword != value)
            {
               m_UserPassword = value;
               ////OnPropertyChanged("UserPassword");
            }
         }
      }

      [Ignore]
      public PhoneInfo UserPhone
      {
         get { return m_UserPhone; }
         set { m_UserPhone = value; }
      }
      public string UserPhoneNumber
      {
         get { return m_UserPhone.PhoneNumber; }
         set
         {
            if (m_UserPhone.PhoneNumber != value)
            {
               m_UserPhone.PhoneNumber = value;
               ////OnPropertyChanged("UserPhoneNumber");
            }
         }
      }

      public string StateCode
      {
         get { return m_StateCode; }
         set
         {
            if (m_StateCode != value)
            {
               m_StateCode = value;
               ////OnPropertyChanged("StateCode");
            }
         }
      }
      public string PostalCode
      {
         get { return m_PostalCode; }
         set
         {
            if (m_PostalCode != value)
            {
               m_PostalCode = value;
               ////OnPropertyChanged("PostalCode");
            }
         }
      }

      public string DeviceId
      {
         get { return m_DeviceId; }
         set
         {
            if (m_DeviceId != value)
            {
               m_DeviceId = value;
               //OnPropertyChanged("DeviceId");
            }
         }
      }
      public string DeviceName
      {
         get { return m_DeviceName; }
         set
         {
            if (m_DeviceName != value)
            {
               m_DeviceName = value;
               //OnPropertyChanged("DeviceName");
            }
         }
      }
      public string IpAddress
      {
         get { return m_IpAddress; }
         set
         {
            if (m_IpAddress != value)
            {
               m_IpAddress = value;
               //OnPropertyChanged("IpAddress");
            }
         }
      }

      public string PinNumber
      {
         get { return m_PinNumber; }
         set 
         {
            if (m_PinNumber != value)
            {
               m_PinNumber = value;
               //OnPropertyChanged("PinNumber");
            } 
         }
      }

      public const String ACCEPT_ESSENTIAL = "AcceptEssential";
      private Boolean m_AcceptEssential;
      [Ignore]
      public Boolean AcceptEssential
      {
         get { return m_AcceptEssential; }
         set
         {
            if (m_AcceptEssential != value)
            {
               m_AcceptEssential = value;
               //OnPropertyChanged(ACCEPT_ESSENTIAL);
            }
         }
      }

      public const String ACCEPT_ALL = "AcceptAll";
      private Boolean m_AcceptAll;
      [Ignore]
      public Boolean AcceptAll
      {
         get { return AcceptPrivacy && AcceptTerms; }
         set
         {
            if (m_AcceptAll != value)
            {
               m_AcceptAll = value;
               //OnPropertyChanged(ACCEPT_ALL);
            }
         }
      }

      private Boolean m_AcceptPrivacy;
      public Boolean AcceptPrivacy
      {
         get { return m_AcceptPrivacy; }
         set
         {
            if (m_AcceptPrivacy != value)
            {
               m_AcceptPrivacy = value;
               //OnPropertyChanged("AcceptPrivacy");
               //OnPropertyChanged(ACCEPT_ALL);
            }
         }
      }

      private Boolean m_AcceptTerms;
      public Boolean AcceptTerms
      {
         get { return m_AcceptTerms; }
         set
         {
            if (m_AcceptTerms != value)
            {
               m_AcceptTerms = value;
               //OnPropertyChanged("AcceptTerms");
               //OnPropertyChanged(ACCEPT_ALL);
            }
         }
      }

      private Boolean m_KeepMeLogged;
      public Boolean KeepMeLogged
      {
         get { return m_KeepMeLogged; }
         set
         {
            if (m_KeepMeLogged != value)
            {
               m_KeepMeLogged = value;
               //OnPropertyChanged("KeepMeLogged");
            }
         }
      }

      #endregion
      #region -- 1.0 - Commands

      //public ICommand LoginCommand { protected set; get; }
      //public ICommand LogoutCommand { protected set; get; }
      //public ICommand SaveRecordCommand { protected set; get; }

      #endregion
      #region -- 1.5 - Initialize Resources

      private async void InitializeInstance(int? idNo = null)
      {
         LocalDatabaseConnection c = new LocalDatabaseConnection();
         var r = await c.CreateTableAsync<DeviceUserViewModel>();
         c.Dispose();
         c = null;
      }

      public DeviceUserViewModel()
      {
         IdNo = -1;
         CreatedDate = DateTime.Now;
         ClearFields();
         InitializeCommands();
      }

      #endregion
      #region -- 2.0 - MVVM Methods

      #endregion
      #region -- 2.0 - MVVM Commands

      private void InitializeCommands()
      {
         //LoginCommand = new Command(Login);
         //LogoutCommand = new Command(Logout);
         //SaveRecordCommand = new Command(SaveRecord);
      }

      public void ResetEditor()
      {

      }

      public void Login()
      {

      }

      public void Logout()
      {

      }

      /// <summary>
      /// Validate Login Input.
      /// </summary>
      /// <returns></returns>
      public Boolean ValidateEssentialInput()
      {
         if (String.IsNullOrWhiteSpace(OrganizationId) ||
             String.IsNullOrWhiteSpace(UserEmail))
            return false;
         return true;
      }

      /// <summary>
      /// Validate Login Input.
      /// </summary>
      /// <returns></returns>
      public Boolean ValidateLoginInput()
      {
         if (!ValidateEssentialInput() ||
             String.IsNullOrWhiteSpace(UserPassword))
            return false;
         return true;
      }

      #endregion
      #region -- 4.0 - Define Services...

      /// <summary>
      /// Save User Record locally.
      /// </summary>
      public void SaveRecord()
      {
         LocalDatabaseConnection c = new LocalDatabaseConnection();
         DeviceUserViewModel record = GetRecord();
         record.UserPassword = m_Password.EncryptedText;
         string errorMessage;
         try
         {
            if (!KeepMeLogged)
               record.UserPassword = String.Empty;
            if (m_UserNo < 0)
            {
               if (IdNo < 0)
               {
                  IdNo = 0;
                  record.IdNo = IdNo;
               }
               c.InsertAsync<DeviceUserViewModel>(record).
                  ContinueWith(t =>
                  {
                     InsertUpdateDone(true);
                  });
            }
            else
            {
               c.UpdateAsync<DeviceUserViewModel>(record).
                  ContinueWith(t =>
                  {
                     InsertUpdateDone(true);
                  });
            }
         }
         catch(Exception ex)
         {
            // TODO: log the exception
            errorMessage = ex.Message;
         }
         finally
         {
            if (c != null)
               c.Dispose();
         }

         c = null;
      }

      public static IResultsLog DeleteRecord()
      {
         IResultsLog results = new ResultLog();
         LocalDatabaseConnection c = new LocalDatabaseConnection();
         try
         {
            Task<int> task = c.DeleteAsync<DeviceUserViewModel>(1);
            results.Succeeded();
         }
         catch (Exception ex)
         {
            results.Failed(ex);
         }
         finally
         {
            if (c != null)
               c.Dispose();
         }
         c = null;
         return results;
      }

      /// <summary>
      /// Read Device User Logged Record.
      /// </summary>
      public async void ReadRecord()
      {
         Boolean isOk = false;
         String errMess;
         LocalDatabaseConnection c = new LocalDatabaseConnection();
         try
         {
            //var l = await c.GetIntIdObjectAsync<DeviceUserViewModel>(1);
            var l = await c.GetItemsAsync<DeviceUserViewModel>();
            if (l != null && l.Count > 0)
            {
               m_UserNo = l[0].IdNo;
               OrganizationId = l[0].OrganizationId;
               UserEmail = l[0].UserEmail;

               m_Password.SetSecretText(l[0].UserPassword);
               UserPassword =
                  l[0].KeepMeLogged ? m_Password.ClearText : String.Empty;

               AcceptTerms = l[0].AcceptTerms;
               AcceptPrivacy = l[0].AcceptPrivacy;
               KeepMeLogged = l[0].KeepMeLogged;
               PinNumber = l[0].PinNumber;
            }
            isOk = true;
         }
         catch(Exception ex)
         {
            errMess = ex.Message;
            isOk = ex.HResult != HRESULT_TABLE_NOT_FOUND;
            AcceptTerms = false;
            AcceptPrivacy = false;
            KeepMeLogged = false;
         }
         finally
         {
            if (c != null)
               c.Dispose();
         }
         c = null;

         if (!isOk)
         {
            InitializeInstance();
            m_UserNo = -1;
            ResetEditor();
         }
         //OnPropertyChanged(ACCEPT_ALL);

         if (m_AutoLoginSubmit != null)
         {
            if (ValidateLoginInput() && AcceptAll && KeepMeLogged)
            {
               m_AutoLoginSubmit();
               m_AutoLoginSubmit = null;
               return;
            }            
         }
         if (m_ShowLoginControl != null)
         {
            m_ShowLoginControl();
            m_ShowLoginControl = null;
         }
      }

      #endregion
      #region -- 4.0 - Support Methods

      public DeviceUserViewModel GetRecord()
      {
         var lastDt = new DateTime();
         var m = new DeviceUserViewModel
         {
            IdNo = IdNo,
            DeviceId = DeviceId,
            DeviceName = DeviceName,
            OrganizationId = OrganizationId,
            OrganizationName = OrganizationName,
            PostalCode = PostalCode,
            StateCode = StateCode,
            UserAlternateId = UserAlternateId,
            UserEmail = UserEmail,
            UserId = UserId,
            UserName = UserName,
            UserNo = UserNo,
            UserPassword = UserPassword,
            UserPhoneNumber = UserPhoneNumber,
            AcceptTerms = AcceptTerms,
            AcceptPrivacy = AcceptPrivacy,
            KeepMeLogged = KeepMeLogged,
            CreatedDate = CreatedDate,
            PinNumber = PinNumber,
            LastUpdateDate = lastDt
         };
         return m;
      }

      public void ClearFields()
      {
         OrganizationId = String.Empty;
         OrganizationName = String.Empty;
         UserName = String.Empty;
         UserId = String.Empty;
         UserAlternateId = String.Empty;
         UserEmail = String.Empty;
         UserPhoneNumber = String.Empty;
         StateCode = String.Empty;
         PostalCode = String.Empty;
         AcceptPrivacy = false;
         AcceptTerms = false;
         KeepMeLogged = false;
         LastUpdateDate = DateTime.Now;
      }

      public void InsertUpdateDone(bool done)
      {

      }

      #endregion
      #region -- 4.0 - From/To Methods

      public EntityContactInfo ToContact()
      {
         EntityContactInfo c = new EntityContactInfo
         {
            AssociationType = ReferenceBaseType.Personal,
            EntityAlias = UserName,
            EntityAlternateId = UserAlternateId,
            EntityDescription = UserName,
            EntityEmail = UserEmail,
            EntityId = UserId,
            EntityPhoneNumber = UserPhoneNumber,
            OrganizationId = OrganizationId,
            OrganizationName = OrganizationName,
            StateCode = StateCode,
            PostalCode = PostalCode
         };
         return c;
      }

      public void FromContact(EntityContactInfo contact)
      {
         OrganizationId = contact.OrganizationId;
         OrganizationName = contact.OrganizationName;
         UserName = contact.EntityAlias;
         UserId = contact.EntityId;
         UserAlternateId = contact.EntityAlternateId;
         UserEmail = contact.EntityEmail;
         UserPhoneNumber = contact.EntityPhoneNumber;
         StateCode = contact.StateCode;
         PostalCode = contact.PostalCode;
      }

      #endregion

   }

}
