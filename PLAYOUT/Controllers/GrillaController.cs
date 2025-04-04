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
        public async Task<IActionResult> Index()

        {
            return View();
        }

       

       
        public async Task<IActionResult> ObtenerProgramacion()
        {
            var programacion = await _programacionRepository.ObtenerProgramacionActualAsync();
            return PartialView("_ProgramacionPartial", programacion);
        }
        public async Task<IActionResult> _VideoMusPlayer()
        {
           
            var videosMusicales= await _musicalRepository.GetAllAsync();
            var VidMu = new VideosMusicales
            {
               VideosMus= videosMusicales
            };
            return PartialView(VidMu);
        }
      
        public async Task<JsonResult> ObtenerCanalProgramacion(Guid canalId)
        {
            
            var canal =  await _programacionService.ObtenerCanalesParaVistaConId(canalId);
         
            return Json(canal);
        }
        public async Task<JsonResult> GetCanalesConProgramacion()
        {
            
            var canalesConProgramacion = await _canalRepository.ObtenerCanalesConProgramacionFutura();
            
            return Json(canalesConProgramacion);
        }
        
        public async Task<IActionResult> tvcable()
        {
            var ListVidMus = await _musicalRepository.GetAllAsync();
            var listSpots = await _spotRepository.GetAllAsync();//adicionado
            var modelParcial = await _programacionService.ObtenerCanalesParaVista();

            var vidmus = new GrillaCanalesModel
            {
                musicalesbo = ListVidMus,
                canalViewModels = modelParcial,
                spots = listSpots//adicionado
            };
            return View(vidmus);

        }

    }
}
