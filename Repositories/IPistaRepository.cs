using Models;

namespace AA1.Repositories
{
    public interface IPistaRepository
    {
        Task<List<Pista>> GetAllAsync();
        Task<Pista?> GetByIdAsync(int id);
        Task AddAsync(Pista pista);
        Task UpdateAsync(Pista pista);
        Task DeleteAsync(int id);
        Task InicializarDatosAsync();
    }
}
