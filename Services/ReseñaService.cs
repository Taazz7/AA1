using AA1.Repositories;
using Models;

namespace AA1.Services
{
    public class ReseñaService : IReseñaService
    {
        private readonly IReseñaRepository _reseñaRepository;

        public ReseñaService (IReseñaRepository reseñaRepository)
        {
            _reseñaRepository = reseñaRepository;
            
        }

        public async Task<List<Reseña>> GetAllAsync()
        {
            return await _reseñaRepository.GetAllAsync();
        }

        public async Task<Reseña?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor que cero.");

            return await _reseñaRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Reseña reseña)
        {
            if (string.IsNullOrWhiteSpace(reseña.Titulo))
                throw new ArgumentException("El titulo no puede estar vacío.");

            if (reseña.Valoracion < 1 && reseña.Valoracion > 5 )
                throw new ArgumentException("La valoracion debe ser entre 1 y 5.");

            await _reseñaRepository.AddAsync(reseña);
        }

        public async Task UpdateAsync(Reseña reseña)
        {
            if (reseña.Valoracion < 1 && reseña.Valoracion > 5 )
                throw new ArgumentException("La valoracion debe ser entre 1 y 5.");

            if (reseña.IdReseña <= 0)
                throw new ArgumentException("El ID no es válido para actualización.");

            if (string.IsNullOrWhiteSpace(reseña.Titulo))
                throw new ArgumentException("El titulo no puede estar vacío.");

            await _reseñaRepository.UpdateAsync(reseña);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID no es válido para eliminación.");

            await _reseñaRepository.DeleteAsync(id);
        }

        public async Task InicializarDatosAsync() {
            await _reseñaRepository.InicializarDatosAsync();
        }

        public async Task<List<Reseña>> GetAllFilteredAsync(DateTime? fecha, int? valoracion, string? orderBy, bool ascending)
        {
            return await _reseñaRepository.GetAllFilteredAsync(fecha, valoracion, orderBy, ascending);
        }
        public async Task<List<Reseña>> GetByReservaIdAsync(int idReserva)
    {
        if (idReserva <= 0)
            throw new ArgumentException("El ID del usuario debe ser mayor que cero.");

        return await _reseñaRepository.GetByReservaIdAsync(idReserva);
    }
        public async Task<Reseña> GetMinPuntAsync(){
            return await _reseñaRepository.GetMinPuntAsync();
        }
        public async Task<Reseña> GetMinFechaAsync(){
            return await _reseñaRepository.GetMinFechaAsync();
        }
        public async Task<int> GetCountAsync(){
            return await _reseñaRepository.GetCountAsync();
        }
    }
}
