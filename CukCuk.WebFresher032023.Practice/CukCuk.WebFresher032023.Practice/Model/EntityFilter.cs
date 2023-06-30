namespace CukCuk.WebFresher032023.Practice.Model
{
    public class EntityFilter
    {
        public int? Page { get; set; }
        public int? Start { get; set; }
        public int? Limit { get; set; }

        public List<EntityItemFilter>? Filters { get; set; }
    }
}
