using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPulse.Shared.Entities.Chipp;

[Table("TypeOfPoblations", Schema = "Chip")]
public class TypeOfPoblation
{
    public int Id { get; set; }

    [Column(TypeName = "varchar(80)")]
    [Display(Name = "Name", ResourceType = typeof(Literals))]
    [MaxLength(80, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Name { get; set; } = null!;

    public ICollection<ChipPoblation>? ChipPoblations { get; set; }

}
