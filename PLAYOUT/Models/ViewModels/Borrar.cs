using PLAYOUT.Models.Domain;

namespace PLAYOUT.Models.ViewModels
{
    public class Borrar
    {
        public List<Musical> musicalesbo { get; set; }
        public List<Spot> spots { get; set; }
        public IEnumerable<CanalViewModel> canalViewModels { get; set; }
    }
}
