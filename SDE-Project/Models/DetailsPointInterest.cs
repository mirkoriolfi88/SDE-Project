using System.ComponentModel;

namespace SDE_Project.Models
{
    public class DetailsPoint
    {
        public string NationCode { get; set; }
        public string NationDescription { get; set; }
        public string CityDescription { get; set; }
        public PointOfInterest item { get; set; }

        public DetailsPoint()
        {
            NationCode = string.Empty;
            NationDescription = string.Empty;
            CityDescription = string.Empty;
            item = new PointOfInterest();
        }
    }
}
