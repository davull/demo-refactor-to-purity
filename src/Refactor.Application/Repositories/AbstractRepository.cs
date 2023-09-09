using Refactor.Application.Data;

namespace Refactor.Application.Repositories;

public abstract class AbstractRepository<T> : IRepository<T> where T : IData
{
    protected readonly IDatabase _database;

    protected AbstractRepository(IDatabase database)
    {
        _database = database;
    }

    public abstract Task<T> Get(Guid id);

    public abstract Task<IEnumerable<T>> GetAll();

    public abstract Task Add(T entity);

    public abstract Task Update(T entity);

    public abstract Task Delete(T entity);
}
