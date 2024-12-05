using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// -----------------------------------------------------------------------------
using Edam.DataObjects.Devices;

/*
using Edam.DataObjects.Services;

namespace Edam.UI.Application;

public class AccountHelper
{

    /// <summary>
    /// Log out user...
    /// </summary>
    public static void OnLogout()
    {
        var sessionId = Edam.Application.Session.SessionId;
        var organizationId = Edam.Application.Session.OrganizationId;

        // get current logged user, if not there is not logged user yet...
        var logUser = Edam.Application.Session.LoggedUser;
        if (logUser == null)
            // TODO: put a mess here...
            return;

        var requestId = logUser.RequestId;
        var referenceId = logUser.ReferenceId;
        IdentityService identityService = IdentityService.GetService();
        identityService.GetLogoutRequest(sessionId, requestId,
           organizationId, referenceId).ContinueWith(task => {
               //device.BeginInvokeOnMainThread(() =>
               {
                   var response = task.Result;
                   if (response != null && response.Success)
                   {
                       // TODO: do clean-up as needed
                       //     : goto login view form...
                   }
                   Edam.Application.Session.LogoutUser();
               }
               //);
           }, TaskContinuationOptions.OnlyOnRanToCompletion);
    }

}
 */
