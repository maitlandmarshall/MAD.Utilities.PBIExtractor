using Microsoft.Identity.Client;

namespace MAD.Utilities.PBIExtractor.Services
{
    public interface IAadAccessTokenProvider
    {
        Task<string> GetAccessToken();
    }
}