using System.ComponentModel.DataAnnotations;

namespace AtlasTravel.MVC.Enums
{
    public enum LandscapeType : byte
    {
        [Display(Name = "Desert")]
        Desert = 1,
        [Display(Name = "Sea")]
        Sea = 2,
        [Display(Name = "Mountain")]
        Mountain = 3,
        [Display(Name = "Forest")]
        Forest = 4,
        [Display(Name = "Volcano")]
        Volcano = 5,
        [Display(Name = "Architectural")]
        Architectural = 6,
    }
}
