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
        private readonly IWebHostEnvironment _hostingEnvironment;
        public VideoController(ISpotRepository spotRepository, IMusicalRepository musicalRepository, IProgramacionService programacionService, IWebHostEnvironment hostEnvironment)
        { 
            _spotRepository = spotRepository;
            _musicalRepository = musicalRepository;
            _programacionService = programacionService;
            _hostingEnvironment = hostEnvironment;
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
            //autodelete();
           // var listSpotsVencidos = await _spotRepository.GetAllExpiredAsync();
            await _spotRepository.EliminarSpotsVencidosAsync(); // elimina spots vencidos
            var ListVidMus = await _musicalRepository.GetAllAsync();
            var modelParcial = await _programacionService.ObtenerCanalesParaVista();
            var listSpots = await _spotRepository.GetAllAsync();//adicionado

            var vidmus = new GrillaCanalesModel
            {
                musicalesbo = ListVidMus,
                canalViewModels = modelParcial,
                spots = listSpots
            };
            return View(vidmus);
           
        }
       
        
    }
}
