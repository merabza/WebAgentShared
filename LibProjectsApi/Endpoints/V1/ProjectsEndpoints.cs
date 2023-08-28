using System.Diagnostics;
using System.Threading.Tasks;
using ApiToolsShared;
using LibProjectsApi.CommandRequests;
using LibProjectsApi.Handlers;
using LibProjectsApi.Mappers;
using LibProjectsApi.QueryRequests;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemToolsShared;
using WebAgentProjectsApiContracts.V1.Requests;
using WebAgentProjectsApiContracts.V1.Routes;
using WebInstallers;

namespace LibProjectsApi.Endpoints.V1;

// ReSharper disable once UnusedType.Global
public sealed class ProjectsEndpoints : IInstaller
{
    public int InstallPriority => 50;
    public int ServiceUsePriority => 50;

    public void InstallServices(WebApplicationBuilder builder, string[] args)
    {
    }

    public void UseServices(WebApplication app)
    {
        //.RequireAuthorization(); იყო //.AddEndpointFilter<ApiKeysChecker>();

        //Console.WriteLine("InstallServices.UseServices Started");
        var group = app.MapGroup(ProjectsApiRoutes.Projects.ProjectBase).RequireAuthorization();

        group.MapPost(ProjectsApiRoutes.Projects.UpdateSettings, UpdateSettings);
        group.MapPost(ProjectsApiRoutes.Projects.Update, Update);
        group.MapPost(ProjectsApiRoutes.Projects.UpdateService, UpdateService);
        group.MapPost(ProjectsApiRoutes.Projects.StopService, StopService);
        group.MapPost(ProjectsApiRoutes.Projects.StartService, StartService);
        group.MapDelete(ProjectsApiRoutes.Projects.RemoveProject, RemoveProject);
        group.MapDelete(ProjectsApiRoutes.Projects.RemoveService, RemoveService);
        group.MapGet(ProjectsApiRoutes.Projects.GetAppSettingsVersion, GetAppSettingsVersion);
        group.MapGet(ProjectsApiRoutes.Projects.GetVersion, GetVersion);
        //Console.WriteLine("InstallServices.UseServices Finished");
    }

