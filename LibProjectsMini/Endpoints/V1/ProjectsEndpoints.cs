using System.Threading.Tasks;
using ApiToolsShared;
using LibProjectsMini.CommandRequests;
using LibProjectsMini.Mappers;
using LibProjectsMini.QueryRequests;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebAgentContracts.V1.Requests;
using WebAgentContracts.V1.Routes;
using WebInstallers;

namespace LibProjectsMini.Endpoints.V1;

public sealed class ProjectsEndpoints : IInstaller
{
    public int InstallPriority => 50;
    public int ServiceUsePriority => 50;

    public void InstallServices(WebApplicationBuilder builder, string[] args)
    {
    }

    public void UseServices(WebApplication app)
    {
        app.MapPost(ProjectsApiRoutes.Projects.UpdateSettings, UpdateSettings).AddEndpointFilter<ApiKeysChecker>();
        app.MapPost(ProjectsApiRoutes.Projects.Update, Update).AddEndpointFilter<ApiKeysChecker>();
        app.MapPost(ProjectsApiRoutes.Projects.UpdateService, UpdateService).AddEndpointFilter<ApiKeysChecker>();
        app.MapPost(ProjectsApiRoutes.Projects.StopService, StopService).AddEndpointFilter<ApiKeysChecker>();
        app.MapPost(ProjectsApiRoutes.Projects.StartService, StartService).AddEndpointFilter<ApiKeysChecker>();
        app.MapDelete(ProjectsApiRoutes.Projects.RemoveProject, RemoveProject).AddEndpointFilter<ApiKeysChecker>();
        app.MapDelete(ProjectsApiRoutes.Projects.RemoveService, RemoveService).AddEndpointFilter<ApiKeysChecker>();
        app.MapGet(ProjectsApiRoutes.Projects.GetAppSettingsVersion, GetAppSettingsVersion)
            .AddEndpointFilter<ApiKeysChecker>();
        app.MapGet(ProjectsApiRoutes.Projects.GetVersion, GetVersion).AddEndpointFilter<ApiKeysChecker>();
    }


    //// POST api/project/updatesettings
    //[HttpPost(InsApiRoutes.Projects.UpdateSettings)]
    private static async Task<IResult> UpdateSettings(ILogger<ProjectsEndpoints> logger,
        [FromBody] UpdateSettingsRequest? request,
        HttpRequest httpRequest, IConfiguration config, IMediator mediator)
    {
        if (request is null)
            return Results.BadRequest(ApiErrors.RequestIsEmpty);
        var command = request.AdaptTo();
        var result = await mediator.Send(command);
        return result.Match(_ => Results.Ok(), Results.BadRequest);
    }

    // POST api/project/update
    //[HttpPost(InsApiRoutes.Projects.Update)]
    private static async Task<IResult> Update(ILogger<ProjectsEndpoints> logger,
        [FromBody] ProjectUpdateRequest? request,
        HttpRequest httpRequest, IConfiguration config, IMediator mediator)
    {
        if (request is null)
            return Results.BadRequest(ApiErrors.RequestIsEmpty);
        var command = request.AdaptTo();
        var result = await mediator.Send(command);
        return result.Match(Results.Ok, Results.BadRequest);
    }

    // POST api/project/update
    //[HttpPost(InsApiRoutes.Projects.UpdateService)]
    private static async Task<IResult> UpdateService(ILogger<ProjectsEndpoints> logger,
        [FromBody] UpdateServiceRequest? request,
        HttpRequest httpRequest, IConfiguration config, IMediator mediator)
    {
        if (request is null)
            return Results.BadRequest(ApiErrors.RequestIsEmpty);
        var command = request.AdaptTo();
        var result = await mediator.Send(command);
        return result.Match(Results.Ok, Results.BadRequest);
    }

