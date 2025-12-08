using AA1.Repositories;
using Models;

namespace AA1.Services
{
    public class PistaService : IPistaService
    {
        private readonly IPistaRepository _pistaRepository;

        public PistaService(IPistaRepository pistaRepository)
        {
            _pistaRepository = pistaRepository;
            
        }

        public async Task<List<Pista>> GetAllAsync()
        {
            return await _pistaRepository.GetAllAsync();
        }

        public async Task<Pista?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor que cero.");

            return await _pistaRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Pista pista)
        {
            if (string.IsNullOrWhiteSpace(pista.Nombre))
                throw new ArgumentException("El nombre del plato no puede estar vacío.");

            if (pista.PrecioHora <= 0)
                throw new ArgumentException("El precio debe ser mayor que cero.");

            await _pistaRepository.AddAsync(pista);
        }

        public async Task UpdateAsync(Pista pista)
        {
            if (pista.IdPista <= 0)
                throw new ArgumentException("El ID no es válido para actualización.");

            if (string.IsNullOrWhiteSpace(pista.Nombre))
                throw new ArgumentException("El nombre del plato no puede estar vacío.");

            await _pistaRepository.UpdateAsync(pista);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID no es válido para eliminación.");

            await _pistaRepository.DeleteAsync(id);
        }

        public async Task InicializarDatosAsync() {
            await _pistaRepository.InicializarDatosAsync();
        }

        public async Task<List<Pista>> GetAllFilteredAsync(string? tipo, bool? activa, string? orderBy, bool ascending)
        {
            return await _pistaRepository.GetAllFilteredAsync(tipo, activa, orderBy, ascending);
        }
    }
}
