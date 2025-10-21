using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPulse.Shared.Entities.Chipp;

[Table("TriningLevels", Schema = "Chip")]
public class TriningLevel
{
    public int Id { get; set; }

    [Column(TypeName = "varchar(30)")]
    [Display(Name = "Name", ResourceType = typeof(Literals))]
    [MaxLength(30, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Name { get; set; } = null!;

    public ICollection<ChipProgram>? ChipProgram { get; set; }
}
