using System.ComponentModel.DataAnnotations;

namespace AtlasTravel.MVC.Enums
{
    public enum BestTimesForVisit
    {
        [Display(Name = "All Season")]
        All_Season = 1,
        [Display(Name = "Winter")]
        Winter = 2,
        [Display(Name = "Spring")]
        Spring = 3,
        [Display(Name = "Summer")]
        Summer = 4,
        [Display(Name = "Autumn")]
        Autumn = 5,
    }
}
