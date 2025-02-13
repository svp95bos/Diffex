using Microsoft.Extensions.DependencyInjection;

namespace Diffex;

/// <summary>
/// Provides extension methods for performing and configuring differences between objects.
/// </summary>
public static class DifferExtensions
{
    /// <summary>
    /// Computes the differences between two objects of the same type.
    /// </summary>
    /// <typeparam name="T">The type of the objects to compare.</typeparam>
    /// <typeparam name="TOutput">The type of the output produced by the formatter.</typeparam>
    /// <param name="first">The first object to compare.</param>
    /// <param name="second">The second object to compare.</param>
    /// <param name="formatter">An optional formatter function to format the list of property differences.</param>
    /// <returns>The formatted differences between the two objects.</returns>
    public static TOutput Diff<T, TOutput>(this T first, T second, Func<List<PropertyDifference>, TOutput> formatter = null)
    {
        return formatter == null
            ? new Diffex<T, TOutput>().Diff(first, second)
            : new Diffex<T, TOutput>(formatter).Diff(first, second);
    }

    /// <summary>
    /// Computes the differences between two objects of the same type and returns the result as a string.
    /// </summary>
    /// <typeparam name="T">The type of the objects to compare.</typeparam>
    /// <param name="first">The first object to compare.</param>
    /// <param name="second">The second object to compare.</param>
    /// <returns>A string representation of the differences between the two objects.</returns>
    public static string Diff<T>(this T first, T second) => new Diffex<T, string>().Diff(first, second);

    /// <summary>
    /// Adds a Diffster service to the specified IServiceCollection.
    /// </summary>
    /// <typeparam name="T">The type of the objects to compare.</typeparam>
    /// <typeparam name="TOutput">The type of the output produced by the formatter.</typeparam>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    /// <param name="formatter">A formatter function to format the list of property differences.</param>
    /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the formatter is null.</exception>
    public static IServiceCollection AddDiffster<T, TOutput>(this IServiceCollection services, Func<List<PropertyDifference>, TOutput> formatter)
    {
        ArgumentNullException.ThrowIfNull(formatter);

        services.AddSingleton(provider => new Diffex<T, TOutput>(formatter));
        return services;
    }
}
