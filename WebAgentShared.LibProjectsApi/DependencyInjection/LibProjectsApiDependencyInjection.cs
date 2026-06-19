using System;
using Microsoft.AspNetCore.Routing;
using Serilog;
using WebAgentShared.LibProjectsApi.Endpoints.V1;

namespace WebAgentShared.LibProjectsApi.DependencyInjection;

public static class LibProjectsApiDependencyInjection
{
    public static bool UseLibProjectsApi(this IEndpointRouteBuilder endpoints, ILogger? debugLogger)
    {
        debugLogger?.Information("{MethodName} Started", nameof(UseLibProjectsApi));

        endpoints.UseProjectsEndpoints(debugLogger);

        debugLogger?.Information("{MethodName} Finished", nameof(UseLibProjectsApi));

        return true;
    }
}
