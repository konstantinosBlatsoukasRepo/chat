using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Chat.Extensions
{
    public static class ConfigurationHostBuilderExtensions
    {

        private const string EnvironmentalPrefix = "MY_ENV_PREFIX_";
        private static readonly Dictionary<string, string> MyCustomMappings = new Dictionary<string, string>();


        // reads configuration values regarding the host
        // those values are usually are in environmental variables or some kind of mapping passed through the CLI
        public static IHostBuilder ConfigureDefaultHostConfiguration(this IHostBuilder hostBuilder,
            string[] commandLineArgs = null)
        {
            return hostBuilder.ConfigureHostConfiguration(configurationBuilder =>
            {
                // adds to configuration environmental variables that are starting with the specified prefix
                configurationBuilder.AddEnvironmentVariables(prefix: EnvironmentalPrefix);

                if (commandLineArgs != null)
                {
                    configurationBuilder.AddCommandLine(commandLineArgs, MyCustomMappings);
                }
            });
        }

        // reads configuration values regarding the app
        // those configurations usually are included in json files (i.e. appsettings.json)
        public static IHostBuilder ConfigureDefaultAppConfiguration(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                var env = context.HostingEnvironment;

                configurationBuilder.SetBasePath(env.ContentRootPath);
                configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                configurationBuilder.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                configurationBuilder.AddConfiguration(context.Configuration);
            });
        }

    }
}
