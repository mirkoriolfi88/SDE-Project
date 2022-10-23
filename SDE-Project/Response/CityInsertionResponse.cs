using SDE_Project.Models;

namespace SDE_Project.Response
{
    public class CityInsertionResponse
    {
        public Esito _esito { get; set; }
        public int IDCity { get; set; }

        public CityInsertionResponse()
        {
            _esito = new Esito();
            IDCity = -1;
        }
    }
}
