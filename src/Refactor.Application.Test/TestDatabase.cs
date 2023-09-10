using Refactor.Application.Data;
using Refactor.Application.Repositories;
using static Refactor.Application.Test.DataDummies;

namespace Refactor.Application.Test;

public class TestDatabase : InMemoryDatabase
{
    public TestDatabase()
    {
        Seed();
    }

    private void Seed()
    {
        var order1Id = new Guid("687b4a03-6f3d-4f42-9ee6-53d5cf7e80d5");
        var order2Id = new Guid("f1cf45fb-33b5-4a3d-b684-2e3a00a0ebea");

        // Customers
        _data.Add(typeof(Customer), new List<IData> { PeterPan, JohnDoe, JaneDoe });

        // Orders
        _data.Add(typeof(Order), new List<IData>
        {
            new Order(order1Id, PeterPan.Id, new DateTime(2021, 01, 01, 10, 30, 00)),
            new Order(order2Id, PeterPan.Id, new DateTime(2021, 01, 11, 12, 30, 00)),
            new Order(new Guid("6a72a608-77ea-4742-80b1-2152daf260c9"), JohnDoe.Id,
                new DateTime(2021, 01, 20, 20, 45, 00))
        });

        // OrderItems
        _data.Add(typeof(OrderItem), new List<IData>
        {
            new OrderItem(new Guid("e008de98-6f3e-4a3e-a81a-e094a923c5c3"), order1Id,
                new Guid("7db60574-f667-46e9-941e-b775a2542e33"), 1, 10.00m),
            new OrderItem(new Guid("84dfe6a1-c537-4468-bb6d-4a93cba99ae8"), order1Id,
                new Guid("cd6b6e1c-8cf2-4769-8205-39f41179c135"), 2, 45.50m),

            new OrderItem(new Guid("0f90b82b-0c29-4417-8f18-da20b734d3a7"), order2Id,
                new Guid("ff08a855-9f82-4f2d-bf98-f0a929f4ef90"), 1, 9.80m),
            new OrderItem(new Guid("84319c54-7eab-416f-8172-bd472635dc88"), order2Id,
                new Guid("2cfcd5d9-dcb6-495d-9878-8d8f6fe85fac"), 1, 10.50m),
            new OrderItem(new Guid("78263e1a-1023-494f-a89e-dbe24d8819de"), order2Id,
                new Guid("89671827-5b8c-40cd-a533-af04224f5f29"), 4, 20.00m),
            new OrderItem(new Guid("3400dfa0-49ee-441c-8b93-a9040f6c83cd"), order2Id,
                new Guid("d583263d-2169-4dc7-acbe-52846b79b7a9"), 2, 30.50m)
        });
    }
}
