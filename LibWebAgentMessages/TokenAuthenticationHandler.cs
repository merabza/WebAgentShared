﻿using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ApiToolsShared;
using ApiToolsShared.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// ReSharper disable ConvertToPrimaryConstructor

namespace LibWebAgentMessages;

//https://dejanstojanovic.net/aspnet/2021/december/supporting-multiple-authentication-schemes-in-aspnet-core-webapi/
public class TokenAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    public TokenAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory loggerFactory, UrlEncoder encoder, IConfiguration configuration) : base(
        options, loggerFactory, encoder)
    {
        _logger = loggerFactory.CreateLogger<ApiKeysChecker>();
        _configuration = configuration;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Request.HttpContext.User.Identity is null)
            return await Task.FromResult(AuthenticateResult.NoResult());

        if (Request.HttpContext.User.Identity.IsAuthenticated)
            return await Task.FromResult(AuthenticateResult.NoResult());

        var apiKey = Request.Query["ApiKey"].ToString();
        var remoteAddress = Request.HttpContext.Connection.RemoteIpAddress;

        if (remoteAddress is null)
            return await Task.FromResult(AuthenticateResult.NoResult());

        var remoteIpAddress = remoteAddress.MapToIPv4().ToString();

        if (!Check(apiKey, remoteIpAddress))
            return await Task.FromResult(AuthenticateResult.NoResult());

        //var headerKeyValue = Request.Headers.SingleOrDefault(h => h.Key.Equals(HeaderKeyNames.ApiKeyAuthenticationKey, StringComparison.InvariantCultureIgnoreCase)).Value.SingleOrDefault();

        //if (string.IsNullOrEmpty(headerKeyValue) || !headerKeyValue.Equals(_authenticationConfig.Key, StringComparison.InvariantCultureIgnoreCase))
        //    return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));

        var claims = new Claim[] { new(ClaimTypes.Name, remoteIpAddress) };
        var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(claimsIdentity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        var user = new GenericPrincipal(claimsIdentity,
            claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray());
        //var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        Request.HttpContext.User = user;

        return await Task.FromResult(AuthenticateResult.Success(ticket));
    }

    private bool Check(string? apiKey, string remoteIpAddress)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            return false;

        var apiKeys = ApiKeysDomain.Create(_configuration, _logger);

        //_logger.LogInformation($"View Api Keys. count is - {apiKeys.ApiKeys.Count}");
        //foreach (ApiKeyAndRemoteIpAddressDomain key in apiKeys.ApiKeys)
        //{
        //    _logger.LogInformation($"RemoteIpAddress is - {key.RemoteIpAddress}");
        //    _logger.LogInformation($"ApiKey is - {key.ApiKey}");
        //}
        //_logger.LogInformation("View Api Keys Finished");

        var ak = apiKeys.AppSettingsByApiKey(apiKey, remoteIpAddress);


        if (ak != null)
            return true;
        _logger.LogError("RemoteIpAddress is - {remoteIpAddress}", remoteIpAddress);
        _logger.LogError("API Key is invalid - {apiKey}", apiKey);
        return false;
    }
}