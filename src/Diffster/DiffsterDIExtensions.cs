using System.Runtime.Serialization;

using Microsoft.Extensions.DependencyInjection;

namespace Diffster
{
    public static class DiffsterExtensions
    {
        public static IServiceCollection AddDiffster<T, TOutput>(
    this IServiceCollection services,
    Func<IServiceProvider, T, IDiffFormatter<TOutput>> formatterFactory,
    Action<IDiffFormatter<TOutput>> configureFormatter = null)
        {
            //if (formatterFactory == null)
            //{
            //    throw new ArgumentNullException(nameof(formatterFactory), "A formatter factory must be provided.");
            //}

            //services.AddTransient(provider =>
            //{
            //    var formatter = formatterFactory(provider);
            //    configureFormatter?.Invoke(formatter);
            //    return new Diffster<T, IDiffFormatter<TOutput>>(IDiffFormatter<TOutput>);
            //});

            return services;
        }
    }
}
