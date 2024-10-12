using PLAYOUT.Models.Domain;

namespace PLAYOUT.Repositories
{
    public interface IMusicalRepository
    {
        Task<Musical> AddAsync(Musical musical);
        Task<List<Musical>> GetAllAsync();
        Task<int> GetLenghtTableAsync();
        Task<int> GetLenghtMaxOrderAsync();
        Task<Musical?> GetAsync(Guid id);
        Task<Musical?> DeleteAsync(Guid id);
        Task<Musical?> UpdateAsync(Musical musical);
        Task<Musical?> UpdateOrdenAsync(Musical musical);
    }
}
