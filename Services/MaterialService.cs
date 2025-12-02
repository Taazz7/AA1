using AA1.Repositories;
using Models;

namespace AA1.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialService(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
            
        }

        public async Task<List<Material>> GetAllAsync()
        {
            return await _materialRepository.GetAllAsync();
        }

        public async Task<Material?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor que cero.");

            return await _materialRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Material material)
        {
            if (string.IsNullOrWhiteSpace(material.Nombre))
                throw new ArgumentException("El nombre del plato no puede estar vacío.");
            
            if (material.Disponibilidad != 1 && material.Disponibilidad != 0)
                throw new ArgumentException("La disponibilidad no puede ser distinta de 1(True) o 0(False).");

            await _materialRepository.AddAsync(material);
        }

        public async Task UpdateAsync(Material material)
        {
            if (material.IdMaterial <= 0)
                throw new ArgumentException("El ID no es válido para actualización.");

            if (string.IsNullOrWhiteSpace(material.Nombre))
                throw new ArgumentException("El nombre del plato no puede estar vacío.");

            await _materialRepository.UpdateAsync(material);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID no es válido para eliminación.");

            await _materialRepository.DeleteAsync(id);
        }

        public async Task InicializarDatosAsync() {
            await _materialRepository.InicializarDatosAsync();
        }
    }
}
