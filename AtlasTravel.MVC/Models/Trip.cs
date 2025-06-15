namespace AtlasTravel.MVC.Models
{
    public class Trip
    {
        public int TripID { get; set; }
        public string Title { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public byte CountryID { get; set; }
        public Country Country { get; set; }
    }
}
