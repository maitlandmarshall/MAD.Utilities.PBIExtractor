namespace MAD.Utilities.PBIExtractor.Api.Models
{
    public class ScanRequest
    {
        public string Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Status { get; set; }
        public PowerBIErrorResponseDetail Error { get; set; }
    }
}
