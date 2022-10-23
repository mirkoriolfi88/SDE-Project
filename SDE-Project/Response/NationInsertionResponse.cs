using SDE_Project.Models;

namespace SDE_Project.Response
{
    public class NationInsertionResponse
    {
        public Esito _esito { get; set; }
        public string NationCode { get; set; }

        public NationInsertionResponse()
        {
            _esito = new Esito();
            NationCode = string.Empty;
        }
    }
}
