using PLAYOUT.Models.ViewModels;

namespace PLAYOUT.Repositories
{
    public interface IProgramacionService
    {
        Task<IEnumerable<CanalViewModel>> ObtenerCanalesParaVista();
        Task<CanalViewModel?> ObtenerCanalesParaVistaConId(Guid Id);
    }
    
}
