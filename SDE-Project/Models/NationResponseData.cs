namespace SDE_Project.Models
{
    public class NationResponseData
    {
        public string NationCode { get; set; }
        public string NationDescription { get; set; }
        public string region { get; set; }
        public string subregion { get; set; }
        public double area { get; set; }
        public List<string> borders { get; set; }
        public List<string> timeZone { get; set; }
        public MapsClass maps { get; set; }
        public List<string> continents { get; set; }
        public List<Currencies> currencies { get; set; }

        public NationResponseData()
        {
            NationCode = string.Empty;
            NationDescription = string.Empty;
            currencies = new List<Currencies>();
        }
    }
}
