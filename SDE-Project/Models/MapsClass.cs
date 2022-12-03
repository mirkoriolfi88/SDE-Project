namespace SDE_Project.Models
{
    public class MapsClass
    {
        public string googleMaps { get; set; }
        public string openStreetMaps { get; set; }

        public MapsClass()
        {
            googleMaps = string.Empty;
            openStreetMaps = string.Empty;
        }
    }
}
