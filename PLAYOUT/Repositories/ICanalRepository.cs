using Azure;
using PLAYOUT.Models.Domain;

namespace PLAYOUT.Repositories
{
    public interface ICanalRepository
    {
        Task<Canal> AddAsync(Canal canal);
        Task<IEnumerable<Canal>> GetAllAsync();
        Task<int> GetLenghtTableAsync();
        Task<Canal?> GetAsync(Guid id);
        Task<Canal?> DeleteAsync(Guid id);
        Task<Canal?> UpdateAsync(Canal canal);
        Task<Canal?> UpdateOrdenAsync(Canal canal);
        Task<IEnumerable<Canal>> ObtenerCanalesConProgramacionFutura();
        Task<Canal?> ObtenerCanalesConProgramacion(Guid id);
    }
}