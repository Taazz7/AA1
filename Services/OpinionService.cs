using AA1.DTOs;
using AA1.Repositories;
using Models;

namespace AA1.Services
{
    public class OpinionService : IOpinionService
    {
        private readonly IOpinionRepository _opinionRepository;

        public OpinionService(IOpinionRepository opinionRepository)
        {
            _opinionRepository = opinionRepository;
            
        }

        public async Task<List<Opinion>> GetAllAsync()
        {
            return await _opinionRepository.GetAllAsync();
        }

        public async Task<Opinion?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor que cero.");

            return await _opinionRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Opinion opinion)
        {
            if (string.IsNullOrWhiteSpace(opinion.Nombre))
                throw new ArgumentException("El nombre no puede estar vacío.");

            if (opinion.Puntuacion < 1 && opinion.Puntuacion >5)
                throw new ArgumentException("Puntuacion debe estar entre 1 y 5");

            await _opinionRepository.AddAsync(opinion);
        }

        public async Task UpdateAsync(Opinion opinion)
        {
            if (opinion.IdOpinion <= 0)
                throw new ArgumentException("El ID no es válido para actualización.");

            if (string.IsNullOrWhiteSpace(opinion.Nombre))
                throw new ArgumentException("El nombre no puede estar vacío.");

            await _opinionRepository.UpdateAsync(opinion);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID no es válido para eliminación.");

            await _opinionRepository.DeleteAsync(id);
        }

        public async Task InicializarDatosAsync() {
            await _opinionRepository.InicializarDatosAsync();
        }

        public async Task<List<OpinionDto>> GetAllFilteredPuntuAsync(int? puntuacion, string? orderBy, bool ascending)
        {
            return await _opinionRepository.GetAllFilteredPuntuAsync(puntuacion, orderBy, ascending);
        }

    }
}
