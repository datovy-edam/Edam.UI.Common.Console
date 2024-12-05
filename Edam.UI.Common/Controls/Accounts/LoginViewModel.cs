using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml;

// -----------------------------------------------------------------------------
using Edam.DataObjects.Entities;
using resource = Edam.Application.ApplicationHelper;
using Edam.Application;
using Edam.DataObjects.Requests;
using appHelper = Edam.UI.Common.Application;
using Edam.DataObjects.Services;
using Edam.UI;
using Edam.Security;
using uiapp = Edam.Application.Settings;
using Edam.Data;
using Edam.UI.Common.Application;
using System.ComponentModel.DataAnnotations;
using Edam.DataObjects.ReferenceData;

namespace Edam.UI.Common.Controls.Accounts;


/// <summary>
/// Allow User to Login or state that the Email or Password is forgotten...
/// </summary>
public class LoginViewModel : DeviceUserViewModel
{

    #region -- 1.00 - Fields and Properties

    public const string TITLE = "Title";
    public const string TITLE_TEXT = "TitleText";
    public const string USER_EMAIL_TEXT = "UserEmail";
    public const string USER_PASSWORD_TEXT = "UserPassword";
    public const string PASSWORD2_TEXT = "Password2";
    public const string REQUEST_ID_TEXT = "RequestId";
    public const string SHOW_REQUEST_ID_TEXT = "ShowRequestId";
    public const string SHOW_PASSWORD2_TEXT = "ShowPassword2";

    public const string SUBMIT_TEXT = "SubmitText";

    public const string CHECK_EMAIL_CODE_TEXT = "CheckForEmailedCode";
    public const string SET_CODE_VISIBLE_TEXT = "IsSetCodeVisible";
    public const string EMAIL_ENABLED_TEXT = "IsEmailEnabled";
    public const string FORGOT_VISIBLE_TEXT = "IsForgotVisible";
    public const string PASSWORD1_VISIBLE_TEXT = "IsPassword1Visible";
    public const string PASSWORD2_VISIBLE_TEXT = "IsPassword2Visible";

    public static readonly string DEFAULT_TITLE_TEXT =
       resource.GetLocalString("SecurityIsLoginTime");
    public static readonly string DEFAULT_FORGOT_TEXT =
       resource.GetLocalString("PromptYouForgot");

    public Int32 MaxWidth { get; set; }

    protected String m_SubmitText;
    public String SubmitText
    {
        get { return m_SubmitText; }
        set
        {
            m_SubmitText = value;
            OnPropertyChanged(SUBMIT_TEXT);
        }
    }

    protected Boolean m_IsForgotVisible;
    public Boolean IsForgotVisible
    {
        get { return m_IsForgotVisible; }
        set
        {
            m_IsForgotVisible = value;
            OnPropertyChanged(FORGOT_VISIBLE_TEXT);
        }
    }

    protected Visibility m_ForgotVisible;
    public Visibility ForgotVisible
    {
        get { return m_ForgotVisible; }
        set
        {
            m_ForgotVisible = value;
            OnPropertyChanged("ForgotVisible");
        }
    }

    protected Boolean m_IsPassword1Visible;
    public Boolean IsPassword1Visible
    {
        get { return m_IsPassword1Visible; }
        set
        {
            m_IsPassword1Visible = value;
            OnPropertyChanged(PASSWORD1_VISIBLE_TEXT);
        }
    }

    protected Visibility m_Password1Visible;
    public Visibility Password1Visible
    {
        get { return m_Password1Visible; }
        set
        {
            m_Password1Visible = value;
            OnPropertyChanged("Password1Visible");
        }
    }

    protected Boolean m_IsPassword2Visible;
    public Boolean IsPassword2Visible
    {
        get { return m_IsPassword2Visible; }
        set
        {
            m_IsPassword2Visible = value;
            OnPropertyChanged(PASSWORD2_VISIBLE_TEXT);
        }
    }

    protected Visibility m_Password2Visible;
    public Visibility Password2Visible
    {
        get { return m_Password2Visible; }
        set
        {
            m_Password2Visible = value;
            OnPropertyChanged("Password2Visible");
        }
    }

