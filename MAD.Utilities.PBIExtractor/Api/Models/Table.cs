namespace MAD.Utilities.PBIExtractor.Api.Models
{
    public class Table
    {
        public string Name { get; set; }
        public IEnumerable<Measure> Measures { get; set; }
    }
}