    // POST api/project/stop
    //[HttpPost(InsApiRoutes.Projects.StopService)]
    private static async Task<IResult> StopService(ILogger<ProjectsEndpoints> logger, string? serviceName,
        [FromQuery] string? apiKey, HttpRequest httpRequest, IConfiguration config, IMediator mediator)
    {
        //StopServiceCommandRequest

        if (serviceName is null)
            return Results.BadRequest(ProjectsErrors.ServiceNameIsEmpty);
        var command = StopServiceCommandRequest.Create(serviceName);
        var result = await mediator.Send(command);
        return result.Match(_ => Results.Ok(), Results.BadRequest);
    }

    // POST api/project/start
    //[HttpPost(InsApiRoutes.Projects.StartService)]
    private static async Task<IResult> StartService(ILogger<ProjectsEndpoints> logger, string? serviceName,
        [FromQuery] string? apiKey, HttpRequest httpRequest, IConfiguration config, IMediator mediator)
    {
        //StartServiceCommandRequest

        if (serviceName is null)
            return Results.BadRequest(ProjectsErrors.ServiceNameIsEmpty);
        var command = StartServiceCommandRequest.Create(serviceName);
        var result = await mediator.Send(command);
        return result.Match(_ => Results.Ok(), Results.BadRequest);
    }

    // POST api/project/remove
    //[HttpDelete(InsApiRoutes.Projects.RemoveProject)]
    private static async Task<IResult> RemoveProject(ILogger<ProjectsEndpoints> logger, string projectName,
        [FromQuery] string? apiKey, HttpRequest httpRequest, IConfiguration config, IMediator mediator)
    {
        //ეს არის პროგრამის წაშლის ის ვარიანტი, როცა პროგრამა სერვისი არ არის
        return await RemoveProjectService(projectName, null, mediator);
    }

    // POST api/project/removeservice
    //[HttpDelete(InsApiRoutes.Projects.RemoveService)]
    private static async Task<IResult> RemoveService(ILogger<ProjectsEndpoints> logger, string projectName,
        string serviceName, [FromQuery] string? apiKey, HttpRequest httpRequest, IConfiguration config,
        IMediator mediator)
    {
        //ეს არის პროგრამის წაშლის ის ვარიანტი, როცა პროგრამა სერვისია
        return await RemoveProjectService(projectName, serviceName, mediator);
    }

    // GET api/project/getappsettingsversion
    //[HttpGet(InsApiRoutes.Projects.GetAppSettingsVersion)]
    //[ApiKeyAuth]
    private static async Task<IResult> GetAppSettingsVersion(ILogger<ProjectsEndpoints> logger, int serverSidePort,
        string apiVersionId, [FromQuery] string? apiKey, HttpRequest httpRequest, IConfiguration config,
        IMediator mediator)
    {
        //GetAppSettingsVersionQuery

        if (string.IsNullOrWhiteSpace(apiVersionId) || serverSidePort == 0)
            return Results.BadRequest(ProjectsErrors.SameParametersAreEmpty);
        var command = GetAppSettingsVersionQueryRequest.Create(serverSidePort, apiVersionId);
        var result = await mediator.Send(command);
        return result.Match(Results.Ok, Results.BadRequest);
    }

    // POST api/project/getversion
    //[HttpGet(InsApiRoutes.Projects.GetVersion)]
    private static async Task<IResult> GetVersion(ILogger<ProjectsEndpoints> logger, int serverSidePort,
        string apiVersionId, [FromQuery] string? apiKey, HttpRequest httpRequest, IConfiguration config,
        IMediator mediator)
    {
        //GetVersionQuery

        if (string.IsNullOrWhiteSpace(apiVersionId) || serverSidePort == 0)
            return Results.BadRequest(ProjectsErrors.SameParametersAreEmpty);
        var command = GetVersionQueryRequest.Create(serverSidePort, apiVersionId);
        var result = await mediator.Send(command);
        return result.Match(Results.Ok, Results.BadRequest);
    }

    private static async Task<IResult> RemoveProjectService(string projectName,
        string? serviceName, IMediator mediator)
    {
        //RemoveProjectServiceCommand

        var command = RemoveProjectServiceCommandRequest.Create(projectName, serviceName);
        var result = await mediator.Send(command);
        return result.Match(_ => Results.Ok(), Results.BadRequest);
    }
}