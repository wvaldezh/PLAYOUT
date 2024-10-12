namespace PLAYOUT.Models.Domain
{
    public class Musical
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaSalida { get; set; }
        public int Orden { get; set; }
    }
}
