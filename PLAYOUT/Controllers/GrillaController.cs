using Microsoft.AspNetCore.Mvc;
using PLAYOUT.Models.ViewModels;
using PLAYOUT.Repositories;

namespace PLAYOUT.Controllers
{
    public class GrillaController : Controller
    {
        private readonly IProgramacionRepository _programacionRepository;
        private readonly ICanalRepository _canalRepository;
        private readonly IMusicalRepository _musicalRepository;
        private readonly IProgramacionService _programacionService;
        private readonly ISpotRepository _spotRepository;
        public GrillaController(ICanalRepository canalRepository, IProgramacionRepository programacionRepository, IMusicalRepository musicalRepository, IProgramacionService programacionService, ISpotRepository spotRepository) 
        {
            _canalRepository = canalRepository;
            _programacionRepository = programacionRepository;
            _musicalRepository = musicalRepository;
            _programacionService = programacionService;
            _spotRepository = spotRepository;
        }
        /*
        public async Task<IActionResult> Canal(Guid id)
        {
            var programas = await _programacionRepository.GetDayPartialPrograAsync(id);
            return View(programas);
        }
        public async Task<IActionResult> ObtenerProgramas(string canal)
        {
            var programas = await _programaService.ObtenerProgramasPorCanalAsync(canal);
            return PartialView("_ProgramasPartial", programas);
        }
        */
        /*
        public async Task<ActionResult> GrillaActual()
        {
            // fechaRef
            //var FechaRef = DateTime.Parse("20/07/2024");
           // var AllDayPrograms = new List<IEnumerable<Programacion>>();
           // var ListDayPrograms = new List<IEnumerable<programModel>>();
            // var AllDayProgramsOb = new programModel();
            //verificar si existen canales 
            var ListaCanales = await _canalRepository.GetAllAsync();
            if(ListaCanales != null)
            {
                foreach(var canales in ListaCanales)
                {
                    // verificar si existen preogramacion en esos canales
                    var programas = await _programacionRepository.GetDayPartialPrograAsync(canales.Id);
                    if (programas != null)
                    {
                        ListDayPrograms.Add((IEnumerable<programModel>)existingDayProgram);
                    }
                    
                    //var ListaProgramas =  await _programacionRepository.
                }
            }
            return View(ListDayPrograms);
        }
        */
        public async Task<IActionResult> Index(int? index)
        {
            /*
            var canales = await _canalRepository.GetAllAsync();
            return View(canales);
            
            var programacion = await _programacionRepository.ObtenerProgramacionActualAsync();
            return View(programacion);
            
            var canales = await _programacionRepository.ObtenerCanalesConProgramacionActualAsync();
            if (canales.Any())
            {
                return RedirectToAction("Canal", new { id = canales.First().Id, index = 0 });
            }
            return View("SinProgramacion");
            */

            var canales = await _programacionRepository.ObtenerCanalesConProgramacionActualAsync();
            if (!canales.Any())
            {
                return View("SinProgramacion");
            }

            int currentIndex = index ?? 0;
            var canal = canales[currentIndex];
            var programas = await _programacionRepository.ObtenerProgramasPorCanalAsync(canal.Id);
            var ListVidMus = await _musicalRepository.GetAllAsync();
            var canalyprogramas = new CanalPrograma
            {
                programacions = programas,
                Direccion = canal.Direccion,
                musicales = ListVidMus
            };

            ViewBag.CanalesCount = canales.Count;
            ViewBag.CurrentIndex = currentIndex;
            //return View(programas);
            return View(canalyprogramas);
        }
        public async Task<IActionResult> Canal(Guid id, int index)
        {
            /* var programas = await _programacionRepository.GetDayPartialPrograAsync(id);
             ViewBag.CanalId = id; // Pasamos el id del canal a la vista
             return View(programas);
            */
            var canales = await _programacionRepository.ObtenerCanalesConProgramacionActualAsync();
            if (!canales.Any())
            {
                return View("SinProgramacion");
            }

            var programas = await _programacionRepository.ObtenerProgramasPorCanalAsync(id);
            ViewBag.CanalId = id;
            ViewBag.Canales = canales;
            ViewBag.Index = index;

            return View(programas);

        }
        /*
        public async Task<IActionResult> ObtenerProgramas(Guid id)
        {
            var programas = await _programacionRepository.GetDayPartialPrograAsync(id);
            return PartialView("_ProgramasPartial", programas);
        }
        */
        public async Task<IActionResult> ObtenerProgramacion()
        {
            var programacion = await _programacionRepository.ObtenerProgramacionActualAsync();
            return PartialView("_ProgramacionPartial", programacion);
        }
        public async Task<IActionResult> _VideoMusPlayer()
        {
            //var VidMu = new VideosMusicales
            var videosMusicales= await _musicalRepository.GetAllAsync();
            var VidMu = new VideosMusicales
            {
               VideosMus= videosMusicales
            };
            return PartialView(VidMu);
        }
        public async Task<IActionResult> borrar()
        {
            var ListVidMus = await _musicalRepository.GetAllAsync();
            var modelParcial = await _programacionService.ObtenerCanalesParaVista();

            var vidmus = new Borrar
            {
                musicalesbo = ListVidMus,
                canalViewModels = modelParcial
            };
            return View(vidmus);
        }
        public async Task<IActionResult> canalShow()
        {
            // var ahora = DateTime.Now;
           // var horalimite = ahora.AddHours(5);
            var model = await _programacionService.ObtenerCanalesParaVista();
            return View(model);
      
        }
        public async Task<IActionResult> borrar2()
        {
            var ListVidMus = await _musicalRepository.GetAllAsync();
            var modelParcial = await _programacionService.ObtenerCanalesParaVista();

            var vidmus = new Borrar
            {
                musicalesbo = ListVidMus,
                canalViewModels = modelParcial
            };
            return View(vidmus);
        }
        public async Task<JsonResult> ObtenerCanalProgramacion(Guid canalId)
        {
            //var canal = _service.ObtenerCanalesConProgramacion().FirstOrDefault(c => c.CanalID == canalId);
            var canal =  await _programacionService.ObtenerCanalesParaVistaConId(canalId);
            //var canal =  await _canalRepository.ObtenerCanalesConProgramacion(canalId);
            return Json(canal);
        }
        public async Task<JsonResult> GetCanalesConProgramacion()
        {
            //var canal = _service.ObtenerCanalesConProgramacion().FirstOrDefault(c => c.CanalID == canalId);
            var canalesConProgramacion = await _canalRepository.ObtenerCanalesConProgramacionFutura();
            //var canal =  await _canalRepository.ObtenerCanalesConProgramacion(canalId);
            return Json(canalesConProgramacion);
        }
        public async Task<IActionResult> borrar3()
        {
            var ListVidMus = await _musicalRepository.GetAllAsync();
            var modelParcial = await _programacionService.ObtenerCanalesParaVista();

            var vidmus = new Borrar
            {
                musicalesbo = ListVidMus,
                canalViewModels = modelParcial
            };
            return View(vidmus);

        }
        public async Task<IActionResult> borrar4()
        {
            var ListVidMus = await _musicalRepository.GetAllAsync();
            var modelParcial = await _programacionService.ObtenerCanalesParaVista();

            var vidmus = new Borrar
            {
                musicalesbo = ListVidMus,
                canalViewModels = modelParcial
            };
            return View(vidmus);

        }
        public async Task<IActionResult> borrar5()
        {
            var ListVidMus = await _musicalRepository.GetAllAsync();
            var modelParcial = await _programacionService.ObtenerCanalesParaVista();

            var vidmus = new Borrar
            {
                musicalesbo = ListVidMus,
                canalViewModels = modelParcial
            };
            return View(vidmus);

        }
        public async Task<IActionResult> borrar6()
        {
            var ListVidMus = await _musicalRepository.GetAllAsync();
            var listSpots = await _spotRepository.GetAllAsync();//adicionado
            var modelParcial = await _programacionService.ObtenerCanalesParaVista();

            var vidmus = new Borrar
            {
                musicalesbo = ListVidMus,
                canalViewModels = modelParcial,
                spots = listSpots//adicionado
            };
            return View(vidmus);

        }

    }
}
