using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ApiContracts.Errors;
using ApiKeyIdentity;
using LibProjectsApi.CommandRequests;
using LibProjectsApi.Handlers;
using LibProjectsApi.Mappers;
using LibProjectsApi.QueryRequests;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SystemToolsShared;
using SystemToolsShared.Errors;
using WebAgentProjectsApiContracts.V1.Requests;
using WebAgentProjectsApiContracts.V1.Routes;
using WebInstallers;

namespace LibProjectsApi.Endpoints.V1;

// ReSharper disable once UnusedType.Global
public sealed class ProjectsEndpoints : IInstaller
{
    public int InstallPriority => 50;
    public int ServiceUsePriority => 50;

    public bool InstallServices(WebApplicationBuilder builder, bool debugMode, string[] args,
        Dictionary<string, string> parameters)
    {
        return true;
    }

    public bool UseServices(WebApplication app, bool debugMode)
    {
        if (debugMode)
            Console.WriteLine($"{GetType().Name}.{nameof(UseServices)} Started");

        var group = app.MapGroup(ProjectsApiRoutes.ApiBase + ProjectsApiRoutes.Projects.ProjectBase)
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
            Console.WriteLine($"{GetType().Name}.{nameof(UseServices)} Finished");

        return true;
    }

    // POST api/projects/updatesettings
    private static async Task<Results<Ok, BadRequest<IEnumerable<Err>>>> UpdateSettings(
        [FromBody] UpdateSettingsRequest? request, ICurrentUserByApiKey currentUserByApiKey, IMediator mediator,
        IMessagesDataManager messagesDataManager, CancellationToken cancellationToken = default)
    {
        var userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateSettings)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(UpdateSettingsCommandHandler)} from {nameof(UpdateSettings)}");

        if (request is null)
            return TypedResults.BadRequest(Err.Create(ApiErrors.RequestIsEmpty));

        var command = request.AdaptTo(userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateSettings)} finished", cancellationToken);
        return result.Match<Results<Ok, BadRequest<IEnumerable<Err>>>>(_ => TypedResults.Ok(),
            errors => TypedResults.BadRequest(errors));
    }

    // POST api/projects/update
    private static async Task<Results<Ok<string>, BadRequest<IEnumerable<Err>>>> Update(
        [FromBody] ProjectUpdateRequest? request, ICurrentUserByApiKey currentUserByApiKey, IMediator mediator,
        IMessagesDataManager messagesDataManager, CancellationToken cancellationToken = default)
    {
        var userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(Update)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(ProjectUpdateCommandHandler)} from {nameof(Update)}");

        if (request is null)
            return TypedResults.BadRequest(Err.Create(ApiErrors.RequestIsEmpty));
        var command = request.AdaptTo(userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(Update)} finished", cancellationToken);
        return result.Match<Results<Ok<string>, BadRequest<IEnumerable<Err>>>>(res => TypedResults.Ok(res),
            errors => TypedResults.BadRequest(errors));
    }

    // POST api/projects/updateservice
    private static async Task<Results<Ok<string>, BadRequest<IEnumerable<Err>>>> UpdateService(
        [FromBody] UpdateServiceRequest? request, ICurrentUserByApiKey currentUserByApiKey, IMediator mediator,
        IMessagesDataManager messagesDataManager, CancellationToken cancellationToken = default)
    {
        var userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(UpdateServiceCommandHandler)} from {nameof(UpdateService)}");

        if (request is null)
            return TypedResults.BadRequest(Err.Create(ApiErrors.RequestIsEmpty));

        var command = request.AdaptTo(userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateService)} finished", cancellationToken);
        return result.Match<Results<Ok<string>, BadRequest<IEnumerable<Err>>>>(res => TypedResults.Ok(res),
            errors => TypedResults.BadRequest(errors));
    }

    // POST api/projects/stop/{projectName}/{environmentName}
    private static async Task<Results<Ok, BadRequest<IEnumerable<Err>>>> StopService([FromRoute] string projectName,
        [FromRoute] string environmentName, ICurrentUserByApiKey currentUserByApiKey, IMediator mediator,
        IMessagesDataManager messagesDataManager, CancellationToken cancellationToken = default)
    {
        var userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(StopService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(StopServiceCommandHandler)} from {nameof(StopService)}");

        var command = StopServiceCommandRequest.Create(projectName, environmentName, userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(StopService)} finished", cancellationToken);
        return result.Match<Results<Ok, BadRequest<IEnumerable<Err>>>>(_ => TypedResults.Ok(),
            errors => TypedResults.BadRequest(errors));
    }

    // POST api/projects/start/{projectName}/{environmentName}
    private static async Task<Results<Ok, BadRequest<IEnumerable<Err>>>> StartService([FromRoute] string projectName,
        [FromRoute] string environmentName, ICurrentUserByApiKey currentUserByApiKey, IMediator mediator,
        IMessagesDataManager messagesDataManager, CancellationToken cancellationToken = default)
    {
        var userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(StartService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(StartServiceCommandHandler)} from {nameof(StartService)}");

        var command = StartServiceCommandRequest.Create(projectName, environmentName, userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(StartService)} finished", cancellationToken);
        return result.Match<Results<Ok, BadRequest<IEnumerable<Err>>>>(_ => TypedResults.Ok(),
            errors => TypedResults.BadRequest(errors));
    }

    // POST api/projects/removeprojectservice/{projectName}/{environmentName}
    private static async Task<Results<Ok, BadRequest<IEnumerable<Err>>>> RemoveProjectService(
        [FromRoute] string projectName, [FromRoute] string environmentName, [FromRoute] bool isService,
        ICurrentUserByApiKey currentUserByApiKey, IMediator mediator, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken = default)
    {
        var userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(RemoveProjectService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(RemoveProjectServiceCommandHandler)} from {nameof(RemoveProjectService)}");

        var command = RemoveProjectServiceRequestCommand.Create(projectName, environmentName, isService, userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(RemoveProjectService)} finished", cancellationToken);
        return result.Match<Results<Ok, BadRequest<IEnumerable<Err>>>>(_ => TypedResults.Ok(),
            errors => TypedResults.BadRequest(errors));
    }

    // GET api/projects/getappsettingsversion/{serverSidePort}/{apiVersionId}
    private static async Task<Results<Ok<string>, BadRequest<IEnumerable<Err>>>> GetAppSettingsVersion(
        [FromRoute] int serverSidePort, [FromRoute] string apiVersionId, ICurrentUserByApiKey currentUserByApiKey,
        IMediator mediator, IMessagesDataManager messagesDataManager, CancellationToken cancellationToken = default)
    {
        var userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(GetAppSettingsVersion)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(GetAppSettingsVersionQueryHandler)} from {nameof(GetAppSettingsVersion)}");

        if (string.IsNullOrWhiteSpace(apiVersionId) || serverSidePort == 0)
            return TypedResults.BadRequest(Err.Create(ProjectsErrors.SameParametersAreEmpty));

        var command = GetAppSettingsVersionQueryRequest.Create(serverSidePort, apiVersionId, userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(GetAppSettingsVersion)} finished", cancellationToken);
        return result.Match<Results<Ok<string>, BadRequest<IEnumerable<Err>>>>(res => TypedResults.Ok(res),
            errors => TypedResults.BadRequest(errors));
    }

    // POST api/projects/getversion/{serverSidePort}/{apiVersionId}
    private static async Task<Results<Ok<string>, BadRequest<IEnumerable<Err>>>> GetVersion(
        [FromRoute] int serverSidePort, [FromRoute] string apiVersionId, ICurrentUserByApiKey currentUserByApiKey,
        IMediator mediator, IMessagesDataManager messagesDataManager, CancellationToken cancellationToken = default)
    {
        var userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(GetVersion)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(GetVersionQueryHandler)} from {nameof(GetVersion)}");

        if (string.IsNullOrWhiteSpace(apiVersionId) || serverSidePort == 0)
            return TypedResults.BadRequest(Err.Create(ProjectsErrors.SameParametersAreEmpty));

        var command = GetVersionQueryRequest.Create(serverSidePort, apiVersionId, userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(GetVersion)} finished", cancellationToken);
        return result.Match<Results<Ok<string>, BadRequest<IEnumerable<Err>>>>(res => TypedResults.Ok(res),
            errors => TypedResults.BadRequest(errors));
    }
}