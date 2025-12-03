using Models;

namespace AA1.Services
{
    public interface IReservaService
    {
        Task<List<Reserva>> GetAllAsync();
        Task<Reserva?> GetByIdAsync(int id);
        Task AddAsync(Reserva reserva);
        Task UpdateAsync(Reserva reserva);
        Task DeleteAsync(int id);
        Task InicializarDatosAsync();
        Task<List<Reserva>> GetByUsuarioIdAsync(int idUsuario); // Reservas de 1 User
    }
}
