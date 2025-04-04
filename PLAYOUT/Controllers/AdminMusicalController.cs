using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using PLAYOUT.Models.ViewModels;
using System;
using System.IO;
using PLAYOUT.Repositories;
using PLAYOUT.Models.Domain;

namespace PLAYOUT.Controllers
{
    public class AdminMusicalController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMusicalRepository _musicalRepository;
        public AdminMusicalController(IWebHostEnvironment hostEnvironment, IMusicalRepository musicalRepository) 
        {
            _hostingEnvironment = hostEnvironment;
            _musicalRepository = musicalRepository;
        } 
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }
        [HttpPost]
        [RequestSizeLimit(1073741824)]
        public async Task<IActionResult> Add(IFormFile videoFile, AddMusicalRequest musicalRequest)
        {
            if (ModelState.IsValid)
            {
                if (videoFile != null && videoFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Musicales");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + videoFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Guarda el archivo en el servidor
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await videoFile.CopyToAsync(fileStream);
                    }
                    var musicalUrl = "/Musicales/" + uniqueFileName;
                    var NumOrder = await _musicalRepository.GetLenghtMaxOrderAsync();
                    // Guarda los datos en la base de datos
                    var musical = new Musical
                    {
                        Titulo = musicalRequest.Titulo,
                        Direccion = musicalUrl,
                        FechaSalida = musicalRequest.FechaSalida,
                        Orden = NumOrder + 1

                    };
                    await _musicalRepository.AddAsync(musical);
                    // video.Id = Guid.NewGuid();
                    //  video.Url = "/videos/" + uniqueFileName;

                    // _context.Add(video);
                    // await _context.SaveChangesAsync();
                    return RedirectToAction("Add");
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var model = await _musicalRepository.GetAllAsync();
            return View(model);  
        }
        [HttpPost]
        public async Task<IActionResult> List(Guid id)
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Delete(Guid id)
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

            var video = await _musicalRepository.GetAsync(id);
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
            var deletedVidMus = await _musicalRepository.DeleteAsync(id);

            if (deletedVidMus != null)
            {
                //show success notification
                return RedirectToAction("list");

            }
            return RedirectToAction("Edit", new { id = id });


            /*

            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
            */
        }
        [HttpGet]
        public async Task<IActionResult> UpdateOrder()
        {
            var DataMusicales = await _musicalRepository.GetAllAsync();

            var newOrderMusicalRecuest = new NewOrderMusicalRecuest
            {
                musicales = DataMusicales

            };
            return View(newOrderMusicalRecuest);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrder([FromBody] List<Guid> musicalesIds)
        {
            if (musicalesIds == null || !musicalesIds.Any())
            {
                return BadRequest("Invalid video order");
            }


            // Assuming videoIds is an ordered list of video IDs reflecting the new order
            int order = 1;
            foreach (var id in musicalesIds)
            {
                var musical = await _musicalRepository.GetAsync(id);
                //var video = await _context.Videos.FindAsync(id);
                if (musical != null)
                {
                    musical.Orden = order++;

                }
                await _musicalRepository.UpdateOrdenAsync(musical);
            }


            //await _canalRepository.

            return Ok(new { message = "Order updated successfully" });
            //return View();
        }
        public async Task<IActionResult> Edit(Guid Id)
        {
            var musicalrequest = await _musicalRepository.GetAsync(Id);
            if (musicalrequest != null)
            {
                var model = new EditMusicalRequest
                {
                    Id = musicalrequest.Id,
                    Titulo = musicalrequest.Titulo,
                    Direccion = musicalrequest.Direccion,
                    FechaSalida = musicalrequest.FechaSalida,
                    Orden = musicalrequest.Orden
                };
                return View(model);
            }
            return View(null);
        }
        [RequestSizeLimit(1073741824)]
        [HttpPost]
        public async Task<IActionResult> Edit(IFormFile videoFile, EditMusicalRequest editMusicalRequest)
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
                var musicalUrl = "/Musicales/" + uniqueFileName;
                // var NumOrder = await _musicalRepository.GetLenghtMaxOrderAsync();
                // Guarda los datos en la base de datos
                var MusicalUpdate = new Musical
                {
                    Id = editMusicalRequest.Id,
                    Titulo = editMusicalRequest.Titulo,
                    Direccion = filePath,
                    FechaSalida = editMusicalRequest.FechaSalida,
                    Orden = editMusicalRequest.Orden

                };
                await _musicalRepository.UpdateAsync(MusicalUpdate);
                // video.Id = Guid.NewGuid();
                //  video.Url = "/videos/" + uniqueFileName;

                // _context.Add(video);
                // await _context.SaveChangesAsync();

                return RedirectToAction("List");
            }
            
            var Musicalmodificado = new Musical
            {
                Id = editMusicalRequest.Id,
                Titulo = editMusicalRequest.Titulo,
                Direccion = editMusicalRequest.Direccion,
                FechaSalida = editMusicalRequest.FechaSalida,
                Orden = editMusicalRequest.Orden

            };
            var Musicalactualizado = await _musicalRepository.UpdateAsync(Musicalmodificado);
            if (Musicalactualizado != null)
            {
                return RedirectToAction("List");
            }
            
            return RedirectToAction("Edit");
            
        }



    }
}
