using Microsoft.EntityFrameworkCore;
using PLAYOUT.Data;
using PLAYOUT.Models.Domain;
using PLAYOUT.Models.ViewModels;

namespace PLAYOUT.Repositories
{
    public class SpotRepository : ISpotRepository
    {
        private readonly PlayOutDbContext _playOutDbContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public SpotRepository(PlayOutDbContext playOutDbContext, IWebHostEnvironment hostEnvironment) 
        { 
            _playOutDbContext = playOutDbContext;
            _hostingEnvironment = hostEnvironment;
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

        public async  Task EliminarSpotsVencidosAsync()
        {
            // Obtener la fecha actual
            DateTime fechaActual = DateTime.Now;

            // Buscar los spots vencidos
            var spotsVencidos = await _playOutDbContext.Spots
                .Where(s => s.FechaSalida < fechaActual)
                .ToListAsync();

            if (spotsVencidos.Any())
            {
                foreach(var spot in spotsVencidos)
                {
                    // Elimina el archivo de video del servidor
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, spot.Direccion.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                }
                // Eliminar los spots vencidos en base de datos
                _playOutDbContext.Spots.RemoveRange(spotsVencidos);

                // Guardar los cambios en la base de datos
                await _playOutDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Spot>> GetAllAsync()
        {
            return await _playOutDbContext.Spots.OrderBy(c => c.Orden).ToListAsync();
        }
        /*
        public  async Task<List<Spot>> GetAllExpiredAsync()
        {
            DateTime ahora = DateTime.Now;
            return await _playOutDbContext.Spots.Where(p => p.FechaSalida < ahora).ToListAsync();
        }
        */
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
