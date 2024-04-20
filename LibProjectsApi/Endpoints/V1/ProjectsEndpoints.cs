using System.Diagnostics;
using System.Text;
using System.Threading;
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

    // POST api/projects/updatesettings
    private static async Task<IResult> UpdateSettings([FromBody] UpdateSettingsRequest? request,
        HttpRequest httpRequest, IMediator mediator, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateSettings)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(UpdateSettingsCommandHandler)} from {nameof(UpdateSettings)}");

        if (request is null)
            return Results.BadRequest(new[] { ApiErrors.RequestIsEmpty });

        var command = request.AdaptTo(userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateSettings)} finished", cancellationToken);
        return result.Match(_ => Results.Ok(), Results.BadRequest);
    }

    // POST api/projects/update
    private static async Task<IResult> Update([FromBody] ProjectUpdateRequest? request, HttpRequest httpRequest,
        IMediator mediator, IMessagesDataManager messagesDataManager, CancellationToken cancellationToken)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(Update)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(ProjectUpdateCommandHandler)} from {nameof(Update)}");

        if (request is null)
            return Results.BadRequest(new[] { ApiErrors.RequestIsEmpty });
        var command = request.AdaptTo(userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(Update)} finished", cancellationToken);
        return result.Match(Results.Ok, Results.BadRequest);
    }

    // POST api/projects/updateservice
    private static async Task<IResult> UpdateService([FromBody] UpdateServiceRequest? request, HttpRequest httpRequest,
        IMediator mediator, IMessagesDataManager messagesDataManager, CancellationToken cancellationToken)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(UpdateServiceCommandHandler)} from {nameof(UpdateService)}");

        if (request is null)
            return Results.BadRequest(new[] { ApiErrors.RequestIsEmpty });
        var command = request.AdaptTo(userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateService)} finished", cancellationToken);
        return result.Match(Results.Ok, Results.BadRequest);
    }

    // POST api/projects/stop/{serviceName}/{environmentName}
    private static async Task<IResult> StopService([FromRoute] string? serviceName, [FromRoute] string environmentName,
        HttpRequest httpRequest, IMediator mediator, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(StopService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(StopServiceCommandHandler)} from {nameof(StopService)}");

        if (serviceName is null)
            return Results.BadRequest(new[] { ProjectsErrors.ServiceNameIsEmpty });

        var command = StopServiceCommandRequest.Create(serviceName, environmentName, userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(StopService)} started", cancellationToken);
        return result.Match(_ => Results.Ok(), Results.BadRequest);
    }

    // POST api/projects/start/{serviceName}/{environmentName}
    private static async Task<IResult> StartService([FromRoute] string? serviceName, [FromRoute] string environmentName,
        HttpRequest httpRequest, IMediator mediator, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(StartService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(StartServiceCommandHandler)} from {nameof(StartService)}");

        if (serviceName is null)
            return Results.BadRequest(new[] { ProjectsErrors.ServiceNameIsEmpty });
        var command = StartServiceCommandRequest.Create(serviceName, environmentName, userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(StartService)} started", cancellationToken);
        return result.Match(_ => Results.Ok(), Results.BadRequest);
    }

    // POST api/projects/remove/{projectName}/{environmentName}
    private static async Task<IResult> RemoveProject([FromRoute] string projectName, [FromRoute] string environmentName,
        HttpRequest httpRequest, IMediator mediator, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(RemoveProject)} started", cancellationToken);

        //ეს არის პროგრამის წაშლის ის ვარიანტი, როცა პროგრამა სერვისი არ არის
        var result = await RemoveProjectService(projectName, null, environmentName, mediator, messagesDataManager,
            httpRequest, cancellationToken);
        await messagesDataManager.SendMessage(userName, $"{nameof(RemoveProject)} finished", cancellationToken);
        return result;
    }

    // POST api/projects/removeservice/{projectName}/{serviceName}/{environmentName}
    private static async Task<IResult> RemoveService([FromRoute] string projectName, [FromRoute] string serviceName,
        [FromRoute] string environmentName, HttpRequest httpRequest, IMediator mediator,
        IMessagesDataManager messagesDataManager, CancellationToken cancellationToken)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(RemoveService)} started", cancellationToken);

        //ეს არის პროგრამის წაშლის ის ვარიანტი, როცა პროგრამა სერვისია
        var result = await RemoveProjectService(projectName, serviceName, environmentName, mediator,
            messagesDataManager,
            httpRequest, cancellationToken);
        await messagesDataManager.SendMessage(userName, $"{nameof(RemoveService)} finished", cancellationToken);
        return result;
    }

    // GET api/projects/getappsettingsversion/{serverSidePort}/{apiVersionId}
    private static async Task<IResult> GetAppSettingsVersion([FromRoute] int serverSidePort,
        [FromRoute] string apiVersionId, HttpRequest httpRequest, IMediator mediator,
        IMessagesDataManager messagesDataManager, CancellationToken cancellationToken)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(GetAppSettingsVersion)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(GetAppSettingsVersionQueryHandler)} from {nameof(GetAppSettingsVersion)}");

        if (string.IsNullOrWhiteSpace(apiVersionId) || serverSidePort == 0)
            return Results.BadRequest(new[] { ProjectsErrors.SameParametersAreEmpty });

        var command = GetAppSettingsVersionQueryRequest.Create(serverSidePort, apiVersionId, userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(GetAppSettingsVersion)} finished", cancellationToken);
        return result.Match(ret => Results.Text(ret, "text/plain", Encoding.UTF8), Results.BadRequest);
    }

    // POST api/projects/getversion/{serverSidePort}/{apiVersionId}
    private static async Task<IResult> GetVersion([FromRoute] int serverSidePort, [FromRoute] string apiVersionId,
        HttpRequest httpRequest, IMediator mediator, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(GetVersion)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(GetVersionQueryHandler)} from {nameof(GetVersion)}");

        if (string.IsNullOrWhiteSpace(apiVersionId) || serverSidePort == 0)
            return Results.BadRequest(new[] { ProjectsErrors.SameParametersAreEmpty });

        var command = GetVersionQueryRequest.Create(serverSidePort, apiVersionId, userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(GetVersion)} finished", cancellationToken);
        return result.Match(ret => Results.Text(ret, "text/plain", Encoding.UTF8), Results.BadRequest);
    }

    private static async Task<IResult> RemoveProjectService(string projectName, string? serviceName,
        string environmentName, ISender mediator, IMessagesDataManager messagesDataManager, HttpRequest httpRequest,
        CancellationToken cancellationToken)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(RemoveProjectService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(RemoveProjectServiceCommandHandler)} from {nameof(RemoveProjectService)}");

        var command = RemoveProjectServiceCommandRequest.Create(projectName, serviceName, environmentName, userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(RemoveProjectService)} finished", cancellationToken);
        return result.Match(_ => Results.Ok(), Results.BadRequest);
    }
}