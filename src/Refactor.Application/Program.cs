using Refactor.Application.Repositories;
using Refactor.Application.Repositories.Implementations;
using Refactor.Application.Repositories.Interfaces;

namespace Refactor.Application;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        RegisterServices(builder.Services);

        var app = builder.Build();

        if (app.Environment.IsDevelopment() ||
            app.Environment.IsEnvironment("Test"))
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    private static IServiceCollection RegisterServices(IServiceCollection services)
    {
        // Register database
        services.AddScoped<IDatabase, InMemoryDatabase>();

        // Register Repositories
        services.AddTransient<CustomerRepository>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<OrderItemRepository>();

        return services;
    }
}
