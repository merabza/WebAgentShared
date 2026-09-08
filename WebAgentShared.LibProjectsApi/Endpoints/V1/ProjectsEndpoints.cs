using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Serilog;
using SystemTools.ApiContracts.Errors;
using SystemTools.Application.Abstractions.Messaging;
using SystemTools.SharedKernel;
using SystemTools.SystemToolsShared;
using WebAgentContracts.WebAgentProjectsApiContracts.V1.Requests;
using WebAgentContracts.WebAgentProjectsApiContracts.V1.Routes;
using WebAgentShared.LibProjectsApi.CommandRequests;
using WebAgentShared.LibProjectsApi.Handlers;
using WebAgentShared.LibProjectsApi.Mappers;
using WebAgentShared.LibProjectsApi.QueryRequests;
using WebSystemTools.ApiKeyIdentity;

namespace WebAgentShared.LibProjectsApi.Endpoints.V1;

// ReSharper disable once UnusedType.Global
public static class ProjectsEndpoints
{
    public static bool UseProjectsEndpoints(this IEndpointRouteBuilder endpoints, ILogger? debugLogger)
    {
        debugLogger?.Information("{MethodName} Started", nameof(UseProjectsEndpoints));

        RouteGroupBuilder group = endpoints.MapGroup(ProjectsApiRoutes.ApiBase + ProjectsApiRoutes.Projects.ProjectBase)
            .RequireAuthorization();

        group.MapGet(ProjectsApiRoutes.Projects.GetAppSettingsVersion, GetAppSettingsVersion);
        group.MapGet(ProjectsApiRoutes.Projects.GetVersion, GetVersion);
        group.MapDelete(ProjectsApiRoutes.Projects.RemoveProjectService, RemoveProjectService);
        group.MapPost(ProjectsApiRoutes.Projects.StartService, StartService);
        group.MapPost(ProjectsApiRoutes.Projects.StopService, StopService);
        group.MapPost(ProjectsApiRoutes.Projects.Update, Update);
        group.MapPost(ProjectsApiRoutes.Projects.UpdateService, UpdateService);
        group.MapPost(ProjectsApiRoutes.Projects.UpdateSettings, UpdateSettings);

        debugLogger?.Information("{MethodName} Finished", nameof(UseProjectsEndpoints));

        return true;
    }

