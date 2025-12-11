using AA1.DTOs;
using Models;

namespace AA1.Repositories
{
    public interface IOpinionRepository
    {
        Task<List<Opinion>> GetAllAsync();
        Task<Opinion?> GetByIdAsync(int id);
        Task<List<OpinionDto>> GetAllFilteredPuntuAsync(int? puntuacion, string? orderBy, bool ascending);
        Task AddAsync(Opinion opinion);
        Task UpdateAsync(Opinion opinion);
        Task DeleteAsync(int idOpinion);
        Task InicializarDatosAsync();
        Task<int> GetTotalCountAsync();
    }
}