    //// POST api/project/updatesettings
    private static async Task<IResult> UpdateSettings([FromBody] UpdateSettingsRequest? request,
        HttpRequest httpRequest, IMediator mediator, IMessagesDataManager messagesDataManager)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateSettings)} started");
        Debug.WriteLine($"Call {nameof(UpdateSettingsCommandHandler)} from {nameof(UpdateSettings)}");

        if (request is null)
            return Results.BadRequest(ApiErrors.RequestIsEmpty);
        var command = request.AdaptTo();
        var result = await mediator.Send(command);

        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateSettings)} finished");
        return result.Match(_ => Results.Ok(), Results.BadRequest);
    }

    // POST api/project/update
    private static async Task<IResult> Update([FromBody] ProjectUpdateRequest? request, HttpRequest httpRequest,
        IMediator mediator, IMessagesDataManager messagesDataManager)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(Update)} started");
        Debug.WriteLine($"Call {nameof(ProjectUpdateCommandHandler)} from {nameof(Update)}");

        if (request is null)
            return Results.BadRequest(ApiErrors.RequestIsEmpty);
        var command = request.AdaptTo(userName);
        var result = await mediator.Send(command);

        await messagesDataManager.SendMessage(userName, $"{nameof(Update)} finished");
        return result.Match(Results.Ok, Results.BadRequest);
    }

    // POST api/project/update
    private static async Task<IResult> UpdateService([FromBody] UpdateServiceRequest? request, HttpRequest httpRequest,
        IMediator mediator, IMessagesDataManager messagesDataManager)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateService)} started");
        Debug.WriteLine($"Call {nameof(UpdateServiceCommandHandler)} from {nameof(UpdateService)}");

        if (request is null)
            return Results.BadRequest(ApiErrors.RequestIsEmpty);
        var command = request.AdaptTo(userName);
        var result = await mediator.Send(command);

        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateService)} finished");
        return result.Match(Results.Ok, Results.BadRequest);
    }

    // POST api/project/stop
    private static async Task<IResult> StopService([FromRoute] string? serviceName, HttpRequest httpRequest,
        IMediator mediator, IMessagesDataManager messagesDataManager)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(StopService)} started");
        Debug.WriteLine($"Call {nameof(StopServiceCommandHandler)} from {nameof(StopService)}");

        if (serviceName is null)
            return Results.BadRequest(ProjectsErrors.ServiceNameIsEmpty);
        var command = StopServiceCommandRequest.Create(serviceName, userName);
        var result = await mediator.Send(command);

        await messagesDataManager.SendMessage(userName, $"{nameof(StopService)} started");
        return result.Match(_ => Results.Ok(), Results.BadRequest);
    }

    // POST api/project/start
    private static async Task<IResult> StartService([FromRoute] string? serviceName, HttpRequest httpRequest,
        IMediator mediator, IMessagesDataManager messagesDataManager)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(StartService)} started");
        Debug.WriteLine($"Call {nameof(StartServiceCommandHandler)} from {nameof(StartService)}");

        if (serviceName is null)
            return Results.BadRequest(ProjectsErrors.ServiceNameIsEmpty);
        var command = StartServiceCommandRequest.Create(serviceName, userName);
        var result = await mediator.Send(command);

        await messagesDataManager.SendMessage(userName, $"{nameof(StartService)} started");
        return result.Match(_ => Results.Ok(), Results.BadRequest);
    }

    // POST api/project/remove
    private static async Task<IResult> RemoveProject([FromRoute] string projectName, HttpRequest httpRequest,
        IMediator mediator, IMessagesDataManager messagesDataManager)
    {
        //ეს არის პროგრამის წაშლის ის ვარიანტი, როცა პროგრამა სერვისი არ არის
        return await RemoveProjectService(projectName, null, mediator, messagesDataManager, httpRequest);
    }

    // POST api/project/removeservice
    private static async Task<IResult> RemoveService([FromRoute] string projectName, [FromRoute] string serviceName,
        HttpRequest httpRequest, IMediator mediator, IMessagesDataManager messagesDataManager)
    {
        //ეს არის პროგრამის წაშლის ის ვარიანტი, როცა პროგრამა სერვისია
        return await RemoveProjectService(projectName, serviceName, mediator, messagesDataManager, httpRequest);
    }

    // GET api/project/getappsettingsversion
    private static async Task<IResult> GetAppSettingsVersion([FromRoute] int serverSidePort,
        [FromRoute] string apiVersionId, HttpRequest httpRequest, IMediator mediator,
        IMessagesDataManager messagesDataManager)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(GetAppSettingsVersion)} started");
        Debug.WriteLine($"Call {nameof(GetAppSettingsVersionQueryHandler)} from {nameof(GetAppSettingsVersion)}");

        if (string.IsNullOrWhiteSpace(apiVersionId) || serverSidePort == 0)
            return Results.BadRequest(ProjectsErrors.SameParametersAreEmpty);
        var command = GetAppSettingsVersionQueryRequest.Create(serverSidePort, apiVersionId, userName);
        var result = await mediator.Send(command);

        await messagesDataManager.SendMessage(userName, $"{nameof(GetAppSettingsVersion)} finished");
        return result.Match(Results.Ok, Results.BadRequest);
    }

    // POST api/project/getversion
    private static async Task<IResult> GetVersion([FromRoute] int serverSidePort, [FromRoute] string apiVersionId,
        HttpRequest httpRequest, IMediator mediator, IMessagesDataManager messagesDataManager)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(GetVersion)} started");
        Debug.WriteLine($"Call {nameof(GetVersionQueryHandler)} from {nameof(GetVersion)}");

        if (string.IsNullOrWhiteSpace(apiVersionId) || serverSidePort == 0)
            return Results.BadRequest(ProjectsErrors.SameParametersAreEmpty);
        var command = GetVersionQueryRequest.Create(serverSidePort, apiVersionId, userName);
        var result = await mediator.Send(command);

        await messagesDataManager.SendMessage(userName, $"{nameof(GetVersion)} finished");
        return result.Match(Results.Ok, Results.BadRequest);
    }

    private static async Task<IResult> RemoveProjectService(string projectName, string? serviceName, ISender mediator,
        IMessagesDataManager messagesDataManager, HttpRequest httpRequest)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(RemoveProjectService)} started");
        Debug.WriteLine($"Call {nameof(RemoveProjectServiceCommandHandler)} from {nameof(RemoveProjectService)}");

        var command = RemoveProjectServiceCommandRequest.Create(projectName, serviceName, userName);
        var result = await mediator.Send(command);

        await messagesDataManager.SendMessage(userName, $"{nameof(RemoveProjectService)} finished");
        return result.Match(_ => Results.Ok(), Results.BadRequest);
    }
}