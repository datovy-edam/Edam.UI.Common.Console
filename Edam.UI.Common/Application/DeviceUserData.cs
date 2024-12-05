using System;
using System.ComponentModel;

// -----------------------------------------------------------------------------
using CommunityToolkit.Mvvm.ComponentModel;
using Edam.Security.Cryptography;
using Edam.DataObjects.Entities;

namespace Edam.UI.Common.Application;


public class DeviceUserData : ObservableObject
{

    #region -- 1.0 - DB Properties and definitions...

    protected readonly string m_Table = "PreferenceItem";

    public string TableName
    {
        get { return m_Table; }
    }

    #endregion

    protected DateTime? m_CreatedDate;
    protected DateTime? m_LastUpdateDate;

    protected int? m_UserNo;
    protected string m_OrganizationId;
    protected string m_OrganizationName;
    protected string m_UserName;
    protected string m_UserId;
    protected string m_UserAlternateId;
    protected string m_UserEmail;

    protected SecretText m_Password = new SecretText();
    protected string m_UserPassword
    {
        get { return m_Password.ClearText; }
        set { m_Password.ClearText = value; }
    }

    protected PhoneInfo m_UserPhone = new PhoneInfo();

    protected string m_StateCode;
    protected string m_PostalCode;

    protected string m_DeviceId;
    protected string m_DeviceName;
    protected string m_IpAddress;

    protected string m_PinNumber;

    public IUserRecordData GetUserData(IUserRecordData user)
    {
        if (user == null)
            return null;

        user.CreatedDate = m_CreatedDate;
        user.LastUpdateDate = m_LastUpdateDate;

        user.UserNo = m_UserNo;
        user.OrganizationId = m_OrganizationId;
        user.OrganizationName = m_OrganizationName;
        user.UserName = m_UserName;
        user.UserId = m_UserId;
        user.UserAlternateId = m_UserAlternateId;
        user.UserEmail = m_UserEmail;
        user.UserPassword = m_UserPassword;
        user.UserPhone.PhoneNumber = m_UserPhone.PhoneNumber;
        user.StateCode = m_StateCode;
        user.PostalCode = m_PostalCode;
        user.PinNumber = m_PinNumber;

        return user;
    }

    public IDeviceUserData GetDeviceUserData(IDeviceUserData data)
    {
        if (data == null)
            return null;

        GetUserData(data);

        data.DeviceId = m_DeviceId;
        data.DeviceName = m_DeviceName;
        data.IpAddress = m_IpAddress;
        return data;
    }

}
