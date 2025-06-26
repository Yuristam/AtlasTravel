namespace AtlasTravel.MVC.Models
{
    public class City
    {
        public int CityID { get; set; }
        public string Name { get; set; }
        public bool IsCapitalCity { get; set; } = false;

        public int LandmarkID { get; set; }
        public Landmark Landmark { get; set; }
    }
}
