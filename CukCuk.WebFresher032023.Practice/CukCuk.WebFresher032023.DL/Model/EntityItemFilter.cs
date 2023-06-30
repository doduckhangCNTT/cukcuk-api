using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.DL.Model
{
    public class EntityItemFilter
    {
        public string? Xtype { get; set; }
        public bool? IsCreateFromFilterRow { get; set; }
        public string? Property { get; set; }
        public string? Operator { get; set; }
        public string? Value { get; set; }
        public string? Type { get; set; }
        public string? Addition { get; set; }
        public string? Group { get; set; }
    }
}
