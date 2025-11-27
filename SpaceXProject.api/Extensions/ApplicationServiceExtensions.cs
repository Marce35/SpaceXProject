using SpaceXProject.api.Shared.Base.ResultPattern.ResultFactory;
using SpaceXProject.api.Shared.Base.ResultPattern.ResultFactory.Interface;

namespace SpaceXProject.api.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IResultFactory, ResultFactory>();

        return services;
    }
}
