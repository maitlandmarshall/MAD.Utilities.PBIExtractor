using Hangfire;
using MAD.Utilities.PBIExtractor.Api;
using MAD.Utilities.PBIExtractor.Jobs;
using Microsoft.Extensions.DependencyInjection;
using MIFCore.Hangfire;
using MIFCore.Settings;
using Newtonsoft.Json;
using Refit;

namespace MAD.Utilities.PBIExtractor
{
    public class Startup
    {
        private const string PowerBIApiUrl = "https://api.powerbi.com/v1.0";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIntegrationSettings<AppConfig>()
                .AddTransient<IAadAccessTokenProvider, AadAccessTokenProvider>()
                .AddScoped<PBIDefinitionConsumer>()
                .AddScoped<AuthHeaderHandler>()
                .AddRefitClient<IPBIApi>(settings => new RefitSettings
                {
                    ContentSerializer = new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
                    {
                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        TypeNameHandling = TypeNameHandling.None
                    })
                })
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(PowerBIApiUrl))
                .AddHttpMessageHandler<AuthHeaderHandler>();
        }

        public void PostConfigure(IRecurringJobManager recurringJobManager)
        {
            recurringJobManager.CreateRecurringJob<PBIDefinitionConsumer>(jobName: "PBIDefinitions", methodCall: y => y.ConsumeDefinitions(), cronSchedule: Cron.Daily(), triggerIfNeverExecuted: true);
        }
    }
}
