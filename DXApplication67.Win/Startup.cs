﻿using System.Configuration;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using Microsoft.EntityFrameworkCore;
using DevExpress.ExpressApp.EFCore;
using DevExpress.EntityFrameworkCore.Security;
using DevExpress.XtraEditors;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.ExpressApp.Design;

namespace DXApplication67.Win;

public class ApplicationBuilder : IDesignTimeApplicationFactory {
    public static WinApplication BuildApplication(string connectionString) {
        var builder = WinApplication.CreateBuilder();
        builder.UseApplication<DXApplication67WindowsFormsApplication>();
        builder.Modules
            .AddAuditTrailEFCore()
            .AddConditionalAppearance()
            .AddValidation(options => {
                options.AllowValidationDetailsAccess = false;
            })
            .Add<DXApplication67.Module.DXApplication67Module>()
        	.Add<DXApplication67WinModule>();
        builder.ObjectSpaceProviders
            .AddSecuredEFCore().WithAuditedDbContext(contexts => {
                contexts.Configure<DXApplication67.Module.BusinessObjects.DXApplication67EFCoreDbContext, DXApplication67.Module.BusinessObjects.DXApplication67AuditingDbContext>(
                    (application, businessObjectDbContextOptions) => {
                        // Uncomment this code to use an in-memory database. This database is recreated each time the server starts. With the in-memory database, you don't need to make a migration when the data model is changed.
                        // Do not use this code in production environment to avoid data loss.
                        // We recommend that you refer to the following help topic before you use an in-memory database: https://docs.microsoft.com/en-us/ef/core/testing/in-memory
                        //businessObjectDbContextOptions.UseInMemoryDatabase("InMemory");
                        businessObjectDbContextOptions.UseSqlServer(connectionString);
                        businessObjectDbContextOptions.UseChangeTrackingProxies();
                        businessObjectDbContextOptions.UseObjectSpaceLinkProxies();
                    },
                    (application, auditHistoryDbContextOptions) => {
                        auditHistoryDbContextOptions.UseSqlServer(connectionString);
                        auditHistoryDbContextOptions.UseChangeTrackingProxies();
                        auditHistoryDbContextOptions.UseObjectSpaceLinkProxies();
                    });
            })
            .AddNonPersistent();
        builder.Security
            .UseIntegratedMode(options => {
                options.RoleType = typeof(PermissionPolicyRole);
                options.UserType = typeof(DXApplication67.Module.BusinessObjects.ApplicationUser);
                options.UserLoginInfoType = typeof(DXApplication67.Module.BusinessObjects.ApplicationUserLoginInfo);
            })
            .UsePasswordAuthentication();
        builder.AddBuildStep(application => {
            application.ConnectionString = connectionString;
#if DEBUG
            if(System.Diagnostics.Debugger.IsAttached && application.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                application.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
        });
        var winApplication = builder.Build();
        return winApplication;
    }

    XafApplication IDesignTimeApplicationFactory.Create()
        => BuildApplication(XafApplication.DesignTimeConnectionString);
}
