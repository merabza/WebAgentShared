using System.Reflection;

namespace LibProjectsMini;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}