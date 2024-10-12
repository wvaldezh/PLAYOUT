using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PLAYOUT.Models.Domain;
using PLAYOUT.Models.ViewModels;
using PLAYOUT.Repositories;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PLAYOUT.Controllers
{
    public class AdminPrograController : Controller
    {
        private readonly IProgramacionRepository _programacionRepository;
        private readonly ICanalRepository _canalRepository;
        public AdminPrograController(IProgramacionRepository programacionRepository, ICanalRepository canalRepository) { 
            _programacionRepository = programacionRepository;
            _canalRepository = canalRepository; 
            
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            string serializedData = TempData["DatosPass"] as string;
            var dayprogramrecover = JsonSerializer.Deserialize<VerifyAddProgra>(serializedData);
            string subdata = dayprogramrecover.SelectedCanal as string;
            var dayprogramcanal = JsonSerializer.Deserialize<CanalSelectedOption>(subdata);

            var canales = await _canalRepository.GetAllAsync();
            var model = new InputModel
            {
                SelectedCanal = dayprogramrecover.SelectedCanal,
                Canal=dayprogramcanal.Name,
                CanalId = dayprogramcanal.Id,
                dateOnly = dayprogramrecover.dateOnly
            };
            /*
            var model = new InputModel
            {
                Canales = canales.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })

            };
            */
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(InputModel model)
        {
            var dateOnly = DateOnly.FromDateTime(model.dateOnly);

            var rows = ParseMultilineData(RemoveTrailingEmptyLines(model.MultilineData), dateOnly);
            // var canalguid = Guid.Parse(model.SelectedCanal);
            //var canalguid = model.CanalId;

           // var canalName = await _canalRepository.GetAsync(canalguid);




            // Procesar los datos
            foreach (var row in rows)
            {
                // Lógica de procesamiento de cada fila
                Console.WriteLine($"Columna1: {row.Columna1}, Columna2: {row.Columna2}");
                var progra = new Programacion
                {
                    CanalName = model.Canal,
                    CanalId = model.CanalId,
                    Hora = row.Columna1,
                    Programa = row.Columna2,
                };
                await _programacionRepository.AddAsync(progra);
            }

            // Redirigir o devolver una vista de éxito
            return RedirectToAction("verificarProgra");
        }

        // Si hay un error en el modelo, vuelve a mostrar la vista con los datos ingresados
        //return View("Add", model);
        /* if (ModelState.IsValid)
         {

     }
        */
        [HttpGet]
        public async Task<IActionResult> verificarProgra()
        {
            var canales = await _canalRepository.GetAllAsync();
            var model = new VerifyAddProgra
            {
                Canales = canales.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })

            };
            return View(model);
            //return View();

        }
        [HttpPost]
        public async Task<IActionResult> verificarProgra(VerifyAddProgra verifyAddProgra)
        {
            string serializedData = verifyAddProgra.SelectedCanal as string;
            var datosCanaSelected = JsonSerializer.Deserialize<CanalSelectedOption>(serializedData);
            //var canalId = Guid.Parse(datosCanaSelected.Id);
            //var existingDayProgram = await _programacionRepository.GetDayPrograAsync(canalId, verifyAddProgra.dateOnly);
            var existingDayProgram = await _programacionRepository.GetDayPrograAsync(datosCanaSelected.Id, verifyAddProgra.dateOnly);
            // List<string> listaDeStrings = new List<string>(existingDayProgram.Select(x => x.ToString()).ToList());
            //TempData["ListaDatos"] = listaDeStrings;
            // TempData["Datos"] = JsonConvert.SerializeObject(existingDayProgram);
            if (existingDayProgram != null && existingDayProgram.Any()) 
            {
                
                var dayprogra = new programModel
                {
                    programacions = existingDayProgram,
                    

                };
                TempData["Datos"] = JsonSerializer.Serialize(dayprogra);

                return RedirectToAction("dayProgramResult", "AdminProgra");
            }
            else
            {
                /*
                var dayPrograPass = new AddProgramPass
                {
                    dateOnly = verifyAddProgra.dateOnly,
                    SelectedCanal = verifyAddProgra.SelectedCanal
                };
                */

                TempData["DatosPass"] = JsonSerializer.Serialize(verifyAddProgra);
                return RedirectToAction("Add", "AdminProgra");
            }

            return View();

        }

        [HttpGet]
        public async Task<IActionResult> dayProgramResult()
        {
            // List<Programacion> datos = TempData["ListaDatos"] as List<Programacion>;
            string serializedData = TempData["Datos"] as string;
            var dayprogramrecover = JsonSerializer.Deserialize<programModel>(serializedData);
            //List<string> datos = JsonConvert.DeserializeObject<List<string>>(serializedData);

            

            //var dayProgram2 = dayProgra;

            if (dayprogramrecover != null)
            {
                
                var dayprograresult = new DayProgramRequest
                {
                    programacions = dayprogramrecover.programacions
                };
               
                return View(dayprograresult);

            }
            
            // var model = dayProgramRequest;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> dayProgramResult(DayProgramRequest dayProgramRequest)
        {
            return View();
        }
        [HttpGet]
     
        private List<RowModel> ParseMultilineData(string multilineData, DateOnly dateOnly)
        {
            var rows = new List<RowModel>();
            // var mulmultilineDataok = RemoveTrailingEmptyLines(multilineData);
            var lines = multilineData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            //var linesok = RemoveTrailingEmptyLines(model.MultilineData)
            if (lines != null)
            {
                foreach (var line in lines)
                {
                    var columns = line.Split(';');
                    TimeOnly time = TimeOnly.Parse(columns[0].Trim());
                    DateTime dateTime = dateOnly.ToDateTime(time);
                    {
                        rows.Add(new RowModel
                        {
                            //  var time= TimeOnly.Parse(columns[0].Trim())
                            Columna1 = dateTime,
                            Columna2 = columns[1].Trim()
                        });
                    }
                }

            }


            return rows;
        }
        private string RemoveTrailingEmptyLines(string multilineData)
        {
            var lines = multilineData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            // Remover líneas vacías al final
            int i = lines.Length - 1;
            while (i >= 0 && string.IsNullOrWhiteSpace(lines[i]))
            {
                i--;
            }

            return string.Join(Environment.NewLine, lines, 0, i + 1);
        }
    }
}
