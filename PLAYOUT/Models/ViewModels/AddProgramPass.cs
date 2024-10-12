using Microsoft.AspNetCore.Mvc.Rendering;

namespace PLAYOUT.Models.ViewModels
{
    public class AddProgramPass
    {
        public DateTime dateOnly { get; set; }
        public string SelectedCanal { get; set; }
    }
}
