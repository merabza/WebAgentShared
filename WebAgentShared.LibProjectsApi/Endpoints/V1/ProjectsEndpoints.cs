using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApiKeyIdentity;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SystemTools.ApiContracts.Errors;
using SystemTools.SystemToolsShared;
using SystemTools.SystemToolsShared.Errors;
using WebAgentContracts.WebAgentProjectsApiContracts.V1.Requests;
using WebAgentContracts.WebAgentProjectsApiContracts.V1.Routes;
using WebAgentShared.LibProjectsApi.CommandRequests;
using WebAgentShared.LibProjectsApi.Handlers;
using WebAgentShared.LibProjectsApi.Mappers;
using WebAgentShared.LibProjectsApi.QueryRequests;

namespace WebAgentShared.LibProjectsApi.Endpoints.V1;

// ReSharper disable once UnusedType.Global
public static class ProjectsEndpoints
{
    public static bool UseProjectsEndpoints(this IEndpointRouteBuilder endpoints, bool debugMode)
    {
        if (debugMode)
        {
            Console.WriteLine($"{nameof(UseProjectsEndpoints)} Started");
        }

        var group = endpoints.MapGroup(ProjectsApiRoutes.ApiBase + ProjectsApiRoutes.Projects.ProjectBase)
            .RequireAuthorization();

        group.MapGet(ProjectsApiRoutes.Projects.GetAppSettingsVersion, GetAppSettingsVersion);
        group.MapGet(ProjectsApiRoutes.Projects.GetVersion, GetVersion);
        group.MapDelete(ProjectsApiRoutes.Projects.RemoveProjectService, RemoveProjectService);
        group.MapPost(ProjectsApiRoutes.Projects.StartService, StartService);
        group.MapPost(ProjectsApiRoutes.Projects.StopService, StopService);
        group.MapPost(ProjectsApiRoutes.Projects.Update, Update);
        group.MapPost(ProjectsApiRoutes.Projects.UpdateService, UpdateService);
        group.MapPost(ProjectsApiRoutes.Projects.UpdateSettings, UpdateSettings);

        if (debugMode)
        {
            Console.WriteLine($"{nameof(UseProjectsEndpoints)} Finished");
        }

        return true;
    }

