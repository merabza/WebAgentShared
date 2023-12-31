﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using WebAgentMessagesContracts.V1.Routes;
using WebInstallers;

namespace LibWebAgentMessages.Endpoints.V1;

// ReSharper disable once UnusedType.Global
public sealed class WebAgentMessagesEndpoints : IInstaller
{
    public int InstallPriority => 70;

    public int ServiceUsePriority => 70;

    public void InstallServices(WebApplicationBuilder builder, string[] args)
    {
    }

    public void UseServices(WebApplication app)
    {
        //Console.WriteLine("WebAgentMessagesEndpoints.UseServices Started");

        app.MapHub<MessagesHub>(MessagesRoutes.Messages.MessagesRoute,
            options => { options.Transports = HttpTransportType.LongPolling; }).RequireAuthorization();

        //Console.WriteLine("WebAgentMessagesEndpoints.UseServices Finished");
    }
}