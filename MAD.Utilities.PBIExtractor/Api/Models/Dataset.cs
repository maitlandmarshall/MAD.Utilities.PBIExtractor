namespace MAD.Utilities.PBIExtractor.Api.Models
{
    public class Dataset
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Table> Tables { get; set; }
    }
}
