namespace AtlasTravel.MVC.Models
{
    public class CityCuisine
    {
        public int CityID { get; set; }
        public City City { get; set; }
        public int CuisineID { get; set; }
        public Cuisine Cuisine { get; set; }
    }
}
