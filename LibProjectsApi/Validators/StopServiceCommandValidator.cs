﻿using FluentValidation;
using LibProjectsApi.CommandRequests;

namespace LibProjectsApi.Validators;

// ReSharper disable once UnusedType.Global
public sealed class StopServiceCommandValidator : AbstractValidator<StopServiceRequestCommand>
{
    public StopServiceCommandValidator()
    {
        RuleFor(x => x.ProjectName).FileName();
        RuleFor(x => x.EnvironmentName).Name();
    }
}