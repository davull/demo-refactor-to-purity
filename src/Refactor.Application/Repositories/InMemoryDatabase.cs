using Refactor.Application.Data;

namespace Refactor.Application.Repositories;

public class InMemoryDatabase : IDatabase
{
    protected readonly IDictionary<Type, ICollection<IData>> _data = new Dictionary<Type, ICollection<IData>>();

    public async Task<T> Get<T>(Guid id) where T : IData =>
        (await GetAll<T>()).Single(d => d.Id == id);

    public Task<IEnumerable<T>> GetAll<T>() where T : IData =>
        _data.TryGetValue(typeof(T), out var data)
            ? Task.FromResult(data.Cast<T>())
            : Task.FromResult<IEnumerable<T>>(Array.Empty<T>());

    public Task Add<T>(T entity) where T : IData
    {
        if (!_data.TryGetValue(typeof(T), out var data))
        {
            data = new List<IData>();
            _data.Add(typeof(T), data);
        }

        data.Add(entity);
        return Task.CompletedTask;
    }

    public Task Update<T>(T entity) where T : IData
    {
        if (!_data.TryGetValue(typeof(T), out var data))
        {
            data = new List<IData>();
            _data.Add(typeof(T), data);
        }

        if (data.Contains(entity))
            data.Remove(entity);

        data.Add(entity);

        return Task.CompletedTask;
    }

    public Task Delete<T>(T entity) where T : IData
    {
        if (!_data.TryGetValue(typeof(T), out var data))
        {
            data = new List<IData>();
            _data.Add(typeof(T), data);
        }

        if (data.Contains(entity))
            data.Remove(entity);

        return Task.CompletedTask;
    }
}
