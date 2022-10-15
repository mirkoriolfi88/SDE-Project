using SDE_Project.Models;

namespace SDE_Project.Response
{
    public class NationOfCity
    {
        public Esito _esito { get; set; }
        public string NationDescription { get; set; }
        public string CityDescription { get; set; }

        public NationOfCity()
        {
            _esito = new Esito();
            NationDescription = string.Empty;
            CityDescription = string.Empty;
        }
    }
}
