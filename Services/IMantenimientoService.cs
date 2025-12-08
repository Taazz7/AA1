using Models;

namespace AA1.Services
{
    public interface IMantenimientoService
    {
        Task<List<Mantenimiento>> GetAllAsync();
        Task<Mantenimiento?> GetByIdAsync(int id);
        Task AddAsync(Mantenimiento mantenimiento);
        Task UpdateAsync(Mantenimiento mantenimiento);
        Task DeleteAsync(int id);

    }
}