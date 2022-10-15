namespace SDE_Project.Models
{
    public class Esito
    {
        public bool Executed { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }

        public Esito()
        {
            Executed = true;
            ErrorCode = string.Empty;
            ErrorDescription = string.Empty;
        }
    }
}
