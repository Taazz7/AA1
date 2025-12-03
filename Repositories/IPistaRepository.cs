using Models;

namespace AA1.Repositories
{
    public interface IPistaRepository
    {
        Task<List<Pista>> GetAllAsync();
        Task<List<Pista>> GetAllFilteredAsync(string? tipo, bool? activa, string? orderBy, bool ascending);
        Task<Pista?> GetByIdAsync(int idPista);
        Task AddAsync(Pista pista);
        Task UpdateAsync(Pista pista);
        Task DeleteAsync(int idPista);
        Task InicializarDatosAsync();
    }
}