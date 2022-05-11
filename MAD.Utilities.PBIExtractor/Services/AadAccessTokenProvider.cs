using Microsoft.Identity.Client;

namespace MAD.Utilities.PBIExtractor.Services
{
    public class AadAccessTokenProvider : IAadAccessTokenProvider
    {
        private readonly AppConfig appConfig;

        public AadAccessTokenProvider(AppConfig appConfig)
        {
            this.appConfig = appConfig;
        }

        public async Task<string> GetAccessToken()
        {
            // Create a confidential client to authorize the app
            var clientApp = ConfidentialClientApplicationBuilder.Create(this.appConfig.ClientId)
                .WithClientSecret(this.appConfig.ClientSecret)
                .WithAuthority($"https://login.microsoftonline.com/{this.appConfig.TenantId}")
                .Build();

            // Acquire an access token if one is not available in cache            
            var result = await clientApp.AcquireTokenForClient(this.appConfig.Scopes).ExecuteAsync();

            return result?.AccessToken;
        }
    }
}
