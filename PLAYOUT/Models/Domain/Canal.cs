namespace PLAYOUT.Models.Domain
{
    public class Canal
    {
        public Guid Id { get; set; }
        public int Orden { get; set; }
        public string Name { get; set; }
        public string Direccion { get; set; }
        public ICollection<Programacion> Programaciones { get; set; }
    }
}
