namespace PLAYOUT.Models.Domain
{
    public class Programacion
    {
       public Guid Id { get; set; }
       public Guid CanalId { get; set; }
       public string CanalName { get; set;}
       public string Programa { get; set;}
       public DateTime Hora { get; set; }
       public Canal Canal { get; set; }


    }
}
