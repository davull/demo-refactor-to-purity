namespace Refactor.Application.Repositories;

public class SqliteDatabase : IDatabase
{
    public Task<T> Get<T>(Guid id) where T : IData => throw new NotImplementedException();

    public Task<IEnumerable<T>> GetAll<T>() where T : IData => throw new NotImplementedException();

    public Task Add<T>(T entity) where T : IData => throw new NotImplementedException();

    public Task Update<T>(T entity) where T : IData => throw new NotImplementedException();

    public Task Delete<T>(T entity) where T : IData => throw new NotImplementedException();
}
