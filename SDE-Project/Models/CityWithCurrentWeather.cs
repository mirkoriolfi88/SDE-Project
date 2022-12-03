using SDE_Project.WeatherClass;

namespace SDE_Project.Models
{
    public class CityWithCurrentWeather
    {
        public int IDCity { get; set; }
        public string CityCode { get; set; }
        public string CodeNation { get; set; }
        public string CityDescription { get; set; }
        public List<CurrentConditions> currentConditions { get; set; }

        public CityWithCurrentWeather()
        {
            IDCity = -1;
            CityCode = string.Empty;
            CodeNation = string.Empty;
            CityDescription = string.Empty;
            currentConditions = new List<CurrentConditions>();
        }
    }
}
