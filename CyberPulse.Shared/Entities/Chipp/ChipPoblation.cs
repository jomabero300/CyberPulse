using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPulse.Shared.Entities.Chipp;

[Table("ChipPoblations", Schema = "Chip")]
public class ChipPoblation
{
    public int Id { get; set; }

    [Display(Name = "Chip", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int ChipId { get; set; }

    [Display(Name = "TypePoblation", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int TypePoblationId { get; set; }

    [Display(Name = "Quantity", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int Quantity { get; set; }

    public TypeOfPoblation TypePoblation { get; set; } = null!;
    public Chip Chip { get; set; } = null!;
}
