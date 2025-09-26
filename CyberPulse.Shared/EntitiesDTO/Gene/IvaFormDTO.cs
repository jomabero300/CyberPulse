using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace CyberPulse.Shared.EntitiesDTO.Gene;

public class IvaFormDTO:IvaDTO
{
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public StatuDTO? Statu { get; set; }
}
