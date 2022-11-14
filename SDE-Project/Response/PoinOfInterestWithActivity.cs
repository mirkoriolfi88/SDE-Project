using SDE_Project.Models;

namespace SDE_Project.Response
{
    public class PoinOfInterestWithActivity
    {
        public PointOfInterest _pointOfInterest { get; set; }
        public List<BusinessActivity> _listActivity { get; set; }

        public PoinOfInterestWithActivity()
        {
            _pointOfInterest = new PointOfInterest();
            _listActivity = new List<BusinessActivity>();
        }
    }
}
