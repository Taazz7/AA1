
using Models;

namespace AA1.Services
{
    public interface IReseñaService
    {
        Task<List<Reseña>> GetAllAsync();
        Task<List<Reseña>> GetAllFilteredAsync(DateTime? fecha, int? valoracion, string? orderBy, bool ascending);
        Task<Reseña?> GetByIdAsync(int id);
        Task AddAsync(Reseña reseña);
        Task UpdateAsync(Reseña reseña);
        Task DeleteAsync(int id);
        Task<Reseña> GetMinPuntAsync();
        Task<Reseña> GetMinFechaAsync();
        Task<int> GetCountAsync();
        Task InicializarDatosAsync();
        Task<List<Reseña>> GetByReservaIdAsync(int idReserva); 
    }
}