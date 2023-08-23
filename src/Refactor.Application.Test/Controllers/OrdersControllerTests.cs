using System.Net;
using FluentAssertions;
using Snapshooter.NUnit;

namespace Refactor.Application.Test.Controllers;

public class OrdersControllerTests : IntegrationTestBase
{
    [Test]
    public async Task Should_Return_Ok()
    {
        var response = await GetAsync("/orders");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task Should_Match_Snapshot()
    {
        var content = await GetStringAsync("/orders");
        content.Should().MatchSnapshot();
    }
}