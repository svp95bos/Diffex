using Microsoft.Extensions.DependencyInjection;

namespace Diffster;

public static class DifferExtensions
{
    public static TOutput Diff<T, TOutput>(this T first, T second, Func<List<PropertyDifference>, TOutput> formatter = null)
    {
        return formatter == null
            ? new Diffster<T, TOutput>().Diff(first, second)
            : new Diffster<T, TOutput>(formatter).Diff(first, second);
    }

    public static string Diff<T>(this T first, T second) => new Diffster<T, string>().Diff(first, second);

    public static IServiceCollection AddDiffster<T, TOutput>(this IServiceCollection services, Func<List<PropertyDifference>, TOutput> formatter)
    {
        ArgumentNullException.ThrowIfNull(formatter);

        services.AddSingleton<Diffster<T, TOutput>>(provider => new Diffster<T, TOutput>(formatter));
        return services;
    }
}

