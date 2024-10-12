using Microsoft.EntityFrameworkCore;
using PLAYOUT.Data;
using PLAYOUT.Models.Domain;
using PLAYOUT.Models.ViewModels;

namespace PLAYOUT.Repositories
{
    public class SpotRepository : ISpotRepository
    {
        private readonly PlayOutDbContext _playOutDbContext;
        public SpotRepository(PlayOutDbContext playOutDbContext) 
        { 
            _playOutDbContext = playOutDbContext;
        } 
        public async  Task<Spot> AddAsync(Spot spot)
        {
            await _playOutDbContext.Spots.AddAsync(spot);
            await _playOutDbContext.SaveChangesAsync();
            return spot;
        }

        public async  Task<Spot?> DeleteAsync(Guid id)
        {
            var existingSpot = await _playOutDbContext.Spots.FindAsync(id);
            if (existingSpot != null)
            {
                _playOutDbContext.Spots.Remove(existingSpot);
                await _playOutDbContext.SaveChangesAsync();

                return existingSpot;

            }
            return null;
        }

        public async Task<List<Spot>> GetAllAsync()
        {
            return await _playOutDbContext.Spots.OrderBy(c => c.Orden).ToListAsync();
        }

        public async  Task<Spot?> GetAsync(Guid id)
        {
            return await _playOutDbContext.Spots.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async  Task<int> GetLenghtMaxOrderAsync()
        {
            int maxValor = _playOutDbContext.Spots.Any() ? _playOutDbContext.Spots.Max(v => v.Orden) : 0;
            return maxValor;
        }

        public async Task<Spot?> UpdateAsync(Spot spot)
        {
            var existingSpot = await _playOutDbContext.Spots.FindAsync(spot.Id);
            if (existingSpot != null)
            {

                existingSpot.Id = spot.Id;
                existingSpot.Titulo = spot.Titulo;
                existingSpot.Direccion = spot.Direccion;
                existingSpot.FechaEntrada = spot.FechaEntrada;
                existingSpot.FechaSalida = spot.FechaSalida;
                existingSpot.Orden = spot.Orden;
                //await _bloggieDbContext.AddAsync(existingTag);
                await _playOutDbContext.SaveChangesAsync();
                return existingSpot;
            }
            return null;
        }

        public async Task<Spot?> UpdateOrdenAsync(Spot spot)
        {
            var existingSpot = await _playOutDbContext.Spots.FindAsync(spot.Id);
            if (existingSpot != null)
            {

                existingSpot.Orden = spot.Orden;
                await _playOutDbContext.SaveChangesAsync();
                return existingSpot;
            }
            return null;
        }
    }
}
