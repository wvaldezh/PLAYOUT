namespace PLAYOUT.Models.ViewModels
{
    public class CanalViewModel
    {
        public Guid CanalID { get; set; }
        public string Nombre { get; set; }
        public string LogoUrl { get; set; }
        public List<ProgramaViewModel> Programas { get; set; }
    }
}
