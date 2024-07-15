﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApiContracts.Errors;
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

    public void InstallServices(WebApplicationBuilder builder, string[] args, Dictionary<string, string> parameters)
    {
    }

    public void UseServices(WebApplication app)
    {
        //.RequireAuthorization(); იყო //.AddEndpointFilter<ApiKeysChecker>();

        //Console.WriteLine("InstallServices.UseServices Started");
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

    // POST api/projects/stop/{projectName}/{environmentName}
    private static async Task<IResult> StopService([FromRoute] string projectName, [FromRoute] string environmentName,
        HttpRequest httpRequest, IMediator mediator, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(StopService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(StopServiceCommandHandler)} from {nameof(StopService)}");

        var command = StopServiceCommandRequest.Create(projectName, environmentName, userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(StopService)} started", cancellationToken);
        return result.Match(_ => Results.Ok(), Results.BadRequest);
    }

    // POST api/projects/start/{projectName}/{environmentName}
    private static async Task<IResult> StartService([FromRoute] string projectName, [FromRoute] string environmentName,
        HttpRequest httpRequest, IMediator mediator, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(StartService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(StartServiceCommandHandler)} from {nameof(StartService)}");

        var command = StartServiceCommandRequest.Create(projectName, environmentName, userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(StartService)} started", cancellationToken);
        return result.Match(_ => Results.Ok(), Results.BadRequest);
    }

    // POST api/projects/removeprojectservice/{projectName}/{environmentName}
    private static async Task<IResult> RemoveProjectService([FromRoute] string projectName,
        [FromRoute] string environmentName, [FromRoute] bool isService,
        HttpRequest httpRequest, IMediator mediator, IMessagesDataManager messagesDataManager,
        CancellationToken cancellationToken)
    {
        var userName = httpRequest.HttpContext.User.Identity?.Name;
        await messagesDataManager.SendMessage(userName, $"{nameof(RemoveProjectService)} started", cancellationToken);
        Debug.WriteLine($"Call {nameof(RemoveProjectServiceCommandHandler)} from {nameof(RemoveProjectService)}");

        var command = RemoveProjectServiceCommandRequest.Create(projectName, environmentName, isService, userName);
        var result = await mediator.Send(command, cancellationToken);

        await messagesDataManager.SendMessage(userName, $"{nameof(RemoveProjectService)} finished", cancellationToken);
        return result.Match(_ => Results.Ok(), Results.BadRequest);
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
}