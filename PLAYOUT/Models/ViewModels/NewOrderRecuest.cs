using Microsoft.AspNetCore.Mvc.Rendering;
using PLAYOUT.Models.Domain;

namespace PLAYOUT.Models.ViewModels
{
    public class NewOrderRecuest
    {
        public IEnumerable<Canal> canales { get; set; }
        public List<Guid> videoIds { get; set; }
    }
}
