using System.Net;
using FluentAssertions;
using Snapshooter.NUnit;

namespace Refactor.Application.Test.Controllers;

public class SwaggerTests : IntegrationTestBase
{
    [TestCase("/swagger")]
    [TestCase("/swagger/index.html")]
    [TestCase("/swagger/v1/swagger.json")]
    public async Task Should_Return_Ok(string path)
    {
        var response = await GetAsync(path);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task Should_Match_Snapshot()
    {
        var content = await GetStringAsync("/swagger/v1/swagger.json");
        content.Should().MatchSnapshot();
    }
}