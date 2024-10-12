using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLAYOUT.Models.ViewModels;
using PLAYOUT.Repositories;

namespace PLAYOUT.Controllers
{
    public class VideoController : Controller
    {
        private readonly ISpotRepository _spotRepository;
        private readonly IProgramacionService _programacionService;
        private readonly IMusicalRepository _musicalRepository;
        public VideoController(ISpotRepository spotRepository, IMusicalRepository musicalRepository, IProgramacionService programacionService)
        { 
            _spotRepository = spotRepository;
            _musicalRepository = musicalRepository;
            _programacionService = programacionService;
        }    
        public IActionResult Index()
        {
            return View();
        }
        public async  Task<JsonResult> ListaVideos()
        {
            var videos = await _spotRepository.GetAllAsync(); // Recupera todos los videos
            return Json(videos); // Devuelve la lista en formato JSON
        }
        public async Task<IActionResult> Spots()
        {
            return View();
        }
        public async Task<IActionResult> play()
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
    }
}
