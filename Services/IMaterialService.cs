using Models;

namespace AA1.Services
{
    public interface IMaterialService
    {
        Task<List<Material>> GetAllAsync();
        Task<Material?> GetByIdAsync(int id);
        Task AddAsync(Material material);
        Task UpdateAsync(Material material);
        Task DeleteAsync(int id);

    }
}