    protected Boolean m_IsEmailEnabled;
    public Boolean IsEmailEnabled
    {
        get { return m_IsEmailEnabled; }
        set
        {
            m_IsEmailEnabled = value;
            OnPropertyChanged(EMAIL_ENABLED_TEXT);
        }
    }

    protected Boolean m_IsSetCodeVisible;
    public Boolean IsSetCodeVisible
    {
        get { return m_IsSetCodeVisible; }
        set
        {
            m_IsSetCodeVisible = value;
            OnPropertyChanged(SET_CODE_VISIBLE_TEXT);
        }
    }

    protected Visibility m_SetCodeVisible;
    public Visibility SetCodeVisible
    {
        get { return m_SetCodeVisible; }
        set
        {
            m_SetCodeVisible = value;
            OnPropertyChanged("SetCodeVisible");
        }
    }

    protected string m_TitleText;
    public string TitleText
    {
        get { return m_TitleText; }
        set
        {
            m_TitleText = value;
            OnPropertyChanged(TITLE_TEXT);
        }
    }

    protected string m_Password2;
    public string Password2
    {
        get { return m_Password2; }
        set
        {
            m_Password2 = value;
            OnPropertyChanged(PASSWORD2_TEXT);
        }
    }

    protected string m_RequestId;
    public string RequestId
    {
        get { return m_RequestId; }
        set
        {
            m_RequestId = value;
            OnPropertyChanged(REQUEST_ID_TEXT);
        }
    }

    protected string m_CheckForEmailedCode;
    public string CheckForEmailedCode
    {
        get { return m_CheckForEmailedCode; }
        set
        {
            m_CheckForEmailedCode = value;
            OnPropertyChanged(CHECK_EMAIL_CODE_TEXT);
        }
    }

    protected Boolean m_GettingEntriesIndicator;
    public Boolean GettingEntriesIndicator
    {
        get { return m_GettingEntriesIndicator; }
        set
        {
            m_GettingEntriesIndicator = value;
            OnPropertyChanged("GettingEntriesIndicator");
            Busy = !m_GettingEntriesIndicator;
        }
    }

    protected Visibility m_EditorAreaVisible;
    public Visibility EditorAreaVisible
    {
        get { return m_EditorAreaVisible; }
        set
        {
            m_EditorAreaVisible = value;
            OnPropertyChanged("EditorAreaVisible");
            BusyVisible = (m_EditorAreaVisible == Visibility.Visible) ?
               Visibility.Collapsed : Visibility.Visible;
        }
    }

    protected Boolean m_Busy;
    public Boolean Busy
    {
        get { return m_Busy; }
        set
        {
            m_Busy = value;
            OnPropertyChanged("Busy");
        }
    }

    protected Visibility m_BusyVisible;
    public Visibility BusyVisible
    {
        get { return m_BusyVisible; }
        set
        {
            m_BusyVisible = value;
            OnPropertyChanged("BusyVisible");
        }
    }

    private UserLoggedInfo m_UserLoggedInfo;
    public UserLoggedInfo LoggedInfo
    {
        get { return m_UserLoggedInfo; }
        set
        {
            m_UserLoggedInfo = value;
        }
    }

    private Action<bool> FollowUpCallBack = null;

    private IdentityService m_IdentityService;

    private Visibility m_DataSourceVisibility;
    public Visibility DataSourceVisibility
    {
        get { return m_DataSourceVisibility; }
        set
        {
            if (m_DataSourceVisibility != value)
            {
                m_DataSourceVisibility = value;
                OnPropertyChanged(nameof(DataSourceVisibility));
            }
        }
    }

    private string m_ConnectionString;
    public string ConnectionString
    {
        get { return m_ConnectionString; }
        set
        {
            if (m_ConnectionString != value)
            {
                m_ConnectionString = value;
                OnPropertyChanged(nameof(ConnectionString));
            }
        }
    }

    #endregion
    #region -- 1.50 - Initialize Resources

    public ICommand OnSubmitCommand { get; private set; }
    public ICommand OnForgotCommand { get; private set; }
    public ICommand OnCancelCommand { get; private set; }

