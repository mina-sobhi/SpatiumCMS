using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.storageAggregate.Input
{
    public class UpdateFileInput
    {
        public string Name { get; set; }
        public string Extention { get; set; }
        public string Caption { get; set; }
        public string FileSize { get; set; }
        public string Alt { get; set; }
        public string? Dimension { get; set; }
    }
}
