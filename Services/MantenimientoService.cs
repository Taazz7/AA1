using AA1.Repositories;
using Models;

namespace AA1.Services
{
    public class MantenimientoService : IMantenimientoService
    {
        private readonly IMantenimientoRepository _mantenimientoRepository;

        public MantenimientoService(IMantenimientoRepository mantenimientoRepository)
        {
            _mantenimientoRepository = mantenimientoRepository;
            
        }

        public async Task<List<Mantenimiento>> GetAllAsync()
        {
            return await _mantenimientoRepository.GetAllAsync();
        }

        public async Task<Mantenimiento?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor que cero.");

            return await _mantenimientoRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Mantenimiento mantenimiento)
        {
            if (string.IsNullOrWhiteSpace(mantenimiento.Nombre))
                throw new ArgumentException("El nombre de la empresa no puede estar vacío.");

            await _mantenimientoRepository.AddAsync(mantenimiento);
        }

        public async Task UpdateAsync(Mantenimiento mantenimiento)
        {
            if (mantenimiento.IdMantenimiento <= 0)
                throw new ArgumentException("El ID no es válido para actualización.");

            if (string.IsNullOrWhiteSpace(mantenimiento.Nombre))
                throw new ArgumentException("El nombre de la empresa no puede estar vacío.");

            await _mantenimientoRepository.UpdateAsync(mantenimiento);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID no es válido para eliminación.");

            await _mantenimientoRepository.DeleteAsync(id);
        }

        

    }
}
