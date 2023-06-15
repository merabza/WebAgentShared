using FluentValidation;
using LibProjectsMini.CommandRequests;

namespace LibProjectsMini.Validators;

// ReSharper disable once UnusedType.Global
public sealed class StartServiceCommandValidator : AbstractValidator<StartServiceCommandRequest>
{
    public StartServiceCommandValidator()
    {
        RuleFor(x => x.ServiceName).FileName();
    }
}