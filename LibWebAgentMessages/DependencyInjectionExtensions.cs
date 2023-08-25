using Microsoft.AspNetCore.Authentication;

namespace LibWebAgentMessages;

public static class DependencyInjectionExtensions
{
    public static AuthenticationBuilder AddApiKeyAuthenticationSchema(this AuthenticationBuilder authentication)
    {
        authentication.AddScheme<AuthenticationSchemeOptions, TokenAuthenticationHandler>(
            AuthenticationSchemaNames.ApiKeyAuthentication, o => { });
        return authentication;
    }
}