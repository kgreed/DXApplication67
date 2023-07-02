using DevExpress.Persistent.BaseImpl.EFCore.AuditTrail;
using System.ComponentModel.DataAnnotations.Schema;

namespace DXApplication67.Module.BusinessObjects
{
    [Table("AuditData")]
    [DevExpress.Persistent.Base.NavigationItem("Logs")]
    public class AuditLog : AuditDataItemPersistent
    {


        // public AuditLog() : base() { UserLogins = new ObservableCollection<ApplicationUserLoginInfo>(); }

    }
}
