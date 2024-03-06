using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.SystemKeys;
namespace Domain.LookupsAggregate
{
    [Table("UserStatus", Schema = DbSchemaKeys.Lookup)]
    public class UserStatus : LookupBase
    {

    }
}
