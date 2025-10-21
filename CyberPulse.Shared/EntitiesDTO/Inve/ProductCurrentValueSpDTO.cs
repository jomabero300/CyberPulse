using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace CyberPulse.Shared.EntitiesDTO.Inve;

public class ProductCurrentValueSpDTO
{
    [Display(Name = "Percentage", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    [Range(0, double.MaxValue, ErrorMessageResourceName = "RequiredWorth", ErrorMessageResourceType = typeof(Literals))]
    public double Percentage { get; set; }
}