    // POST api/projects/updatesettings
    private static async Task<Results<Ok, BadRequest<Error[]>>> UpdateSettings(
        [FromBody] UpdateSettingsRequest? request, ICurrentUserByApiKey currentUserByApiKey,
        ICommandHandler<UpdateSettingsRequestCommand> handler, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken = default)
    {
        string userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateSettings)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(UpdateSettingsCommandHandler)} from {nameof(UpdateSettings)}");

        if (request is null)
        {
            return TypedResults.BadRequest(ApiErrors.RequestIsEmpty.ToErrorArray());
        }

        UpdateSettingsRequestCommand command = request.AdaptTo(userName);
        Result result = await handler.Handle(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateSettings)} finished", cancellationToken);
        return result.Match<Results<Ok, BadRequest<Error[]>>>(() => TypedResults.Ok(),
            failure => TypedResults.BadRequest(failure.Error.ToErrorArray()));
    }

    // POST api/projects/update
    private static async Task<Results<Ok<string>, BadRequest<Error[]>>> Update([FromBody] ProjectUpdateRequest? request,
        ICurrentUserByApiKey currentUserByApiKey, ICommandHandler<ProjectUpdateRequestCommand, string> handler,
        IMessagesDataManager messagesDataManager, CancellationToken cancellationToken = default)
    {
        string userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(Update)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(ProjectUpdateCommandHandler)} from {nameof(Update)}");

        if (request is null)
        {
            return TypedResults.BadRequest(ApiErrors.RequestIsEmpty.ToErrorArray());
        }

        ProjectUpdateRequestCommand command = request.AdaptTo(userName);
        Result<string> result = await handler.Handle(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(Update)} finished", cancellationToken);
        return result.Match<string, Results<Ok<string>, BadRequest<Error[]>>>(res => TypedResults.Ok(res),
            failure => TypedResults.BadRequest(failure.Error.ToErrorArray()));
    }

    // POST api/projects/updateservice
    private static async Task<Results<Ok<string>, BadRequest<Error[]>>> UpdateService(
        [FromBody] UpdateServiceRequest? request, ICurrentUserByApiKey currentUserByApiKey,
        ICommandHandler<UpdateServiceRequestCommand, string> handler, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken = default)
    {
        string userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(UpdateServiceCommandHandler)} from {nameof(UpdateService)}");

        if (request is null)
        {
            return TypedResults.BadRequest(ApiErrors.RequestIsEmpty.ToErrorArray());
        }

        UpdateServiceRequestCommand command = request.AdaptTo(userName);
        Result<string> result = await handler.Handle(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(UpdateService)} finished", cancellationToken);
        return result.Match<string, Results<Ok<string>, BadRequest<Error[]>>>(res => TypedResults.Ok(res),
            failure => TypedResults.BadRequest(failure.Error.ToErrorArray()));
    }

    // POST api/projects/stop/{projectName}/{environmentName}
    private static async Task<Results<Ok, BadRequest<Error[]>>> StopService([FromRoute] string projectName,
        [FromRoute] string environmentName, ICurrentUserByApiKey currentUserByApiKey,
        ICommandHandler<StopServiceRequestCommand> handler, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken = default)
    {
        string userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(StopService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(StopServiceCommandHandler)} from {nameof(StopService)}");

        var command = StopServiceRequestCommand.Create(projectName, environmentName, userName);
        Result result = await handler.Handle(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(StopService)} finished", cancellationToken);
        return result.Match<Results<Ok, BadRequest<Error[]>>>(() => TypedResults.Ok(),
            failure => TypedResults.BadRequest(failure.Error.ToErrorArray()));
    }

    // POST api/projects/start/{projectName}/{environmentName}
    private static async Task<Results<Ok, BadRequest<Error[]>>> StartService([FromRoute] string projectName,
        [FromRoute] string environmentName, ICurrentUserByApiKey currentUserByApiKey,
        ICommandHandler<StartServiceRequestCommand> handler, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken = default)
    {
        string userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(StartService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(StartServiceCommandHandler)} from {nameof(StartService)}");

        var command = StartServiceRequestCommand.Create(projectName, environmentName, userName);
        Result result = await handler.Handle(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(StartService)} finished", cancellationToken);
        return result.Match<Results<Ok, BadRequest<Error[]>>>(() => TypedResults.Ok(),
            failure => TypedResults.BadRequest(failure.Error.ToErrorArray()));
    }

    // POST api/projects/removeprojectservice/{projectName}/{environmentName}
    private static async Task<Results<Ok, BadRequest<Error[]>>> RemoveProjectService([FromRoute] string projectName,
        [FromRoute] string environmentName, [FromRoute] bool isService, ICurrentUserByApiKey currentUserByApiKey,
        ICommandHandler<RemoveProjectServiceRequestCommand> handler, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken = default)
    {
        string userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(RemoveProjectService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(RemoveProjectServiceCommandHandler)} from {nameof(RemoveProjectService)}");

        var command = RemoveProjectServiceRequestCommand.Create(projectName, environmentName, isService, userName);
        Result result = await handler.Handle(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(RemoveProjectService)} finished", cancellationToken);
        return result.Match<Results<Ok, BadRequest<Error[]>>>(() => TypedResults.Ok(),
            failure => TypedResults.BadRequest(failure.Error.ToErrorArray()));
    }

    // GET api/projects/getappsettingsversion/{serverSidePort}/{apiVersionId}
    private static async Task<Results<ContentHttpResult, BadRequest<Error[]>>> GetAppSettingsVersion(
        [FromRoute] int serverSidePort, [FromRoute] string apiVersionId, ICurrentUserByApiKey currentUserByApiKey,
        IQueryHandler<GetAppSettingsVersionRequestQuery, string> handler, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken = default)
    {
        string userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(GetAppSettingsVersion)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(GetAppSettingsVersionQueryHandler)} from {nameof(GetAppSettingsVersion)}");

        if (string.IsNullOrWhiteSpace(apiVersionId) || serverSidePort == 0)
        {
            return TypedResults.BadRequest(ProjectsErrors.SameParametersAreEmpty.ToErrorArray());
        }

        var query = GetAppSettingsVersionRequestQuery.Create(serverSidePort, apiVersionId, userName);
        Result<string> result = await handler.Handle(query, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(GetAppSettingsVersion)} finished", cancellationToken);

        return result.Match<string, Results<ContentHttpResult, BadRequest<Error[]>>>(
            ret => TypedResults.Text(ret, "text/plain", Encoding.UTF8),
            failure => TypedResults.BadRequest(failure.Error.ToErrorArray()));
    }

    // POST api/projects/getversion/{serverSidePort}/{apiVersionId}
    private static async Task<Results<ContentHttpResult, BadRequest<Error[]>>> GetVersion(
        [FromRoute] int serverSidePort, [FromRoute] string apiVersionId, ICurrentUserByApiKey currentUserByApiKey,
        IQueryHandler<GetVersionRequestQuery, string> handler, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken = default)
    {
        string userName = currentUserByApiKey.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(GetVersion)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(GetVersionQueryHandler)} from {nameof(GetVersion)}");

        if (string.IsNullOrWhiteSpace(apiVersionId) || serverSidePort == 0)
        {
            return TypedResults.BadRequest(ProjectsErrors.SameParametersAreEmpty.ToErrorArray());
        }

        var query = GetVersionRequestQuery.Create(serverSidePort, apiVersionId, userName);
        Result<string> result = await handler.Handle(query, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(GetVersion)} finished", cancellationToken);

        return result.Match<string, Results<ContentHttpResult, BadRequest<Error[]>>>(
            ret => TypedResults.Text(ret, "text/plain", Encoding.UTF8),
            failure => TypedResults.BadRequest(failure.Error.ToErrorArray()));
    }
}
