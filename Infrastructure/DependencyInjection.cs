using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "MyCleanArchitectureDB"), ServiceLifetime.Singleton);
                services.AddSingleton<IDomainEventService, DomainEventService>();
                services.AddSingleton<IUnitOfWork, UnitOfWork.UnitOfWork>();
                services.AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>));
                services.AddSingleton<ITodoItemRepository, TodoItemRepository>();
                services.AddSingleton<ITodoListRepository, TodoListRepository>(); 
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
                services.AddScoped<IDomainEventService, DomainEventService>();
                services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
                services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
                services.AddTransient<ITodoItemRepository, TodoItemRepository>();
                services.AddTransient<ITodoListRepository, TodoListRepository>();
            }
            services.AddSingleton<ITimer, TimerService>();

            return services;
        }
    }
}
