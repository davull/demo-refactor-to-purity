using System.Net;
using FluentAssertions;
using FluentAssertions.Execution;
using Refactor.Application.Models;
using Snapshooter.NUnit;

namespace Refactor.Application.Test.Controllers;

public class OrdersControllerTests : IntegrationTestBase
{
    [TestCase("/orders")]
    [TestCase("/orders?startDate=2021-01-01")]
    [TestCase("/orders?endDate=2021-01-01")]
    [TestCase("/orders?startDate=2021-01-01&endDate=2021-01-31")]
    public async Task Should_Return_Ok(string path)
    {
        var response = await GetAsync(path);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task Should_Match_Snapshot()
    {
        var content = await GetStringAsync("/orders");
        content.Should().MatchSnapshot();
    }

    [Test]
    public async Task Should_Return_Orders()
    {
        var orders = await GetAsync<Order[]>("/orders?startDate=2021-01-01&endDate=2021-01-20");

        using var _ = new AssertionScope();

        orders.Should().HaveCount(2);

        orders.Should().OnlyContain(o => o.OrderDate >= new DateTime(2021, 1, 1) &&
                                         o.OrderDate <= new DateTime(2021, 1, 20));
    }

    [Test]
    public async Task Should_Add_Order()
    {
        var newOrder = new Order(
            Guid.NewGuid(),
            new Customer(new Guid("cd8e3d04-e178-4977-b3c7-16eaf3ec9c36"), "Peter", "Pan", "peter.pan@example.com"),
            new[] { new OrderItem(Guid.NewGuid(), Guid.NewGuid(), 1, 119, 100, 19, 19, 119, 100) },
            DateTime.Now);

        var response = await PostAsync("/orders", newOrder);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
