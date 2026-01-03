using System;
using LibProjectsApi.Endpoints.V1;
using Microsoft.AspNetCore.Routing;

namespace LibProjectsApi.DependencyInjection;

public static class LibProjectsApiDependencyInjection
{
    public static bool UseLibProjectsApi(this IEndpointRouteBuilder endpoints, bool debugMode)
    {
        if (debugMode)
            Console.WriteLine($"{nameof(UseLibProjectsApi)} Started");

        endpoints.UseProjectsEndpoints(debugMode);

        if (debugMode)
            Console.WriteLine($"{nameof(UseLibProjectsApi)} Finished");

        return true;
    }
}