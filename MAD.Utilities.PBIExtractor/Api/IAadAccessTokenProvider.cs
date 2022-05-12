using Microsoft.Identity.Client;

namespace MAD.Utilities.PBIExtractor.Api
{
    public interface IAadAccessTokenProvider
    {
        Task<string> GetAccessToken();
    }
}