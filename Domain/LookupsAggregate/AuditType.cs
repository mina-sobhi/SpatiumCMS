using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.SystemKeys;

namespace Domain.LookupsAggregate
{
    [Table("AuditType", Schema = DbSchemaKeys.Lookup)]
    public class AuditType:LookupBase
    {
    }
}
