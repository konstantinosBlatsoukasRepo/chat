using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Chat.Extensions
{
    public static class LoggingHostBuilderExtensions
    {
        private const string LoggingSection = "Logging";
        private const string FileLoggingSection = "File";
        private const string FileLoggingEnabledSection = "Enabled";

        public static IHostBuilder ConfigureDefaultLogging(
            this IHostBuilder hostBuilder, string configurationSection = LoggingSection)
        {
            return hostBuilder.ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                var loggingConfiguration = hostingContext.Configuration.GetSection(configurationSection);
                loggingBuilder.AddConfiguration(loggingConfiguration);
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();

                var fileConfiguration = loggingConfiguration.GetSection(FileLoggingSection);
                var fileEnabledSection = fileConfiguration?.GetSection(FileLoggingEnabledSection);
                if (!bool.TryParse(fileEnabledSection?.Value, out var enabled) || !enabled)
                {
                    return;
                }
            });
        }
    }
}
