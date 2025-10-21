using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace CyberPulse.Shared.EntitiesDTO.Chipp;

public class TypeOfPoblationDTO
{
    public int Id { get; set; }

    [Display(Name = "Name", ResourceType = typeof(Literals))]
    [MaxLength(80, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Name { get; set; } = null!;

    [Display(Name = "Value", ResourceType = typeof(Literals))]
    [Range(0, int.MaxValue, ErrorMessageResourceName = "ValueRange", ErrorMessageResourceType = typeof(Literals))]
    public int Quantity { get; set; }
    public int ChipDTOId { get; set; }
    public ChipDTO? ChipDTO { get; set; }
}
