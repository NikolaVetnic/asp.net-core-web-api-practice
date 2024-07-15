using Microsoft.Extensions.DependencyInjection;
using SolidToken.SpecFlow.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TodoApi.Test;
public static class SpecFlowConfiguration
{
    [ScenarioDependencies]
    public static IServiceCollection CreateServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<WebApplicationFactory<Program>>();

        return services;
    }
}