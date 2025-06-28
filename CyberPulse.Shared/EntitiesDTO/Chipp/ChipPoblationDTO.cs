using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace CyberPulse.Shared.EntitiesDTO.Chipp;

public class ChipPoblationDTO
{
    public int Id { get; set; }

    [Display(Name = "Chip", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int ChipDTOId { get; set; }

    [Display(Name = "TypePoblation", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int TypePoblationId { get; set; }

    [Display(Name = "Count", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int Count { get; set; }


    public ChipDTO? ChipDTO { get; set; }
    public TypeOfPoblation? TypePoblation { get; set; }
}
