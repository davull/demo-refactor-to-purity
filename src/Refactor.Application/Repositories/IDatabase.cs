using Refactor.Application.Data;

namespace Refactor.Application.Repositories;

public interface IDatabase
{
    Task<T> Get<T>(Guid id) where T : IData;

    Task<IEnumerable<T>> GetAll<T>() where T : IData;

    Task Add<T>(T entity) where T : IData;

    Task Update<T>(T entity) where T : IData;

    Task Delete<T>(T entity) where T : IData;
}
