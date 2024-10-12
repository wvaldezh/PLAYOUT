using PLAYOUT.Models.Domain;

namespace PLAYOUT.Models.ViewModels
{
    public class CanalPrograma
    {
        public List<Programacion> programacions { get; set; }
        public string Direccion { get; set; }
        public List<Musical> musicales { get; set; }
    }
}