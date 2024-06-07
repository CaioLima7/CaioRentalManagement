using Catalog.API.Entities;

namespace Catalog.API.Repositories.Interfaces
{
    public interface IMotorcycleRepository
    {
        Task<IEnumerable<Motorcycle>> GetAllAsync();
        Task<Motorcycle> GetByIdAsync(int id);
        Task AddAsync(Motorcycle motorcycle);
        Task UpdateAsync(Motorcycle motorcycle);
        Task DeleteAsync(int id);
    }
}
