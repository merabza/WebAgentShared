//using ApiToolsShared;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

//namespace LibProjectsMini.Filters;

////minimal API-სთვის ამ ატრიბუტმა არ იმუშავა. 
////ამის მაგვარი ფილტრის გამოყენება შესაძლებელი იქნება 7.0 დოტნეტის გამოსვლის მერე,
////სადაც ფილტრები გათვალისწინებულია
//[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//internal sealed class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
//{

//    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//    {
//        var apiKey = context.HttpContext.Request.Query["ApiKey"].ToString();
//        var remoteAddress = context.HttpContext.Connection.RemoteIpAddress;

//        if (remoteAddress is null)
//        {
//            context.Result = new UnauthorizedResult();
//            return;
//        }

//        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
//        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ApiKeyAuthAttribute>>();

//        var apiKeysChecker = new ApiKeysChecker(logger, configuration);

//        if (!apiKeysChecker.Check(apiKey, remoteAddress.MapToIPv4().ToString()))
//        {
//            context.Result = new UnauthorizedResult();
//            return;
//        }

//        await next();
//    }
//}

