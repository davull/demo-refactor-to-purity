using Refactor.Application.Repositories;
using Refactor.Application.Repositories.Implementations;
using Refactor.Application.Repositories.Interfaces;
using Refactor.Application.Services;

namespace Refactor.Application;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        RegisterServices(builder.Services)
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
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
        services.AddSingleton<IDatabase, InMemoryDatabase>();

        // Register Repositories
        services.AddTransient<ICustomerRepository, CustomerRepository>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<IOrderItemRepository, OrderItemRepository>();

        // Register Services
        services.AddSingleton<ITaxService, TaxService>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IOrderItemService, OrderItemService>();

        return services;
    }
}
