using Microsoft.AspNetCore.Mvc;
using PLAYOUT.Models.Domain;
using PLAYOUT.Models.ViewModels;
using PLAYOUT.Repositories;

namespace PLAYOUT.Controllers
{
    public class AdminSpotController : Controller
    {
        private readonly ISpotRepository _spotRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public AdminSpotController(IWebHostEnvironment hostEnvironment, ISpotRepository spotRepository) 
        {
            _spotRepository = spotRepository;
            _hostingEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Add()
        {
            return View();
        }
        [HttpPost]
        [RequestSizeLimit(1073741824)]
        public async Task<IActionResult> Add(IFormFile videoFile, AddSpotRequest spotRequest)
        {
            if (ModelState.IsValid)
            {
                if (videoFile != null && videoFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Spots");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + videoFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    DateTime fechaahora = DateTime.Now;

                    // Guarda el archivo en el servidor
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await videoFile.CopyToAsync(fileStream);
                    }
                    var musicalUrl = "/Spots/" + uniqueFileName;
                    var NumOrder = await _spotRepository.GetLenghtMaxOrderAsync();
                    // Guarda los datos en la base de datos
                    var spot = new Spot
                    {
                        Titulo = spotRequest.Titulo,
                        Direccion = musicalUrl,
                        FechaEntrada = fechaahora,
                        FechaSalida = spotRequest.FechaSalida,
                        Orden = NumOrder + 1

                    };
                    await _spotRepository.AddAsync(spot);
                    // video.Id = Guid.NewGuid();
                    //  video.Url = "/videos/" + uniqueFileName;

                    // _context.Add(video);
                    // await _context.SaveChangesAsync();
                    return RedirectToAction("Add");
                }
            }
            return View();
        }
        public async Task<IActionResult> List()
        {
            var spots = await _spotRepository.GetAllAsync();
            return View(spots);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var spotrequest = await _spotRepository.GetAsync(Id);
            if (spotrequest != null)
            {
                var model = new EditSpotRequest
                {
                    Id = spotrequest.Id,
                    Titulo = spotrequest.Titulo,
                    Direccion = spotrequest.Direccion,
                    FechaEntrada = spotrequest.FechaEntrada,
                    FechaSalida = spotrequest.FechaSalida,
                    Orden = spotrequest.Orden
                };
                return View(model);
            }
            return View(null);
        }
        [RequestSizeLimit(1073741824)]
        [HttpPost]
        public async Task<IActionResult> Edit(IFormFile videoFile, EditSpotRequest editSpotRequest)
        {
            if (videoFile != null && videoFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Spots");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + videoFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Guarda el archivo en el servidor
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await videoFile.CopyToAsync(fileStream);
                }
                var musicalUrl = "/Spots/" + uniqueFileName;
               // var NumOrder = await _musicalRepository.GetLenghtMaxOrderAsync();
                // Guarda los datos en la base de datos
                var SpotUpdate = new Spot
                {
                    Id = editSpotRequest.Id,
                    Titulo = editSpotRequest.Titulo,
                    Direccion = filePath,
                    FechaEntrada = editSpotRequest.FechaEntrada,
                    FechaSalida = editSpotRequest.FechaSalida,
                    Orden = editSpotRequest.Orden

                };
                await _spotRepository.UpdateAsync(SpotUpdate);
                // video.Id = Guid.NewGuid();
                //  video.Url = "/videos/" + uniqueFileName;

                // _context.Add(video);
                // await _context.SaveChangesAsync();

                return RedirectToAction("List");
            }

            var Spotmodificado = new Spot
            {
                Id = editSpotRequest.Id,
                Titulo = editSpotRequest.Titulo,
                Direccion = editSpotRequest.Direccion,
                FechaEntrada = editSpotRequest.FechaEntrada,
                FechaSalida = editSpotRequest.FechaSalida,
                Orden = editSpotRequest.Orden

            };
            var canalactualizado = await _spotRepository.UpdateAsync(Spotmodificado);
            if (canalactualizado != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit");
        }
        public async Task<IActionResult> Delete(EditSpotRequest editSpotRequest)
        {
            /*
            var ID = Guid.Parse(id);
            var deletedVidMus = await _musicalRepository.DeleteAsync(ID);

            if (deletedVidMus != null)
            {
                //show success notification
                return RedirectToAction("list");

            }
            return RedirectToAction("Edit", new { id = ID });
            */

            var video = await _spotRepository.GetAsync(editSpotRequest.Id);
            if (video == null)
            {
                return NotFound();
            }

            // Elimina el archivo de video del servidor
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, video.Direccion.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // Elimina el registro de la base de datos
            var deletedSpot = await _spotRepository.DeleteAsync(editSpotRequest.Id);

            if (deletedSpot != null)
            {
                //show success notification
                return RedirectToAction("list");

            }
            return RedirectToAction("Edit", new { id = editSpotRequest.Id });

        }
        [HttpGet]
        public async Task<IActionResult> UpdateOrder()
        {
            var DataSpots = await _spotRepository.GetAllAsync();

            var newOrderSpotRequest = new NewOrderSpotRecuest
            {
                spots = DataSpots

            };
            return View(newOrderSpotRequest);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrder([FromBody] List<Guid> videoIds)
        {
            if (videoIds == null || !videoIds.Any())
            {
                return BadRequest("Invalid video order");
            }


            // Assuming videoIds is an ordered list of video IDs reflecting the new order
            int order = 1;
            foreach (var id in videoIds)
            {
                var spot = await _spotRepository.GetAsync(id);
                //var video = await _context.Videos.FindAsync(id);
                if (spot != null)
                {
                    spot.Orden = order++;

                }
                await _spotRepository.UpdateOrdenAsync(spot);
            }


            //await _canalRepository.

            return Ok(new { message = "Order updated successfully" });
            return View();
        }
    }
}
