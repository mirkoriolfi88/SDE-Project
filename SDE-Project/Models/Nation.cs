
namespace SDE_Project.Models
{
    public class Nation
    {
        public string NationCode { get; set; }
        public string NationDescription { get; set; }

        public Nation()
        {
            NationCode = string.Empty;
            NationDescription = string.Empty;
        }
    }
}
