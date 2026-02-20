using System;
using Microsoft.AspNetCore.Routing;
using WebAgentShared.LibProjectsApi.Endpoints.V1;

namespace WebAgentShared.LibProjectsApi.DependencyInjection;

public static class LibProjectsApiDependencyInjection
{
    public static bool UseLibProjectsApi(this IEndpointRouteBuilder endpoints, bool debugMode)
    {
        if (debugMode)
        {
            Console.WriteLine($"{nameof(UseLibProjectsApi)} Started");
        }

        endpoints.UseProjectsEndpoints(debugMode);

        if (debugMode)
        {
            Console.WriteLine($"{nameof(UseLibProjectsApi)} Finished");
        }

        return true;
    }
}
