using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.SystemKeys;

namespace Domain.LookupsAggregate
{
    [Table("RoleIcon", Schema = DbSchemaKeys.Lookup)]
    public class RoleIcon:LookupBase
    {
        public string IconPath { get; set; }
    }
}
