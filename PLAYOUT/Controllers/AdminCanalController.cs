using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PLAYOUT.Models.Domain;
using PLAYOUT.Models.ViewModels;
using PLAYOUT.Repositories;
using System;
using System.IO;

namespace PLAYOUT.Controllers
{
    public class AdminCanalController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ICanalRepository _canalRepository;
        private readonly IImageRepository _imageRepository;
        public AdminCanalController(IWebHostEnvironment hostEnvironment, ICanalRepository canalRepository, IImageRepository imageRepository)
        {
            _hostEnvironment = hostEnvironment;
            _canalRepository = canalRepository;
            _imageRepository = imageRepository;
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
        [HttpGet]
        public async Task<IActionResult> UploadImage()
        {
            return View();

        }

        /*
        public async Task<IActionResult> Add(AddCanalRequest addCanalRequest)
        {
            return RedirectToAction("Index", "Home");
        }
        */
        [HttpPost]
        public async Task<IActionResult> Add(IFormFile image, AddCanalRequest addCanalRequest)
        {
            if (image != null && image.Length > 0 && addCanalRequest.Name!=null )
            {
                try
                {
                    // Generar un nombre único para evitar colisiones
                    string fileName = Path.GetFileNameWithoutExtension(image.FileName);
                    string extension = Path.GetExtension(image.FileName);
                    string uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                    string path = Path.Combine(_hostEnvironment.WebRootPath, "UploadedImages", uniqueFileName);

                    // Guardar el archivo en la carpeta
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    // Guardar la URL en la base de datos
                    var imageUrl = "/UploadedImages/" + uniqueFileName;
                    var NumOrder = await _canalRepository.GetLenghtTableAsync();
                    var record = new Canal
                    {
                        Name = addCanalRequest.Name,
                        Orden = NumOrder + 1,
                        //Name = "CNN",
                        Direccion = imageUrl
                    };
                    await _canalRepository.AddAsync(record);
                   

                    return Json(new { message = "Image uploaded successfully!" });
                }
                catch (Exception ex)
                {
                    return Json(new { message = "Error: " + ex.Message });
                }
            }
            return Json(new { message = "No file selected!" });
        }
        public async Task<IActionResult> List()
        {
            var canales = await _canalRepository.GetAllAsync(); 
            return View(canales);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var canalrequest = await _canalRepository.GetAsync(Id);
            if(canalrequest != null)
            {
                var model = new EditCanalRequest
                {
                    Id = canalrequest.Id,
                    Name = canalrequest.Name,
                    Direccion = canalrequest.Direccion,
                    Orden = canalrequest.Orden
                };
                return View(model);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(IFormFile image, EditCanalRequest editCanalRequest)
        {
            //bloque para cambiar de imagen inicio
            if (image != null && image.Length > 0 && editCanalRequest.Name != null)
            {
                try
                {
                    // Generar un nombre único para evitar colisiones
                    string fileName = Path.GetFileNameWithoutExtension(image.FileName);
                    string extension = Path.GetExtension(image.FileName);
                    string uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                    string path = Path.Combine(_hostEnvironment.WebRootPath, "UploadedImages", uniqueFileName);

                    // Guardar el archivo en la carpeta
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    // Guardar la URL en la base de datos
                    var imageUrl = "/UploadedImages/" + uniqueFileName;
                    // var NumOrder = await _canalRepository.GetLenghtTableAsync();
                    var record = new Canal
                    {
                        Id = editCanalRequest.Id,
                        Name = editCanalRequest.Name,
                        Orden = editCanalRequest.Orden,
                        Direccion = imageUrl
                    };
                    var channelupdate = await _canalRepository.UpdateAsync(record);
                    if (channelupdate != null)
                    {
                        return RedirectToAction("List");
                    }
                    return RedirectToAction("Edit");


                    return Json(new { message = "Image uploaded successfully!" });
                }
                catch (Exception ex)
                {
                    return Json(new { message = "Error: " + ex.Message });
                }
            }
            //bloque para cambiar de imagen fin
            var canalmodificado = new Canal
            {
                Id = editCanalRequest.Id,
                Name = editCanalRequest.Name,
                Orden = editCanalRequest.Orden,
                Direccion = editCanalRequest.Direccion

            };
           var canalactualizado = await _canalRepository.UpdateAsync(canalmodificado); 
            if(canalactualizado != null)
            {
               return  RedirectToAction("List");
            }
            return RedirectToAction("Edit");
        }
        public async Task<IActionResult> Delete( EditCanalRequest editCanalRequest)
        {
            //decir al repositorio para eliminar el canal
            var deletedCanal = await _canalRepository.DeleteAsync(editCanalRequest.Id);
            if (deletedCanal != null)
            {
                // show success notification
                return RedirectToAction("List");
            }
            //display the response
            return RedirectToAction("List");
            return View();
        }
        public IActionResult subida()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> subida(IFormFile image)
        {
            return View();
        }
        public void OnPost(IFormFile file)
        {

        }
        [HttpGet]
        public async Task<IActionResult> UpdateOrder()
        {
            var DataCanales = await _canalRepository.GetAllAsync();

            var newOrderRequest = new NewOrderRecuest
            {
                canales = DataCanales

            };
            return View(newOrderRequest);
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
                var canal = await _canalRepository.GetAsync(id);
                //var video = await _context.Videos.FindAsync(id);
                if (canal != null)
                {
                    canal.Orden = order++;

                }
                await _canalRepository.UpdateOrdenAsync(canal);
            }
            
            
           //await _canalRepository.

           return Ok(new { message = "Order updated successfully" });
            return View();
        }

    }
}
