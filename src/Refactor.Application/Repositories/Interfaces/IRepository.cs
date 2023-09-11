namespace Refactor.Application.Repositories.Interfaces;

public interface IRepository<T> where T : IData
{
    Task<T> Get(Guid id);

    Task<IEnumerable<T>> GetAll();

    Task Add(T entity);

    Task Update(T entity);

    Task Delete(T entity);
}
