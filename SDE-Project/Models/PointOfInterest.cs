namespace SDE_Project.Models
{
    public class PointOfInterest
    {
        public int ID { get; set; }
        public int IDCity { get; set; }
        public string Description { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public PointOfInterest()
        {
            ID = -1;
            IDCity = -1;
            Description = string.Empty;
            Latitude = string.Empty;
            Longitude = string.Empty;
        }
    }
}
