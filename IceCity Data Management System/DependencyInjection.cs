using IceCity_Data_Management_System.Configuration;
using IceCity_Data_Management_System.Persistence.Repositories;
using IceCity_Data_Management_System.Services.Implementations;
using IceCity_Data_Management_System.Services.Interfaces;

namespace IceCity_Data_Management_System
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            var conString = configuration.GetConnectionString("DefaultConnection") ??
                            throw new InvalidOperationException("ConnectionString 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(conString));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IHouseRepository, HouseRepository>();
            services.AddScoped<ICatShelterRepository, CatShelterRepository>();
            services.AddScoped<ISensorReadingRepository, SensorReadingRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IHouseService, HouseService>();
            services.AddScoped<ICatShelterService, CatShelterService>();
            services.AddScoped<ISensorReadingService, SensorReadingService>();

            var configSingleton = AppConfig.GetInstance(configuration);
            services.AddSingleton(configSingleton);

            return services;
        }
    }
}
