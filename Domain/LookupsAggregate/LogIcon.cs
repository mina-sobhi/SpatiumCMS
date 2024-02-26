using Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.SystemKeys;

namespace Domain.LookupsAggregate
{
    [Table("LogIcons",Schema = DbSchemaKeys.Lookup)]
    public class LogIcon:LookupBase
    {
        public string Path { get; set; }
    }
}
