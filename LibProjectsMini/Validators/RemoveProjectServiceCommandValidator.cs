using FluentValidation;
using LibProjectsMini.CommandRequests;

namespace LibProjectsMini.Validators;

// ReSharper disable once UnusedType.Global
public sealed class RemoveProjectServiceCommandValidator : AbstractValidator<RemoveProjectServiceCommandRequest>
{
    public RemoveProjectServiceCommandValidator()
    {
        RuleFor(x => x.ServiceName).FileName();
        RuleFor(x => x.ProjectName).FileName();
    }
}