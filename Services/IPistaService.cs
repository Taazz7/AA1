using Models;

namespace AA1.Services
{
    public interface IPistaService
    {
        Task<List<Pista>> GetAllAsync();
        Task<Pista?> GetByIdAsync(int id);
        Task AddAsync(Pista pista);
        Task UpdateAsync(Pista pista);
        Task DeleteAsync(int id);
        Task InicializarDatosAsync();

    }
}
