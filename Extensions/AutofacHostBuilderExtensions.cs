using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Chat.Extensions
{
    public static class AutofacHostBuilderExtensions
    {
        public static IHostBuilder UseAutofacServiceProvider(this IHostBuilder hostBuilder)
        {
            var providerFactory = new AutofacServiceProviderFactory();

            return hostBuilder.UseServiceProviderFactory(providerFactory);
        }
    }
}
