namespace PLAYOUT.Models.ViewModels
{
    public class EditCanalRequest
    {
        public Guid Id { get; set; }
        public int Orden { get; set; }
        public string Name { get; set; }
        public string Direccion { get; set; }
    }
}
