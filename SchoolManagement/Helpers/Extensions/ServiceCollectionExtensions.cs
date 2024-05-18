using ProgramApi.Helpers.AutoMapper;
using ProgramApi.Helpers.ConfigurationSettings.ConfigManager;

namespace ProgramApi.Helpers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOtherServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            
            return services;
        }
    }
}