    // POST api/projects/updatesettings
    private static async Task<Results<Ok, BadRequest<Err[]>>> UpdateSettings([FromBody] UpdateSettingsRequest? request,
        ICurrentUserByApiKey currentUserByApiKey, IMediator mediator, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken = default)
    {
        var userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateSettings)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(UpdateSettingsCommandHandler)} from {nameof(UpdateSettings)}");

        if (request is null)
        {
            return TypedResults.BadRequest(Err.Create(ApiErrors.RequestIsEmpty));
        }

        var command = request.AdaptTo(userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateSettings)} finished", cancellationToken);
        return result.Match<Results<Ok, BadRequest<Err[]>>>(_ => TypedResults.Ok(),
            errors => TypedResults.BadRequest(errors));
    }

    // POST api/projects/update
    private static async Task<Results<Ok<string>, BadRequest<Err[]>>> Update([FromBody] ProjectUpdateRequest? request,
        ICurrentUserByApiKey currentUserByApiKey, IMediator mediator, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken = default)
    {
        var userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(Update)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(ProjectUpdateCommandHandler)} from {nameof(Update)}");

        if (request is null)
        {
            return TypedResults.BadRequest(Err.Create(ApiErrors.RequestIsEmpty));
        }

        var command = request.AdaptTo(userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(Update)} finished", cancellationToken);
        return result.Match<Results<Ok<string>, BadRequest<Err[]>>>(res => TypedResults.Ok(res),
            errors => TypedResults.BadRequest(errors));
    }

    // POST api/projects/updateservice
    private static async Task<Results<Ok<string>, BadRequest<Err[]>>> UpdateService(
        [FromBody] UpdateServiceRequest? request, ICurrentUserByApiKey currentUserByApiKey, IMediator mediator,
        IMessagesDataManager messagesDataManager, CancellationToken cancellationToken = default)
    {
        var userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(UpdateServiceCommandHandler)} from {nameof(UpdateService)}");

        if (request is null)
        {
            return TypedResults.BadRequest(Err.Create(ApiErrors.RequestIsEmpty));
        }

        var command = request.AdaptTo(userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateService)} finished", cancellationToken);
        return result.Match<Results<Ok<string>, BadRequest<Err[]>>>(res => TypedResults.Ok(res),
            errors => TypedResults.BadRequest(errors));
    }

    // POST api/projects/stop/{projectName}/{environmentName}
    private static async Task<Results<Ok, BadRequest<Err[]>>> StopService([FromRoute] string projectName,
        [FromRoute] string environmentName, ICurrentUserByApiKey currentUserByApiKey, IMediator mediator,
        IMessagesDataManager messagesDataManager, CancellationToken cancellationToken = default)
    {
        var userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(StopService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(StopServiceCommandHandler)} from {nameof(StopService)}");

        var command = StopServiceRequestCommand.Create(projectName, environmentName, userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(StopService)} finished", cancellationToken);
        return result.Match<Results<Ok, BadRequest<Err[]>>>(_ => TypedResults.Ok(),
            errors => TypedResults.BadRequest(errors));
    }

    // POST api/projects/start/{projectName}/{environmentName}
    private static async Task<Results<Ok, BadRequest<Err[]>>> StartService([FromRoute] string projectName,
        [FromRoute] string environmentName, ICurrentUserByApiKey currentUserByApiKey, IMediator mediator,
        IMessagesDataManager messagesDataManager, CancellationToken cancellationToken = default)
    {
        var userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(StartService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(StartServiceCommandHandler)} from {nameof(StartService)}");

        var command = StartServiceRequestCommand.Create(projectName, environmentName, userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(StartService)} finished", cancellationToken);
        return result.Match<Results<Ok, BadRequest<Err[]>>>(_ => TypedResults.Ok(),
            errors => TypedResults.BadRequest(errors));
    }

    // POST api/projects/removeprojectservice/{projectName}/{environmentName}
    private static async Task<Results<Ok, BadRequest<Err[]>>> RemoveProjectService([FromRoute] string projectName,
        [FromRoute] string environmentName, [FromRoute] bool isService, ICurrentUserByApiKey currentUserByApiKey,
        IMediator mediator, IMessagesDataManager messagesDataManager, CancellationToken cancellationToken = default)
    {
        var userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(RemoveProjectService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(RemoveProjectServiceCommandHandler)} from {nameof(RemoveProjectService)}");

        var command = RemoveProjectServiceRequestCommand.Create(projectName, environmentName, isService, userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(RemoveProjectService)} finished", cancellationToken);
        return result.Match<Results<Ok, BadRequest<Err[]>>>(_ => TypedResults.Ok(),
            errors => TypedResults.BadRequest(errors));
    }

    // GET api/projects/getappsettingsversion/{serverSidePort}/{apiVersionId}
    private static async Task<IResult> GetAppSettingsVersion([FromRoute] int serverSidePort,
        [FromRoute] string apiVersionId, ICurrentUserByApiKey currentUserByApiKey, IMediator mediator,
        IMessagesDataManager messagesDataManager, CancellationToken cancellationToken = default)
    {
        var userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(GetAppSettingsVersion)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(GetAppSettingsVersionQueryHandler)} from {nameof(GetAppSettingsVersion)}");

        if (string.IsNullOrWhiteSpace(apiVersionId) || serverSidePort == 0)
        {
            return TypedResults.BadRequest(Err.Create(ProjectsErrors.SameParametersAreEmpty));
        }

        var command = GetAppSettingsVersionRequestQuery.Create(serverSidePort, apiVersionId, userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(GetAppSettingsVersion)} finished", cancellationToken);

        return result.Match(ret => Results.Text(ret, "text/plain", Encoding.UTF8), Results.BadRequest);
    }

    // POST api/projects/getversion/{serverSidePort}/{apiVersionId}
    private static async Task<IResult> GetVersion([FromRoute] int serverSidePort, [FromRoute] string apiVersionId,
        ICurrentUserByApiKey currentUserByApiKey, IMediator mediator, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken = default)
    {
        var userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(GetVersion)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(GetVersionQueryHandler)} from {nameof(GetVersion)}");

        if (string.IsNullOrWhiteSpace(apiVersionId) || serverSidePort == 0)
        {
            return TypedResults.BadRequest(Err.Create(ProjectsErrors.SameParametersAreEmpty));
        }

        var command = GetVersionRequestQuery.Create(serverSidePort, apiVersionId, userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(GetVersion)} finished", cancellationToken);

        return result.Match(ret => Results.Text(ret, "text/plain", Encoding.UTF8), Results.BadRequest);
    }
}
