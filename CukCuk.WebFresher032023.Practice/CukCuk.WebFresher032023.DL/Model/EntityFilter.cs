using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.DL.Model
{
    public class EntityFilter
    {
        public int? Page { get; set; }
        public int? Start { get; set; }
        public int? Limit { get; set; }

        public List<EntityItemFilter>? Filters { get; set; }
    }
}