    public LoginViewModel() : base()
    {
        DataSourceVisibility = String.IsNullOrWhiteSpace(
           DataSources.GetDefaultConnectionString()) ?
              Visibility.Visible : Visibility.Collapsed;

        OnSubmitCommand = new Command(OnSubmit);
        OnForgotCommand = new Command(OnForgot);
        OnCancelCommand = new Command(OnCancel);
        OnCancel();

        MaxWidth = 400;
        TitleText = resource.GetLocalString("SecurityLogin");
        SubmitText = TitleText;

        GettingEntriesIndicator = true;
        EditorAreaVisible = Visibility.Collapsed;
        BusyVisible = Visibility.Visible;
        AutoLogin();
        m_IdentityService = IdentityService.GetService();

        string cstring = DataSources.GetDefaultConnectionString();
        if (String.IsNullOrWhiteSpace(cstring))
        {
            ConnectionString = DataSources.DEFAULT_CONNECTION_STRING;
        }
    }

    #endregion
    #region -- 4.00 - Supporting Methods

    /// <summary>
    /// The user has already logged, persist his info as possible...
    /// </summary>
    /// <param name="user">logged info</param>
    private void UserPinFollowUp(UserLoggedInfo user)
    {
        m_UserLoggedInfo = user;
        GettingEntriesIndicator = true;
        EditorAreaVisible = Visibility.Visible;

        // follow-up by getting the Pin
        appHelper.ApplicationHelper.PinLogin(this);
    }

    /// <summary>
    /// The user has already logged, persist his info as possible...
    /// </summary>
    /// <param name="user">logged info</param>
    private void PersistUser(UserLoggedInfo user)
    {
        GettingEntriesIndicator = true;
        EditorAreaVisible = Visibility.Visible;

        Session.SetUser(user);

        // successful login... Goto next screen
        appHelper.ApplicationHelper.ResetApplication();

        SaveRecord();
    }
    public void PersistUser(PinNumber pin, Action<bool> followUpCallBack = null)
    {
        FollowUpCallBack = followUpCallBack;
        PinNumber = pin.HashValue;
        m_UserLoggedInfo.PinNumber = pin.HashValue;
        PersistUser(m_UserLoggedInfo);
    }

    public new void ResetEditor()
    {
        base.ResetEditor();
        BusyVisible = Visibility.Collapsed;
        EditorAreaVisible = Visibility.Visible;
    }

    /// <summary>
    /// If possible out-login.
    /// </summary>
    public void AutoLogin()
    {
        m_ShowLoginControl = () => ResetEditor();
        m_AutoLoginSubmit = () => SubmitLogin();
        ReadRecord();
    }

    public void CheckInput()
    {
        AcceptEssential = ValidateEssentialInput();
        AcceptAll = ValidateLoginInput() && AcceptPrivacy && AcceptTerms;
    }

    #endregion
    #region -- 4.00 - Define Services

    /// <summary>
    /// Submit Login...
    /// </summary>
    private async void SubmitLogin()
    {
        if (!ValidateLoginInput())
        {
            Session.ShowMessageBox("Incomplete Input",
               "Need Organization ID, " +
               "Email and Password, enter those and try again...");
            return;
        }

        var sessionId = String.Empty;
        var email = UserEmail;
        var passwd = UserPassword;

        GettingEntriesIndicator = false;
        EditorAreaVisible = Visibility.Collapsed;

        var response = await m_IdentityService.GetLoginRequest(
            sessionId, OrganizationId, email, passwd);
        if (response != null && response.Success)
        {
            UserPinFollowUp(response.ResponseData);
        }
        else
        {
            TitleText =
            resource.GetLocalString("SecurityFaileLoginRetry");
            if (FollowUpCallBack != null)
                FollowUpCallBack(false);
        }
    }

    /// <summary>
    /// Check that given email is well formatted if any is given...
    /// </summary>
    /// <returns>true is returned if email is usable</returns>
    private bool CheckEmail()
    {
        Boolean isGood = !String.IsNullOrWhiteSpace(UserEmail);
        if (!isGood)
            TitleText = resource.GetLocalString("SecurityMustEnterEmail");
        return isGood;
    }

