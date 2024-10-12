namespace PLAYOUT.Models.ViewModels
{
    public class EditSpotRequest
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaEntrada { get; set; }
        public int Orden { get; set; }
    }
}
