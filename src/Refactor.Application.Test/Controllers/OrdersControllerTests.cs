using System.Net;
using FluentAssertions;
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
}
