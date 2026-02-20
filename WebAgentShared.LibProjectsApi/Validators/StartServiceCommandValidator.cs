using FluentValidation;
using WebAgentShared.LibProjectsApi.CommandRequests;

namespace WebAgentShared.LibProjectsApi.Validators;

// ReSharper disable once UnusedType.Global
public sealed class StartServiceCommandValidator : AbstractValidator<StartServiceRequestCommand>
{
    public StartServiceCommandValidator()
    {
        RuleFor(x => x.ProjectName).FileName();
        RuleFor(x => x.EnvironmentName).Name();
    }
}
