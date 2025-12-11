using Models;

namespace AA1.Repositories
{
    public interface IReseñaRepository
    {
        Task<List<Reseña>> GetAllAsync();
        Task<List<Reseña>> GetAllFilteredAsync(DateTime? fecha, int? valoracion, string? orderBy, bool ascending);
        Task<Reseña?> GetByIdAsync(int IdReseña);
        Task AddAsync(Reseña pista);
        Task UpdateAsync(Reseña pista);
        Task DeleteAsync(int IdReseña);
        Task InicializarDatosAsync();
        Task<int> GetCountAsync();
        Task<List<Reseña>> GetByReservaIdAsync(int idReserva);
        Task<Reseña> GetMinPuntAsync();
        Task<Reseña> GetMinFechaAsync();
    }
}