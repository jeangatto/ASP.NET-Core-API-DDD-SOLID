using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SGP.Shared.Abstractions;

namespace SGP.Shared.Extensions;

public static class ServiceProviderExtensions
{
    public static TOptions GetOptions<TOptions>(this IServiceProvider serviceProvider)
        where TOptions : class, IAppOptions => serviceProvider.GetRequiredService<IOptions<TOptions>>().Value;
}
