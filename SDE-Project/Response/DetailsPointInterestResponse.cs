using SDE_Project.Models;

namespace SDE_Project.Response
{
    public class DetailsPointInterestResponse
    {
        public Esito esito { get; set; }
        public List<DetailsPoint> details { get; set; }

        public DetailsPointInterestResponse()
        {
            esito = new Esito();
            details = new List<DetailsPoint>();
        }
    }
}
