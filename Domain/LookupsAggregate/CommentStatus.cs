using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.SystemKeys;

namespace Domain.LookupsAggregate
{
    [Table("CommentStatus",Schema =DbSchemaKeys.Lookup)]
    public class CommentStatus:LookupBase
    {
    }
}
