using Microsoft.EntityFrameworkCore;
using PLAYOUT.Data;
using PLAYOUT.Models.Domain;
using System.Runtime.CompilerServices;

namespace PLAYOUT.Repositories
{
    public class MusicalRepository : IMusicalRepository
    {
        private readonly PlayOutDbContext _playOutDbContext;
        public MusicalRepository(PlayOutDbContext playOutDbContext) 
        {
            _playOutDbContext = playOutDbContext;
        }
        public async  Task<Musical> AddAsync(Musical musical)
        {
            await _playOutDbContext.Musicales.AddAsync(musical);
            await _playOutDbContext.SaveChangesAsync();
            return musical;
        }

        public async Task<Musical?> DeleteAsync(Guid id)
        {
            var existingVidMus = await _playOutDbContext.Musicales.FindAsync(id);
            if (existingVidMus != null)
            {
                _playOutDbContext.Musicales.Remove(existingVidMus);
                await _playOutDbContext.SaveChangesAsync();

                return existingVidMus;

            }
            return null;
        }
        /*
        public async Task<IEnumerable<Musical>> GetAllAsync()
        {
            return await _playOutDbContext.Musicales.ToListAsync();
        }
        */
        public async  Task<Musical?> GetAsync(Guid id)
        {
            return await _playOutDbContext.Musicales.FindAsync(id);
        }

        public async  Task<int> GetLenghtMaxOrderAsync()
        {
            int maxValor = _playOutDbContext.Musicales.Any() ? _playOutDbContext.Musicales.Max(v => v.Orden) : 0;
            return maxValor;
        }

        public async Task<int> GetLenghtTableAsync()
        {
            var NumOrder = await _playOutDbContext.Musicales.CountAsync();
            return NumOrder;
        }

        public async Task<Musical?> UpdateAsync(Musical musical)
        {
            var existingMusical = await _playOutDbContext.Musicales.FindAsync(musical.Id);
            if (existingMusical != null)
            {

                existingMusical.Id = musical.Id;
                existingMusical.Titulo = musical.Titulo;
                existingMusical.Direccion = musical.Direccion;
                //existingMusical.FechaEntrada = musical.FechaEntrada;
                existingMusical.FechaSalida = musical.FechaSalida;
                existingMusical.Orden = musical.Orden;
                //await _bloggieDbContext.AddAsync(existingTag);
                await _playOutDbContext.SaveChangesAsync();
                return existingMusical;
            }
            return null;
        }

        public async Task<Musical?> UpdateOrdenAsync(Musical musical)
        {
            var existingMusical = await _playOutDbContext.Musicales.FindAsync(musical.Id);
            if (existingMusical != null)
            {

                existingMusical.Orden = musical.Orden;
                await _playOutDbContext.SaveChangesAsync();
                return existingMusical;
            }
            return null;
        }

        public async Task<List<Musical>> GetAllAsync()
        {
            return await _playOutDbContext.Musicales.OrderBy(c => c.Orden).ToListAsync();
        }
    }
}
