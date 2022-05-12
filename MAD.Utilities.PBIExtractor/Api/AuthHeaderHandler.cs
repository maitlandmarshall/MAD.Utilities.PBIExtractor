using System.Net.Http.Headers;

namespace MAD.Utilities.PBIExtractor.Api
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly IAadAccessTokenProvider aadAccessTokenProvider;

        public AuthHeaderHandler(IAadAccessTokenProvider aadAccessTokenProvider)
        {
            this.aadAccessTokenProvider = aadAccessTokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await aadAccessTokenProvider.GetAccessToken();            

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);            

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
