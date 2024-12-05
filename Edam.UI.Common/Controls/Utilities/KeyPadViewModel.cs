using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

// -----------------------------------------------------------------------------
using CommunityToolkit.Mvvm.ComponentModel;
using Edam.DataObjects;
using resource = Edam.Application.ApplicationHelper;
using Edam.Security;
using Edam.UI.Common.Controls.Accounts;


namespace Edam.UI.Common.Controls.Utilities
{

   public class KeyPadViewModel : ObservableObject
   {
      private const int MAX_FAIL_COUNT = 3;
      private int m_FailCount = -1;
      private string m_Title;
      private PinNumber m_PinNumber = new PinNumber();
      public UserControl OKButton { get; set; }

      public bool ForceLogoutMaxFailCountIndicator { get; set; }

      public string TitleText
      {
         get { return m_Title; }
         set
         {
            if (m_Title != value)
            {
               m_Title = value;
               OnPropertyChanged(DataElementName.TitleText);
            }
         }
      }

      public string PinNumberText
      {
         get { return m_PinNumber.Value; }
         set
         {
            if (m_PinNumber.Value != value)
            {
               m_PinNumber.Value = value;
               OnPropertyChanged("PinNumberText");
               OKEnabled = (m_PinNumber.Length >= m_PinMinLength);
            }
         }
      }

      public string PinHashValue
      {
         get
         {
            return m_PinNumber.HashValue;
         }
      }

      private object m_ParentState = null;
      public object ParentState
      {
         get { return m_ParentState; }
         set
         {
            m_ParentState = value;
            SetUpTitle();
         }
      }

      private Int16 m_PinMinLength = 4;
      private bool m_ValidatePin = false;

      private bool m_OKEnabled = false;
      public bool OKEnabled
      {
         get { return m_PinNumber.Length > m_PinMinLength; }
         set
         {
            if (m_OKEnabled != value)
            {
               m_OKEnabled = value;
               OnPropertyChanged("OKEnabled");
               if (OKButton != null)
                  OKButton.IsEnabled = value;
            }
         }
      }

      public KeyPadViewModel() : base()
      {
         SetUpTitle();
         OKButton = null;
         ForceLogoutMaxFailCountIndicator = true;
      }

      private LoginViewModel GetParentModel()
      { 
         if (m_ParentState == null)
            return null;
         return ParentState as LoginViewModel;
      }

      /// <summary>
      /// Set-Up Title label base on given Pin Number availability.
      /// </summary>
      private void SetUpTitle()
      {
         var lvm = GetParentModel();
         m_ValidatePin = lvm != null &&
           !String.IsNullOrWhiteSpace(lvm.PinNumber);
         TitleText = m_ValidatePin ? resource.GetString("PinNumberEnter") :
            String.Format(resource.GetString("PinSelect"),
               m_PinMinLength.ToString());
         OKEnabled = false;
      }

      public void ShowMinLength()
      {
         TitleText = String.Format(
            resource.GetString("LengthMin"), m_PinMinLength.ToString());
      }

      public void OnOKPressed(string text)
      {
         if (m_PinNumber.Length >= m_PinMinLength)
         {
            PinNumberText = text;
            CompareAndContinue();
         }
         else
         {
            ShowMinLength();
         }
      }

      private void ForceLogout()
      {
         Application.DeviceUserViewModel.DeleteRecord();
         Microsoft.UI.Xaml.Application.Current.Exit();
      }

      public void ClearAll()
      {
         m_FailCount = 0;
         SetUpTitle();
      }

      /// <summary>
      /// Validate PinNumber with LoginViewModel PinNumber Hash if any is 
      /// available.
      /// </summary>
      /// <returns>true if both are the same Hash</returns>
      public bool Validate()
      {
         var lvm = GetParentModel();
         if (lvm == null)
            return false;

         return m_PinNumber.HashCompare(lvm.PinNumber);
      }

      private string GetFailMessage(string label = null)
      {
         m_FailCount++;
         if (ForceLogoutMaxFailCountIndicator && m_FailCount > MAX_FAIL_COUNT)
         {
            ForceLogout();
         }
         return "Fail " +
            (String.IsNullOrWhiteSpace(label) ? String.Empty : label) +
            (m_FailCount > 0 ? m_FailCount.ToString() : String.Empty) +
            ", Try Again";
      }

      public void SaveAndContinue()
      {
         var lvm = GetParentModel();
         if (lvm == null)
            return;
         lvm.PersistUser(m_PinNumber, (x) => {
            if (!x)
            {
               TitleText = GetFailMessage("Persist");
            }
         });
      }

      public void CompareAndContinue()
      {
         if (m_ValidatePin)
         {
            if (!Validate())
            {
               TitleText = GetFailMessage("Match");
               return;
            }
            Application.ApplicationHelper.ResetApplication();
         }
         else
         {
            SaveAndContinue();
         }
      }

   }

}
