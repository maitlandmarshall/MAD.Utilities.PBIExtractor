using Microsoft.Identity.Client;

namespace MAD.Utilities.PBIExtractor.Api
{
    public class AadAccessTokenProvider : IAadAccessTokenProvider
    {
        private readonly AppConfig appConfig;
        private const string MicrosoftAuthUrl = "https://login.microsoftonline.com";

        public AadAccessTokenProvider(AppConfig appConfig)
        {
            this.appConfig = appConfig;
        }

        public async Task<string> GetAccessToken()
        {
            // Create a confidential client to authorize the app
            var clientApp = ConfidentialClientApplicationBuilder.Create(appConfig.ClientId)
                .WithClientSecret(appConfig.ClientSecret)
                .WithAuthority($"{MicrosoftAuthUrl}/{appConfig.TenantId}")
                .Build();

            // Acquire an access token if one is not available in cache            
            var result = await clientApp.AcquireTokenForClient(appConfig.Scopes).ExecuteAsync();

            return result?.AccessToken;
        }
    }
}
