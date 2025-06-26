using AtlasTravel.MVC.Enums;

namespace AtlasTravel.MVC.Models
{
    public class Landmark
    {
        public int LandmarkID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BestTimesForVisit BestVisitTime { get; set; }
        public LandscapeType LandscapeType { get; set; }
    }
}
