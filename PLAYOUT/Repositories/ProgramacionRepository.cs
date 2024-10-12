using Microsoft.EntityFrameworkCore;
using PLAYOUT.Data;
using PLAYOUT.Models.Domain;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace PLAYOUT.Repositories
{
    public class ProgramacionRepository : IProgramacionRepository
    {
        private readonly PlayOutDbContext _playOutDbContext;
        public ProgramacionRepository(PlayOutDbContext playOutDbContext)
        {
            _playOutDbContext = playOutDbContext;

        }
        public async Task<Programacion> AddAsync(Programacion programacion)
        {
            await _playOutDbContext.Programaciones.AddAsync(programacion);
            await _playOutDbContext.SaveChangesAsync();
            return programacion;

            
        }

        public async Task<Dictionary<Canal, List<Programacion>>> CanalesConProgramacionAsync()
        {
            throw new NotImplementedException();
            /*

            //var ahora = DateTime.Parse("20/07/2024 10:27:00");
            //var feRest = DateTime.Parse("1:30:00");
            var ahora = DateTime.Now;
            //var ahora = ahoraf.AddHours(-2);
            var canales = await _playOutDbContext.Canales
                .Where(c => _playOutDbContext.Programaciones.Any(p => p.CanalId == c.Id && p.Hora >= ahora))
                .ToListAsync();

            var programacion = new Dictionary<Canal, List<Programacion>>();

            foreach (var canal in canales)
            {
                //var programas = await ObtenerProgramasPorCanalAsync(canal.Id);
                var programas = await ObtenerProgramasPorCanalAsync(canal.Id);
                programacion[canal] = programas;
            }

            return programacion;
            */
            /*
           return await _playOutDbContext.Canales.Where(c => _playOutDbContext.Programaciones.Any(p => p.Hora > DateTime.Now))
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Direccion,
                Programaciones = _playOutDbContext.Programaciones
                    .Where(p => p.Hora > DateTime.Now)
                    .OrderBy(p => p.Hora)
                    .Take(5)
            })
            .ToList();
            //return canalconprogramas;
            */
        }
        
        public Task<Programacion?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Programacion>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Programacion?> GetAsync(Guid id)
        {
            return await _playOutDbContext.Programaciones.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Programacion?>> GetDayPartialPrograAsync(Guid id)
        {
            var FechaRef = DateTime.Parse("20/07/2024 10:27:00");
            return await _playOutDbContext.Programaciones.Where(p => p.CanalId == id && p.Hora >= FechaRef)
            .OrderBy(p => p.Hora)
            .Take(5)
            .ToListAsync();
        }

        public async  Task<IEnumerable<Programacion?>> GetDayPrograAsync(Guid id, DateTime dateOnly)
        {
            //DateTime dateTime = dateOnly.ToDateTime();

            var dayProgra =  await _playOutDbContext.Programaciones.Where(x => x.CanalId == id && x.Hora.Date == dateOnly.Date).ToListAsync();

            return dayProgra;
        }

        public async Task<List<Canal>> ObtenerCanalesConProgramacionActualAsync()
        {
            //var ahora = DateTime.Parse("20/07/2024 10:27:00");
            // DateTime feRest = Convert.ToDatetime("1:30:00");
            var ahora = DateTime.Now;
           // var ahora = ahoraf.AddHours(-2);
            return await _playOutDbContext.Canales
                .Where(c => _playOutDbContext.Programaciones.Any(p => p.CanalId == c.Id && p.Hora >= ahora))
                .ToListAsync();
        }

        public async Task<Dictionary<Canal, List<Programacion>>> ObtenerProgramacionActualAsync()
        {
            //var ahora = DateTime.Parse("20/07/2024 10:27:00");
            //var feRest = DateTime.Parse("1:30:00");
            var ahora = DateTime.Now;
            //var ahora = ahoraf.AddHours(-2);
            var canales = await _playOutDbContext.Canales
                .Where(c => _playOutDbContext.Programaciones.Any(p => p.CanalId == c.Id && p.Hora >= ahora))
                .ToListAsync();

            var programacion = new Dictionary<Canal, List<Programacion>>();

            foreach (var canal in canales)
            {
                //var programas = await ObtenerProgramasPorCanalAsync(canal.Id);
                var programas = await  ObtenerProgramasPorCanalAsync(canal.Id);
                programacion[canal] = programas;
            }

            return programacion;
        }

        public async Task<List<Programacion>> ObtenerProgramasPorCanalAsync(Guid canalId)
        {
            //var ahora = DateTime.Parse("20/07/2024 10:27:00");
            // DateTime feRest = Convert.ToDatetime("1:30:00");
            var ahora = DateTime.Now;
            //DateTime ahora = ahoraf.AddHours(2);
            return await _playOutDbContext.Programaciones
                .Where(p => p.CanalId == canalId && p.Hora >= ahora)
                .OrderBy(p => p.Hora)
                .Take(5)
                .ToListAsync();
        }

        public Task<Programacion?> UpdateAsync(Programacion programacion)
        {
            throw new NotImplementedException();
        }
       
    }
}
