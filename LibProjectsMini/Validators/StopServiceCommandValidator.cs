using FluentValidation;
using LibProjectsMini.CommandRequests;

namespace LibProjectsMini.Validators;

// ReSharper disable once UnusedType.Global
public sealed class StopServiceCommandValidator : AbstractValidator<StopServiceCommandRequest>
{
    public StopServiceCommandValidator()
    {
        RuleFor(x => x.ServiceName).FileName();
    }
}