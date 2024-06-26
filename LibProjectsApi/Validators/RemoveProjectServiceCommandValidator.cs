﻿using FluentValidation;
using LibProjectsApi.CommandRequests;

namespace LibProjectsApi.Validators;

// ReSharper disable once UnusedType.Global
public sealed class RemoveProjectServiceCommandValidator : AbstractValidator<RemoveProjectServiceCommandRequest>
{
    public RemoveProjectServiceCommandValidator()
    {
        RuleFor(x => x.ProjectName).FileName();
        RuleFor(x => x.EnvironmentName).Name();
    }
}