using Models;

namespace AA1.Repositories
{
    public interface IReservaRepository
    {
        Task<List<Reserva>> GetAllAsync();
        Task<Reserva?> GetByIdAsync(int id);
        Task AddAsync(Reserva reserva);
        Task UpdateAsync(Reserva reserva);
        Task DeleteAsync(int id);
        Task InicializarDatosAsync();
    }
}
