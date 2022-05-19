using Microsoft.PowerBI.Api;
using Microsoft.Rest;

namespace MAD.Utilities.PBIExtractor.Services
{
    public class PowerBIClientFactory
    {
        private readonly IAadAccessTokenProvider aadAccessTokenProvider;

        public PowerBIClientFactory(IAadAccessTokenProvider aadAccessTokenProvider)
        {
            this.aadAccessTokenProvider = aadAccessTokenProvider;
        }

        public async Task<IPowerBIClient> Create()
        {
            var accessToken = await this.aadAccessTokenProvider.GetAccessToken();

            var tokenCredentials = new TokenCredentials(accessToken);

            return new PowerBIClient(tokenCredentials);
        }
    }
}
