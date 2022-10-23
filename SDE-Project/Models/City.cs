namespace SDE_Project.Models
{
    public class City
    {
        public int IDCity { get; set; }
        public string CityCode { get; set; }
        public string CodeNation { get; set; }
        public string CityDescription { get; set; }

        public City()
        {
            IDCity = -1;
            CityCode = string.Empty;
            CodeNation = string.Empty;
            CityDescription = string.Empty;
        }
    }
}
