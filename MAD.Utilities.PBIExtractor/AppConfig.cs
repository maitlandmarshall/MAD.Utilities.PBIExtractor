namespace MAD.Utilities.PBIExtractor
{
    public class AppConfig
    {
        public string ConnectionString { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }
        public IEnumerable<string> Scopes { get; set; }
    }
}
