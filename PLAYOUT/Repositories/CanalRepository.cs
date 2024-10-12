using Azure;
using Microsoft.EntityFrameworkCore;
using PLAYOUT.Data;
using PLAYOUT.Models.Domain;

namespace PLAYOUT.Repositories
{
    public class CanalRepository : ICanalRepository
    {
        private readonly PlayOutDbContext _playOutDbContext;
        public CanalRepository(PlayOutDbContext playOutDbContext) 
        { 
            _playOutDbContext = playOutDbContext;
        
        }
        public async Task<Canal> AddAsync(Canal canal)
        {
            await _playOutDbContext.Canales.AddAsync(canal);    
            await _playOutDbContext.SaveChangesAsync();
            return canal;
        }

        public async Task<Canal?> DeleteAsync(Guid id)
        {
            var exisitingCanal = await _playOutDbContext.Canales.FindAsync(id);
            if (exisitingCanal != null)
            {
                _playOutDbContext.Canales.Remove(exisitingCanal);
                await _playOutDbContext.SaveChangesAsync();
                return exisitingCanal;
            }
            return null;
        }

        public async Task<IEnumerable<Canal>> GetAllAsync()
        {
            return await _playOutDbContext.Canales.ToListAsync();
        }

        public async Task<Canal?> GetAsync(Guid id)
        {
            return await _playOutDbContext.Canales.FirstOrDefaultAsync(x => x.Id == id);
        }


        public async  Task<int> GetLenghtTableAsync()
        {
            var NumOrder = await _playOutDbContext.Canales.CountAsync();
            return NumOrder;
        }

        public async Task<Canal?> ObtenerCanalesConProgramacion(Guid id)
        {
            var ahora = DateTime.Now.AddHours(-2);
            var horalimite = ahora.AddHours(5);
            /*
             
            return _playOutDbContext.Canales
            .Where(c => c.Programaciones.Any(p => p.Hora > DateTime.Now))
            .Include(c => c.Programaciones)
            .ToList();
            */

            return  _playOutDbContext.Canales
            .Where(c => c.Programaciones.Any(p => p.Hora >= ahora && p.Hora < horalimite))
            .FirstOrDefault(c => c.Id == id);
        }

        public async  Task<IEnumerable<Canal>> ObtenerCanalesConProgramacionFutura()
        {
            var ahora = DateTime.Now.AddHours(-2);
            var horalimite = ahora.AddHours(5);
            /*
             
            return _playOutDbContext.Canales
            .Where(c => c.Programaciones.Any(p => p.Hora > DateTime.Now))
            .Include(c => c.Programaciones)
            .ToList();
            */
            /*
            return _playOutDbContext.Canales
            .Where(c => c.Programaciones.Any(p => p.Hora >= ahora && p.Hora < horalimite))
            .Include(c => c.Programaciones)
            .ToList();
            */
            return _playOutDbContext.Canales
            .Where(c => c.Programaciones.Any(p => p.Hora >= ahora && p.Hora < horalimite))
            .Include(c => c.Programaciones)
            .OrderBy(c =>c.Orden)
            .ToList();
        }

        public async Task<Canal?> UpdateAsync(Canal canal)
        {
            var existingCanal = await _playOutDbContext.Canales.FindAsync(canal.Id);
            if (existingCanal != null)
            {
                existingCanal.Name = canal.Name;
                existingCanal.Orden = canal.Orden;
                existingCanal.Direccion = canal.Direccion;
                //await _bloggieDbContext.AddAsync(existingTag);
                await _playOutDbContext.SaveChangesAsync();
                return existingCanal;
            }
            return null;
        }

        public async  Task<Canal?> UpdateOrdenAsync(Canal canal)
        {
            var existingCanal = await _playOutDbContext.Canales.FindAsync(canal.Id);
            if (existingCanal != null)
            {
                
                existingCanal.Orden = canal.Orden;
                await _playOutDbContext.SaveChangesAsync();
                return existingCanal;
            }
            return null;
        }
    }
}
