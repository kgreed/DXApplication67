﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.Persistent.BaseImpl.EFCore.AuditTrail;

namespace DXApplication67.Module.BusinessObjects;

[DefaultProperty(nameof(UserName))]
public class ApplicationUser : PermissionPolicyUser, ISecurityUserWithLoginInfo {
    public ApplicationUser() : base() {
        UserLogins = new ObservableCollection<ApplicationUserLoginInfo>();
    }

    [Browsable(false)]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual IList<ApplicationUserLoginInfo> UserLogins { get; set; }

    IEnumerable<ISecurityUserLoginInfo> IOAuthSecurityUser.UserLogins => UserLogins.OfType<ISecurityUserLoginInfo>();

    ISecurityUserLoginInfo ISecurityUserWithLoginInfo.CreateUserLoginInfo(string loginProviderName, string providerUserKey) {
        ApplicationUserLoginInfo result = ((IObjectSpaceLink)this).ObjectSpace.CreateObject<ApplicationUserLoginInfo>();
        result.LoginProviderName = loginProviderName;
        result.ProviderUserKey = providerUserKey;
        result.User = this;
        return result;
    }
}
