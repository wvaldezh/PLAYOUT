using PLAYOUT.Models.Domain;

namespace PLAYOUT.Models.ViewModels
{
    public class NewOrderSpotRecuest
    {
        public IEnumerable<Spot> spots { get; set; }
        public List<Guid> spotIds { get; set; }
    }
}
