namespace Refactor.Application.Test;

public class IntegrationTestBase
{
    private readonly TestWebApplicationFactory _factory;

    protected readonly HttpClient Client;

    public IntegrationTestBase()
    {
        _factory = new TestWebApplicationFactory();
        Client = _factory.CreateClient();
    }

    protected async Task<HttpResponseMessage> GetAsync(string path)
        => await Client.GetAsync(path);

    protected async Task<string> GetStringAsync(string path)
        => await Client.GetStringAsync(path);
}
