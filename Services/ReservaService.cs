using AA1.Repositories;
using Models;

namespace AA1.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _reservaRepository;

        public ReservaService(IReservaRepository reservaRepository)
        {
            _reservaRepository = reservaRepository;
            
        }

        public async Task<List<Reserva>> GetAllAsync()
        {
            return await _reservaRepository.GetAllAsync();
        }

        public async Task<Reserva?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor que cero.");

            return await _reservaRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Reserva reserva)
        {
            if (reserva.Horas <= 0)
                throw new ArgumentException("Las horas debe ser mayor que cero.");

            await _reservaRepository.AddAsync(reserva);
        }

        public async Task UpdateAsync(Reserva reserva)
        {
            if (reserva.Horas <= 0)
                throw new ArgumentException("Las horas no son válidas para actualización.");
            
            await _reservaRepository.UpdateAsync(reserva);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID no es válido para eliminación.");

            await _reservaRepository.DeleteAsync(id);
        }

        public async Task InicializarDatosAsync() {
            await _reservaRepository.InicializarDatosAsync();
        }


// reservas 1 user

    public async Task<List<Reserva>> GetByUsuarioIdAsync(int idUsuario)
    {
        if (idUsuario <= 0)
            throw new ArgumentException("El ID del usuario debe ser mayor que cero.");

        return await _reservaRepository.GetByUsuarioIdAsync(idUsuario);
    }




    }
}
