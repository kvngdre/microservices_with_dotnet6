using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        Task<IReadOnlyCollection<T>> GetAllAsync();

        Task<T> FindByIdAsync(Guid id);

        Task InsertAsync(T entity);

        Task UpdateAsync(T entity);

        Task RemoveAsync(Guid id);
    }
}