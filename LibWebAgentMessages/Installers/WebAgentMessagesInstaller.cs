﻿using LibWebAgentMessages.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using WebInstallers;

namespace LibWebAgentMessages.Installers;

// ReSharper disable once UnusedType.Global
public sealed class WebAgentMessagesInstaller : IInstaller
{
    public int InstallPriority => 30;
    public int ServiceUsePriority => 30;

    public void InstallServices(WebApplicationBuilder builder, string[] args)
    {
        //Console.WriteLine("WebAgentMessagesInstaller.InstallServices Started");

        //builder.Services.AddSingleton<IProgressDataManager, ProgressDataManager>();
        builder.Services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandler>();
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("CustomHubAuthorizatioPolicy",
                policy => { policy.Requirements.Add(new CustomAuthorizationRequirement()); });
        });
        builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
        builder.Services.AddSignalR().AddJsonProtocol(options =>
        {
            options.PayloadSerializerOptions.PropertyNamingPolicy = null;
        }).AddHubOptions<MessagesHub>(options => { options.EnableDetailedErrors = true; });

        //Console.WriteLine("WebAgentMessagesInstaller.InstallServices Finished");
    }

    public void UseServices(WebApplication app)
    {
        app.UseAuthorization();
    }
}