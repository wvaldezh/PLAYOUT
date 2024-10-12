using Microsoft.AspNetCore.Mvc.Rendering;

namespace PLAYOUT.Models.ViewModels
{
    public class InputModel
    {
        public string MultilineData { get; set; }
        public string Canal { get; set; }
        public Guid CanalId { get; set; }
        public DateTime dateOnly { get; set; }
        public IEnumerable<SelectListItem> Canales { get; set; }
        public string SelectedCanal { get; set; } 
    }
}
