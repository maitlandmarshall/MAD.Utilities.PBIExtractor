using Hangfire;
using MAD.Utilities.PBIExtractor.Jobs;
using MAD.Utilities.PBIExtractor.Services;
using Microsoft.Extensions.DependencyInjection;
using MIFCore.Hangfire;
using MIFCore.Settings;

namespace MAD.Utilities.PBIExtractor
{
    public class Startup
    {        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIntegrationSettings<AppConfig>()
                .AddTransient<IAadAccessTokenProvider, AadAccessTokenProvider>()
                .AddTransient<PowerBIClientFactory>()
                .AddScoped<PBIDefinitionConsumer>();
        }

        public void PostConfigure(IRecurringJobManager recurringJobManager)
        {
            recurringJobManager.CreateRecurringJob<PBIDefinitionConsumer>(jobName: "PBIDefinitions", methodCall: y => y.ConsumeDefinitions(), cronSchedule: Cron.Daily(), triggerIfNeverExecuted: true);
        }
    }
}
