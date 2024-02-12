using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.SystemKeys;

namespace Domain.LookupsAggregate
{
    [Table("PostStatus",Schema =DbSchemaKeys.Lookup)]
    public class PostStatus:LookupBase
    {

    }
}