    /// <summary>
    /// Change Password...
    /// </summary>
    private async void ChangePassword()
    {
        var sessionId = String.Empty;
        var deviceId = String.Empty;
        var password = IsPassword2Visible ? Password2 : String.Empty;
        var request = (String.IsNullOrWhiteSpace(RequestId)) ?
           RequestValidationStatus.PendingValidation :
           RequestValidationStatus.PasswordChangeDone;
        var response = await
           m_IdentityService.GetPasswordChangeVerification(sessionId,
              RequestId, deviceId, OrganizationId, UserEmail, password,
                 request);

        if (response != null && response.Success)
        {
            if (request != RequestValidationStatus.PendingValidation)
            {
                GettingEntriesIndicator = true;
                EditorAreaVisible = Visibility.Visible;

                OnCancel();
                TitleText = resource.GetLocalString(
                   "SecurityChangePassword");
            }
        }
        else
            TitleText = resource.GetLocalString(
               "SecurityFaileLoginRetry");
    }

    #endregion
    #region -- 4.00 - Command Methods

    /// <summary>
    /// Submit Login Request...
    /// </summary>
    public void OnSubmit()
    {
        // make sure we do have a default connection string...
        if (DataSourceVisibility == Visibility.Visible)
        {
            uiapp.AppSettings.SetDefaultConnectionString(ConnectionString);
        }

        // check that we have an email
        if (!CheckEmail())
            return;

        TitleText = resource.GetString("PromptWorking");
        if (!IsForgotVisible)
        {
            if (IsPassword2Visible)
            {
                if (UserPassword != Password2)
                {
                    TitleText = resource.GetString("SecurityNoPasswordMatch");
                    return;
                }
                if (String.IsNullOrWhiteSpace(RequestId))
                {
                    TitleText = resource.GetString(
                       "SecurityNoEnterCodeAndResubmit");
                    return;
                }
                ChangePassword();
            }
            else
            {
                TitleText = resource.GetString("SecurityEnterMatchingEmails");
                SubmitText = resource.GetString("SecurityChangePassword");

                IsSetCodeVisible = false;
                SetCodeVisible = Visibility.Collapsed;

                UserPassword = String.Empty;
                Password2 = String.Empty;

                IsPassword1Visible = true;
                Password1Visible = Visibility.Visible;
                IsPassword2Visible = true;
                Password2Visible = Visibility.Visible;
                GettingEntriesIndicator = true;
                EditorAreaVisible = Visibility.Visible;
            }
        }
        else
        {
            SubmitLogin();
        }
    }

    /// <summary>
    /// Submit the Forgot Request...
    /// </summary>
    public void OnForgot()
    {
        if (!CheckEmail())
            return;

        TitleText = DEFAULT_FORGOT_TEXT;
        CheckForEmailedCode =
           resource.GetString("SecurityCheckForEmailedCode");

        IsEmailEnabled = false;

        IsSetCodeVisible = true;
        SetCodeVisible = Visibility.Visible;
        IsForgotVisible = false;
        ForgotVisible = Visibility.Collapsed;
        IsPassword1Visible = false;
        Password1Visible = Visibility.Collapsed;
        IsPassword2Visible = false;
        Password2Visible = Visibility.Collapsed;

        SubmitText = resource.GetString("SecuritySendCode");
        GettingEntriesIndicator = false;
        EditorAreaVisible = Visibility.Collapsed;
        ChangePassword();
        GettingEntriesIndicator = true;
        EditorAreaVisible = Visibility.Visible;
    }

    /// <summary>
    /// Cancel Requests...
    /// </summary>
    public void OnCancel()
    {
        TitleText = DEFAULT_TITLE_TEXT;
        RequestId = String.Empty;
        UserPassword = String.Empty;
        Password2 = String.Empty;

        SubmitText = resource.GetString("SecurityLogin");

        IsEmailEnabled = true;

        EditorAreaVisible = Visibility.Visible;
        IsSetCodeVisible = false;
        SetCodeVisible = Visibility.Collapsed;
        IsForgotVisible = true;
        ForgotVisible = Visibility.Visible;
        IsPassword1Visible = true;
        Password1Visible = Visibility.Visible;
        IsPassword2Visible = false;
        Password2Visible = Visibility.Collapsed;
        GettingEntriesIndicator = true;
        EditorAreaVisible = Visibility.Visible;

        AcceptPrivacy = false;
        AcceptTerms = false;
        KeepMeLogged = false;
    }

    #endregion

}
