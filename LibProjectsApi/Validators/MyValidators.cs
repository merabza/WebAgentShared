using System.Runtime.InteropServices;
using FluentValidation;

namespace LibProjectsApi.Validators;

public static class MyValidators
{
    public static void FilePath<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        var rules = ruleBuilder.NotEmpty().MaximumLength(256);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            rules.Matches(@"^/|(/[\w-]+)+$");
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            rules.Matches(@"^(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.$]+)\\(?:[\w]+\\)*\w([\w.])+$");
    }

    public static void FileName<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        ruleBuilder.NotEmpty().Matches(@"^[\w\-.]+$").MaximumLength(256);
    }

    public static void Name<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        ruleBuilder.NotEmpty().Matches(@"^\w+$").MaximumLength(32);
    }

    public static void DateMask<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        ruleBuilder.NotEmpty().Matches(@"^[yMdHms]+$").MaximumLength(20);
    }

    public static void FileExtension<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        ruleBuilder.NotEmpty().Matches(@"^\.[\w\-]+$").MaximumLength(256);
    }
}