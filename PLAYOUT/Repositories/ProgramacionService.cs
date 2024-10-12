using PLAYOUT.Models.Domain;
using PLAYOUT.Models.ViewModels;

namespace PLAYOUT.Repositories
{
    public class ProgramacionService: IProgramacionService
    {
        private readonly ICanalRepository _canalRepository;
        public ProgramacionService( ICanalRepository canalRepository) 
        {
            _canalRepository = canalRepository;
        }

        public async Task<IEnumerable<CanalViewModel>> ObtenerCanalesParaVista()
        {
            var canales = await _canalRepository.ObtenerCanalesConProgramacionFutura();
            var ahora = DateTime.Now.AddHours(-2);

            var result =  canales.Select(c => new CanalViewModel
            {
                CanalID = c.Id,
                Nombre = c.Name,
                LogoUrl = c.Direccion,
                Programas = c.Programaciones
                    .Where(p => p.Hora >= ahora)
                    .OrderBy(p => p.Hora)
                    .Take(5) // Tomar los próximos 5 programas
                    .Select(p => new ProgramaViewModel
                    {
                        TituloDePrograma = p.Programa,
                        HoraDeEmision = p.Hora
                    }).ToList()
            });

            return result;

        }

        public async  Task<CanalViewModel?> ObtenerCanalesParaVistaConId(Guid Id)
        {
            var canales = await _canalRepository.ObtenerCanalesConProgramacionFutura();
            var ahora = DateTime.Now.AddHours(-2);

            var result = canales.Select(c => new CanalViewModel
            {
                CanalID = c.Id,
                Nombre = c.Name,
                LogoUrl = c.Direccion,
                Programas = c.Programaciones
                    .Where(p => p.Hora >= ahora)
                    .OrderBy(p => p.Hora)
                    .Take(5) // Tomar los próximos 5 programas
                    .Select(p => new ProgramaViewModel
                    {
                        TituloDePrograma = p.Programa,
                        HoraDeEmision = p.Hora
                    }).ToList()
            }).FirstOrDefault(c => c.CanalID == Id);

            return result;


        }
    }
}
