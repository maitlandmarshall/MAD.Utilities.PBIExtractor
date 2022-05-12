namespace MAD.Utilities.PBIExtractor.Api.Models
{
    public class Workspace
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Dataset> Datasets { get; set; }
    }
}
