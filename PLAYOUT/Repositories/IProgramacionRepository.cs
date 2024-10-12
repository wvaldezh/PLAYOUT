using PLAYOUT.Models.Domain;

namespace PLAYOUT.Repositories
{
    public interface IProgramacionRepository
    {
        Task<IEnumerable<Programacion>> GetAllAsync();
        Task<Programacion?> GetAsync(Guid id);
        Task<IEnumerable<Programacion?>> GetDayPrograAsync(Guid id, DateTime dateOnly);
        Task<Dictionary<Canal, List<Programacion>>> ObtenerProgramacionActualAsync();
        Task<List<Programacion>> ObtenerProgramasPorCanalAsync(Guid canalId);
        Task<IEnumerable<Programacion?>> GetDayPartialPrograAsync(Guid id);
        Task<List<Canal>> ObtenerCanalesConProgramacionActualAsync();
        Task<Programacion> AddAsync(Programacion programacion);
        Task<Programacion?> UpdateAsync(Programacion programacion);
        Task<Programacion?> DeleteAsync(Guid id);
        Task<Dictionary<Canal, List<Programacion>>> CanalesConProgramacionAsync();
        
    }
}
