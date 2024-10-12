using PLAYOUT.Models.Domain;

namespace PLAYOUT.Repositories
{
    public interface ISpotRepository
    {
        Task<Spot> AddAsync(Spot spot);
        Task<List<Spot>> GetAllAsync();
        Task<Spot?> GetAsync(Guid id);
        Task<Spot?> DeleteAsync(Guid id);
        Task<Spot?> UpdateAsync(Spot spot);
        Task<Spot?> UpdateOrdenAsync(Spot spot);
        Task<int> GetLenghtMaxOrderAsync();
    }
}
