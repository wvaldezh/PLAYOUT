using System.ComponentModel.DataAnnotations;

namespace PLAYOUT.Models.ViewModels
{
    public class ProgramaViewModel
    {
        public string TituloDePrograma { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime HoraDeEmision { get; set; }
    }
}
